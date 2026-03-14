using System.Drawing;
using System.Drawing.Imaging;

namespace JPEG.Images;

class Matrix(int height, int width)
{
    public readonly Pixel[,] Pixels = new Pixel[height, width];
    public readonly int Height = height;
    public readonly int Width = width;

    public static unsafe explicit operator Matrix(Bitmap bmp)
    {
        var height = bmp.Height - bmp.Height % 8;
        var width = bmp.Width - bmp.Width % 8;
        var matrix = new Matrix(height, width);

        var rect = new Rectangle(0, 0, width, height);
        var data = bmp.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        try
        {
            var basePtr = (byte*)data.Scan0;
            var stride = data.Stride;

            for (var y = 0; y < height; y++)
            {
                var row = basePtr + y * stride;
                for (var x = 0; x < width; x++)
                {
                    var b = row[x * 3 + 0];
                    var g = row[x * 3 + 1];
                    var r = row[x * 3 + 2];

                    matrix.Pixels[y, x] = new Pixel(r, g, b, PixelFormat.Rgb);
                }
            }
        }
        finally
        {
            bmp.UnlockBits(data);
        }

        return matrix;
    }

    public static unsafe explicit operator Bitmap(Matrix matrix)
    {
        var bmp = new Bitmap(matrix.Width, matrix.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        var rect = new Rectangle(0, 0, matrix.Width, matrix.Height);
        var data = bmp.LockBits(rect, ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        try
        {
            var basePtr = (byte*)data.Scan0;
            var stride = data.Stride;

            for (var y = 0; y < matrix.Height; y++)
            {
                var row = basePtr + y * stride;
                for (var x = 0; x < matrix.Width; x++)
                {
                    var pixel = matrix.Pixels[y, x];

                    row[x * 3 + 0] = (byte)ToByte(pixel.B);
                    row[x * 3 + 1] = (byte)ToByte(pixel.G);
                    row[x * 3 + 2] = (byte)ToByte(pixel.R);
                }
            }
        }
        finally
        {
            bmp.UnlockBits(data);
        }

        return bmp;
    }

    private static int ToByte(double d)
    {
        var val = (int)d;
        return val switch
        {
            > byte.MaxValue => byte.MaxValue,
            < byte.MinValue => byte.MinValue,
            _ => val
        };
    }
}