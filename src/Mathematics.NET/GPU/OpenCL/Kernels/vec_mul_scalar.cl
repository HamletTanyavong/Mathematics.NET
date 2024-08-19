__kernel void vec_mul_scalar(__global const double* vector,
                             const double scalar,
                             const int length,
                             __global double* result)
{
    int i = get_global_id(0);
    result[i] = vector[i] * scalar;
}
