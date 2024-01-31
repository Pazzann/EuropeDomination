using System.Collections.Generic;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;

namespace EuropeDominationDemo.Scripts.UI;

public class GUICallStack
{
	private List<GUIHandler> _guiHandlers;

	public GUICallStack(List<GUIHandler> guiHandlers)
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
}
