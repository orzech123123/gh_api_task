using System.Net;
using CSharpFunctionalExtensions;
using RestSharp;

namespace GhApiTask.Clients
{
    public class GithubApiClient
    {
        private const string ApiUrl = "https://api.github.com";
        private const string CommitsUrlPart = "repos/{0}/{1}/commits";

        public async Task<Result<IEnumerable<GithubCommitResponse>>> GetCommitsAsync(string username, string repository)
        {
            var client = new RestClient(ApiUrl);

            var request = new RestRequest(string.Format(CommitsUrlPart, username, repository));

            var response = await client.ExecuteAsync<IEnumerable<GithubCommitResponse>>(request);

            if (!response.IsSuccessful)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return Result.Failure<IEnumerable<GithubCommitResponse>>($"Result not found for {username}/{repository}.");
                }

                return Result.Failure<IEnumerable<GithubCommitResponse>>($"Failed to fetch commits.");
            }

            return Result.Success(response.Data);
        }
    }

    public class GithubCommitResponse
    {
        public string Sha { get; set; }
        public CommitResponse Commit { get; set; }
    }

    public class CommitResponse
    {
        public string Message { get; set; }
        public CommitterResponse Committer { get; set; }
    }

    public class CommitterResponse
    {
        public string Email { get; set; }
    }
}
