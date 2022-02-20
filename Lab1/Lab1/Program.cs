using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1
{
    class Program
    {
        public static int[] mas = {233, 303, 81, 129, 200, 82, 115, 228, 64, 17,
                                   67, 648, 29, 39, 210, 10, 94, 465, 135, 312,
                                   606, 698, 15, 764, 32, 45, 54, 13, 116, 24,
                                   477, 16, 841, 95, 3, 79, 118, 208, 9, 59, 171,
                                   295, 78, 67, 38, 57, 91, 18, 39, 324, 416,
                                   270, 114, 25, 675, 287, 374, 119, 227, 5,
                                   109, 94, 171, 226, 183, 350, 27, 64, 433, 88,
                                   167, 152, 159, 319, 8, 162, 36, 488, 65, 77,
                                   307, 522, 140, 65, 355, 482, 180, 29, 342,
                                   233, 117, 182, 184, 113, 86, 630, 476, 136, 397, 66};

        public static decimal γ = 0.62M;
        public static int troubleFreePropability = 275;
        public static int failRate = 648;

        public static List<decimal> fs = new List<decimal>();
        public static List<decimal> ps = new List<decimal>();
        
        public static decimal pi;
        public static decimal pi1;

        public static decimal d;
        public static decimal T;
        static void Main(string[] args)
        {
            Dictionary<decimal, decimal> map = new Dictionary<decimal, decimal>();
            Array.Sort(mas);

            PrintSortedArray(mas);

            decimal sum = mas.Sum();
            PrintMean(sum);
            PrintTcp(mas.Max(), 10);

            PrintLimitsOfIntervals(mas.Max(), 10);
            PrintFs(mas.Max(), 10);

            PrintPs(mas.Max(), 10);

            FindPs();

            PrintStatisticalWorkOnFailure(mas.Max(), 10);

            PrintProbabilityOfTroubleFreeOperationAndFailureIntensity(mas.Max(), 10);
        }

        public static void PrintSortedArray(int[] sortedArray)
        {
            Console.WriteLine("Вiдсортований список значень:");
            for (int i = 0; i < sortedArray.Length; i++)
            {
                Console.Write(sortedArray[i] + " ");
            }
            Console.WriteLine();
        }

        public static decimal CalculationOfMean(decimal arraySum)
        {
            return arraySum / mas.Length;
        }

        public static void PrintMean(decimal sum)
        {
            Console.WriteLine($"\nСереднє значення:\n{CalculationOfMean(sum)}\n");
        }

        public static decimal CalculationOfTcp(decimal maxArrayValue, int numberOfIntervals)
        {
            return maxArrayValue / numberOfIntervals;
        }

        public static void PrintTcp(decimal maxArrayValue, int numberOfIntervals)
        {
            Console.WriteLine($"Tcp = {CalculationOfTcp(maxArrayValue, numberOfIntervals)}");
        }

        public static Dictionary<decimal, decimal> CalculationOfIntervalLimits(decimal maxArrayValue, int numberOfIntervals)
        {
            decimal upperLimit = 0;
            Dictionary<decimal, decimal> limits = new Dictionary<decimal, decimal>();
            for (int i = 0; i < numberOfIntervals; i++)
            {
                decimal bottomLimit = upperLimit;
                upperLimit += CalculationOfTcp(maxArrayValue, numberOfIntervals);
                limits.Add(bottomLimit, upperLimit);
            }

            return limits;
        }

        public static void PrintLimitsOfIntervals(decimal maxArrayValue, int numberOfIntervals)
        {
            Console.WriteLine("Границi iнтервалiв:");
            foreach (KeyValuePair<decimal, decimal> interval in CalculationOfIntervalLimits(maxArrayValue, numberOfIntervals))
            {
                Console.WriteLine($"Iнтевал вiд {interval.Key} до {interval.Value};");
            }
        }

        public static decimal CalculationOfF(decimal maxArrayValue, int numberOfIntervals, List<decimal> values)
        {
            decimal f = values.Count / (mas.Length * CalculationOfTcp(maxArrayValue, numberOfIntervals));
            return f;
        }

        public static void PrintFs(decimal maxArrayValue, int numberOfIntervals)
        {
            Console.WriteLine("\nЗначення статистичної щiльностi розподiлу ймовiрностi вiдмови: ");
            foreach (KeyValuePair<decimal, decimal> interval in CalculationOfIntervalLimits(maxArrayValue, numberOfIntervals))
            {
                List<decimal> values = new List<decimal>();
                foreach (int value in mas)
                {
                    if (value >= interval.Key && value <= interval.Value)
                    {
                        values.Add(value);
                    }
                }
                fs.Add(CalculationOfF(maxArrayValue, numberOfIntervals,values));
            }
            for (int i = 0; i < numberOfIntervals; i++)
            {
                Console.WriteLine($"-для {i + 1}-го iнтервалу: f = {Math.Round(fs[i], 6)}");
            }
        }

        public static void CalculationOfP(decimal maxArrayValue, int numberOfIntervals)
        {
            decimal p = 0;
            ps.Add(1.0M);
            foreach (decimal f in fs)
            {
                p += f * CalculationOfTcp(maxArrayValue, numberOfIntervals);
                ps.Add(Math.Round(1 - p, 6));
            }
        }

        public static void PrintPs(decimal maxArrayValue, int numberOfIntervals)
        {
            CalculationOfP(maxArrayValue, numberOfIntervals);
            Console.WriteLine("\nЙмовiрнiсть безвiдмовної роботи пристрою на час правої границi iнтервалу: ");
            for (int i = 0; i < ps.Count; i++)
            {
                if (i == 0) { Console.WriteLine($"- для 0: p({i}) = {Math.Round(ps[i], 6)}"); }
                else { Console.WriteLine($"- для {i}-го iнтервалу: p({i}) = {Math.Round(ps[i], 6)}"); }
            }
        }

        public static void FindPs()
        {
            for (int i = 0; i < ps.Count; i++)
            {
                if (ps[i] > γ && γ > ps[i + 1])
                {
                    pi1 = ps[i + 1];
                    pi = ps[i];
                }
            }
        }

        public static decimal CalculationOfD()
        {
            return (pi1 - γ) / (pi1 - pi);
        }
        public static decimal CalculationOfT(decimal maxArrayValue, int numberOfIntervals)
        {
            return CalculationOfTcp(maxArrayValue, numberOfIntervals) - 
                (CalculationOfTcp(maxArrayValue, numberOfIntervals) * Math.Round(CalculationOfD(), 2));
        }

        public static void PrintStatisticalWorkOnFailure(decimal maxArrayValue, int numberOfIntervals)
        {
            Console.WriteLine($"\nСтатистичний y-вiдсотковий наробiток на вiдмову:\nd = {Math.Round(CalculationOfD(), 2)}" +
                $"\nT = {Math.Round(CalculationOfT(maxArrayValue, numberOfIntervals), 2)}");
        }    
        
        public static void PrintProbabilityOfTroubleFreeOperationAndFailureIntensity(decimal maxArrayValue, int numberOfIntervals)
        {
            decimal lowerLimit = 0;
            int intervalNumber = 0;
            decimal s = 0;
            foreach (KeyValuePair<decimal, decimal> i in CalculationOfIntervalLimits(maxArrayValue,numberOfIntervals))
            {
                if (i.Key <= troubleFreePropability && i.Value >= troubleFreePropability)
                {
                    lowerLimit = i.Key;
                    break;
                }
                intervalNumber++;
            }
            
            for (int i = 0; i <= intervalNumber; i++)
            {
                if (i <= intervalNumber - 1) { s += fs[i] * CalculationOfTcp(maxArrayValue, numberOfIntervals); }
                if (i == intervalNumber) { s += fs[i] * (troubleFreePropability - lowerLimit); }
            }
            Console.WriteLine($"\nЙмовiрнiсть бузвiдмовної роботи на час {troubleFreePropability} годин: " +
                $"\nР({troubleFreePropability}) = {Math.Round(1 - s, 5)}\n");

            Console.WriteLine($"Iнтенсивнiсть вiдмов на час {troubleFreePropability} годин: " +
                $"\nλ({troubleFreePropability}) = {Math.Round((fs[intervalNumber] / (1 - s)), 6)}");
        }
    }
}
