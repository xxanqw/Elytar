using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Elytar.Services;

public interface IImageProcessingService
{
    Image<Rgba32> GetAvatar(Image<Rgba32> skin, int size);
    Image<Rgba32> GetHeadRender(Image<Rgba32> skin, int size);
    Image<Rgba32> GetSkinFront(Image<Rgba32> skin, int size, bool isSlim);
    Image<Rgba32> GetSkinBack(Image<Rgba32> skin, int size, bool isSlim);
    Image<Rgba32> GetBust(Image<Rgba32> skin, int size, bool isSlim);
}
