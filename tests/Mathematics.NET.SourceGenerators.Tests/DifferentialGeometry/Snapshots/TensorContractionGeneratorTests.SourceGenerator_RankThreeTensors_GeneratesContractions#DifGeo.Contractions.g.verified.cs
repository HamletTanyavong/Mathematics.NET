//HintName: DifGeo.Contractions.g.cs
// Auto-generated code
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.DifferentialGeometry;
public static partial class DifGeo
{
    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI3, TI4> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI3, TI4> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, Index<Upper, TCI>, TI4> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, Index<Upper, TCI>, TI4> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, Index<Lower, TCI>, TI4> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, Index<Lower, TCI>, TI4> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, TI4, Index<Upper, TCI>> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI1, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, TI4, Index<Upper, TCI>> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, TI4, Index<Lower, TCI>> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI1, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, TI4, Index<Lower, TCI>> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, Index<Lower, TCI>, TI2> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI3, TI4> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, Index<Lower, TCI>, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI3, TI4> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, Index<Upper, TCI>, TI2> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI3, TI4> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, Index<Upper, TCI>, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI3, TI4> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, Index<Lower, TCI>, TI2> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, Index<Upper, TCI>, TI4> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, Index<Lower, TCI>, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, Index<Upper, TCI>, TI4> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, Index<Upper, TCI>, TI2> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, Index<Lower, TCI>, TI4> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, Index<Upper, TCI>, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, Index<Lower, TCI>, TI4> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, Index<Lower, TCI>, TI2> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, TI4, Index<Upper, TCI>> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, Index<Lower, TCI>, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, TI4, Index<Upper, TCI>> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, Index<Upper, TCI>, TI2> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, TI4, Index<Lower, TCI>> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, Index<Upper, TCI>, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, TI4, Index<Lower, TCI>> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, TI2, Index<Lower, TCI>> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI3, TI4> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, TI2, Index<Lower, TCI>> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Upper, TCI>, TI3, TI4> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, TI2, Index<Upper, TCI>> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI3, TI4> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, TI2, Index<Upper, TCI>> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, Index<Lower, TCI>, TI3, TI4> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, TI2, Index<Lower, TCI>> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, Index<Upper, TCI>, TI4> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, TI2, Index<Lower, TCI>> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, Index<Upper, TCI>, TI4> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, TI2, Index<Upper, TCI>> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, Index<Lower, TCI>, TI4> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, TI2, Index<Upper, TCI>> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, Index<Lower, TCI>, TI4> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, TI2, Index<Lower, TCI>> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, TI4, Index<Upper, TCI>> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, TI2, Index<Lower, TCI>> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, TI4, Index<Upper, TCI>> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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

    public static Tensor<Array4x4x4x4<TN>, TN, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, TI2, Index<Upper, TCI>> a, in IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, TI4, Index<Lower, TCI>> b)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN>, TN, TI1, TI2, Index<Upper, TCI>> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN>, TN, TI3, TI4, Index<Lower, TCI>> where TN : IComplex<TN>, IDifferentiableFunctions<TN> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN> array = new();
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
