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
                return 1.00 / (1.00 + Math.Exp(-x));
            }

            public static double derivative(double x)
            {
                return x * (1 - x);
            }
        }

        class hiddenNeuron
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

                biasWeight = 1;
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
                get { return sigmoid.output((weights[0] * inputs[0]) + (weights[1] * inputs[1])  + (weights[2] * inputs[2]) + (weights[3] * inputs[3]) + biasWeight); }
            }

        }

        class outputNeuron
        {

            //2-2-1 network
            //4-2-4
            public double[] inputs = new double[2];
            public double[] weights = new double[2];
            public double error;

            private double biasWeight;

            private Random r = new Random();
            public void randomizeWeights()
            {
                weights[0] = r.NextDouble();
                weights[1] = r.NextDouble();

                biasWeight = 1;
            }

            public void adjustWeights()
            {
                weights[0] += error * inputs[0];
                weights[1] += error * inputs[1];

                biasWeight += error;
            }
            public double output
            {
                get { return sigmoid.output(weights[0] * inputs[0] + weights[1] * inputs[1] + biasWeight); }
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
            hiddenNeuron hiddenNeuron1 = new hiddenNeuron();
            hiddenNeuron hiddenNeuron2 = new hiddenNeuron();

            //Output neurons
            outputNeuron outputNeuron1 = new outputNeuron();
            outputNeuron outputNeuron2 = new outputNeuron();
            outputNeuron outputNeuron3 = new outputNeuron();
            outputNeuron outputNeuron4 = new outputNeuron();


            hiddenNeuron1.randomizeWeights();
            hiddenNeuron2.randomizeWeights();

            outputNeuron1.randomizeWeights();
            outputNeuron2.randomizeWeights();
            outputNeuron3.randomizeWeights();
            outputNeuron4.randomizeWeights();





            double epoch = 0;

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


                    Console.WriteLine("Hidden Neuron outPut1 = " + hiddenNeuron1.output + " and  Hidden Neuron output2 = " + hiddenNeuron2.output+"\n");

                    Console.WriteLine("{0} = {1} \n {2} = {3} \n {4} = {5}\n {6} = {7} \n", inputs[i, 0], outputNeuron1.output,
                                                                                            inputs[i, 1], outputNeuron2.output,
                                                                                            inputs[i, 2], outputNeuron3.output,
                                                                                       inputs[i, 3], outputNeuron4.output);

                    // 2) back propagation (adjusts weights)

                    // adjusts the weight of the output neuron, based on it's error
                    
                    outputNeuron1.error = sigmoid.derivative(outputNeuron1.output) * (results[i, 0] - outputNeuron1.output);
                    

                    outputNeuron2.error = sigmoid.derivative(outputNeuron2.output) * (results[i, 1] - outputNeuron2.output);
                   

                    outputNeuron3.error = sigmoid.derivative(outputNeuron3.output) * (results[i, 2] - outputNeuron3.output);
                    

                    outputNeuron4.error = sigmoid.derivative(outputNeuron4.output) * (results[i, 3] - outputNeuron4.output);
                   


                    // then adjusts the hidden neurons' weights, based on their errors
                    hiddenNeuron1.error = sigmoid.derivative(hiddenNeuron1.output) *  (outputNeuron1.error * outputNeuron1.weights[0] + 
                        outputNeuron2.error * outputNeuron2.weights[0] + outputNeuron3.error * outputNeuron3.weights[0] + outputNeuron4.error * outputNeuron4.weights[0]);

                    hiddenNeuron2.error = sigmoid.derivative(hiddenNeuron2.output) *  (outputNeuron1.error * outputNeuron1.weights[1] +
                        outputNeuron2.error * outputNeuron2.weights[1] + outputNeuron3.error * outputNeuron3.weights[1] + outputNeuron4.error * outputNeuron4.weights[1]);

                    hiddenNeuron1.adjustWeights();
                    hiddenNeuron2.adjustWeights();
                    outputNeuron1.adjustWeights();
                    outputNeuron2.adjustWeights();
                    outputNeuron3.adjustWeights();
                    outputNeuron4.adjustWeights();
             
                }

            } while (epoch < 2000);

            Console.ReadLine();
        }
    }
}
