
using System.Collections.Generic;
using Godot;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;

namespace EuropeDominationDemo.Scripts.UI;

public delegate void GUIEventDelegate(GUIEvent @event);
public partial class GUI : Control
{
	public GUICallMulticaster AllGUIHandlersControls;



	public event GUIEventDelegate GUIGlobalEvent = null;
	
	
	public void Init()
	{
		List<GUIHandler> allGUIHandlers = new List<GUIHandler>();
		
		
		allGUIHandlers.Add(GetNode<GUILandProvinceWindow>("./GuiProvinceWindow"));
		allGUIHandlers.Add(GetNode<GUITimeWindow>("./GuiTimeWindow"));
		allGUIHandlers.Add(GetNode<GUIMiniMapWindow>("./GuiMiniMap"));
		allGUIHandlers.Add(GetNode<GUICountryInfo>("./GuiCountryInfo"));
		allGUIHandlers.Add(GetNode<GUIConsole>("./Console"));
		allGUIHandlers.Add(GetNode<GUIInfoBox>("./GuiInfoBox"));
		allGUIHandlers.Add(GetNode<GUIArmyViewer>("./GuiArmyViewer"));
		allGUIHandlers.Add(GetNode<GUICountryWindow>("GuiCountryWindow"));
		allGUIHandlers.Add(GetNode<GUIDiplomacyWindow>("./GuiDiplomacyWindow"));
		allGUIHandlers.Add(GetNode<GUIBattleWindow>("./GuiBattleWindow"));

		foreach (var guiHandler in allGUIHandlers)
		{
			guiHandler.GUIEvent += SendGUIEvent;
		}
		
		AllGUIHandlersControls = new GUICallMulticaster(allGUIHandlers);
		AllGUIHandlersControls.Init();
	}

	public void InputHandle(InputEvent @event)
	{
		AllGUIHandlersControls.InputHandle(@event);
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
