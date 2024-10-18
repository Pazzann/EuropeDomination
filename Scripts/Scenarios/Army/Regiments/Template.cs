using System;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;

[Serializable]
[JsonDerivedType(typeof(ArmyInfantryRegimentTemplate), typeDiscriminator: "armyInfantryRegimentTemplate")]
[JsonDerivedType(typeof(ArmyArtilleryRegimentTemplate), typeDiscriminator: "armyArtilleryRegimentTemplate")]
[JsonDerivedType(typeof(ArmyCavalryRegimentTemplate), typeDiscriminator: "armyCavalryRegimentTemplate")]

[JsonDerivedType(typeof(TransportShipTemplate), typeDiscriminator: "transportShipTemplate")]
[JsonDerivedType(typeof(LightShipTemplate), typeDiscriminator: "lightShipTemplate")]
[JsonDerivedType(typeof(HeavyShipTemplate), typeDiscriminator: "heavyShipTemplate")]
[JsonDerivedType(typeof(MediumShipTemplate), typeDiscriminator: "medicalShipTemplate")]
public abstract class Template
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Template(string name, int id)
    {
        Name = name;
        Id = id;
    }

    public abstract int TrainingTime { get; }
    public abstract float Cost { get; }
}