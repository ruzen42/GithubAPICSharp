namespace GithubAPICSharp.Models;

public record QueryRepoInfoResponse()
{
    public string Username { get; init; } = "";
    public string RepoName { get; init; } = "";
    public int Stars { get; init; } = 0;
    public string Language { get; init; } = "";
}