using System;

namespace JPEG;

public static class DCT
{
    private const int N = 8;

    private static readonly double[,] CosTable = BuildCosTable();

    private static readonly double[] Alpha =
    [
        1.0 / Math.Sqrt(2.0), 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0
    ];

    public static double[,] DCT2D(double[,] input)
    {
        var output = new double[N, N];
        DCT2D(input, output);
        return output;
    }

    private static void DCT2D(double[,] input, double[,] output)
    {
        for (var u = 0; u < N; u++)
        for (var v = 0; v < N; v++)
        {
            var sum = 0.0;
            for (var x = 0; x < N; x++)
            for (var y = 0; y < N; y++)
                sum += input[x, y] * CosTable[u, x] * CosTable[v, y];

            output[u, v] = 0.25 * Alpha[u] * Alpha[v] * sum;
        }
    }

    public static void IDCT2D(double[,] input, double[,] output)
    {
        for (var x = 0; x < N; x++)
        for (var y = 0; y < N; y++)
        {
            var sum = 0.0;

            for (var u = 0; u < N; u++)
            for (var v = 0; v < N; v++)
                sum += Alpha[u] * Alpha[v] * input[u, v] * CosTable[u, x] * CosTable[v, y];

            output[x, y] = 0.25 * sum;
        }
    }

    private static double[,] BuildCosTable()
    {
        var table = new double[N, N];
        for (var u = 0; u < N; u++)
        for (var x = 0; x < N; x++)
            table[u, x] = Math.Cos((2.0 * x + 1.0) * u * Math.PI / (2.0 * N));

        return table;
    }
}