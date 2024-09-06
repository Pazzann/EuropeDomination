using Godot;
using System;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUICountryWindow : GUIHandler
{
	public override void Init()
	{
		
	}
	
	public override void InputHandle(InputEvent @event)
	{
		
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUIShowCountryWindowEvent:
				Visible = true;
				return;
		}
	}

	private void _onCloseMenuPressed()
	{
		Visible = false;
	}
}
