namespace GithubAPICSharp.Models.UserInfo;

public readonly record struct QueryUserInfoResponse(
    string Username,
    string Bio,
    string Email,
    string DataCreated,
    int ReposCount = 0,
    int Followers = 0,
    List<string> Tags = null!
);