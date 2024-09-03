using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Utils;
using Godot;


public partial class GUIGoodEditPanel : PanelContainer
{
	[Signal]
	public delegate void GoodChangePressedEventHandler(int goodId);
	
	private GridContainer _goodContainer;
	private PackedScene _goodBox;
	public void Init()
	{
		_goodContainer = GetNode<GridContainer>("MarginContainer/ScrollContainer/GridContainer");
		_goodBox = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIGoodSelector.tscn");
		
		for (int i = 0; i < EngineState.MapInfo.Scenario.Goods.Count; i++)
		{
			var WhyDoIEVENNEEDTHISSHIT = i;
			var a = _goodBox.Instantiate();
			a.GetChild<AnimatedTextureRect>(0).SetFrame(i);
			a.GetChild(0).GetChild<Button>(0).Pressed += () => _goodPressed(WhyDoIEVENNEEDTHISSHIT);
			_goodContainer.AddChild(a);
				
		}
	}

	private void _goodPressed(int id)
	{
		EmitSignal(SignalName.GoodChangePressed, id);
	}

}
