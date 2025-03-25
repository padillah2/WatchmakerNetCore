using System;
using System.Collections.Generic;

namespace BiomorphNetcore
{
    public abstract class BiomorphFactoryBase : IBiomorphFactory
    {
        /** The total number of genes that make up a biomorph. */
        public const int GENE_COUNT = 9;
        /** The minimum permitted value for most genes. */
        public const int GENE_MIN = -5;
        /** The maximum permitted value for most genes. */
        public const int GENE_MAX = 5;
        /** The index of the gene that controls biomporph size. */
        public const int LENGTH_GENE_INDEX = 8;
        /** The minimum permitted value for the length gene. */
        public const int LENGTH_GENE_MIN = 1;
        /** The maximum permitted value for the length gene. */
        public const int LENGTH_GENE_MAX = 7;

        public virtual Biomorph generateCandidateList()
        {
            Random rand = new();
            int[] genes = new int[GENE_COUNT];

            for (int i = 0; i < GENE_COUNT - 1; i++)
            {
                // First 8 genes have values between -5 and 5.
                genes[i] = rand.Next(11) - 5;
            }
            // Last genes ha a value between 1 and 7.
            genes[LENGTH_GENE_INDEX] = rand.Next(LENGTH_GENE_MAX) + 1;

            return new Biomorph(genes);
        }

        public abstract List<Biomorph> MutatePopulation(Biomorph rootPopulation);
    }
}