namespace JPEG.Images;

public readonly struct Pixel
{
    private readonly double r;
    private readonly double g;
    private readonly double b;

    private readonly double y;
    private readonly double cb;
    private readonly double cr;

    public Pixel(double firstComponent, double secondComponent, double thirdComponent, PixelFormat pixelFormat)
    {
        if (pixelFormat == PixelFormat.Rgb)
        {
            r = firstComponent;
            g = secondComponent;
            b = thirdComponent;

            y = 16.0 + (65.738 * r + 129.057 * g + 24.064 * b) / 256.0;
            cb = 128.0 + (-37.945 * r - 74.494 * g + 112.439 * b) / 256.0;
            cr = 128.0 + (112.439 * r - 94.154 * g - 18.285 * b) / 256.0;
        }
        else
        {
            y = firstComponent;
            cb = secondComponent;
            cr = thirdComponent;

            r = (298.082 * y + 408.583 * cr) / 256.0 - 222.921;
            g = (298.082 * y - 100.291 * cb - 208.120 * cr) / 256.0 + 135.576;
            b = (298.082 * y + 516.412 * cb) / 256.0 - 276.836;
        }
    }

    public double R => r;
    public double G => g;
    public double B => b;

    public double Y => y;
    public double Cb => cb;
    public double Cr => cr;
}