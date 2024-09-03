using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIFactory : Control
{
	[Signal]
	public delegate void TrasportationRouteMenuPressedEventHandler();
	
	
	private PanelContainer _recipePanel;
	private VBoxContainer _recipeSpawner;
	private PackedScene _recipeScene;


	private PackedScene _recipeIngredientBox;
	private Factory _currentlyShownFactory;
	private TextureRect _outputGood;
	private GridContainer _recipeIngredientBoxSpawner;
	private Button _deleteButton;
	private Button _transportButton;
	public void Init()
	{
		_recipePanel = GetNode<PanelContainer>("PanelContainer2");
		_recipeSpawner =
			GetNode<VBoxContainer>("PanelContainer2/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer");
		_recipeScene = GD.Load<PackedScene>("res://Prefabs/GUIRecipe.tscn");
		
		

		int i = 0;
		foreach (var recipe in EngineState.MapInfo.Scenario.Recipes)
		{
			var a = _recipeScene.Instantiate() as GUIRecipe;
			a.SetInfo(recipe);
			var b = i;
			a.GetChild<Button>(1).Pressed += () => ChooseRecipe(b);
			_recipeSpawner.AddChild(a);
			i++;
		}
		
		_recipeIngredientBox = GD.Load<PackedScene>("res://Prefabs/GUIGoodFactoryInfo.tscn");
		_outputGood = GetNode<TextureRect>(
			"PanelContainer/MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/OutputGood");
		_recipeIngredientBoxSpawner =
			GetNode<GridContainer>(
				"PanelContainer/MarginContainer/VBoxContainer/HBoxContainer2/IngredientsContainer/GridContainer");
		_deleteButton =
			GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/Button2");
		_transportButton =
			GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/Button3");

	}
	
	public void ShowData(Factory factory)
	{
		_currentlyShownFactory = factory;
		foreach (var item in _recipeIngredientBoxSpawner.GetChildren())
		{
			item.QueueFree();
		}

		if (factory.Recipe == null)
		{
			_outputGood.GetChild<AnimatedTextureRect>(0).Texture = null;
			_deleteButton.Visible = false;
			_transportButton.Visible = false;
			return;
		}

		_deleteButton.Visible = true;
		_transportButton.Visible = true;
		_outputGood.GetChild<AnimatedTextureRect>(0).SetFrame(factory.Recipe.Output.Id);
		
		foreach (var ingredient in factory.Recipe.Ingredients)
		{
			var a = _recipeIngredientBox.Instantiate();
			a.GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(ingredient.Key.Id);
			a.GetChild<Label>(1).Text = ingredient.Value.ToString("N1");
			_recipeIngredientBoxSpawner.AddChild(a);
		}
	}

	private void ChooseRecipe(int id)
	{
		_currentlyShownFactory.Recipe = EngineState.MapInfo.Scenario.Recipes[id];
		ShowData(_currentlyShownFactory);
		_recipePanel.Visible = false;
	}

	private void _onChangeRecipeButtonPressed()
	{
		_recipePanel.Visible = true;
	}

	private void _onDeleteRecipePressed()
	{
		_currentlyShownFactory.Recipe = null;
		_currentlyShownFactory.ProductionRate = 0.1f;
		_currentlyShownFactory.TransportationRoute = null;
		ShowData(_currentlyShownFactory);
	}
	private void _onCloseRecipePanel()
	{
		_recipePanel.Visible = false;
	}

	private void _onTransportButtonPressed()
	{
		EmitSignal(SignalName.TrasportationRouteMenuPressed);
	}
}
