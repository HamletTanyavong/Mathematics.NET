__kernel void comp_mat_mul(__global const complex* matA,
                           __global const complex* matB,
                           const int k,
                           __global complex* result)
{
    int row = get_global_id(0);
    int col = get_global_id(1);
    int width = get_local_size(1) * get_num_groups(1);

    int aIndex = row * k;
    int bIndex = col;

    complex sum = { .re = 0, .im = 0 };

    for (int i = 0; i < k; i++)
    {
        sum = comp_add(sum, comp_mul(matA[aIndex], matB[bIndex]));
        aIndex++;
        bIndex += width;
    }

    result[row * width + col] = sum;
}
