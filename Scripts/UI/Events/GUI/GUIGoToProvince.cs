﻿namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUIGoToProvince : GUIEvent
{
    public int Id;

    public GUIGoToProvince(int id)
    {
        Id = id;
    }
}