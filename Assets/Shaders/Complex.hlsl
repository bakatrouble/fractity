#define COMPLEX_TYPE double
#define COMPLEX_TYPE2 double2

static const COMPLEX_TYPE pi = 3.14159265;
static const COMPLEX_TYPE e = 2.71828183;

inline COMPLEX_TYPE2 c_add(COMPLEX_TYPE2 c1, COMPLEX_TYPE2 c2)
{
    COMPLEX_TYPE a = c1.x;
    COMPLEX_TYPE b = c1.y;
    COMPLEX_TYPE c = c2.x;
    COMPLEX_TYPE d = c2.y;
    return COMPLEX_TYPE2(a + c, b + d);
}
inline COMPLEX_TYPE2 c_sub(COMPLEX_TYPE2 c1, COMPLEX_TYPE2 c2)
{
    COMPLEX_TYPE a = c1.x;
    COMPLEX_TYPE b = c1.y;
    COMPLEX_TYPE c = c2.x;
    COMPLEX_TYPE d = c2.y;
    return COMPLEX_TYPE2(a - c, b - d);
}
inline COMPLEX_TYPE2 c_mul(COMPLEX_TYPE2 c1, COMPLEX_TYPE2 c2)
{
    COMPLEX_TYPE a = c1.x;
    COMPLEX_TYPE b = c1.y;
    COMPLEX_TYPE c = c2.x;
    COMPLEX_TYPE d = c2.y;
    return COMPLEX_TYPE2(a*c - b*d, b*c + a*d);
}
inline COMPLEX_TYPE2 c_div(COMPLEX_TYPE2 c1, COMPLEX_TYPE2 c2)
{
    COMPLEX_TYPE a = c1.x;
    COMPLEX_TYPE b = c1.y;
    COMPLEX_TYPE c = c2.x;
    COMPLEX_TYPE d = c2.y;
    COMPLEX_TYPE real = (a*c + b*d) / (c*c + d*d);
    COMPLEX_TYPE imag = (b*c - a*d) / (c*c + d*d);
    return COMPLEX_TYPE2(real, imag);
}
inline COMPLEX_TYPE c_abs(COMPLEX_TYPE2 c)
{
    return sqrt(c.x*c.x + c.y*c.y);
}
inline COMPLEX_TYPE2 c_pol(COMPLEX_TYPE2 c)
{
    COMPLEX_TYPE a = c.x;
    COMPLEX_TYPE b = c.y;
    COMPLEX_TYPE z = c_abs(c);
    COMPLEX_TYPE f = atan2(b, a);
    return COMPLEX_TYPE2(z, f);
}
inline COMPLEX_TYPE2 c_rec(COMPLEX_TYPE2 c)
{
    COMPLEX_TYPE z = abs(c.x);
    COMPLEX_TYPE f = c.y;
    COMPLEX_TYPE a = z * cos(f);
    COMPLEX_TYPE b = z * sin(f);
    return COMPLEX_TYPE2(a, b);
}
inline COMPLEX_TYPE2 c_pow(COMPLEX_TYPE2 base, COMPLEX_TYPE2 exp)
{
    COMPLEX_TYPE2 b = c_pol(base);
    COMPLEX_TYPE r = b.x;
    COMPLEX_TYPE f = b.y;
    COMPLEX_TYPE c = exp.x;
    COMPLEX_TYPE d = exp.y;
    COMPLEX_TYPE z = pow(r, c) * pow(e, -d * f);
    COMPLEX_TYPE fi = d * log(r) + c * f;
    COMPLEX_TYPE2 rpol = COMPLEX_TYPE2(z, fi);
    return c_rec(rpol);
}
inline COMPLEX_TYPE2 c_pow(COMPLEX_TYPE2 base, COMPLEX_TYPE exp)
{
    return c_pow(base, COMPLEX_TYPE2(exp, 0));
}
inline COMPLEX_TYPE2 c_con(COMPLEX_TYPE2 c)
{
    return COMPLEX_TYPE2(c.x, -c.y);
}
