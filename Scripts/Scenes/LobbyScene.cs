using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Utils;
using Godot;
using Steamworks;
using Steamworks.Data;
using Image = Godot.Image;

namespace EuropeDominationDemo.Scripts.Scenes;

public partial class LobbyScene : Node2D
{
	private Camera _camera;
	private Sprite2D _mapSprite;

	private PackedScene _lobbyPlayerList;
	private VBoxContainer _lobbyPlayerListContainer;

	private Dictionary<int, string> _selectedProvincesPlayers = new Dictionary<int, string>();

	public override void _Ready()
	{
		_mapSprite = GetNode<Sprite2D>("Map");
		GlobalResources.MapTexture = _mapSprite.Texture.GetImage();
		Scenario scenario = new EuropeScenario();
		EngineState.MapInfo = new MapData(scenario);
		EngineState.MapInfo.Scenario.PlayerList = new Dictionary<int, string>();
		EngineState.MapInfo.Scenario.PlayerList.Add(0, "currentPlayer");
		EngineState.PlayerCountryId = 0;
		_mapUpdate();
		EngineState.MapInfo.Scenario.ChangeGameMode(GameModes.RandomSpawn);
		EngineState.MapInfo.Scenario.ResourceMode = ResourceModes.RandomSpawn;
		_camera = GetNode<Camera>("Camera");
		_camera.Reset(new Rect2(Vector2.Zero, GetNode<Sprite2D>("Map").Texture.GetSize()));

		_lobbyPlayerList = GD.Load<PackedScene>("res://Prefabs/ScenesPrefabs/LobbyPlayerWindow.tscn");
		_lobbyPlayerListContainer =
			GetNode<VBoxContainer>("CanvasLayer/PanelContainer/MarginContainer/VBoxContainer/LobbyPlayersContainer/PlayersContainer");
		_drawPlayersList();
		if (MultiplayerState.MultiplayerMode)
		{
			SteamMatchmaking.OnLobbyMemberJoined += _onLobbyMemberJoined;
			SteamMatchmaking.OnLobbyMemberLeave += _onLobbyMemberLeave;
		}
		
	}

	private async void _drawPlayersList()
	{
		foreach (var child in _lobbyPlayerListContainer.GetChildren())
		{
			child.QueueFree();
		}

		if (!MultiplayerState.MultiplayerMode)
		{
			var a = _lobbyPlayerList.Instantiate() as PanelContainer;
			a.GetChild(0).GetChild<ColorRect>(0).Visible = false;
			a.GetChild(0).GetChild<Label>(2).Text = SteamState.Name;
			a.GetChild(0).GetChild<TextureRect>(1).Texture = SteamForGodotUtils.ImageTextureFromSteamImage(await SteamFriends.GetMediumAvatarAsync(SteamState.SteamId));
			_lobbyPlayerListContainer.AddChild(a);
		}
		else
		{
			foreach (var member in MultiplayerState.Lobby?.Members)
			{
				var a = _lobbyPlayerList.Instantiate() as PanelContainer;
				a.GetChild(0).GetChild<ColorRect>(0).Visible = member.Id == MultiplayerState.Lobby?.Owner.Id;
				a.GetChild(0).GetChild<Label>(2).Text = member.Name;
				a.GetChild(0).GetChild<TextureRect>(1).Texture = SteamForGodotUtils.ImageTextureFromSteamImage(await member.GetMediumAvatarAsync());
				_lobbyPlayerListContainer.AddChild(a);
			}
		}
	}

	private void _onLobbyMemberLeave(Lobby lobby, Friend friend)
	{
		_drawPlayersList();
	}
	private void _onLobbyMemberJoined(Lobby lobby, Friend friend)
	{
		_drawPlayersList();
	}

	private void _onMainMenuPressed()
	{
		if (MultiplayerState.MultiplayerMode)
		{
			MultiplayerState.Lobby?.Leave();
			MultiplayerState.MultiplayerMode = false;
			MultiplayerState.Lobby = null;
		}
		GetTree().ChangeSceneToFile("res://main.tscn");
	}

	

	private void _mapUpdate()
	{
		ShaderMaterial mapMaterial = (_mapSprite.Material as ShaderMaterial);
		mapMaterial.SetShaderParameter("colors", EngineState.MapInfo.MapColors);
		mapMaterial.SetShaderParameter("viewMod", 0);
		mapMaterial.SetShaderParameter("selectedID", -1);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		_camera.HandleInput(@event);
		var tileId = _findTile();
		if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false } &&
			EngineState.MapInfo.Scenario.GameMode == GameModes.SelectionSpawn)
		{
			if (_selectedProvincesPlayers.ContainsKey(tileId) ||
				EngineState.MapInfo.Scenario.Map[tileId] is not LandProvinceData)
				return;

			if (_selectedProvincesPlayers.Count(d => d.Value == "currentPlayer") > 0)
				foreach (var pair in _selectedProvincesPlayers.Where(d => d.Value == "currentPlayer"))
				{
					_selectedProvincesPlayers.Remove(pair.Key);
					LandProvinceData p = (EngineState.MapInfo.Scenario.Map[pair.Key] as LandColonizedProvinceData);
					p = new UncolonizedProvinceData(p.Id, p.Name, p.Terrain, p.Good,
						(p as LandColonizedProvinceData).Modifiers);
					EngineState.MapInfo.Scenario.Map[pair.Key] = p;
				}

			_selectedProvincesPlayers.Add(tileId, "currentPlayer");
			(EngineState.MapInfo.Scenario.Map[tileId] as UncolonizedProvinceData).CurrentlyColonizedByCountry =
				EngineState.MapInfo.Scenario.Countries[0];
			EngineState.MapInfo.Scenario.Map[tileId] =
				(EngineState.MapInfo.Scenario.Map[tileId] as UncolonizedProvinceData).ConvertToLandProvince();
			EngineState.MapInfo.Scenario.Countries[0].CapitalId = tileId;
			(EngineState.MapInfo.Scenario.Map[tileId] as LandColonizedProvinceData).Development = 10;
			_mapUpdate();
		}
	}

	private int _findTile()
	{
		var mousePos = GetLocalMousePosition();
		var iMousePos = new Vector2I((int)mousePos.X, (int)mousePos.Y);
		if (!GlobalResources.MapTexture.GetUsedRect().HasPoint(iMousePos)) return -1;

		var tileId = GameMath.GetProvinceId(GlobalResources.MapTexture.GetPixelv(iMousePos));

		if (tileId < 0 || tileId >= EngineState.MapInfo.Scenario.Map.Length)
			return -3;

		return tileId;
	}

	private void _onResourceSpawnModeSelected(int id)
	{
		EngineState.MapInfo.Scenario.ResourceMode = (ResourceModes)id;
	}

	private void _onStartPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/GameScene.tscn");
	}

	private void _onGameModeButtonItemSelected(int itemIndex)
	{
		EngineState.MapInfo.Scenario.ChangeGameMode((GameModes)itemIndex);
		_mapUpdate();
	}
}
