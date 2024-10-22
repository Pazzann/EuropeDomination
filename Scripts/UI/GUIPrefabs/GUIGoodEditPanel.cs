using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIPrefabs;

public partial class GUIGoodEditPanel : PanelContainer
{
	[Signal]
	public delegate void GoodChangePressedEventHandler(int goodId);

	private PackedScene _goodBox;

	private GridContainer _goodContainer;

	public void Init()
	{
		_goodContainer = GetNode<GridContainer>("MarginContainer/ScrollContainer/GridContainer");
		_goodBox = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIGoodSelector.tscn");

		ChangeGoods(EngineState.MapInfo.Scenario.Goods);
	}

	public void ChangeGoods(Good[] newGoods)
	{
		foreach (var child in _goodContainer.GetChildren()) child.QueueFree();

		foreach (var good in newGoods)
		{
			var a = _goodBox.Instantiate();
			a.GetChild<AnimatedTextureRect>(0).SpriteFrames = GlobalResources.GoodSpriteFrames;
			a.GetChild<AnimatedTextureRect>(0).SetFrame(good.Id);
			a.GetChild(0).GetChild<Button>(0).Pressed += () => _goodPressed(good.Id);
			_goodContainer.AddChild(a);
		}
	}

	private void _goodPressed(int id)
	{
		EmitSignal(SignalName.GoodChangePressed, id);
	}
}
