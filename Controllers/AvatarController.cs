using Elytar.Services;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Elytar.Controllers;

[ApiController]
[Route("")]
public class AvatarController : ControllerBase
{
    private readonly IElyByService _elyByService;
    private readonly IImageProcessingService _imageProcessingService;

    public AvatarController(IElyByService elyByService, IImageProcessingService imageProcessingService)
    {
        _elyByService = elyByService;
        _imageProcessingService = imageProcessingService;
    }

    [HttpGet("avatar/{username}")]
    public async Task<IActionResult> GetAvatar(string username, [FromQuery] int size = 64)
    {
        if (string.IsNullOrWhiteSpace(username) || !System.Text.RegularExpressions.Regex.IsMatch(username, "^[a-zA-Z0-9_]+$"))
            return BadRequest("Invalid username.");

        if (size < 8 || size > 512)
            return BadRequest("Size must be between 8 and 512.");

        var skinBytes = await _elyByService.GetSkinAsync(username);
        if (skinBytes == null)
            return NotFound("Skin not found.");

        using var skinImage = Image.Load<Rgba32>(skinBytes);
        using var avatar = _imageProcessingService.GetAvatar(skinImage, size);

        var ms = new MemoryStream();
        await avatar.SaveAsPngAsync(ms);
        ms.Position = 0;

        var imageUrl = $"{Request.Scheme}://{Request.Host}/avatar/{username}?size=64";
        Response.Headers["Link"] = $"<{imageUrl}>; rel=\"icon\"; type=\"image/png\"";

        return File(ms, "image/png");
    }

    [HttpGet("skin/{username}")]
    public async Task<IActionResult> GetSkinFront(string username, [FromQuery] int size = 100)
    {
        return await RenderSkin(username, size, (img, s, isSlim) => _imageProcessingService.GetSkinFront(img, s, isSlim));
    }

    [HttpGet("skin/back/{username}")]
    public async Task<IActionResult> GetSkinBack(string username, [FromQuery] int size = 100)
    {
        return await RenderSkin(username, size, (img, s, isSlim) => _imageProcessingService.GetSkinBack(img, s, isSlim));
    }

    [HttpGet("bust/{username}")]
    public async Task<IActionResult> GetBust(string username, [FromQuery] int size = 100)
    {
        return await RenderSkin(username, size, (img, s, isSlim) => _imageProcessingService.GetBust(img, s, isSlim));
    }

    private async Task<IActionResult> RenderSkin(string username, int size, Func<Image<Rgba32>, int, bool, Image<Rgba32>> renderFunc)
    {
        if (string.IsNullOrWhiteSpace(username) || !System.Text.RegularExpressions.Regex.IsMatch(username, "^[a-zA-Z0-9_]+$"))
            return BadRequest("Invalid username.");

        if (size < 8 || size > 512)
            return BadRequest("Size must be between 8 and 512.");

        var context = await _elyByService.GetSkinContextAsync(username);
        if (context == null || context.SkinBytes == null)
            return NotFound("Skin not found.");

        using var skinImage = Image.Load<Rgba32>(context.SkinBytes);
        using var resultImage = renderFunc(skinImage, size, context.IsSlim);

        var ms = new MemoryStream();
        await resultImage.SaveAsPngAsync(ms);
        ms.Position = 0;

        var imageUrl = $"{Request.Scheme}://{Request.Host}/avatar/{username}?size=64";
        Response.Headers["Link"] = $"<{imageUrl}>; rel=\"icon\"; type=\"image/png\"";

        return File(ms, "image/png");
    }
}
