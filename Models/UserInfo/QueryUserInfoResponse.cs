namespace GithubAPICSharp.Models;

public record QueryUserInfoResponse
{
    public string Username { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string DataCreated { get; set; } = null!;
    public int ReposCount { get; set; } 
    public int Followers { get; set; } 
    public int DiskUsage { get; set; }
    public List<string> Tags { get; set; } = null!;
}