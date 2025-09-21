namespace GithubAPICSharp.Models.QueryRepoInfo;

public readonly record struct QueryRepoInfoResponse(
    string Username,
    string RepoName,
    string Description,
    string DataCreated,
    string License,
    int Stars = 0,
    int Issues = 0,
    string Language = null!,
    List<string> Tags = null!
);