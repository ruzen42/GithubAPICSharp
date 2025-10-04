namespace GithubAPICSharp.Models;

public record QueryUserInfoRequest(string User);

public record QueryUserInfoResponse(
    string Username,
    string Bio,
    string Email,
    string DataCreated,
    int ReposCount = 0,
    int Followers = 0
);

public record QueryRepoInfoRequest(string User, string Repo);

public record QueryRepoInfoResponse(
    string Username,
    string RepoName,
    string Description,
    string DataCreated,
    string License,
    int Stars = 0,
    int Issues = 0,
    string Language = null!
);
