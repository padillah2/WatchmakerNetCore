using System;
using System.Collections.Generic;

namespace BiomorphNetcore
{
    public class DawsonFactory : BiomorphFactoryBase
    {
        public override List<Biomorph> MutatePopulation(Biomorph rootPopulation)
        {
            List<Biomorph> mutatedPopulation = new List<Biomorph>(GENE_COUNT);
            int mutatedGene = 0;
            int mutation = 1;
            for (int i = 0; i < (GENE_COUNT * 2); i++)
            {
                int[] genes = rootPopulation.GetGenotype();

                mutation *= -1; // Alternate between incrementing and decrementing.
                if (mutation == 1) // After gene has been both incremented and decremented, move to next one.
                {
                    mutatedGene = (mutatedGene + 1) % GENE_COUNT;
                }

                genes[mutatedGene] += mutation;
                int min = mutatedGene == LENGTH_GENE_INDEX ? LENGTH_GENE_MIN : GENE_MIN;
                int max = mutatedGene == LENGTH_GENE_INDEX ? LENGTH_GENE_MAX : GENE_MAX;
                if (genes[mutatedGene] > max)
                {
                    genes[mutatedGene] = min;
                }
                else if (genes[mutatedGene] < min)
                {
                    genes[mutatedGene] = max;
                }

                mutatedPopulation.Add(new Biomorph(genes));
            }

            return mutatedPopulation;

        }
    }
}
