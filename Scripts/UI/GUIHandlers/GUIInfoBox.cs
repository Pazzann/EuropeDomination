using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;


namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIInfoBox : GUIHandler
{
	private RichTextLabel _infoLabel;
	private PanelContainer _infoBox;
	public override void Init()
	{
		_infoLabel = GetNode<RichTextLabel>("BoxContainer/MarginContainer/RichTextLabel");
		_infoBox = GetNode<PanelContainer>("BoxContainer");
		_infoLabel.BbcodeEnabled = true;
		return;
	}

	public override void _Process(double delta)
	{
		_infoBox.Position = GetViewport().GetMousePosition() + new Vector2(10, 10);
	}

	public override void InputHandle(InputEvent @event)
	{
	}
	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUIShowInfoBoxProvinceEvent e:
				_infoBox.Size = new Vector2(_infoBox.Size.X, 10);
				_infoLabel.Text = "[color=yellow]" + e.ProvinceData.Name + "[/color]" + "\n"
								  + ((e.ProvinceData is LandProvinceData landProvinceData)? "Good: "+ landProvinceData.Good.Name:"")
								  +  ((e.ProvinceData is LandProvinceData landProvinceData2)? "\n" + "Terrain	: "+ landProvinceData2.Terrain.Name:"")
								  +  ((e.ProvinceData is LandColonizedProvinceData landProvinceData3)? "\n" + "Developement	: "+ landProvinceData3.Development:"")
					+ ((EngineState.DebugMode)?"Debug data:" + "\n" + "Id:" + e.ProvinceData.Id  + ((e.ProvinceData is LandColonizedProvinceData landProvinceData4)?"\n" + "Owner ID: " + landProvinceData4.Owner: "") :"");
				Visible = true;
				return;
			case ToGUIHideInfoBox:
				_infoLabel.Text = "";
				Visible = false;
				return;
			default:
				return;
		}
		
	}
}
