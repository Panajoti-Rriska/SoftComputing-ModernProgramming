using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backPropagation
{
    class Program
    {
        static void Main(string[] args)
        {
            train();
        }

        class sigmoid
        {
            public static double output(double x)
            {
                return 1.0 / (1.0 + Math.Exp(-x));
            }

            public static double derivative(double x)
            {
                return x * (1 - x);
            }
        }

        class Neuron
        {

            //2-2-1 network
            //4-2-4
            public double[] inputs = new double[4];
            public double[] weights = new double[4];
            public double error;

            private double biasWeight;

            private Random r = new Random();
            public void randomizeWeights()
            {
                weights[0] = r.NextDouble();
                weights[1] = r.NextDouble();
                weights[2] = r.NextDouble();
                weights[3] = r.NextDouble();

                biasWeight = r.NextDouble();
            }

            public void adjustWeights()
            {
                weights[0] += error * inputs[0];
                weights[1] += error * inputs[1];
                weights[2] += error * inputs[2];
                weights[3] += error * inputs[3];

                biasWeight += error;
            }
            public double output
            {
                get { return sigmoid.output(weights[0] * inputs[0] + weights[1] * inputs[1]  + biasWeight); }
            }

        }

        public static void train()
        {
            //[row,collum]

            double[,] inputs = 
            {
                { 1, 0, 0, 0},
                { 0, 1, 0, 0},
                { 0, 0, 1, 0},
                { 0, 0, 0, 1}
            };

            // desired results
            double[,] results = 
            {
                { 1, 0, 0, 0},
                { 0, 1, 0, 0},
                { 0, 0, 1, 0},
                { 0, 0, 0, 1}
            };

            //hIDDEN NEURONS
            Neuron hiddenNeuron1 = new Neuron();
            Neuron hiddenNeuron2 = new Neuron();

            //Output neurons
            Neuron outputNeuron1 = new Neuron();
            Neuron outputNeuron2 = new Neuron();
            Neuron outputNeuron3 = new Neuron();
            Neuron outputNeuron4 = new Neuron();


            hiddenNeuron1.randomizeWeights();
            hiddenNeuron2.randomizeWeights();

            outputNeuron1.randomizeWeights();
            outputNeuron2.randomizeWeights();
            outputNeuron3.randomizeWeights();
            outputNeuron4.randomizeWeights();





            int epoch = 0;

            do
            {
                epoch++;

                //4 different patterns
                for (int i = 0; i < 4; i++)
                {
                    // 1) forward propagation (calculates output)
                    hiddenNeuron1.inputs = new double[] { inputs[i, 0], inputs[i, 1], inputs[i, 2], inputs[i, 3] };
                    hiddenNeuron2.inputs = new double[] { inputs[i, 0], inputs[i, 1], inputs[i, 2], inputs[i, 3] };

                    //Here were work should be done
                    //Error happens cause we have adjusting weights on 2 different occasion, one with 4 inputs and the other with only 2 inputs from hidden layer 
                    outputNeuron1.inputs = new double[] { hiddenNeuron1.output, hiddenNeuron2.output };
                    outputNeuron2.inputs = new double[] { hiddenNeuron1.output, hiddenNeuron2.output };
                    outputNeuron3.inputs = new double[] { hiddenNeuron1.output, hiddenNeuron2.output };
                    outputNeuron4.inputs = new double[] { hiddenNeuron1.output, hiddenNeuron2.output };


                    Console.WriteLine("{0} xor {1} = {2}", inputs[i, 0], inputs[i, 1], outputNeuron1.output);

                    // 2) back propagation (adjusts weights)

                    // adjusts the weight of the output neuron, based on it's error
                    
                    outputNeuron1.error = sigmoid.derivative(outputNeuron1.output) * (results[i, 0] - outputNeuron1.output);
                    outputNeuron1.adjustWeights();

                    outputNeuron2.error = sigmoid.derivative(outputNeuron2.output) * (results[i, 1] - outputNeuron2.output);
                    outputNeuron2.adjustWeights();

                    outputNeuron3.error = sigmoid.derivative(outputNeuron3.output) * (results[i, 2] - outputNeuron3.output);
                    outputNeuron3.adjustWeights();

                    outputNeuron4.error = sigmoid.derivative(outputNeuron1.output) * (results[i, 3] - outputNeuron4.output);
                    outputNeuron4.adjustWeights();


                    // then adjusts the hidden neurons' weights, based on their errors
                    hiddenNeuron1.error = sigmoid.derivative(hiddenNeuron1.output) * outputNeuron1.error * outputNeuron1.weights[0];
                    hiddenNeuron2.error = sigmoid.derivative(hiddenNeuron2.output) * outputNeuron1.error * outputNeuron1.weights[1];

                    hiddenNeuron1.adjustWeights();
                    hiddenNeuron2.adjustWeights();

                  
                }

            } while (epoch < 2000);

            Console.ReadLine();
        }
    }
}
