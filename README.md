# Elytar

A high-performance, Minotar-like skin avatar API for `Ely.by`, built with `.NET 9` and `SixLabors.ImageSharp`.

## Features

- **Ely.by Integration**: Fetches skins directly from Ely.by's skin system.
- **Smart Rendering**:
  - **Avatar**: 2D Face + Hat layer.
  - **Bust**: Head & Shoulders view with armor layer.
  - **Full Body**: Front and Back views of the complete skin.
- **Slim Skin Support**: Automatically detects and correctly renders "Alex" (slim/3px arm) models based on Ely.by metadata.
- **Dynamic Favicons**: Browser tabs show the avatar of the user being viewed via smart headers and routing.
- **Performance**: In-memory caching for skin data to reduce external API calls.

## Prerequisites

- .NET 9 SDK
- AspNet Runtime 9

## Running

1. Restore dependencies:
   ```bash
   dotnet restore
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Open `http://localhost:5000` in your browser.

## API Endpoints

All endpoints support an optional `size` parameter (default: 180, min: 8, max: 512).

### Avatar
```
GET /avatar/{username}?size={pixels}
```
Returns the standard face view (8x8 head + hat).

### Bust
```
GET /bust/{username}?size={pixels}
```
Returns a head and shoulders render, including the second layer.

### Body (Front)
```
GET /skin/{username}?size={pixels}
```
Returns the full body front view.

### Body (Back)
```
GET /skin/back/{username}?size={pixels}
```
Returns the full body back view.

## Project Structure

- `Controllers/`:
  - `AvatarController.cs`: Handles image generation endpoints.
  - `HomeController.cs`: Serves the index page and handles favicon logic.
- `Services/`:
  - `ElyByService.cs`: Fetches skin textures and metadata (detects slim models).
  - `ImageProcessingService.cs`: Handles cropping, resizing, and compositing using ImageSharp.
- `Program.cs`: Application entry point and dependency injection configuration.
