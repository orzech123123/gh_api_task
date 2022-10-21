using CommandLine;
using CSharpFunctionalExtensions;
using GhApiTask.Clients;
using Microsoft.Data.Sqlite;

namespace GhApiTask
{
    public class Program
    {
        private const string DbName = "commits.db";

        public static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args)
                .MapResult(async options =>
                    {
                        var githubApiClient = new GithubApiClient();

                        await githubApiClient.GetCommitsAsync(options.Username, options.Repository)
                            .Tap(commits => PrintCommits(options.Repository, commits))
                            .Tap(commits => SaveCommits(options.Username, options.Repository, commits))
                            .TapError(error => Console.WriteLine($"Error: {error}"));
                    },
                    _ => Task.FromResult(0));
        }

        private static void PrintCommits(string repository, IEnumerable<GithubCommitResponse> commits)
        {
            Console.WriteLine("Commits:");
            foreach (var commit in commits)
            {
                Console.WriteLine($"[{repository}]/[{commit.Sha}]: {commit.Commit.Message} [{commit.Commiter}]");
            }
        }

        private static void SaveCommits(string username, string repository, IEnumerable<GithubCommitResponse> commits)
        {
            CreateDbIfNotExist(DbName);

            // using var connection = new SqliteConnection($"Data Source={DbName}");
            // connection.Open();
            //

        }

        private static void CreateDbIfNotExist(string databaseFileName)
        {
            if (File.Exists(databaseFileName))
            {
                return;
            }

            File.WriteAllBytes(databaseFileName, Array.Empty<byte>());
        }
    }
}
