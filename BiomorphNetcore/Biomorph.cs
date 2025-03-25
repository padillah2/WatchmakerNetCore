using System;

namespace BiomorphNetcore
{
    public class Biomorph
    {

        private int[] genes;
        private int[][]? phenotype;

        public Biomorph(int[] Genes)
        {
            genes = new int[Genes.Length];
            Genes.CopyTo(genes, 0);
        }

        public int[] GetGenotype()
        {
            return (int[])genes.Clone();
        }

        /**
         * Returns an array of integers that represent the graphical pattern
         * determined by the biomorph's genes.
         * @return A 2-dimensional array containing the 8-element dx and dy
         * arrays required to draw the biomorph.
         */
        public int[][] getPatternPhenotype()
        {
            if (phenotype == null)
            {
                // Decode the genes as per Dawkins' rules.
                int[] dx = new int[DawsonFactory.GENE_COUNT - 1];
                dx[3] = genes[0];
                dx[4] = genes[1];
                dx[5] = genes[2];

                dx[1] = -dx[3];
                dx[0] = -dx[4];
                dx[7] = -dx[5];

                dx[2] = 0;
                dx[6] = 0;

                int[] dy = new int[DawsonFactory.GENE_COUNT - 1];
                dy[2] = genes[3];
                dy[3] = genes[4];
                dy[4] = genes[5];
                dy[5] = genes[6];
                dy[6] = genes[7];

                dy[0] = dy[4];
                dy[1] = dy[3];
                dy[7] = dy[5];

                phenotype = new int[][] { dx, dy };
            }
            return phenotype;
        }

        /**
        * @return The value of the gene that controls the size of this biomorph.
        */
        public int getLengthPhenotype()
        {
            return genes[DawsonFactory.LENGTH_GENE_INDEX];
        }

        internal Biomorph Clone()
        {
            return new Biomorph(genes);
        }

    }
}
