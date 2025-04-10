using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ZXing;
using ZXing.Common;

namespace IDMToolsEX.Utility;

public static class BarcodeGenerator
{
    public static async Task<Bitmap?> GenerateBarcodeImageAsync(string text)
    {
        return await Task.Run(() =>
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 500,
                    Width = 1500,
                    Margin = 5
                }
            };

            var pixelData = writer.Write(text);

            using var stream = new MemoryStream();
            var bitmap = new Avalonia.Media.Imaging.WriteableBitmap(
                new Avalonia.PixelSize(pixelData.Width, pixelData.Height),
                new Avalonia.Vector(96, 96),
                Avalonia.Platform.PixelFormat.Bgra8888,
                Avalonia.Platform.AlphaFormat.Premul);

            using var lockedBuffer = bitmap.Lock();
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

                        buffer[(y * lockedBuffer.RowBytes / 4) + x] = color.ToUInt32();
                    }
                }
            }

            return bitmap;
        });
    }
}
