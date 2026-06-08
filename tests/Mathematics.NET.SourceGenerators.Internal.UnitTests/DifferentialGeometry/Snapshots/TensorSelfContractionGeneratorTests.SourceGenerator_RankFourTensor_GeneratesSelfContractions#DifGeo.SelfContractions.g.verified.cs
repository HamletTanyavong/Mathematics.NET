//HintName: DifGeo.SelfContractions.g.cs
// Auto-generated code
using System.Numerics;
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.LinearAlgebra.Abstractions;

namespace Mathematics.NET.DifferentialGeometry;
public static partial class DifGeo
{
    public static Tensor<Matrix4x4<TN, TB>, TN, TB, TI1, TI2> Contract<TR4T, TN, TB, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, Index<Lower, TCI>, TI1, TI2> tensor)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, Index<Lower, TCI>, TI1, TI2> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex
    {
        Matrix4x4<TN, TB> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += tensor[k, k, i, j];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<TN, TB>, TN, TB, TI1, TI2> Contract<TR4T, TN, TB, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI1, Index<Upper, TCI>, TI2> tensor)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI1, Index<Upper, TCI>, TI2> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex
    {
        Matrix4x4<TN, TB> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += tensor[k, i, k, j];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<TN, TB>, TN, TB, TI1, TI2> Contract<TR4T, TN, TB, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI1, Index<Lower, TCI>, TI2> tensor)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI1, Index<Lower, TCI>, TI2> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex
    {
        Matrix4x4<TN, TB> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += tensor[k, i, k, j];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<TN, TB>, TN, TB, TI1, TI2> Contract<TR4T, TN, TB, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI1, TI2, Index<Upper, TCI>> tensor)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, Index<Lower, TCI>, TI1, TI2, Index<Upper, TCI>> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex
    {
        Matrix4x4<TN, TB> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += tensor[k, i, j, k];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<TN, TB>, TN, TB, TI1, TI2> Contract<TR4T, TN, TB, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI1, TI2, Index<Lower, TCI>> tensor)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, Index<Upper, TCI>, TI1, TI2, Index<Lower, TCI>> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex
    {
        Matrix4x4<TN, TB> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += tensor[k, i, j, k];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<TN, TB>, TN, TB, TI1, TI2> Contract<TR4T, TN, TB, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Lower, TCI>, Index<Upper, TCI>, TI2> tensor)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Lower, TCI>, Index<Upper, TCI>, TI2> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex
    {
        Matrix4x4<TN, TB> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += tensor[i, k, k, j];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<TN, TB>, TN, TB, TI1, TI2> Contract<TR4T, TN, TB, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Upper, TCI>, Index<Lower, TCI>, TI2> tensor)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Upper, TCI>, Index<Lower, TCI>, TI2> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex
    {
        Matrix4x4<TN, TB> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += tensor[i, k, k, j];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<TN, TB>, TN, TB, TI1, TI2> Contract<TR4T, TN, TB, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Lower, TCI>, TI2, Index<Upper, TCI>> tensor)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Lower, TCI>, TI2, Index<Upper, TCI>> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex
    {
        Matrix4x4<TN, TB> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += tensor[i, k, j, k];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<TN, TB>, TN, TB, TI1, TI2> Contract<TR4T, TN, TB, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Upper, TCI>, TI2, Index<Lower, TCI>> tensor)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, TI1, Index<Upper, TCI>, TI2, Index<Lower, TCI>> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex
    {
        Matrix4x4<TN, TB> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += tensor[i, k, j, k];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<TN, TB>, TN, TB, TI1, TI2> Contract<TR4T, TN, TB, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Lower, TCI>, Index<Upper, TCI>> tensor)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Lower, TCI>, Index<Upper, TCI>> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex
    {
        Matrix4x4<TN, TB> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += tensor[i, j, k, k];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<TN, TB>, TN, TB, TI1, TI2> Contract<TR4T, TN, TB, TCI, TI1, TI2>(in IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Upper, TCI>, Index<Lower, TCI>> tensor)
        where TR4T : IRankFourTensor<TR4T, Array4x4x4x4<TN, TB>, TN, TB, TB, TI1, TI2, Index<Upper, TCI>, Index<Lower, TCI>> where TN : IComplex<TN, TB, TB>, IDifferentiableFunctions<TN> where TB : IBinaryFloatingPointIeee754<TB>, IMinMaxValue<TB> where TCI : IIndexName where TI1 : IIndex where TI2 : IIndex
    {
        Matrix4x4<TN, TB> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += tensor[i, j, k, k];
                }
            }
        }

        return new(matrix);
    }
}
