using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios.Goods;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings.FactoryRecipies;

public class Recipe
{
    public Dictionary<Good, double> Ingredients;
    public Good Output;

    public Recipe(Dictionary<Good, double> ingredients, Good output)
    {
        Ingredients = ingredients;
        Output = output;
    }
}