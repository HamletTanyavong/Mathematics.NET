#include "GPU/OpenCL/Kernels/complex.h"

__kernel void comp_mat_mul(__global const complex* matA,
                           __global const complex* matB,
                           const int k,
                           const int width,
                           __global complex* result)
{
    int row = get_global_id(0);
    int col = get_global_id(1);

    int aIndex = row * k;
    int bIndex = col;
    int cIndex = row * width + col;

    complex sum = { .re = 0, .im = 0 };

    for (int i = 0; i < k; i++)
    {
        sum = comp_add(sum, comp_mul(matA[aIndex], matB[bIndex]));
        aIndex++;
        bIndex += width;
    }

    result[cIndex] = sum;
}
