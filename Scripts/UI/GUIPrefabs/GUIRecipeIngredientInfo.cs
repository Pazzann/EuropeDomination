using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIPrefabs;

public partial class GUIRecipeIngredientInfo : PanelContainer
{
	public void SetInfo(Good good, double amount)
	{
		GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(good.Id);
		GetChild(0).GetChild<Label>(1).Text = good.ToString();
		GetChild(0).GetChild<Label>(2).Text = amount.ToString("N1");
	}
}
