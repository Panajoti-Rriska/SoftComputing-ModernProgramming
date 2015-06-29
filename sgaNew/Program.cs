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
            //  Crossover		= 70%
            //  Mutation		=  3%
            //  Population size = 100
            //  Generations		= 1000
            //  Genome size		= 1(one dimension-x-)
            GeneticAlgorythm geneticAlgorythm = new GeneticAlgorythm(0.7f, 0.03f, 100, 1000, 1);
            geneticAlgorythm.FitnessFunction = new GeneticAlgorythmFunction(theFuction);
            geneticAlgorythm.Elitism = true;
            geneticAlgorythm.Start();

            float value;
            float fitness;
            geneticAlgorythm.GetBestValue(out value, out fitness);

            System.Console.WriteLine("Best fitness({0}):", fitness);
            System.Console.WriteLine("{0} x value", value);


            Console.ReadLine();
        }

        //Our function
        public static float theFuction(float value)
        {
            if (value == null)
                throw new ArgumentOutOfRangeException("no value for funtion");

            float x = value;

            float functionValue = (float)((Math.Exp(x) * Math.Sin(10 * Math.PI * x) + 1) / x) + 5;

            return functionValue;
        }
    }
}
