#include "GPU\OpenCL\Kernels\complex.h"

// Basic operations

complex comp_add(complex z, complex w) {
    complex result = {
        .re = z.re + w.re,
        .im = z.im + w.im
    };
    return result;
}

complex comp_conjugate(complex z) {
    complex result = {
        .re = z.re,
        .im = -z.im
    };
    return result;
}

complex comp_div(complex z, complex w) {
    double a = z.re;
    double b = z.im;
    double c = w.re;
    double d = w.im;

    complex result;
    if (fabs(d) < fabs(c)) {
        double u = d / c;
        result.re = (a + b * u) / (c + d * u);
        result.im = (b - a * u) / (c + d * u);
    } else {
        double u = c / d;
        result.re = (b + a * u) / (d + c * u);
        result.im = (b * u - a) / (d + c * u);
    }
    return result;
}

complex comp_from_polar(double magnitude, double phase) {
    complex result = {
        .re = magnitude * cos(phase),
        .im = magnitude * sin(phase)
    };
    return result;
}

double comp_magnitude(complex z) {
    return hypot(z.re, z.im);
}

complex comp_mul(complex z, complex w) {
    complex result = {
        .re = z.re * w.re - z.im * w.im,
        .im = z.re * w.im + w.re * z.im
    };
    return result;
}

double comp_phase(complex z) {
    return atan2(z.im, z.re);
}

complex comp_reciprocate(complex z) {
    if (z.re == 0 && z.im == 0) {
        COMP_INFINITY;
    }

    double u = z.re * z.re + z.im * z.im;
    complex result = {
        .re = z.re / u,
        .im = -z.im / u
    };
    return result;
}

complex comp_sub(complex z, complex w) {
    complex result = {
        .re = z.re - w.re,
        .im = z.im - w.im
    };
    return result;
}
