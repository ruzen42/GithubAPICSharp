using GithubAPICSharp.Models;

namespace GithubAPICSharp.Services;

public interface IGitHubService
{
    Task<QueryRepoInfoResponse> GetRepoInfoAsync(string user, string repo);
    Task<QueryUserInfoResponse> GetUserInfoAsync(string user);
}