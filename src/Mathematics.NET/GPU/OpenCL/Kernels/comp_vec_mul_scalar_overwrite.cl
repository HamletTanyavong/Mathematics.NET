#include "GPU/OpenCL/Kernels/complex.h"

__kernel void comp_vec_mul_scalar_overwrite(__global complex* vector, const complex scalar) {
    int i = get_global_id(0);
    vector[i] = comp_mul(vector[i], scalar);
}
