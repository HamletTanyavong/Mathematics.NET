#ifndef COMPLEX_H
#define COMPLEX_H

#define COMP_IM = ((complex){ .re = 0, .im = 1 });
#define COMP_INFINITY ((complex){ .re = INFINITY, .im = INFINITY });
#define COMP_NAN ((complex){ .re = NAN, .im = NAN });

#define Re(z) z.re
#define Im(z) z.im

typedef struct __attribute__((packed)) {
    double re;
    double im;
} complex;

complex comp_add(complex z, complex w);
complex comp_conjugate(complex z);
complex comp_div(complex z, complex w);
complex comp_from_polar(double magnitude, double phase);
double comp_magnitude(complex z);
complex comp_mul(complex z, complex w);
double comp_phase(complex z);
complex comp_reciprocate(complex z);
complex comp_sub(complex z, complex w);

#endif
