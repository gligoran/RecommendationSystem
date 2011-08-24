using System;
using System.Security.Cryptography;

namespace RecommendationSystem.QualityTesting
{
    public class CryptoRandom : Random
    {
        private RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private byte[] uint32Buffer = new byte[4];

        public CryptoRandom()
        {}

        public CryptoRandom(Int32 ignoredSeed)
        {}

        public override Int32 Next()
        {
            rng.GetBytes(uint32Buffer);
            return BitConverter.ToInt32(uint32Buffer, 0) & 0x7FFFFFFF;
        }

        public override Int32 Next(Int32 maxValue)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("maxValue");
            return Next(0, maxValue);
        }

        public override Int32 Next(Int32 minValue, Int32 maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("minValue");
            if (minValue == maxValue)
                return minValue;
            Int64 diff = maxValue - minValue;
            while (true)
            {
                rng.GetBytes(uint32Buffer);
                var rand = BitConverter.ToUInt32(uint32Buffer, 0);

                var max = (1 + (Int64)UInt32.MaxValue);
                var remainder = max % diff;
                if (rand < max - remainder)
                    return (Int32)(minValue + (rand % diff));
            }
        }

        public override double NextDouble()
        {
            rng.GetBytes(uint32Buffer);
            var rand = BitConverter.ToUInt32(uint32Buffer, 0);
            return rand / (1.0 + UInt32.MaxValue);
        }

        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            rng.GetBytes(buffer);
        }
    }
}