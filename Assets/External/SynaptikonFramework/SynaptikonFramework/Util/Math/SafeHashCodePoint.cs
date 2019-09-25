using System;
namespace SynaptikonFramework.Util.Math {
    public class SafeHashCodePoint {

        public SafeHashCodePoint(int x, int y) {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public override bool Equals(Object obj) {
            if (!(obj is SafeHashCodePoint)) return false;

            SafeHashCodePoint p = (SafeHashCodePoint)obj;
            return X == p.X & Y == p.Y;
        }

        // Creates different hashcodes for same pairs of values (eg (x:1, y:2) and (x:2, y:1))
        // to be safe to use in a HashTable. There were problems in FlashGlance on some devices where elements could not be removed because of
        // same hash codes
        public override int GetHashCode() {
            return ShiftAndWrap(X.GetHashCode(), 2) ^ Y.GetHashCode();
        }

        private int ShiftAndWrap(int value, int positions) {
            positions = positions & 0x1F;

            // Save the existing bit pattern, but interpret it as an unsigned integer.
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            // Preserve the bits to be discarded.
            uint wrapped = number >> (32 - positions);
            // Shift and wrap the discarded bits.
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }
    }
}
