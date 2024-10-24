using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.NavalWeapon;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods;


[Serializable]
[JsonPolymorphic]
[JsonDerivedType(typeof(ConsumableGood), typeDiscriminator: "consumableGood")]
[JsonDerivedType(typeof(HarvestedGood), typeDiscriminator: "harvestedGood")]

[JsonDerivedType(typeof(Helmet), typeDiscriminator: "helmetGood")]
[JsonDerivedType(typeof(Horse), typeDiscriminator: "horseGood")]
[JsonDerivedType(typeof(Weapon.Weapon), typeDiscriminator: "weaponGood")]
[JsonDerivedType(typeof(Wheel), typeDiscriminator: "wheelGood")]
[JsonDerivedType(typeof(Boots), typeDiscriminator: "bootsGood")]
[JsonDerivedType(typeof(Armor), typeDiscriminator: "armorGood")]
[JsonDerivedType(typeof(AdditionalSlotGood), typeDiscriminator: "additionalSlotGood")]

[JsonDerivedType(typeof(ArtilleryWeapon), typeDiscriminator: "artilleryWeaponGood")]
[JsonDerivedType(typeof(LandWeapon), typeDiscriminator: "landWeaponGood")]
[JsonDerivedType(typeof(InfantryWeapon), typeDiscriminator: "infantryWeaponGood")]

[JsonDerivedType(typeof(HeavyNavalWeapon), typeDiscriminator: "heavyNavalWeaponGood")]
[JsonDerivedType(typeof(LightNavalWeapon), typeDiscriminator: "lightNavalWeaponGood")]
[JsonDerivedType(typeof(MediumNavalWeapon), typeDiscriminator: "mediumNavalWeaponGood")]
[JsonDerivedType(typeof(NavalWeapon), typeDiscriminator: "navalWeaponGood")]
public class Good

{
    public int Id { get;init;  }
    public Vector3 Color { get;init;  }
    public string Name { get; init; }
    public float Cost { get;init;  }

    public Good(int id, string name, Vector3 color, float cost)
    {
        Id = id;
        Name = name;
        Color = color;
        Cost = cost;
    }

    [JsonConstructor]
    public Good()
    {
        
    }


    public static double[] DefaultGoods(int count, Dictionary<int, double> notNullGoods = null)
    {
        var a = new double[count];
        if (notNullGoods == null) return a;
        foreach (var kvp in notNullGoods) a[kvp.Key] = kvp.Value;

        return a;
    }

    public static bool CheckIfMeetsRequirements(double[] availableGoods, double[] neededGoods)
    {
        for (var i = 0; i < availableGoods.Length; i++)
            if (availableGoods[i] - neededGoods[i] < 0)
                return false;

        return true;
    }

    public static double[] DecreaseGoodsByGoods(double[] availableGoods, double[] neededGoods)
    {
        for (var i = 0; i < availableGoods.Length; i++) availableGoods[i] -= neededGoods[i];

        return availableGoods;
    }

    [Obsolete("Use ShowNonZeroGoods and check if resulting Text is empty")]
    public static bool IsDifferentFromNull(double[] resources)
    {
        foreach (var t in resources)
            if (t > 0)
                return true;

        return false;
    }
}