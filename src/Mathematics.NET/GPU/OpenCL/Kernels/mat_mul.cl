__kernel void mat_mul(__global const double* matA,
                      __global const double* matB,
                      const int k,
                      const int width,
                      __global double* result)
{
    int row = get_global_id(0);
    int col = get_global_id(1);

    int aIndex = row * k;
    int bIndex = col;
    int cIndex = row * width + col;

    double sum = 0;
    for (int i = 0; i < k; i++)
    {
        sum += matA[aIndex] * matB[bIndex];
        aIndex++;
        bIndex += width;
    }

    result[cIndex] = sum;
}
