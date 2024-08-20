using System.Collections.Generic;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;
using Godot;

namespace EuropeDominationDemo.Scripts.UI;

public class GUICallMulticaster
{
	private readonly List<GUIHandler> _guiHandlers;

	public GUICallMulticaster(List<GUIHandler> guiHandlers)
	{
		_guiHandlers = guiHandlers;
	}
	
	public void Init()
	{
		foreach (var guiHandler in _guiHandlers)
		{
			guiHandler.Init();
		}
	}

	public void ToGUIHandleEvent(ToGUIEvent @event)
	{
		foreach (var guiHandler in _guiHandlers)
		{
			guiHandler.ToGUIHandleEvent(@event);
		}
	}

	public void InputHandle(InputEvent @event)
	{
		foreach (var guiHandler in _guiHandlers)
		{
			guiHandler.InputHandle(@event);
		}
	}
}
