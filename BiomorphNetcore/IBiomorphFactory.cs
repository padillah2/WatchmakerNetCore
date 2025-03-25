using System.Collections.Generic;

namespace BiomorphNetcore
{
    public interface IBiomorphFactory
    {
        Biomorph generateCandidateList();
        List<Biomorph> MutatePopulation(Biomorph rootPopulation);
    }
}