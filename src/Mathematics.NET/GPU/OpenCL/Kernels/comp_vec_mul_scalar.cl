__kernel void comp_vec_mul_scalar(__global const complex* vector,
                                  const complex scalar,
                                  __global complex* result)
{
    int i = get_global_id(0);
    result[i] = comp_mul(vector[i], scalar);
}
