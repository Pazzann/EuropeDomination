using EuropeDominationDemo.Scripts.GlobalStates;
using Godot;
using Steamworks;

namespace EuropeDominationDemo.Scripts.Scenes;

public partial class MainScene : TextureRect
{
	private PanelContainer _multiplayerPanel;
	private TabContainer _settingsPanel;
	private PackedScene _lobbyScene;
	
	private CallResult<LobbyCreated_t> OnLobbyCreatedCallResult;
	public override void _Ready()
	{
		_multiplayerPanel = GetNode<PanelContainer>("MultiplayerLobbys");
		_settingsPanel = GetNode<TabContainer>("MainSettings");
		_lobbyScene = GD.Load<PackedScene>("res://Scenes/LobbyScene.tscn");
		
		OnLobbyCreatedCallResult = CallResult<LobbyCreated_t>.Create(OnLobbyCreated);
	}

	private void _onSinglePlayerPressed()
	{
		MultiplayerState.MultiplayerMode = false;
		GetTree().ChangeSceneToFile("res://Scenes/LobbyScene.tscn");
	}

	private void _onMultiPlayerPressed()
	{
		_multiplayerPanel.Visible = true;
	}
	
	private void _onSettingsPressed()
	{
		
	}

	#region Multiplayer lobbies

	private void _onCreateLobbyButtonPressed()
	{
		
		GD.Print("Create Lobby Pressed");
		var data = SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 16);
		OnLobbyCreatedCallResult.Set(data);
		GD.Print("SteamMatchmaking.CreateLobby(" + ELobbyType.k_ELobbyTypePublic + ", " + 1 + ") : " + data);
	}
	
	void OnLobbyCreated(LobbyCreated_t pCallback, bool bIOFailure)
	{
		
		GD.Print("[" + LobbyCreated_t.k_iCallback + " - LobbyCreated] - " + pCallback.m_eResult + " -- " + pCallback.m_ulSteamIDLobby);
		if (pCallback.m_eResult == EResult.k_EResultOK)
		{
			MultiplayerState.MultiplayerMode = true;
			MultiplayerState.LobbyId = (CSteamID)pCallback.m_ulSteamIDLobby;
			MultiplayerState.LobbyOwnerId = SteamUser.GetSteamID();
			MultiplayerState.LobbyMembers.Add(MultiplayerState.LobbyOwnerId);
			GD.Print("LobbyId: " + MultiplayerState.LobbyId);
			GD.Print("LobbyOwnerId: " + MultiplayerState.LobbyOwnerId);
			GetTree().ChangeSceneToFile("res://Scenes/LobbyScene.tscn");
			SteamMatchmaking.SetLobbyData(MultiplayerState.LobbyId, "name", "Test Lobby!");
			GD.Print(SteamMatchmaking.GetLobbyData(MultiplayerState.LobbyId, "name"));
		}
	}

	#endregion
}
