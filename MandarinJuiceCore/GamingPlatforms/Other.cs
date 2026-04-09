namespace MandarinJuiceCore.GamingPlatforms;

public class Other : IGamingPlatform
{
    public string AppId { get; set; } = string.Empty;
    public string UserIdInput { get; set; } = string.Empty;
    public string UserIdOutput { get; set; } = string.Empty;
    public uint ParseVariant { get; set; } = 0;

    public void OpenStoreProductPage()
    {
        // do nothing;
    }

    public ulong GetParsedUserIdInput() => ParseUserId(UserIdInput);
    public ulong GetParsedUserIdOutput() => ParseUserId(UserIdOutput);
    
    public ulong ParseUserId(string userId) => Convert.ToUInt64(userId);
    public ulong ParseUserId(uint userId) => userId;
    public ulong ParseUserId(ulong userId) => userId;
}