using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace sgaNew
{
    public class CompareFitness : IComparer
    {
        public CompareFitness()
        {

        }

        public int Compare( object x, object y)
        {
            if (!(x is Genome) || !(y is Genome))
                throw new ArgumentException("Not of type Genome");

            if (((Genome)x).Fitness > ((Genome)y).Fitness)
                return 1;
            else if (((Genome)x).Fitness == ((Genome)y).Fitness)
                return 0;
            else
                return -1;
        }
    }
}
