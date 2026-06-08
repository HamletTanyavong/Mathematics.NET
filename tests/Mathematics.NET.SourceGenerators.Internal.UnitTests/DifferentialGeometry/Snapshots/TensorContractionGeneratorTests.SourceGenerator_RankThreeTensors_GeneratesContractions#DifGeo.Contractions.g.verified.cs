//HintName: DifGeo.Contractions.g.cs
// Auto-generated code
using System.Numerics;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.DifferentialGeometry;
public static partial class DifGeo
{
    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI1, TI2> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI3, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI1, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI3, TI4> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[m, i, j] * right[m, k, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI1, TI2> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, Index<Upper, TCI>, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI1, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, Index<Upper, TCI>, TI4> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[m, i, j] * right[k, m, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI1, TI2> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, Index<Lower, TCI>, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI1, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, Index<Lower, TCI>, TI4> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[m, i, j] * right[k, m, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI1, TI2> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, TI4, Index<Upper, TCI>> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI1, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, TI4, Index<Upper, TCI>> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[m, i, j] * right[k, l, m];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI1, TI2> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, TI4, Index<Lower, TCI>> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI1, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, TI4, Index<Lower, TCI>> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[m, i, j] * right[k, l, m];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Lower, TCI>, TI2> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI3, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Lower, TCI>, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI3, TI4> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[i, m, j] * right[m, k, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Upper, TCI>, TI2> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI3, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Upper, TCI>, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI3, TI4> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[i, m, j] * right[m, k, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Lower, TCI>, TI2> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, Index<Upper, TCI>, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Lower, TCI>, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, Index<Upper, TCI>, TI4> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[i, m, j] * right[k, m, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Upper, TCI>, TI2> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, Index<Lower, TCI>, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Upper, TCI>, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, Index<Lower, TCI>, TI4> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[i, m, j] * right[k, m, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Lower, TCI>, TI2> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, TI4, Index<Upper, TCI>> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Lower, TCI>, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, TI4, Index<Upper, TCI>> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[i, m, j] * right[k, l, m];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Upper, TCI>, TI2> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, TI4, Index<Lower, TCI>> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Upper, TCI>, TI2> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, TI4, Index<Lower, TCI>> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[i, m, j] * right[k, l, m];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Lower, TCI>> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI3, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Lower, TCI>> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI3, TI4> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[i, j, m] * right[m, k, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Upper, TCI>> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI3, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Upper, TCI>> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI3, TI4> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[i, j, m] * right[m, k, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Lower, TCI>> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, Index<Upper, TCI>, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Lower, TCI>> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, Index<Upper, TCI>, TI4> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[i, j, m] * right[k, m, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Upper, TCI>> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, Index<Lower, TCI>, TI4> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Upper, TCI>> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, Index<Lower, TCI>, TI4> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[i, j, m] * right[k, m, l];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Lower, TCI>> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, TI4, Index<Upper, TCI>> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Lower, TCI>> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, TI4, Index<Upper, TCI>> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[i, j, m] * right[k, l, m];
                        }
                    }
                }
            }
        }

        return new(array);
    }

    public static Tensor<Array4x4x4x4<TN, TB>, TN, TB, TI1, TI2, TI3, TI4> Contract<TLR3T, TRR3T, TN, TB, TCI, TI1, TI2, TI3, TI4>(in IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Upper, TCI>> left, in IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, TI4, Index<Lower, TCI>> right)
        where TLR3T : IRankThreeTensor<TLR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Upper, TCI>> where TRR3T : IRankThreeTensor<TRR3T, Array4x4x4<TN, TB>, TN, TB, TB, TI3, TI4, Index<Lower, TCI>> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex where TI3 : IIndex where TI4 : IIndex
    {
        Array4x4x4x4<TN, TB> array = new();
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
                            array[i, j, k, l] += left[i, j, m] * right[k, l, m];
                        }
                    }
                }
            }
        }

        return new(array);
    }
}
