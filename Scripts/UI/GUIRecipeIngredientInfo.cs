using Godot;
using System;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Utils;

public partial class GUIRecipeIngredientInfo : PanelContainer
{
	public void SetInfo(Good good, double amount)
	{
		GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame((int)good);
		GetChild(0).GetChild<Label>(1).Text = good.ToString();
		GetChild(0).GetChild<Label>(2).Text = amount.ToString("N1");
	}
}
