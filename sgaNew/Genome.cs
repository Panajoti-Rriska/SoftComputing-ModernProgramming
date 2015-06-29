using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sgaNew
{
    public class Genome
    {
        public float m_genes;
        private float m_fitness;
        static Random m_random = new Random();
        int p = 6, // precision of solution
            bitsNum;  // number of bits needed to encode the solution

        private static float m_mutationRate;

        public Genome()
        {
            CreateGenes();
        }
        public Genome(bool createGenes)
        {
            if (createGenes)
                CreateGenes();
        }

        public Genome(ref float genes)
        {
            m_genes = genes;
        }

        private void CreateGenes()
        {
            //range of x's we want from [0.5,2.5]
            var maximum = 2.5;
            var minimum = 0.5;
            m_genes = (float)(minimum +( m_random.NextDouble()*(maximum-minimum))) ; 
        }
        public int[] Bits { get; set; } // encoded solution
        private int bitsLenght = 6;
        public void SwapBit(int index)  // swaps a single bit at specified index
        {
            if (Bits[index] == 0)
                Bits[index] = 1;
            else
                Bits[index] = 0;
        }

        public void Crossover(ref Genome parent1,ref Genome parent2,  out Genome child1, out Genome child2)
        {
            int pos = (int)(m_random.NextDouble() * bitsLenght);
            child1 = new Genome(true);
            child2 = new Genome(true);

            //Conversion from float to bits
            var bytesParent1 = BitConverter.GetBytes(parent1.m_genes);
            BitArray bitsParent1 = new BitArray(bytesParent1);

            var bytesParent2 = BitConverter.GetBytes(parent1.m_genes);
            BitArray bitsParent2 = new BitArray(bytesParent1);

            BitArray childBits1 = new BitArray(bitsLenght);
            BitArray childBits2 = new BitArray(bitsLenght);


            for (int i = 0; i < bitsLenght; i++)
            {
                if(pos >= bitsLenght)
                {
                    childBits1[i] = bitsParent2[i];
                    childBits2[i] = bitsParent1[i]; 

                }else
                {
                    childBits1[i] = bitsParent1[i];
                    childBits2[i] = bitsParent2[i]; 
                }
            }

            int[] tempArray = new int[1];
            int[] tempArray2 = new int[1];


            childBits1.CopyTo(tempArray, 0);
            childBits2.CopyTo(tempArray2, 0);


            if((float)tempArray[0] <2.5f && (float)tempArray[0]>0.5f)
            {
                child1.m_genes = (float)tempArray[0];
                child2.m_genes = (float)tempArray2[0];
            }
           
            
      
            //Switch bits between childs of given parents
        }

        public void Mutate()
        {
            //Switch 0 to 1 bit

            if (m_random.NextDouble() < m_mutationRate)
            {
                //Conversion from float to bits
                var bytesGene = BitConverter.GetBytes(m_genes);
                BitArray bitsGene = new BitArray(bytesGene);

                int pos = (int)(m_random.NextDouble() * bitsLenght);

                bitsGene[pos] = !bitsGene[pos];

                int[] tempArray = new int[1];
                //getting our bits to Int
                bitsGene.CopyTo(tempArray, 0);

                if ((float)tempArray[0] < 2.5f && (float)tempArray[0] > 0.5f)
                {
                    m_genes = (float)tempArray[0];
                }


            }
              
        }

        public float Genes()
        {
            return m_genes;
        }

        public float GenesValue()
        {
            return m_genes;
        }

        public void Output()
        {

                System.Console.WriteLine("{0:F4}", m_genes);
            
            System.Console.Write("\n");
        }

        public void GetValues(ref float values)
        {

                values = m_genes;
        }

        #region Properties
        public float Fitness
        {
            get
            {
                return m_fitness;
            }
            set
            {
                m_fitness = value;
            }
        }

        public static float MutationRate
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

        #endregion 
    }
}
