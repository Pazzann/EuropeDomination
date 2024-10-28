﻿using System;
using System.Text.Json.Serialization;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

[Serializable]
public class TransportShipTemplate : ShipTemplate
{
    public TransportShipTemplate(string name,  int owner) : base(name,  owner)
    {
    }
    [JsonConstructor]
    public TransportShipTemplate()
    {
    }
}