using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Elytar.Services;

public class ImageProcessingService : IImageProcessingService
{
    public Image<Rgba32> GetAvatar(Image<Rgba32> skin, int size)
    {
        var head = ExtractFace(skin, 8, 8, 40, 8);

        head.Mutate(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(size, size),
            Sampler = KnownResamplers.NearestNeighbor
        }));

        return head;
    }

    public Image<Rgba32> GetHeadRender(Image<Rgba32> skin, int size)
    {
        // Extract faces
        var front = ExtractFace(skin, 8, 8, 40, 8);
        var top = ExtractFace(skin, 8, 0, 40, 0);
        var right = ExtractFace(skin, 0, 8, 32, 8);

        return GetAvatar(skin, size);
    }

    public Image<Rgba32> GetSkinFront(Image<Rgba32> skin, int size, bool isSlim)
    {
        return BuildBody(skin, size, true, isSlim);
    }

    public Image<Rgba32> GetSkinBack(Image<Rgba32> skin, int size, bool isSlim)
    {
        return BuildBody(skin, size, false, isSlim);
    }

    public Image<Rgba32> GetBust(Image<Rgba32> skin, int size, bool isSlim)
    {
        using var fullBody = GetSkinFront(skin, 160, isSlim); // Render at higher res first
        // Crop top half (head + shoulders)
        var bust = fullBody.Clone(ctx => ctx.Crop(new Rectangle(0, 0, fullBody.Width, fullBody.Height / 2)));

        bust.Mutate(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(size, size),
            Sampler = KnownResamplers.NearestNeighbor
        }));

        return bust;
    }

    private Image<Rgba32> BuildBody(Image<Rgba32> skin, int size, bool front, bool isSlim)
    {
        var is64x64 = skin.Height == 64;
        var canvas = new Image<Rgba32>(16, 32);
        int armWidth = isSlim ? 3 : 4;

        // Head
        var head = ExtractPart(skin, front ? 8 : 24, 8, 8, 8, front ? 40 : 56, 8);
        canvas.Mutate(ctx => ctx.DrawImage(head, new Point(4, 0), 1f));

        // Body
        var body = ExtractPart(skin, front ? 20 : 32, 20, 8, 12, front ? 20 : 32, 36);
        canvas.Mutate(ctx => ctx.DrawImage(body, new Point(4, 8), 1f));

        // Arms
        Image<Rgba32> leftArm, rightArm;

        if (front)
        {
            // Right Arm (User's Left)
            rightArm = ExtractPart(skin, 44, 20, armWidth, 12, 44, 36);

            // Left Arm (User's Right)
            if (is64x64)
                leftArm = ExtractPart(skin, 36, 52, armWidth, 12, 52, 52);
            else
            {
                leftArm = rightArm.Clone(ctx => ctx.Flip(FlipMode.Horizontal));
            }

            canvas.Mutate(ctx => ctx.DrawImage(rightArm, new Point(isSlim ? 1 : 0, 8), 1f));
            canvas.Mutate(ctx => ctx.DrawImage(leftArm, new Point(12, 8), 1f));
        }
        else
        {
            // Back View
            Image<Rgba32> rightArmBack = ExtractPart(skin, isSlim ? 51 : 52, 20, armWidth, 12, isSlim ? 51 : 52, 36);
            Image<Rgba32> leftArmBack;

            if (is64x64)
                leftArmBack = ExtractPart(skin, isSlim ? 43 : 44, 52, armWidth, 12, isSlim ? 59 : 60, 52);
            else
                leftArmBack = rightArmBack.Clone(ctx => ctx.Flip(FlipMode.Horizontal));

            canvas.Mutate(ctx => ctx.DrawImage(leftArmBack, new Point(isSlim ? 1 : 0, 8), 1f));
            canvas.Mutate(ctx => ctx.DrawImage(rightArmBack, new Point(12, 8), 1f));
        }

        // Legs
        Image<Rgba32> leftLeg, rightLeg;

        if (front)
        {
            rightLeg = ExtractPart(skin, 4, 20, 4, 12, 4, 36);

            if (is64x64)
                leftLeg = ExtractPart(skin, 20, 52, 4, 12, 4, 52);
            else
                leftLeg = rightLeg.Clone(ctx => ctx.Flip(FlipMode.Horizontal));

            canvas.Mutate(ctx => ctx.DrawImage(rightLeg, new Point(4, 20), 1f));
            canvas.Mutate(ctx => ctx.DrawImage(leftLeg, new Point(8, 20), 1f));
        }
        else
        {
            Image<Rgba32> rightLegBack = ExtractPart(skin, 12, 20, 4, 12, 12, 36);
            Image<Rgba32> leftLegBack;

            if (is64x64)
                leftLegBack = ExtractPart(skin, 28, 52, 4, 12, 12, 52);
            else
                leftLegBack = rightLegBack.Clone(ctx => ctx.Flip(FlipMode.Horizontal));

            canvas.Mutate(ctx => ctx.DrawImage(leftLegBack, new Point(4, 20), 1f));
            canvas.Mutate(ctx => ctx.DrawImage(rightLegBack, new Point(8, 20), 1f));
        }

        canvas.Mutate(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(size / 2, size),
            Sampler = KnownResamplers.NearestNeighbor
        }));

        return canvas;
    }

    private Image<Rgba32> ExtractPart(Image<Rgba32> skin, int x, int y, int w, int h, int hatX, int hatY)
    {
        var part = skin.Clone(ctx => ctx.Crop(new Rectangle(x, y, w, h)));

        // Only try to extract hat/layer if it fits in the image
        if (hatX + w <= skin.Width && hatY + h <= skin.Height)
        {
            var layer = skin.Clone(ctx => ctx.Crop(new Rectangle(hatX, hatY, w, h)));
            part.Mutate(ctx => ctx.DrawImage(layer, new Point(0, 0), 1f));
        }

        return part;
    }

    private Image<Rgba32> ExtractFace(Image<Rgba32> skin, int x, int y, int hatX, int hatY)
    {
        return ExtractPart(skin, x, y, 8, 8, hatX, hatY);
    }
}
