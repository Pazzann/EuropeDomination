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
		//TODO: change
		var defaultProvince = EngineState.MapInfo.Scenario.Map[0] as LandProvinceData;
		
		_goodContainer = GetNode<GridContainer>("MarginContainer/ScrollContainer/GridContainer");
		_goodBox = GD.Load<PackedScene>("res://Prefabs/GUIGoodSelector.tscn");
		
		for (int i = 0; i < defaultProvince.Resources.Length; i++)
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
