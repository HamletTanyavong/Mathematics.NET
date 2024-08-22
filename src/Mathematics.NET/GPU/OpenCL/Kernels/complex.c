typedef struct __attribute__((packed)) {
    double re;
    double im;
} complex;

inline complex comp_add(const complex z, const complex w) {
    complex result;
    result.re = z.re + w.re;
    result.im = z.im + w.im;
    return result;
}

inline complex comp_div(const complex z, const complex w) {
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

inline complex comp_mul(const complex z, const complex w) {
    complex result;
    result.re = z.re * w.re - z.im * w.im;
    result.im = z.re * w.im + w.re * z.im;
    return result;
}

inline complex comp_sub(const complex z, const complex w) {
    complex result;
    result.re = z.re - w.re;
    result.im = z.im - w.im;
    return result;
}
