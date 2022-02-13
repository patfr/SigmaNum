using System.Numerics;

namespace Sigma
{
    public class SN : SigmaNum
    {

    }

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public class SigmaNum
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        /* Static readonly properties */
        private static readonly BigInteger MaxLayer = new(1e100);
        private static readonly double MaxSafeInteger = 9007199254740991;
        private static readonly double LayerDown = Math.Log10(9007199254740991);

        /* Public properties */
        public int Sign => S;
        public double Mantissa => M;
        public BigInteger Layer => L;
        public double Exponent =>
            Sign == 0 ? 0 :
            L == 0 ? E :
            L == 1 ? Math.Floor(Math.Pow(10, E)) :
            double.PositiveInfinity;

        /* Private properties */
        public int S => Math.Sign(M);
        public double M { get; set; }
        public BigInteger L { get; set; }
        public double E { get; set; }

        /* Constructors */
        public SigmaNum() { M = 0; L = 0; }
        public SigmaNum(SigmaNum n) { M = n.M; L = n.L; }
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
        private void FromDouble(double n) { M = n; L = 0; Normalize(); }

        /* Instance methods */
        public void Normalize()
        {
            if (Sign == 0) { M = 0; L = 0; E = 0; return; }
            double R = 0;
            if (M < 1 || M >= 10)
                R = Math.Floor(Math.Log10(Math.Abs(M)));
            M /= Math.Pow(10, R);
            if (L < 0) L = 0;
            if (L == 0) E += R;
            else if (L == 1 && R != 0) E += Math.Log10(Math.Abs(R));
            else if (L == 2 && R != 0) E += Math.Log10(Math.Log10(Math.Abs(R)));
            if (Math.Abs(E) > MaxSafeInteger) { L++; E = Math.Sign(E) * Math.Log10(Math.Abs(E)); }
            double AE = Math.Abs(E);
            double SE = Math.Sign(E);
            while (AE < LayerDown && L > 0) { L--; AE = Math.Pow(10, AE); }
            E = SE * AE;
            if (L > MaxLayer) L = MaxLayer;
        }

        /* Static methods */
        public static SigmaNum Abs(SigmaNum n) { SigmaNum s = new(n); s.M = Math.Abs(s.M); return s; }
        public static SigmaNum Absolute(SigmaNum n) => Abs(n);
        public static SigmaNum AbsoluteValue(SigmaNum n) => Abs(n);
        public static SigmaNum Max(SigmaNum n1, SigmaNum n2) => Cmp(n1, n2) >= 0 ? new(n1) : new(n2);
        public static SigmaNum Min(SigmaNum n1, SigmaNum n2) => Cmp(n1, n2) <= 0 ? new(n1) : new(n2);
        public static SigmaNum Neg(SigmaNum n) => -n;
        public static SigmaNum Negate(SigmaNum n) => Neg(n);

        /* Comparison functions */
        public static bool Equal(SigmaNum n1, SigmaNum n2) => Cmp(n1, n2) == 0;
        public static bool Eq(SigmaNum n1, SigmaNum n2) => Equal(n1, n2);
        public static bool GreaterThan(SigmaNum n1, SigmaNum n2) => Cmp(n1, n2) == 1;
        public static bool GT(SigmaNum n1, SigmaNum n2) => GreaterThan(n1, n2);
        public static bool GreaterThanOrEqualTo(SigmaNum n1, SigmaNum n2) => Cmp(n1, n2) >= 0;
        public static bool GTE(SigmaNum n1, SigmaNum n2) => GreaterThanOrEqualTo(n1, n2);
        public static bool LessThan(SigmaNum n1, SigmaNum n2) => Cmp(n1, n2) == -1;
        public static bool LT(SigmaNum n1, SigmaNum n2) => LessThan(n1, n2);
        public static bool LessThanOrEqualTo(SigmaNum n1, SigmaNum n2) => Cmp(n1, n2) <= 0;
        public static bool LTE(SigmaNum n1, SigmaNum n2) => LessThanOrEqualTo(n1, n2);
        private static int Cmp(SigmaNum n1, SigmaNum n2) => // returns 1 if n1 is bigger, returns -1 if n2 is bigger, return 0 if they are equal
            n1.L > n2.L ? 1 : n1.L < n2.L ? -1 : // Layer comparison
            n1.E > n2.E ? 1 : n1.E < n2.E ? -1 : // Exponent comparison
            n1.M > n2.M ? 1 : n1.M < n2.M ? -1 : // Mantissa comparison
            0;

        public static SigmaNum operator -(SigmaNum n) { SigmaNum s = new(n); s.M = -s.M; return s; }

        /* Comparison operators */
        public static bool operator ==(SigmaNum n1, SigmaNum n2) => Equal(n1, n2);
        public static bool operator !=(SigmaNum n1, SigmaNum n2) => !Equal(n1, n2);
        public static bool operator >(SigmaNum n1, SigmaNum n2) => GreaterThan(n1, n2);
        public static bool operator <(SigmaNum n1, SigmaNum n2) => LessThan(n1, n2);
        public static bool operator >=(SigmaNum n1, SigmaNum n2) => GreaterThanOrEqualTo(n1, n2);
        public static bool operator <=(SigmaNum n1, SigmaNum n2) => LessThanOrEqualTo(n1, n2);

        public override string ToString() // Temporary override for testing
        {
            return $"Sign: {S}; Mantissa: {M}; Exponent: {E}; Layer: {L};";
        }
    }
}
