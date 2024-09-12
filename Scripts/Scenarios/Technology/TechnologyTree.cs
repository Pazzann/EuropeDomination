using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.Scenarios.Technology;

public class TechnologyTree
{
    public string Name { get; }
    public List<TechnologyLevel> TechnologyLevels;

    public TechnologyTree(string name, List<TechnologyLevel> technologyLevels)
    {
        Name = name;
        TechnologyLevels = technologyLevels;
    }
}