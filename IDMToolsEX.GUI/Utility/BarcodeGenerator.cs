using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ZXing;
using ZXing.Common;

namespace IDMToolsEX.Utility;

public static class BarcodeGenerator
{
    public static async Task<Bitmap?> GenerateBarcodeImageAsync(string text, CancellationToken token)
    {
        return await Task.Run(() =>
        {
            token.ThrowIfCancellationRequested();

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

            if (string.IsNullOrEmpty(text))
                return null;

            var pixelData = writer.Write(text);

            using var stream = new MemoryStream();
            var bitmap = new WriteableBitmap(
                new PixelSize(pixelData.Width, pixelData.Height),
                new Vector(96, 96),
                PixelFormat.Bgra8888,
                AlphaFormat.Premul);

            using var lockedBuffer = bitmap.Lock();
            unsafe
            {
                var buffer = (uint*)lockedBuffer.Address;
                for (var y = 0; y < pixelData.Height; y++)
                {
                    token.ThrowIfCancellationRequested();
                    for (var x = 0; x < pixelData.Width; x++)
                    {
                        var offset = (y * pixelData.Width + x) * 4;
                        var color = Color.FromArgb(
                            pixelData.Pixels[offset + 3],
                            pixelData.Pixels[offset + 2],
                            pixelData.Pixels[offset + 1],
                            pixelData.Pixels[offset]);

                        buffer[y * lockedBuffer.RowBytes / 4 + x] = color.ToUInt32();
                    }
                }
            }

            return bitmap;
        }, token);
    }
}
