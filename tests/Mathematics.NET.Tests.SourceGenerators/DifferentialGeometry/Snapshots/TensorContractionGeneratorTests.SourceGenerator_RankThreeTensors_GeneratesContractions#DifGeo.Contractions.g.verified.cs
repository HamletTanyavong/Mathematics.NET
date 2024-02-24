// Auto-generated code
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;
public static partial class DifGeo
{
    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2> a, in IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, IC>, I3, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2> where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, IC>, I3, I4> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[m, i, j] * b[m, k, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2> a, in IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Upper, IC>, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2> where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Upper, IC>, I4> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[m, i, j] * b[k, m, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2> a, in IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Lower, IC>, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2> where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Lower, IC>, I4> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[m, i, j] * b[k, m, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2> a, in IRankThreeTensor<U, Array4x4x4<V>, V, I3, I4, Index<Upper, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Lower, IC>, I1, I2> where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, I4, Index<Upper, IC>> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[m, i, j] * b[k, l, m];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2> a, in IRankThreeTensor<U, Array4x4x4<V>, V, I3, I4, Index<Lower, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, Index<Upper, IC>, I1, I2> where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, I4, Index<Lower, IC>> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[m, i, j] * b[k, l, m];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> a, in IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I3, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I3, I4> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, m, j] * b[m, k, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2> a, in IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, IC>, I3, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2> where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, IC>, I3, I4> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, m, j] * b[m, k, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> a, in IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Upper, IC>, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Upper, IC>, I4> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, m, j] * b[k, m, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2> a, in IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Lower, IC>, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2> where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Lower, IC>, I4> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, m, j] * b[k, m, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> a, in IRankThreeTensor<U, Array4x4x4<V>, V, I3, I4, Index<Upper, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Lower, IC>, I2> where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, I4, Index<Upper, IC>> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, m, j] * b[k, l, m];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2> a, in IRankThreeTensor<U, Array4x4x4<V>, V, I3, I4, Index<Lower, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, Index<Upper, IC>, I2> where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, I4, Index<Lower, IC>> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, m, j] * b[k, l, m];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>> a, in IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I3, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>> where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Upper, IC>, I3, I4> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, j, m] * b[m, k, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>> a, in IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, IC>, I3, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>> where U : IRankThreeTensor<U, Array4x4x4<V>, V, Index<Lower, IC>, I3, I4> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, j, m] * b[m, k, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>> a, in IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Upper, IC>, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>> where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Upper, IC>, I4> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, j, m] * b[k, m, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>> a, in IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Lower, IC>, I4> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>> where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, Index<Lower, IC>, I4> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, j, m] * b[k, m, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>> a, in IRankThreeTensor<U, Array4x4x4<V>, V, I3, I4, Index<Upper, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Lower, IC>> where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, I4, Index<Upper, IC>> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, j, m] * b[k, l, m];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<V>, V, I1, I2, I3, I4> Contract<T, U, V, IC, I1, I2, I3, I4>(in IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>> a, in IRankThreeTensor<U, Array4x4x4<V>, V, I3, I4, Index<Lower, IC>> b)
        where T : IRankThreeTensor<T, Array4x4x4<V>, V, I1, I2, Index<Upper, IC>> where U : IRankThreeTensor<U, Array4x4x4<V>, V, I3, I4, Index<Lower, IC>> where V : IComplex<V>, IDifferentiableFunctions<V> where IC : ISymbol where I1 : IIndex where I2 : IIndex where I3 : IIndex where I4 : IIndex
    {
        Array4x4x4x4<V> array = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        for (int m = 0; m < 4; m++)
                        {
                            array[i, j, k, l] += a[i, j, m] * b[k, l, m];
                        }
                    }
                }
            }
        }

        return new(array);
    }
}
