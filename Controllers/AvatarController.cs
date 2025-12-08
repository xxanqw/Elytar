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
    [HttpGet("avatar/{username}/{size}")]
    [HttpGet("avatar/{username}/{size}.png")]
    [HttpGet("{username}/{size}")]
    [HttpGet("{username}/{size}.png")]
    public async Task<IActionResult> GetAvatar(string username, int size = 64)
    {
        if (string.IsNullOrWhiteSpace(username) || !System.Text.RegularExpressions.Regex.IsMatch(username, @"^[\p{L}0-9\-_.!$%^&*()\[\]:;]+$"))
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

        var imageUrl = $"{Request.Scheme}://{Request.Host}/avatar/{username}?size={size}";
        Response.Headers["Link"] = $"<{imageUrl}>; rel=\"icon\"; type=\"image/png\"";

        return File(ms, "image/png");
    }

    [HttpGet("body/{username}")]
    [HttpGet("body/{username}/{size}")]
    [HttpGet("body/{username}/{size}.png")]
    public async Task<IActionResult> GetBodyFront(string username, int size = 100)
    {
        return await RenderSkin(username, size, (img, s, isSlim) => _imageProcessingService.GetSkinFront(img, s, isSlim));
    }

    [HttpGet("body/back/{username}")]
    [HttpGet("body/back/{username}/{size}")]
    [HttpGet("body/back/{username}/{size}.png")]
    public async Task<IActionResult> GetBodyBack(string username, int size = 100)
    {
        return await RenderSkin(username, size, (img, s, isSlim) => _imageProcessingService.GetSkinBack(img, s, isSlim));
    }

    [HttpGet("skin/{username}")]
    public async Task<IActionResult> GetSkin(string username)
    {
        if (string.IsNullOrWhiteSpace(username) || !System.Text.RegularExpressions.Regex.IsMatch(username, @"^[\p{L}0-9\-_.!$%^&*()\[\]:;]+$"))
            return BadRequest("Invalid username.");

        var skinBytes = await _elyByService.GetSkinAsync(username);
        if (skinBytes == null)
            return NotFound("Skin not found.");

        return File(skinBytes, "image/png");
    }

    [HttpGet("download/{username}")]
    public async Task<IActionResult> DownloadSkin(string username)
    {
        if (string.IsNullOrWhiteSpace(username) || !System.Text.RegularExpressions.Regex.IsMatch(username, @"^[\p{L}0-9\-_.!$%^&*()\[\]:;]+$"))
            return BadRequest("Invalid username.");

        var skinBytes = await _elyByService.GetSkinAsync(username);
        if (skinBytes == null)
            return NotFound("Skin not found.");

        return File(skinBytes, "image/png", $"{username}.png");
    }

    [HttpGet("bust/{username}")]
    [HttpGet("bust/{username}/{size}")]
    [HttpGet("bust/{username}/{size}.png")]
    public async Task<IActionResult> GetBust(string username, int size = 100)
    {
        return await RenderSkin(username, size, (img, s, isSlim) => _imageProcessingService.GetBust(img, s, isSlim));
    }

    private async Task<IActionResult> RenderSkin(string username, int size, Func<Image<Rgba32>, int, bool, Image<Rgba32>> renderFunc)
    {
        if (string.IsNullOrWhiteSpace(username) || !System.Text.RegularExpressions.Regex.IsMatch(username, @"^[\p{L}0-9\-_.!$%^&*()\[\]:;]+$"))
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
