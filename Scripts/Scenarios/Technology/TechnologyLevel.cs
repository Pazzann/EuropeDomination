﻿using System;
using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.Scenarios.Technology;

public class TechnologyLevel
{
    public TechnologyLevel(List<Technology> technologies, DateTime date)
    {
        Technologies = technologies;
        Date = date;
    }

    public List<Technology> Technologies { get; }
    public DateTime Date { get; }
}