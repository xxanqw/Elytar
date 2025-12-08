namespace Elytar.Services;

public interface IElyByService
{
    Task<byte[]?> GetSkinAsync(string username);
    Task<SkinContext?> GetSkinContextAsync(string username);
}

public record SkinContext(byte[] SkinBytes, bool IsSlim);
