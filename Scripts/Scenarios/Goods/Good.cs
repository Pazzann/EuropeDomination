﻿using System.Collections.Generic;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods;

public abstract class Good
{
    public Good(int id, string name, Vector3 color)
    {
        Id = id;
        Name = name;
        Color = color;
    }

    public int Id { get; }
    public Vector3 Color { get; }
    public string Name { get; }


    public static double[] DefaultGoods(Dictionary<int, double> notNullGoods = null)
    {
        var a = new double[] { 0, 0, 0, 0 };
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
}