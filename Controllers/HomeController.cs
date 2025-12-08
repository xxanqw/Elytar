using Microsoft.AspNetCore.Mvc;

namespace Elytar.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        var html = @"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Elytar - Ely.by Avatar API</title>
        <link rel=""icon"" href=""/avatar/kizaru123"" type=""image/png"">
        <link rel=""preconnect"" href=""https://fonts.googleapis.com"">
        <link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin>
        <link href=""https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&family=Roboto+Mono&display=swap"" rel=""stylesheet"">
        <style>
        :root {
            --md-sys-color-primary: #006c4c;
            --md-sys-color-on-primary: #ffffff;
            --md-sys-color-primary-container: #89f8c7;
            --md-sys-color-on-primary-container: #002114;
            --md-sys-color-surface: #fbfdf9;
            --md-sys-color-on-surface: #191c1a;
            --md-sys-color-surface-variant: #dbe5de;
            --md-sys-color-on-surface-variant: #404944;
        }

        body {
            font-family: 'Roboto', system-ui, sans-serif;
            background-color: var(--md-sys-color-surface);
            color: var(--md-sys-color-on-surface);
            margin: 0;
            padding: 2rem;
            line-height: 1.6;
        }

        .container { max-width: 1000px; margin: 0 auto; }

        .header { margin-bottom: 5rem; padding-top: 2rem; border-bottom: 1px solid var(--md-sys-color-surface-variant); padding-bottom: 2rem; }
        h1 {
            font-size: 3rem;
            font-weight: 400;
            margin: 0;
            color: var(--md-sys-color-on-surface);
        }
        .subtitle {
            color: var(--md-sys-color-on-surface-variant);
            font-size: 1.2rem;
            margin-top: 0.5rem;
        }

        .section {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 5rem;
            gap: 4rem;
        }

        .info {
            flex: 1;
            min-width: 0;
        }

        h2 {
            margin-top: 0;
            font-size: 1.75rem;
            font-weight: 700;
            color: var(--md-sys-color-on-surface);
            margin-bottom: 1rem;
        }

        p {
            color: var(--md-sys-color-on-surface-variant);
            margin-bottom: 1.5rem;
            font-size: 1rem;
        }

        .endpoint {
            background: var(--md-sys-color-primary-container);
            color: var(--md-sys-color-on-primary-container);
            padding: 1.25rem;
            border-radius: 8px;
            font-family: 'Roboto Mono', monospace;
            font-size: 0.95rem;
            white-space: nowrap;
            overflow-x: auto;
        }

        .visual {
            flex: 0 0 auto;
            display: flex;
            gap: 1rem;
        }

        .visual img {
            width: 180px;
            height: 180px;
            object-fit: contain;
            image-rendering: pixelated;
            background: #fff;
            padding: 10px;
        }

        .footer {
            text-align: center;
            margin-top: 5rem;
            padding-top: 2rem;
            border-top: 1px solid var(--md-sys-color-surface-variant);
            font-size: 0.875rem;
            color: var(--md-sys-color-on-surface-variant);
        }
        
        .disclaimer {
            background: #fff3e0;
            color: #e65100;
            padding: 10px;
            border-radius: 8px;
            margin-top: 10px;
            display: inline-block;
        }

        a { color: var(--md-sys-color-primary); text-decoration: none; font-weight: 500; }
        a:hover { text-decoration: underline; }

        @media (max-width: 768px) {
            .section { flex-direction: column; gap: 2rem; }
            .visual { width: 100%; justify-content: center; }
        }
        </style>
    </head>
    <body>
        <div class=""container"">
        <div class=""header"">
            <h1 style=""font-weight: 600; font-size: 4rem;"">Elytar</h1>
            <div class=""subtitle"">High-performance skin avatar API for Ely.by</div>
        </div>

        <!-- Avatar Section -->
        <div class=""section"">
            <div class=""info"">
            <h2>Avatar</h2>
            <p>The standard face view of the player. Perfect for chat heads, tab lists, and comments.</p>
            <div class=""endpoint"">
                GET /avatar/{username}?size={pixels}<br>
                GET /{username}/{size}<br>
                GET /{username}/{size}.png
            </div>
            </div>
            <div class=""visual"">
            <img src=""/avatar/kizaru123?size=180"" alt=""kizaru123"">
            </div>
        </div>

        <!-- Bust Section -->
        <div class=""section"">
            <div class=""info"">
            <h2>Bust</h2>
            <p>A head and shoulders view. Great for profile pages and user cards.</p>
            <div class=""endpoint"">
                GET /bust/{username}?size={pixels}<br>
                GET /bust/{username}/{size}<br>
                GET /bust/{username}/{size}.png
            </div>
            </div>
            <div class=""visual"">
            <img src=""/bust/CocoNutBlack?size=180"" alt=""CocoNutBlack"">
            </div>
        </div>

        <!-- Body Front Section -->
        <div class=""section"">
            <div class=""info"">
            <h2>Body (Front)</h2>
            <p>The full body view from the front. Shows off the player's outfit in all its glory.</p>
            <div class=""endpoint"">
                GET /body/{username}?size={pixels}<br>
                GET /body/{username}/{size}<br>
                GET /body/{username}/{size}.png
            </div>
            </div>
            <div class=""visual"">
            <img src=""/body/AdiK_0din4re88?size=180"" alt=""AdiK_0din4re88"">
            </div>
        </div>

        <!-- Body Back Section -->
        <div class=""section"">
            <div class=""info"">
            <h2>Body (Back)</h2>
            <p>The full body view from the back. Because sometimes you need to see the back design. Naughty you!</p>
            <div class=""endpoint"">
                GET /body/back/{username}?size={pixels}<br>
                GET /body/back/{username}/{size}<br>
                GET /body/back/{username}/{size}.png
            </div>
            </div>
            <div class=""visual"">
            <img src=""/body/back/KEBEN?size=180"" alt=""KEBEN"">
            </div>
        </div>

        <!-- Raw Skin Section -->
        <div class=""section"">
            <div class=""info"">
            <h2>Raw Skin</h2>
            <p>The original skin texture file. Useful for skin editors or custom rendering.</p>
            <div class=""endpoint"">
                GET /skin/{username}<br>
                GET /download/{username}
            </div>
            </div>
            <div class=""visual"">
            <img src=""/skin/kizaru123"" style=""width: 128px; height: 64px;"" alt=""Raw Skin"">
            </div>
        </div>

        <div class=""footer"">
            <p style=""margin-bottom:0"">Inspired by <a href=""https://minotar.net"" target=""_blank"">Minotar</a>.</p>
            <p style=""margin-top:0"">Powered by .NET 10 & ImageSharp</p>
            <div class=""disclaimer"">
            <strong>Disclaimer:</strong> Not affiliated with Ely.by, Minecraft, Mojang, or Microsoft.
            </div>
        </div>
        </div>
    </body>
    </html>";

        return Content(html, "text/html");
    }

    [HttpGet("favicon.ico")]
    public IActionResult Favicon()
    {
        var referer = Request.Headers.Referer.ToString();
        if (!string.IsNullOrEmpty(referer) && Uri.TryCreate(referer, UriKind.Absolute, out var uri))
        {
            var path = uri.AbsolutePath.Trim('/');
            if (path.StartsWith("avatar/") || path.StartsWith("head/"))
            {
                var parts = path.Split('/');
                if (parts.Length >= 2)
                {
                    var username = parts[1];
                    return Redirect($"/avatar/{username}?size=64");
                }
            }
        }

        return Redirect("/avatar/kizaru123?size=64");
    }

    [HttpGet("robots.txt")]
    public IActionResult Robots()
    {
        var content = "User-agent: *\nAllow: /$\nDisallow: /";
        return Content(content, "text/plain");
    }
}
