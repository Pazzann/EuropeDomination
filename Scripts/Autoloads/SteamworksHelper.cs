using System;
using Godot;
using Steamworks;

namespace EuropeDominationDemo.Scripts.GlobalStates;

public partial class SteamworksHelper : Node
{
    public override void _Ready()
    {
        if(!SteamAPI.IsSteamRunning()) GetTree().Quit();
        try
        {
            var err = SteamAPI.InitEx(out var ErrorMessage);
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
    /*
     *  SteamClient.Init(AppId, true);
     *  SteamId = SteamClient.SteamId;
     *  Name = SteamClient.Name;
     */

    public override void _Process(double delta)
    {
        SteamAPI.RunCallbacks();
        SteamInput.RunFrame();
    }
}