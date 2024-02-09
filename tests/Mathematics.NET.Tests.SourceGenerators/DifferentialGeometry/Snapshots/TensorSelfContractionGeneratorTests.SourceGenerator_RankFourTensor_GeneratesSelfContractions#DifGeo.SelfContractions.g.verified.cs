// Auto-generated code
using Mathematics.NET.DifferentialGeometry.Abstractions;
using Mathematics.NET.LinearAlgebra;
using Mathematics.NET.LinearAlgebra.Abstractions;
using Mathematics.NET.Symbols;

namespace Mathematics.NET.DifferentialGeometry;
public static partial class DifGeo
{
    public static Tensor<Matrix4x4<U>, U, I1, I2> Contract<T, U, IC, I1, I2>(in IRankFourTensor<T, Array4x4x4x4<U>, U, Index<Upper, IC>, Index<Lower, IC>, I1, I2> a)
        where T : IRankFourTensor<T, Array4x4x4x4<U>, U, Index<Upper, IC>, Index<Lower, IC>, I1, I2> where U : IComplex<U> where IC : ISymbol where I1 : IIndex where I2 : IIndex
    {
        Matrix4x4<U> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k, k, i, j];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<U>, U, I1, I2> Contract<T, U, IC, I1, I2>(in IRankFourTensor<T, Array4x4x4x4<U>, U, Index<Lower, IC>, I1, Index<Upper, IC>, I2> a)
        where T : IRankFourTensor<T, Array4x4x4x4<U>, U, Index<Lower, IC>, I1, Index<Upper, IC>, I2> where U : IComplex<U> where IC : ISymbol where I1 : IIndex where I2 : IIndex
    {
        Matrix4x4<U> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k, i, k, j];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<U>, U, I1, I2> Contract<T, U, IC, I1, I2>(in IRankFourTensor<T, Array4x4x4x4<U>, U, Index<Upper, IC>, I1, Index<Lower, IC>, I2> a)
        where T : IRankFourTensor<T, Array4x4x4x4<U>, U, Index<Upper, IC>, I1, Index<Lower, IC>, I2> where U : IComplex<U> where IC : ISymbol where I1 : IIndex where I2 : IIndex
    {
        Matrix4x4<U> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k, i, k, j];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<U>, U, I1, I2> Contract<T, U, IC, I1, I2>(in IRankFourTensor<T, Array4x4x4x4<U>, U, Index<Lower, IC>, I1, I2, Index<Upper, IC>> a)
        where T : IRankFourTensor<T, Array4x4x4x4<U>, U, Index<Lower, IC>, I1, I2, Index<Upper, IC>> where U : IComplex<U> where IC : ISymbol where I1 : IIndex where I2 : IIndex
    {
        Matrix4x4<U> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k, i, j, k];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<U>, U, I1, I2> Contract<T, U, IC, I1, I2>(in IRankFourTensor<T, Array4x4x4x4<U>, U, Index<Upper, IC>, I1, I2, Index<Lower, IC>> a)
        where T : IRankFourTensor<T, Array4x4x4x4<U>, U, Index<Upper, IC>, I1, I2, Index<Lower, IC>> where U : IComplex<U> where IC : ISymbol where I1 : IIndex where I2 : IIndex
    {
        Matrix4x4<U> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[k, i, j, k];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<U>, U, I1, I2> Contract<T, U, IC, I1, I2>(in IRankFourTensor<T, Array4x4x4x4<U>, U, I1, Index<Lower, IC>, Index<Upper, IC>, I2> a)
        where T : IRankFourTensor<T, Array4x4x4x4<U>, U, I1, Index<Lower, IC>, Index<Upper, IC>, I2> where U : IComplex<U> where IC : ISymbol where I1 : IIndex where I2 : IIndex
    {
        Matrix4x4<U> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, k, k, j];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<U>, U, I1, I2> Contract<T, U, IC, I1, I2>(in IRankFourTensor<T, Array4x4x4x4<U>, U, I1, Index<Upper, IC>, Index<Lower, IC>, I2> a)
        where T : IRankFourTensor<T, Array4x4x4x4<U>, U, I1, Index<Upper, IC>, Index<Lower, IC>, I2> where U : IComplex<U> where IC : ISymbol where I1 : IIndex where I2 : IIndex
    {
        Matrix4x4<U> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, k, k, j];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<U>, U, I1, I2> Contract<T, U, IC, I1, I2>(in IRankFourTensor<T, Array4x4x4x4<U>, U, I1, Index<Lower, IC>, I2, Index<Upper, IC>> a)
        where T : IRankFourTensor<T, Array4x4x4x4<U>, U, I1, Index<Lower, IC>, I2, Index<Upper, IC>> where U : IComplex<U> where IC : ISymbol where I1 : IIndex where I2 : IIndex
    {
        Matrix4x4<U> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, k, j, k];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<U>, U, I1, I2> Contract<T, U, IC, I1, I2>(in IRankFourTensor<T, Array4x4x4x4<U>, U, I1, Index<Upper, IC>, I2, Index<Lower, IC>> a)
        where T : IRankFourTensor<T, Array4x4x4x4<U>, U, I1, Index<Upper, IC>, I2, Index<Lower, IC>> where U : IComplex<U> where IC : ISymbol where I1 : IIndex where I2 : IIndex
    {
        Matrix4x4<U> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, k, j, k];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<U>, U, I1, I2> Contract<T, U, IC, I1, I2>(in IRankFourTensor<T, Array4x4x4x4<U>, U, I1, I2, Index<Lower, IC>, Index<Upper, IC>> a)
        where T : IRankFourTensor<T, Array4x4x4x4<U>, U, I1, I2, Index<Lower, IC>, Index<Upper, IC>> where U : IComplex<U> where IC : ISymbol where I1 : IIndex where I2 : IIndex
    {
        Matrix4x4<U> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, j, k, k];
                }
            }
        }

        return new(matrix);
    }

    public static Tensor<Matrix4x4<U>, U, I1, I2> Contract<T, U, IC, I1, I2>(in IRankFourTensor<T, Array4x4x4x4<U>, U, I1, I2, Index<Upper, IC>, Index<Lower, IC>> a)
        where T : IRankFourTensor<T, Array4x4x4x4<U>, U, I1, I2, Index<Upper, IC>, Index<Lower, IC>> where U : IComplex<U> where IC : ISymbol where I1 : IIndex where I2 : IIndex
    {
        Matrix4x4<U> matrix = new();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    matrix[i, j] += a[i, j, k, k];
                }
            }
        }

        return new(matrix);
    }
}
