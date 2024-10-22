using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EuropeDominationDemo.Scripts.Scenarios.Technology;

[Serializable]
public class TechnologyLevel
{
    public TechnologyLevel(List<Technology> technologies, DateTime date)
    {
        Technologies = technologies;
        Date = date;
    }

    [JsonConstructor]
    public TechnologyLevel()
    {
        
    }

    public List<Technology> Technologies { get; init; }
    public DateTime Date { get; init; }
}