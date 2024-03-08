using System;
using System.Collections;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
#pragma warning disable SYSLIB0023

namespace RSAEncryption
{
    internal class Program
    {
        public static List<int> primes = new List<int>();
        private static byte[] Random512Bits()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[64];
                rng.GetBytes(bytes);

                return bytes;
            }
        }

        private static BigInteger GenerateLargePrime()
        {
            BigInteger result = 0 ;

            while (result == 0)
            {
                byte[] bytes = Random512Bits();
                BigInteger bigInteger = new BigInteger(bytes);
                bigInteger = BigInteger.Abs(bigInteger);
                bool valid = true;
                if (bigInteger.IsEven)
                    bigInteger++;

                for (int i = 0; i < primes.Count; i++)
                {
                    if (bigInteger % primes[i] == 0)
                    {
                        Console.WriteLine("Not A Prime");
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    result = bigInteger;
                    break;
                }
            }
            return result;
        }

        static void Main(string[] args)
        {
            BigInteger q;
            BigInteger p;
            BigInteger z;
            BigInteger ModValue;
            BigInteger Decrypt;
            BigInteger Encrypt = 0;

            primes.Add(2);
            int nextPrime = 3;
            while (primes.Count < 2000)
            {
                int sqrt = (int)Math.Sqrt(nextPrime);
                bool isPrime = true;
                for (int i = 0; (int)primes[i] <= sqrt; i++)
                {
                    if (nextPrime % primes[i] == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    primes.Add(nextPrime);
                }
                nextPrime += 2;
            }


            q = GenerateLargePrime();
            p = GenerateLargePrime();
            ModValue = p * q;
            z = (p - 1)*(q - 1);

            while (Encrypt < 1 || Encrypt > z)
                Encrypt = new BigInteger(Random512Bits());

            Decrypt = (Encrypt - 1) % z;

            // Public Key Bundle: (n, e)
            // Private Key Bundle: (d, e)

            string TestString = "Hello, how was your day? I think you stink";

            byte[] bytes = Encoding.ASCII.GetBytes(TestString);

            BigInteger plainText = new BigInteger(bytes);

            BigInteger cipherText = (plainText * Encrypt) % ModValue;

            byte[] badBytes = cipherText.ToByteArray();

            Console.WriteLine(Encoding.ASCII.GetString(badBytes));


            BigInteger DecodedText = (cipherText / Decrypt) % ModValue;

            byte[] decodedBytes = plainText.ToByteArray();

            Console.WriteLine(Encoding.ASCII.GetString(decodedBytes));


        }
    }
}