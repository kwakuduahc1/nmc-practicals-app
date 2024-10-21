using System.Security.Cryptography;

namespace NursingPracticals.Controllers.Helpers
{
    public static class PaymentsHelper
    {
        public static string RandomString(int length = 6)
        {
            string alphabet = "ABCDEFGHJKLMNPQRTUVWXYZ1234567890";
            var outOfRange = byte.MaxValue + 1 - (byte.MaxValue + 1) % alphabet.Length;
            return string.Concat(
                Enumerable
                    .Repeat(0, byte.MaxValue)
                    .Select(e => RandomByte(Random.Shared.Next(6, 8)))
                    .Where(randomByte => randomByte < outOfRange)
                    .Take(length)
                    .Select(randomByte => alphabet[randomByte % alphabet.Length])
            );
        }

        static byte RandomByte(int len)
        {
            var bytes = new byte[len];
            RandomNumberGenerator.Create().GetBytes(bytes);
            return bytes.Single();
        }
    }
}
