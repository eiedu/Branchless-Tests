using System;
using System.ComponentModel.DataAnnotations;

namespace Branchless_Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //Random test
            int[] B = new int[1000000];
            Random R = new Random();
            for (int i = 0; i < B.Length; i++)
            {
                B[i] = R.Next(0, 3*128);
            }

            int[] C = new int[1000000];
            B.CopyTo(C, 0);
            for (int i = 0; i < 1000000; i++)
            {
                C[i] = C[i] + 128 * (C[i] > 255 ? 1 : 0) - 128 * (C[i] > 127 ? 1 : 0);
            }

            var Average = new TimeSpan(0);
            for (int j = 0; j < 100; j++)
            {
                int[] A = new int[1000000];
                B.CopyTo(A, 0);
                var StartTime = DateTime.Now;
                for (int i = 0; i < 1000000; i++)
                {
                    A[i] = A[i] + 128 * (A[i] > 255 ? 1 : 0) - 128 * (A[i] > 127 ? 1 : 0);
                }
                Average += DateTime.Now - StartTime;
                if (j == 0)
                {
                    for (int i = 0; i < 1000000; i++)
                    {
                        if (A[i] != C[i])
                        {
                            Console.WriteLine($"sumtingwong | i:{i,6} | C:{C[i],6} | A:{A[i],6} | B:{B[i]}");
                            break;
                        }
                    }
                }
            }
            Average /= 100;
            Console.WriteLine($"Branched in: {Average.TotalMilliseconds}ms");

            Average = new TimeSpan(0);
            for (int j = 0; j < 100; j++)
            {
                int[] A = new int[1000000];
                B.CopyTo(A, 0);
                var StartTime = DateTime.Now;
                for (int i = 0; i < 1000000; i++)
                {
                    A[i] -= 128 * (((A[i] & 128) >> 7) - ((A[i] & 128) >> 7) * ((A[i] & 256) >> 8));
                }
                Average += DateTime.Now - StartTime;
                if (j == 0)
                {
                    for (int i = 0; i < 1000000; i++)
                    {
                        if (A[i] != C[i])
                        {
                            Console.WriteLine($"sumtingwong | i:{i,6} | C:{C[i],6} | A:{A[i],6} | B:{B[i]}");
                            break;
                        }
                    }
                }
            }
            Average /= 100;
            Console.WriteLine($"Branchless in: {Average.TotalMilliseconds}ms");


            Average = new TimeSpan(0);
            for (int j = 0; j < 100; j++)
            {
                int[] A = new int[1000000];
                B.CopyTo(A, 0);
                var StartTime = DateTime.Now;
                for (int i = 0; i < 1000000; i++)
                {
                    A[i] -= (A[i] & ((A[i] & 128) ^ ((A[i] & 256) >> 1)));
                }
                Average += DateTime.Now - StartTime;
                if (j == 0)
                {
                    for (int i = 0; i < 1000000; i++)
                    {
                        if (A[i] != C[i])
                        {
                            Console.WriteLine($"sumtingwong | i:{i,6} | C:{C[i],6} | A:{A[i],6} | B:{B[i]}");
                            break;
                        }
                    }
                }
            }
            Average /= 100;
            Console.WriteLine($"Branchless xor in: {Average.TotalMilliseconds}ms");

            Average = new TimeSpan(0);
            for (int j = 0; j < 100; j++)
            {
                int[] A = new int[1000000];
                B.CopyTo(A, 0);
                var StartTime = DateTime.Now;
                for (int i = 0; i < 1000000; i++)
                {
                    A[i] = A[i] + 128 * Convert.ToInt32(A[i] > 255) - 128 * Convert.ToInt32(A[i] > 127);
                }
                Average += DateTime.Now - StartTime;
                if (j == 0)
                {
                    for (int i = 0; i < 1000000; i++)
                    {
                        if (A[i] != C[i])
                        {
                            Console.WriteLine($"sumtingwong | i:{i,6} | C:{C[i],6} | A:{A[i],6} | B:{B[i]}");
                            break;
                        }
                    }
                }
            }
            Average /= 100;
            Console.WriteLine($"Converted Branchless in: {Average.TotalMilliseconds}ms");

            unsafe
            {
                Average = new TimeSpan(0);
                for (int j = 0; j < 100; j++)
                {
                    int[] A = new int[1000000];
                    B.CopyTo(A, 0);
                    var StartTime = DateTime.Now;
                    for (int i = 0; i < 1000000; i++)
                    {
                        bool a = A[i] > 255;
                        byte* aa = (byte*)&a;

                        bool b = A[i] > 127;
                        byte* bb = (byte*)&b;

                        A[i] = A[i] + 128 * *aa - 128 * *bb;
                    }
                    Average += DateTime.Now - StartTime;
                    if (j == 0)
                    {
                        for (int i = 0; i < 1000000; i++)
                        {
                            if (A[i] != C[i])
                            {
                                Console.WriteLine($"sumtingwong | i:{i,6} | C:{C[i],6} | A:{A[i],6} | B:{B[i]}");
                                break;
                            }
                        }
                    }
                }
                Average /= 100;
                Console.WriteLine($"Unsafe Converted Branchless in: {Average.TotalMilliseconds}ms");
            }
        }
    }
}
