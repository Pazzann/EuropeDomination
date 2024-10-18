using System;
using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.Scenarios.Technology;

[Serializable]
public class TechnologyTree
{
    public string Name { get; }
    public List<TechnologyLevel> TechnologyLevels { get; }

    public TechnologyTree(string name, List<TechnologyLevel> technologyLevels)
    {
        Name = name;
        TechnologyLevels = technologyLevels;
    }
}