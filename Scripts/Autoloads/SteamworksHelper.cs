using System;
using Godot;
using Steamworks;

namespace EuropeDominationDemo.Scripts.GlobalStates;

public partial class SteamworksHelper : Node
{
    public override void _Ready()
    {
        //if(!SteamClient.IsValid) GetTree().Quit();
        Dispatch.OnDebugCallback = ( type, str, server ) =>
        {
            GD.Print( $"[Callback {type} {(server ? "server" : "client")}]" );
            GD.Print( str );
            GD.Print( $"" );
        };
        
        try
        {
            SteamClient.Init( 480, true );
            SteamState.SteamId = SteamClient.SteamId;
            SteamState.Name = SteamClient.Name;
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
            SteamClient.Shutdown();
        }
        catch (Exception e)
        {
            GD.Print("Steam API Error:" + e.Message);
        }
    }
    

    public override void _Process(double delta)
    {
        SteamClient.RunCallbacks();
    }
}