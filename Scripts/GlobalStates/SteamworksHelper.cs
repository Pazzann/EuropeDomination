using System;
using Godot;
using Steamworks;

namespace EuropeDominationDemo.Scripts.GlobalStates;

public partial class SteamworksHelper : Node
{
    public override void _Ready()
    {
        GD.Print("Steam is running:" + SteamAPI.IsSteamRunning());
        try
        {
            GD.Print(SteamAPI.Init() ? "Steam API init" : "Steam API init failed");
        }
        catch (Exception e)
        {
            GD.Print("Steam API Error:" + e.Message);
        }
    }

    public override void _ExitTree()
    {
        try
        {
            SteamAPI.Shutdown();
        }
        catch (Exception e)
        {
            GD.Print("Steam API Error:" + e.Message);
        }
    }
}