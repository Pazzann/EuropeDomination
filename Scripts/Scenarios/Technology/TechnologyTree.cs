using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EuropeDominationDemo.Scripts.Scenarios.Technology;

[Serializable]
public class TechnologyTree
{
    public string Name { get; init; }
    public List<TechnologyLevel> TechnologyLevels { get; init;  }

    [JsonConstructor]
    public TechnologyTree()
    {
        
    }
    
    public TechnologyTree(string name, List<TechnologyLevel> technologyLevels)
    {
        Name = name;
        TechnologyLevels = technologyLevels;
    }
}