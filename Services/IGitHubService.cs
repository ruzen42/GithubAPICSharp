using GithubAPICSharp.Models;

namespace GithubAPICSharp.Services;

public interface IGitHubService
{
    Task<QueryRepoInfoResponse> GetRepoInfoAsync(string url);
    Task<QueryUserInfoResponse> GetUserInfoAsync(string url);
}