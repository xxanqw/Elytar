using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Elytar.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        var version = Assembly.GetEntryAssembly()
            ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion ?? "0.0.0";
        
        if (version.Contains('+')) version = version.Split('+')[0];

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
    <link href=""https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&family=JetBrains+Mono:wght@400;500&display=swap"" rel=""stylesheet"">
    <style>
        :root {
            --bg-color: #0f172a;
            --card-bg: #1e293b;
            --text-primary: #f8fafc;
            --text-secondary: #94a3b8;
            --accent: #10b981;
            --accent-hover: #059669;
            --code-bg: #020617;
            --border: #334155;
        }

        * { box-sizing: border-box; }

        body {
            font-family: 'Inter', system-ui, sans-serif;
            background-color: var(--bg-color);
            color: var(--text-primary);
            margin: 0;
            padding: 0;
            line-height: 1.6;
        }

        .container {
            max-width: 1100px;
            margin: 0 auto;
            padding: 2rem;
        }

        header {
            text-align: center;
            margin-bottom: 3rem;
            padding: 2rem 0 2rem;
            border-bottom: 1px solid var(--border);
        }

        h1 {
            font-size: 3.5rem;
            font-weight: 800;
            margin: 0;
            background: linear-gradient(to right, #34d399, #3b82f6);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            letter-spacing: -0.05em;
        }

        .subtitle {
            color: var(--text-secondary);
            font-size: 1.25rem;
            margin-top: 1rem;
        }

        .badges {
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 0.75rem;
            margin-top: 1rem;
        }

        .version-badge {
            background: rgba(16, 185, 129, 0.1);
            color: var(--accent);
            padding: 0.25rem 0.75rem;
            border-radius: 9999px;
            font-size: 0.875rem;
            font-weight: 600;
            border: 1px solid rgba(16, 185, 129, 0.2);
        }

        .github-btn {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            background: rgba(255, 255, 255, 0.1);
            color: var(--text-primary);
            padding: 0.25rem 0.75rem;
            border-radius: 9999px;
            font-size: 0.875rem;
            font-weight: 600;
            border: 1px solid var(--border);
            transition: all 0.2s;
            text-decoration: none;
        }
        .github-btn:hover {
            background: rgba(255, 255, 255, 0.2);
            text-decoration: none;
        }

        /* Tabs */
        .tabs {
            display: flex;
            justify-content: center;
            gap: 1rem;
            margin-bottom: 3rem;
        }

        .tab-btn {
            background: transparent;
            border: none;
            color: var(--text-secondary);
            font-size: 1rem;
            font-weight: 600;
            padding: 0.75rem 1.5rem;
            cursor: pointer;
            border-radius: 9999px;
            transition: all 0.2s;
        }

        .tab-btn:hover {
            color: var(--text-primary);
            background: rgba(255,255,255,0.05);
        }

        .tab-btn.active {
            color: var(--bg-color);
            background: var(--accent);
        }

        .tab-content {
            display: none;
            animation: fadeIn 0.3s ease;
        }

        .tab-content.active {
            display: block;
        }

        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(10px); }
            to { opacity: 1; transform: translateY(0); }
        }

        /* Documentation Cards */
        .endpoint-card {
            background: var(--card-bg);
            border-radius: 16px;
            padding: 2rem;
            margin-bottom: 2rem;
            border: 1px solid var(--border);
        }

        .endpoint-section {
            display: grid;
            grid-template-columns: 1fr 200px;
            gap: 2rem;
            align-items: center;
        }

        .endpoint-info h2 {
            margin-top: 0;
            color: var(--accent);
            font-size: 1.5rem;
        }

        .endpoint-desc {
            color: var(--text-secondary);
            margin-bottom: 1.5rem;
        }

        .divider {
            border: 0;
            height: 1px;
            background: var(--border);
            margin: 2rem 0;
        }

        .code-block {
            background: var(--code-bg);
            padding: 1rem;
            border-radius: 8px;
            font-family: 'JetBrains Mono', monospace;
            font-size: 0.9rem;
            color: #e2e8f0;
            border: 1px solid var(--border);
            overflow-x: auto;
        }

        .code-line {
            display: block;
            margin-bottom: 0.5rem;
        }
        .code-line:last-child { margin-bottom: 0; }
        .method { color: #a78bfa; font-weight: bold; margin-right: 0.5rem; }
        .url { color: #93c5fd; }

        .preview-image {
            width: 180px;
            height: 180px;
            background: rgba(0,0,0,0.2);
            border-radius: 12px;
            padding: 10px;
            image-rendering: pixelated;
            object-fit: contain;
            border: 1px solid var(--border);
        }

        /* Playground */
        .playground-container {
            background: var(--card-bg);
            border-radius: 16px;
            padding: 2rem;
            border: 1px solid var(--border);
        }

        .controls {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 1.5rem;
            margin-bottom: 2rem;
        }

        .control-group {
            display: flex;
            flex-direction: column;
            gap: 0.5rem;
        }

        label {
            font-size: 0.875rem;
            font-weight: 600;
            color: var(--text-secondary);
        }

        input, select {
            background: var(--bg-color);
            border: 1px solid var(--border);
            color: var(--text-primary);
            padding: 0.75rem;
            border-radius: 8px;
            font-family: inherit;
            font-size: 1rem;
        }

        input:focus, select:focus {
            outline: none;
            border-color: var(--accent);
        }

        .pg-result {
            background: var(--bg-color);
            border-radius: 12px;
            padding: 2rem;
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 1.5rem;
            border: 1px solid var(--border);
        }

        .pg-url-display {
            background: var(--code-bg);
            padding: 1rem;
            border-radius: 8px;
            font-family: 'JetBrains Mono', monospace;
            color: var(--accent);
            word-break: break-all;
            text-align: center;
            cursor: pointer;
        }
        
        .pg-url-display:hover {
            opacity: 0.8;
        }

        footer {
            text-align: center;
            margin-top: 4rem;
            color: var(--text-secondary);
            font-size: 0.9rem;
        }

        a { color: var(--accent); text-decoration: none; }
        a:hover { text-decoration: underline; }

        @media (max-width: 768px) {
            .endpoint-section { grid-template-columns: 1fr; }
            .preview-image { margin: 0 auto; }
            h1 { font-size: 2.5rem; }
        }
    </style>
</head>
<body>
    <div class=""container"">
        <header>
            <h1>Elytar</h1>
            <div class=""badges"">
                <div class=""version-badge"">v{{VERSION}}</div>
                <a href=""https://github.com/xxanqw/Elytar"" target=""_blank"" class=""github-btn"">
                    <svg height=""16"" width=""16"" viewBox=""0 0 16 16"" fill=""currentColor"">
                        <path d=""M8 0C3.58 0 0 3.58 0 8c0 3.54 2.29 6.53 5.47 7.59.4.07.55-.17.55-.38 0-.19-.01-.82-.01-1.49-2.01.37-2.53-.49-2.69-.94-.09-.23-.48-.94-.82-1.13-.28-.15-.68-.52-.01-.53.63-.01 1.08.58 1.23.82.72 1.21 1.87.87 2.33.66.07-.52.28-.87.51-1.07-1.78-.2-3.64-.89-3.64-3.95 0-.87.31-1.59.82-2.15-.08-.2-.36-1.02.08-2.12 0 0 .67-.21 2.2.82.64-.18 1.32-.27 2-.27.68 0 1.36.09 2 .27 1.53-1.04 2.2-.82 2.2-.82.44 1.1.16 1.92.08 2.12.51.56.82 1.27.82 2.15 0 3.07-1.87 3.75-3.65 3.95.29.25.54.73.54 1.48 0 1.07-.01 1.93-.01 2.2 0 .21.15.46.55.38A8.013 8.013 0 0016 8c0-4.42-3.58-8-8-8z""></path>
                    </svg>
                    GitHub
                </a>
            </div>
            <div class=""subtitle"">High-performance skin avatar API for Ely.by</div>
        </header>

        <nav class=""tabs"">
            <button class=""tab-btn active"" onclick=""switchTab('docs')"">Documentation</button>
            <button class=""tab-btn"" onclick=""switchTab('playground')"">Playground</button>
        </nav>

        <main id=""docs"" class=""tab-content active"">
            <div class=""endpoint-card"" style=""display: block;"">
                <!-- Avatar -->
                <div class=""endpoint-section"">
                    <div class=""endpoint-info"">
                        <h2>Avatar</h2>
                        <p class=""endpoint-desc"">The standard face view of the player. Perfect for chat heads, tab lists, and comments.</p>
                        <div class=""code-block"">
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/avatar/{username}?size={pixels}</span></span>
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/{username}/{size}</span></span>
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/{username}/{size}.png</span></span>
                        </div>
                    </div>
                    <img src=""/avatar/kizaru123?size=180"" alt=""Avatar Preview"" class=""preview-image"">
                </div>

                <hr class=""divider"">

                <!-- Bust -->
                <div class=""endpoint-section"">
                    <div class=""endpoint-info"">
                        <h2>Bust</h2>
                        <p class=""endpoint-desc"">A head and shoulders view. Great for profile pages and user cards.</p>
                        <div class=""code-block"">
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/bust/{username}?size={pixels}</span></span>
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/bust/{username}/{size}</span></span>
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/bust/{username}/{size}.png</span></span>
                        </div>
                    </div>
                    <img src=""/bust/CocoNutBlack?size=180"" alt=""Bust Preview"" class=""preview-image"">
                </div>

                <hr class=""divider"">

                <!-- Body Front -->
                <div class=""endpoint-section"">
                    <div class=""endpoint-info"">
                        <h2>Body (Front)</h2>
                        <p class=""endpoint-desc"">The full body view from the front. Shows off the player's outfit in all its glory.</p>
                        <div class=""code-block"">
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/body/{username}?size={pixels}</span></span>
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/body/{username}/{size}</span></span>
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/body/{username}/{size}.png</span></span>
                        </div>
                    </div>
                    <img src=""/body/AdiK_0din4re88?size=180"" alt=""Body Front Preview"" class=""preview-image"">
                </div>

                <hr class=""divider"">

                <!-- Body Back -->
                <div class=""endpoint-section"">
                    <div class=""endpoint-info"">
                        <h2>Body (Back)</h2>
                        <p class=""endpoint-desc"">The full body view from the back. Because sometimes you need to see the back design.</p>
                        <div class=""code-block"">
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/body/back/{username}?size={pixels}</span></span>
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/body/back/{username}/{size}</span></span>
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/body/back/{username}/{size}.png</span></span>
                        </div>
                    </div>
                    <img src=""/body/back/KEBEN?size=180"" alt=""Body Back Preview"" class=""preview-image"">
                </div>

                <hr class=""divider"">

                <!-- Raw Skin -->
                <div class=""endpoint-section"">
                    <div class=""endpoint-info"">
                        <h2>Raw Skin</h2>
                        <p class=""endpoint-desc"">The original skin texture file. Useful for skin editors or custom rendering.</p>
                        <div class=""code-block"">
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/skin/{username}</span></span>
                            <span class=""code-line""><span class=""method"">GET</span><span class=""url"">/download/{username}</span></span>
                        </div>
                    </div>
                    <img src=""/skin/Ellind"" alt=""Raw Skin Preview"" class=""preview-image"">
                </div>
            </div>
        </main>

        <main id=""playground"" class=""tab-content"">
            <div class=""playground-container"">
                <div class=""controls"">
                    <div class=""control-group"">
                        <label for=""pg-username"">Username</label>
                        <input type=""text"" id=""pg-username"" value=""kizaru123"" placeholder=""Ely.by username"">
                    </div>
                    <div class=""control-group"">
                        <label for=""pg-size"">Size (px)</label>
                        <input type=""number"" id=""pg-size"" value=""180"" min=""8"" max=""512"">
                    </div>
                    <div class=""control-group"">
                        <label for=""pg-type"">Type</label>
                        <select id=""pg-type"">
                            <option value=""avatar"">Avatar</option>
                            <option value=""bust"">Bust</option>
                            <option value=""body"">Body (Front)</option>
                            <option value=""body/back"">Body (Back)</option>
                            <option value=""skin"">Raw Skin</option>
                        </select>
                    </div>
                </div>

                <div class=""pg-result"">
                    <img id=""pg-image"" src=""/avatar/kizaru123?size=180"" class=""preview-image"" style=""width: auto; max-width: 100%; max-height: 300px;"">
                    <div id=""pg-error"" style=""display: none; color: #ef4444; font-weight: bold; text-align: center; padding: 2rem;"">
                        <div style=""font-size: 2rem; margin-bottom: 0.5rem;"">😕</div>
                        Skin not found
                    </div>
                    <div class=""pg-url-display"" id=""pg-url"" onclick=""copyUrl()"" title=""Click to copy"">
                        /avatar/kizaru123?size=180
                    </div>
                </div>
            </div>
        </main>

        <footer>
            <p>Powered by .NET 9 & ImageSharp</p>
            <p style=""opacity: 0.6; font-size: 0.8rem;"">Not affiliated with Ely.by, Minecraft, Mojang, or Microsoft.</p>
        </footer>
    </div>

    <script>
        function switchTab(tabId) {
            document.querySelectorAll('.tab-content').forEach(el => el.classList.remove('active'));
            document.querySelectorAll('.tab-btn').forEach(el => el.classList.remove('active'));
            
            document.getElementById(tabId).classList.add('active');
            event.target.classList.add('active');
        }

        const inputs = ['pg-username', 'pg-size', 'pg-type'];
        inputs.forEach(id => {
            document.getElementById(id).addEventListener('input', updatePlayground);
        });

        const img = document.getElementById('pg-image');
        const errorMsg = document.getElementById('pg-error');

        img.onerror = function() {
            img.style.display = 'none';
            errorMsg.style.display = 'block';
        };

        img.onload = function() {
            img.style.display = 'block';
            errorMsg.style.display = 'none';
        };

        function updatePlayground() {
            const username = document.getElementById('pg-username').value || 'kizaru123';
            const size = document.getElementById('pg-size').value;
            const type = document.getElementById('pg-type').value;
            
            let url = '';
            if (type === 'skin') {
                url = `/skin/${username}`;
                document.getElementById('pg-size').parentElement.style.opacity = '0.5';
                document.getElementById('pg-size').disabled = true;
            } else {
                url = `/${type}/${username}?size=${size}`;
                document.getElementById('pg-size').parentElement.style.opacity = '1';
                document.getElementById('pg-size').disabled = false;
            }

            const fullUrl = window.location.origin + url;
            document.getElementById('pg-image').src = url;
            document.getElementById('pg-url').textContent = fullUrl;
        }

        function copyUrl() {
            const url = document.getElementById('pg-url').textContent;
            navigator.clipboard.writeText(url).then(() => {
                const original = document.getElementById('pg-url').style.color;
                document.getElementById('pg-url').style.color = '#4ade80';
                setTimeout(() => {
                    document.getElementById('pg-url').style.color = original;
                }, 500);
            });
        }
        
        // Initialize
        updatePlayground();
    </script>
</body>
</html>";

        return Content(html.Replace("{{VERSION}}", version), "text/html");
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
