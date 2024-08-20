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
	public override void InputHandle(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent)
			_infoBox.Position = mouseEvent.Position + new Vector2(10, 10);
	}
	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUIShowInfoBoxProvinceEvent e:
				_infoLabel.Text = "[color=yellow]" + "Namehere" + "[/color]" + "\n"
				                  + ((e.ProvinceData is LandProvinceData landProvinceData)? "Good: "+ landProvinceData.Good:"")
				                  + ((e.ProvinceData is LandProvinceData landProvinceData2)? "Good: "+ landProvinceData2.Terrain:"")
					+ ((EngineState.DebugMode)?"Debug data:" + "\n" + "Id:" + e.ProvinceData.Id :"");
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
