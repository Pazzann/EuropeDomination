using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.Scenarios.Technology;

public class TechnologyTree
{
    private List<TechnologyLevel> TechnologyLevels;

    public TechnologyTree(List<TechnologyLevel> technologyLevels)
    {
        TechnologyLevels = technologyLevels;
    }
}