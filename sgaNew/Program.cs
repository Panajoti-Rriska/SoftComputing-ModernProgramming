using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sgaNew
{
    public class Program
    {
        static void Main(string[] args)
        {
            StartSGA();
        }

        public static void StartSGA()
        {
            Console.Write("its 1:00 am \n");
            //  Crossover		= 70%
            //  Mutation		=  3%
            //  Population size = 100
            //  Generations		= 1000
            //  Genome size		= 1(one dimension-x-)
            GeneticAlgorythm geneticAlgorythm = new GeneticAlgorythm(0.7, 0.03, 100, 1000, 1);
            geneticAlgorythm.FitnessFunction = new GeneticAlgorythmFunction(theFuction);
            geneticAlgorythm.Elitism = true;
            geneticAlgorythm.Start();

            double[] values;
            double fitness;
            geneticAlgorythm.GetBestValue(out values, out fitness);

            System.Console.WriteLine("Best fitness({0}):", fitness);
            for (int i = 0; i < values.Length; i++)
                System.Console.WriteLine("{0} x value", values[i]);


            Console.ReadLine();
        }

        //Our function
        public static double theFuction(double value)
        {
            if (value == null)
                throw new ArgumentOutOfRangeException("no value for funtion");

            double x = value;

            double functionValue = (Math.Exp(x) * Math.Sin(10 * Math.PI * x) + 1) / x + 5;

            return functionValue;
        }
    }
}
