using MandarinJuiceCore.Infrastructure;

namespace MandarinJuiceCore.GamingPlatforms;

public class Epic : IGamingPlatform
{
    public string AppId { get; set; } = string.Empty;
    public string UserIdInput { get; set; } = string.Empty;
    public string UserIdOutput { get; set; } = string.Empty;
    public uint ParseVariant { get; set; } = 0;

    public const string StoreBaseUrl = "https://store.epicgames.com/en/p";
    public void OpenStoreProductPage() => $"{StoreBaseUrl}/{AppId}".OpenUrl();

    public ulong GetParsedUserIdInput() => ParseUserId(UserIdInput);
    public ulong GetParsedUserIdOutput() => ParseUserId(UserIdOutput);

    public ulong ParseUserId(string userId) 
        => ParseUserIdInternal(uint.Parse(userId));

    public ulong ParseUserId(uint userId) 
        => ParseUserIdInternal(userId);

    public ulong ParseUserId(ulong userId)
        => ParseUserIdInternal((uint)userId);

    private ulong ParseUserIdInternal(uint userId)
    {
        return ParseVariant switch
        {
            0 => ~userId | 0xFFFFFFFF00000000,
            _ => throw new NotSupportedException($"The Epic ID variant '{ParseVariant}' is not supported.")
        };
    }
}