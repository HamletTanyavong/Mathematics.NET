__kernel void mat_mul(__global const double* matA,
                      __global const double* matB,
                      const int k,
                      __global double* result)
{
    int row = get_global_id(0);
    int col = get_global_id(1);
    int width = get_local_size(1) * get_num_groups(1);

    int aIndex = row * k;
    int bIndex = col;

    double sum = 0;
    for (int i = 0; i < k; i++)
    {
        sum += matA[aIndex] * matB[bIndex];
        aIndex++;
        bIndex += width;
    }

    result[row * width + col] = sum;
}
