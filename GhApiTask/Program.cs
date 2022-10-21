using CommandLine;
using CSharpFunctionalExtensions;
using GhApiTask.Clients;
using GhApiTask.Database;
using GhApiTask.Database.Records;
using Microsoft.EntityFrameworkCore;

namespace GhApiTask
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args)
                .MapResult(async options =>
                    {
                        var githubApiClient = new GithubApiClient();

                        await githubApiClient.GetCommitsAsync(options.Username, options.Repository)
                            .Tap(commits => PrintCommits(options.Repository, commits))
                            .Tap(commits => SaveCommitsAsync(options.Username, options.Repository, commits))
                            .TapError(error => Console.WriteLine($"Error: {error}"));
                    },
                    _ => Task.FromResult(0));
        }

        private static void PrintCommits(string repository, IEnumerable<GithubCommitResponse> commits)
        {
            Console.WriteLine("Commits:");
            foreach (var commit in commits)
            {
                Console.WriteLine($"[{repository}]/[{commit.Sha}]: {commit.Commit.Message} [{commit.Commit.Committer.Email}]");
            }
        }

        private static async Task SaveCommitsAsync(string username, string repository, IEnumerable<GithubCommitResponse> commits)
        {
            var databaseContext = new DatabaseContext();

            var existingShas = await databaseContext.Commits
                .Select(c => c.Sha)
                .ToListAsync();

            foreach (var commit in commits)
            {
                if (existingShas.Any(sha => sha == commit.Sha))
                {
                    continue;
                }

                databaseContext.Commits.Add(new CommitRecord
                {
                    Username = username,
                    Repository = repository,
                    Sha = commit.Sha,
                    Commiter = commit.Commit.Committer.Email,
                    Message = commit.Commit.Message
                });
            }

            await databaseContext.SaveChangesAsync();
        }
    }
}
