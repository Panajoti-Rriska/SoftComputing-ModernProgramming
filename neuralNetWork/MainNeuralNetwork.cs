using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetWork
{
    class MainNeuralNetwork
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("\nBegin Neural Network demo demonstarting AND gate\n");
                int[,] input = new int[,] { { 1, 0 }, { 1, 1 }, { 0, 1 }, { 0, 0 } };
                int[] outputs = { 0, 1, 0, 0 };

                double learningRate = 1;
                double totalError = 1;

                NeuralNetwork nm = new NeuralNetwork(input, outputs);
                nm.Training(learningRate, totalError);

                Console.WriteLine("Results:");

                for (int i = 0; i < 4; i++)
                    Console.WriteLine(nm.calculateOutput(input[i, 0], input[i, 1]));

               
                Console.WriteLine("End Neural Network demo\n");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal: " + ex.Message);
                Console.ReadLine();
            }
        }
    }

    class NeuralNetwork
    {
        private int[,] input;
        private int[] outputs;
        private double[] weights;

        public NeuralNetwork(int[,] _inputs, int[] _outputs)
        {
            input = _inputs;
            outputs = _outputs;
            Random r = new Random();

            weights = new double[3] { r.NextDouble(), r.NextDouble(), r.NextDouble() };
          
        }

        public void Training(double learningRate,double totalError)
        {
            while (totalError > 0.2)
            {
                totalError = 0;
                for (int i = 0; i < 4; i++)
                {
                    int output = calculateOutput(input[i, 0], input[i, 1]);
                    int error = outputs[i] - output;
                    Console.WriteLine(error);
                    weights[0] += learningRate * error * input[i, 0];
                    weights[1] += learningRate * error * input[i, 1];
                    weights[2] += learningRate * error * 1;

                    totalError += Math.Abs(error);
                }
            }
        }

        public int calculateOutput(double input1, double input2)
        {
            double sum = input1 * weights[0] + input2 * weights[1] + 1 * weights[2];
            return (sum >= 0) ? 1 : 0;
        }

    }
}


