using System;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.Technology;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIInfoBox : GUIHandler
{
	private PanelContainer _infoBox;
	public static AdvancedLabel Info;

	public override void Init()
	{
		Info = GetNode<AdvancedLabel>("BoxContainer/MarginContainer/AdvancedLabel");
		_infoBox = GetNode<PanelContainer>("BoxContainer");
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
			case ToGUIShowInfoBoxEvent e:
				Info.Clear();
				_infoBox.Size = new Vector2(10, 10);
				Visible = true;
				return;
			case ToGUIHideInfoBox:
				Info.Clear();
				_infoBox.Size = new Vector2(10, 10);
				Visible = false;
				return;
			default:
				return;
		}
	}
}
