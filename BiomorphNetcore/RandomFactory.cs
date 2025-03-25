using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomorphNetcore
{
    internal class RandomFactory : BiomorphFactoryBase
    {
        public override Biomorph generateCandidateList()
        {
            int[] genes = new int[GENE_COUNT];

            // First 8 genes have values between -5 and 5.
            genes[0] = 0;
            genes[1] = 0;
            genes[2] = 0;
            genes[3] = 0;
            genes[4] = 0;
            genes[5] = 0;
            genes[6] = 0;
            genes[7] = 0;
            genes[8] = 4;

            return new Biomorph(genes);
        }

        public override List<Biomorph> MutatePopulation(Biomorph rootPopulation)
        {
            Random random = new Random();
            List<Biomorph> mutatedPopulation = new List<Biomorph>(GENE_COUNT);
            int mutatedGene = 0;
            int mutation = 1;

            int mutateCount = random.Next(GENE_COUNT);
            List<int> alreadyMutated = new List<int>(mutateCount);
            int[] genes = rootPopulation.GetGenotype();

            for (int i = 0; i <= mutateCount; i++)
            {
                //Pick a random gene that has noit been mutated
                do
                {
                    mutatedGene = random.Next(GENE_COUNT);
                } while (alreadyMutated.Contains(mutatedGene));

                alreadyMutated.Add(mutatedGene);

                if (mutatedGene == LENGTH_GENE_INDEX)
                {
                    //Random change
                    mutation = random.Next(LENGTH_GENE_MIN, LENGTH_GENE_MAX);
                }
                else
                {
                    //Random change
                    mutation = random.Next(GENE_MIN, GENE_MAX);
                }

                genes[mutatedGene] += mutation;

                mutatedPopulation.Add(new Biomorph(genes));
            }

            return mutatedPopulation;
        }
    }
}
