using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenes;

public partial class LobbyScene : Node2D
{
	private Camera _camera;
	private Sprite2D _mapSprite;
	public override void _Ready()
	{
		_mapSprite = GetNode<Sprite2D>("Map");
		
		Scenario scenario = new EuropeScenario(_mapSprite.Texture.GetImage());
		EngineState.MapInfo = new MapData(scenario);
		EngineState.MapInfo.Scenario.PlayerList = new Dictionary<int, string>();
		EngineState.MapInfo.Scenario.PlayerList.Add( 0, "currentPlayer" );
		EngineState.PlayerCountryId = 0;
		_mapUpdate();
		EngineState.MapInfo.Scenario.ChangeGameMode(GameModes.RandomSpawn);
		_camera = GetNode<Camera>("Camera");
		_camera.Reset(new Rect2(Vector2.Zero, GetNode<Sprite2D>("Map").Texture.GetSize()));
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
