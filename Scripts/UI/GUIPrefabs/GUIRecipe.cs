using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings.FactoryRecipies;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIPrefabs;

public partial class GUIRecipe : PanelContainer
{
    public void SetInfo(Recipe recipe)
    {
        GetNode("MarginContainer/HBoxContainer/GuiGood").GetChild<AnimatedTextureRect>(0).SetFrame(recipe.Output.Id);
        var guiIngredientInfo = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIRecipeIngredientInfo.tscn");
        var spawner = GetNode<VBoxContainer>("MarginContainer/HBoxContainer/ScrollContainer/VBoxContainer");
        foreach (var ingredient in recipe.Ingredients)
        {
            var a = guiIngredientInfo.Instantiate() as GUIRecipeIngredientInfo;
            a.SetInfo(ingredient.Key, ingredient.Value);
            spawner.AddChild(a);
        }
    }
}