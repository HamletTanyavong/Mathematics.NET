__kernel void vec_mul_scalar_overwrite(__global double* vector, const double scalar) {
    int i = get_global_id(0);
    vector[i] *= scalar;
}
