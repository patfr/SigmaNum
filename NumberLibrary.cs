using System.Numerics;

namespace SigmaNum
{
    public class SN : SigmaNum
    {

    }

    public class Sigma : SigmaNum
    {
        
    }

    public class SigmaNum
    {
        public int Sign => S;
        public double Mantissa => M;
        public BigInteger Layer => L;
        public double Exponent => E;

        private int S => Math.Sign(M);
        private double M { get; set; }
        private BigInteger L { get; set; }
        private double E { get; set; }

        public SigmaNum() { M = 0; L = 0; E = 0; }
        public SigmaNum(SigmaNum n) { M = n.M; L = n.L; E = n.E; }
        public SigmaNum(sbyte n) => FromDouble(n);
        public SigmaNum(byte n) => FromDouble(n);
        public SigmaNum(short n) => FromDouble(n);
        public SigmaNum(ushort n) => FromDouble(n);
        public SigmaNum(int n) => FromDouble(n);
        public SigmaNum(uint n) => FromDouble(n);
        public SigmaNum(long n) => FromDouble(n);
        public SigmaNum(ulong n) => FromDouble(n);
        public SigmaNum(float n) => FromDouble(n);
        public SigmaNum(double n) => FromDouble(n);
        public SigmaNum(decimal n) => FromDouble((double)n);
        private void FromDouble(double n) { E = Math.Floor(Math.Log10(n)); M = (Math.Log10(n) - E) * 10; L = 0; }

        public static SigmaNum Abs(SigmaNum n) { SigmaNum s = new(n); s.M = Math.Abs(s.M); return s; }
        public static SigmaNum Absolute(SigmaNum n) => Abs(n);
        public static SigmaNum AbsoluteValue(SigmaNum n) => Abs(n);
        public static SigmaNum Neg(SigmaNum n) => -n;
        public static SigmaNum Negate(SigmaNum n) => Neg(n);

        public static SigmaNum operator -(SigmaNum n) { SigmaNum s = new(n); s.M = -s.M; return s; }
    }
}
