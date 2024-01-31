
using System.Collections.Generic;
using Godot;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;

namespace EuropeDominationDemo.Scripts.UI;

public delegate void GUIEventDelegate(GUIEvent @event);
public partial class GUI : Control
{
	public GUICallStack AllGUIHandlersControls;



	public event GUIEventDelegate GUIGlobalEvent = null;
	
	
	public void Init()
	{
		List<GUIHandler> allGUIHandlers = new List<GUIHandler>();
		
		
		allGUIHandlers.Add(GetNode<GUIHandler>("./GuiProvinceWindow"));
		allGUIHandlers.Add(GetNode<GUIHandler>("./GuiTimeWindow"));
		allGUIHandlers.Add(GetNode<GUIHandler>("./GuiMiniMap"));
		allGUIHandlers.Add(GetNode<GUIHandler>("./GuiCountryInfo"));

		foreach (var guiHandler in allGUIHandlers)
		{
			guiHandler.GUIEvent += SendGUIEvent;
		}
		
		AllGUIHandlersControls = new GUICallStack(allGUIHandlers);
		AllGUIHandlersControls.Init();
	}

	public void ToGUIEventHandler(ToGUIEvent @event)
	{
		AllGUIHandlersControls.ToGUIHandleEvent(@event);
	}

	public void SendGUIEvent(GUIEvent @event)
	{ 
		GUIGlobalEvent.Invoke(@event);
	}

}
