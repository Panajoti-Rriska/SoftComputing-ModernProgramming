using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace sgaNew
{
    public delegate double GeneticAlgorythmFunction(double value);

    public class GeneticAlgorythm
    {
        private double m_mutationRate;
        private double m_crossoverRate;
        private int m_populationSize;
        private int m_generationSize;
        private int m_genomeSize;
        private double m_totalFitness;
        private string m_strFitness;
        private bool m_elitism;

        private ArrayList m_thisGeneration;
        private ArrayList m_nextGeneration;
        private ArrayList m_fitnessTable;

        static Random m_random = new Random();

        static private GeneticAlgorythmFunction getFitness;

        public GeneticAlgorythmFunction FitnessFunction
        {
            get
            {
                return getFitness;
            }
            set
            {
                getFitness = value;
            }
        }

        #region Properties
        public bool Elitism
        {
            get
            {
                return m_elitism;
            }
            set
            {
                m_elitism = value;
            }
        }

        public double MutationRate
        {
            get
            {
                return m_mutationRate;
            }
            set
            {
                m_mutationRate = value;
            }
        }

        public double CrossoverRate
        {
            get
            {
                return m_crossoverRate;
            }
            set
            {
                m_crossoverRate = value;
            }
        }

        public int GenomeSize
        {
            get
            {
                return m_genomeSize;
            }
            set
            {
                m_genomeSize = value;
            }
        }

        public int Generations
        {
            get
            {
                return m_generationSize;
            }
            set
            {
                m_generationSize = value;
            }
        }

        public int PopulationSize
        {
            get
            {
                return m_populationSize;
            }
            set
            {
                m_populationSize = value;
            }
        }

        public string FitnessFile
        {
            get
            {
                return m_strFitness;
            }
            set
            {
                m_strFitness = value;
            }
        }

        #endregion


        /// <summary>
        /// Default constructor sets mutation rate to 5%, crossover to 80%, population to 100,
        /// and generations to 2000.
        /// </summary>
        /// 
        public GeneticAlgorythm()
        {
            InitialValues();
            m_mutationRate = 0.03;
            m_crossoverRate = 0.70;
            m_populationSize = 100;
            m_generationSize = 1000;
            m_strFitness = "";

        }

        public GeneticAlgorythm(double crossoverRate,double mutationRate, int populationSize, int generationSize, int genomeSize)
        {
            InitialValues();
            m_mutationRate = mutationRate;
            m_crossoverRate = crossoverRate;
            m_populationSize = populationSize;
            m_generationSize = generationSize;
            m_genomeSize = genomeSize;
            m_strFitness = "";

        }

        public GeneticAlgorythm(int genomeSize)
        {
            InitialValues();
            m_genomeSize = genomeSize;
        }

        public void InitialValues()
        {
            m_elitism = false;
        }

        /// <summary>
        /// Method which starts the execution.
        /// </summary>
        public void Start()
        {
            if (getFitness == null)
                throw new ArgumentNullException("Need to supply fitness function");
            if (m_genomeSize == 0)
                throw new IndexOutOfRangeException("Genome size not set");

            //  Create the fitness table.
            m_fitnessTable = new ArrayList();
            m_thisGeneration = new ArrayList(m_generationSize);
            m_nextGeneration = new ArrayList(m_generationSize);
            Genome.MutationRate = m_mutationRate;

            CreateGenomes();
            //order based on fitness
            OrderPopulation();

            //Time to create new generation
            for (int i = 0; i < m_generationSize; i++)
            {
                CreateNextGeneration();
                OrderPopulation();
                
            }

        }

        private void CreateNextGeneration()
        {
            m_nextGeneration.Clear();
            Genome g = null;
            if (m_elitism)
                g = (Genome)m_thisGeneration[m_populationSize - 1];

            for (int i = 0; i < m_populationSize; i += 2)
            {
                int candidatex1 = RouletteSelection();
                int candidatex2 = RouletteSelection();
                Genome parent1, parent2, child1, child2;
                parent1 = ((Genome)m_thisGeneration[candidatex1]);
                parent2 = ((Genome)m_thisGeneration[candidatex2]);

                if (m_random.NextDouble() < m_crossoverRate)
                {
                    parent1.Crossover(ref parent2, out child1, out child2);
                }
                else
                {
                    child1 = parent1;
                    child2 = parent2;
                }
                child1.Mutate();
                child2.Mutate();

                m_nextGeneration.Add(child1);
                m_nextGeneration.Add(child2);
            }

            if (m_elitism && g != null)
                m_nextGeneration[0] = g;

            //Apply new generation as current generation
            m_thisGeneration.Clear();
            for (int i = 0; i < m_populationSize; i++)
                m_thisGeneration.Add(m_nextGeneration[i]);
        }

        private int RouletteSelection()
        {
            double randomFitness = m_random.NextDouble() * m_totalFitness;

            int idx = -1;
            int mid;
            int first = 0;
            int last = m_populationSize - 1;
            mid = (last - first) / 2;

            //  ArrayList's BinarySearch is for exact values only
            while (idx == -1 && first <= last)
            {
                if (randomFitness < (double)m_fitnessTable[mid])
                {
                    last = mid;
                }
                else if (randomFitness > (double)m_fitnessTable[mid])
                {
                    first = mid;
                }
                mid = (first + last) / 2;
                //  lies between i and i+1
                if ((last - first) == 1)
                    idx = last;
            }
            return idx;

        }

        private void OrderPopulation()
        {
            m_totalFitness = 0;

            for (int i = 0; i < m_populationSize; i++)
            {
                Genome g = ((Genome)m_thisGeneration[i]);
                //here we get the actuall fitness value
                g.Fitness = FitnessFunction(g.GenesValue());
                //The sum of all fitnesses
                m_totalFitness += g.Fitness;
            }

            //SORT the current generation

            m_thisGeneration.Sort(new CompareFitness());

            //  now sorted in order of fitness.
            double fitness = 0.0;
            m_fitnessTable.Clear();
            for (int i = 0; i < m_populationSize; i++)
            {
                fitness += ((Genome)m_thisGeneration[i]).Fitness;
                m_fitnessTable.Add((double)fitness);
            }
        }

        private void CreateGenomes()
        {
            for (int i = 0; i < m_populationSize; i++)
            {
                Genome g = new Genome(m_genomeSize);
                m_thisGeneration.Add(g);
            }
        }

        public void GetBestValue(out double[] values, out double fitness)
        {
            Genome g = ((Genome)m_thisGeneration[m_populationSize - 1]);

            for (int i = 0; i < m_populationSize; i++)
            {
                Genome gTest = ((Genome)m_thisGeneration[i]);
                System.Console.WriteLine("{0} x value", gTest.GenesValue());
                
            }

            values = new double[g.Length];
            g.GetValues(ref values);
            fitness = (double)g.Fitness;
        }

    }
}
