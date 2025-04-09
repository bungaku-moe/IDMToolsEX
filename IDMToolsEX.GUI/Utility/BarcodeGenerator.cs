using System.IO;
using Avalonia.Media.Imaging;
using ZXing;
using ZXing.Common;

namespace IDMToolsEX.Utility;

public static class BarcodeGenerator
{
    public static Bitmap GenerateBarcodeImage(string text)
    {
        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.CODE_128,
            Options = new EncodingOptions
            {
                Height = 100,
                Width = 300,
                Margin = 10
            }
        };

        var pixelData = writer.Write(text);

        using var stream = new MemoryStream();
        var bitmap = new Avalonia.Media.Imaging.WriteableBitmap(
            new Avalonia.PixelSize(pixelData.Width, pixelData.Height),
            new Avalonia.Vector(96, 96),
            Avalonia.Platform.PixelFormat.Bgra8888,
            Avalonia.Platform.AlphaFormat.Premul);

        using (var lockedBuffer = bitmap.Lock())
        {
            unsafe
            {
                var buffer = (uint*)lockedBuffer.Address;
                for (int y = 0; y < pixelData.Height; y++)
                {
                    for (int x = 0; x < pixelData.Width; x++)
                    {
                        var offset = (y * pixelData.Width + x) * 4;
                        var color = Avalonia.Media.Color.FromArgb(
                            pixelData.Pixels[offset + 3],
                            pixelData.Pixels[offset + 2],
                            pixelData.Pixels[offset + 1],
                            pixelData.Pixels[offset]);

                        buffer[(y * lockedBuffer.RowBytes / 4) + x] = color.ToUint32();
                    }
                }
            }
        }

        return bitmap;
    }
}
