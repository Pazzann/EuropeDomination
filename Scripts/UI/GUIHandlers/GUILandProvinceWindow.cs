using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.GUI.ProvinceEvents;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIPrefabs;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUILandProvinceWindow : GUIHandler
{
	private bool _guestMode;
	private bool _isCurrentlyShown = false;


	public override void Init()
	{
		_provinceInit();
		_transportationMenuInit();
		_specialBuildingsInit();
		_buildingsInit();
	}

	
	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGuiHideProvinceDataEvent:
				_hideProvinceWindow();
				return;
			case ToGuiShowLandProvinceDataEvent e:
			{
				_transportationHandler.Visible = false;
				_showProvinceWindow();
				_currentColonizedProvinceData = e.ShowColonizedProvinceData;
				_closeAllTabs();
				_specialBuildingsTabBar.CurrentTab = 0;
				_currentTab = 0;
				_updateTemplates();
				_showTab(_currentTab);
				_setProvinceInfo(e.ShowColonizedProvinceData);
				return;
			}
			case ToGUIUpdateLandProvinceDataEvent e:
			{
				_currentColonizedProvinceData = e.UpdateColonizedProvinceData;
				_setProvinceInfo(e.UpdateColonizedProvinceData);
				return;
			}
			case ToGUIShowCountryWindowEvent:
			case ToGUIShowArmyViewerEvent:
			case ToGUIShowDiplomacyWindow:
			{
				_hideProvinceWindow();
				return;
			}
			default:
				return;
		}
	}

	public override void InputHandle(InputEvent @event)
	{
	}
	
	private void _showProvinceWindow()
	{
		if (_isCurrentlyShown) return;
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(570.0f, 0.0f), 0.4f);
		_isCurrentlyShown = true;
	}

	private void _hideProvinceWindow()
	{
		if (!_isCurrentlyShown) return;
		_buildingsMenu.Visible = false;
		_transportationHandler.Visible = false;
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", new Vector2(0.0f, 0.0f), 0.4f);
		_isCurrentlyShown = false;
	}

	#region Province Data
	
	private TextureRect _provinceSprite;
	private Label _provinceName;
	private AnimatedSprite2D _provinceFlag;
	private AnimatedSprite2D _provinceGood;
	private AnimatedSprite2D _provinceTerrain;
	private GUIResources _provinceResources;
	private Label _provinceDev;
	private Button _provinceDevButton;

	private RichTextLabel _provinceStats;
	
	private LandColonizedProvinceData _currentColonizedProvinceData;

	private void _provinceInit()
	{
		_provinceSprite = GetNode<TextureRect>("HBoxContainer4/ProvinceWindowSprite");
		_provinceSprite.MouseEntered += () => InvokeGUIEvent(new GUIHideInfoBoxEvent());
		


		_provinceName = GetNode<Label>("HBoxContainer4/ProvinceWindowSprite/ProvinceName");
		_provinceFlag = GetNode<AnimatedSprite2D>("HBoxContainer4/ProvinceWindowSprite/Flag");
		_provinceGood = GetNode<AnimatedSprite2D>("HBoxContainer4/ProvinceWindowSprite/Good");
		_provinceTerrain = GetNode<AnimatedSprite2D>("HBoxContainer4/ProvinceWindowSprite/Terrain");
		_provinceDev = GetNode<Label>("HBoxContainer4/ProvinceWindowSprite/Dev");
		_provinceDevButton = GetNode<Button>("HBoxContainer4/ProvinceWindowSprite/DevButton");
		_provinceDevButton.MouseEntered += () => _showDevInfoBox();

		_provinceStats = GetNode<RichTextLabel>("HBoxContainer4/ProvinceWindowSprite/StatsLabel");

		_provinceResources = GetNode<GUIResources>("HBoxContainer4/ProvinceWindowSprite/ResourcesContainer/Control");
		_provinceResources.Init();

	}

	private void _setProvinceInfo(LandColonizedProvinceData colonizedProvinceData)
	{
		_guestMode = colonizedProvinceData.Owner != EngineState.PlayerCountryId;


		_provinceName.Text = colonizedProvinceData.Name;
		_provinceFlag.Frame = colonizedProvinceData.Owner;
		_provinceGood.Frame = colonizedProvinceData.Good.Id;
		_provinceTerrain.Frame = colonizedProvinceData.Terrain.Id;
		_provinceResources.DrawResources(colonizedProvinceData);
		_provinceDev.Text = colonizedProvinceData.Development.ToString();

		_provinceStats.Text =
			$"Tax income: {colonizedProvinceData.TaxIncome:F1}\nManpower growth:{colonizedProvinceData.ManpowerGrowth:N0}";

		_provinceDevButton.Visible = !_guestMode;

		if (_transportationHandler.Visible)
			_showTransportationMenu();

		_setBuildingMenuInfo(colonizedProvinceData);

		_setBuildingsInfo(colonizedProvinceData);
		
		_setUnlockedRecipyInfo();

		_showTab(_currentTab);
	}

	

	private void _onExitBuildingMenuButtonPressed()
	{
		_buildingsMenu.Visible = false;
	}


	private void _onCloseProvinceMenuButtonPressed()
	{
		_hideProvinceWindow();
	}

	private void _onDestroyBuildingPressed(int id)
	{
		InvokeGUIEvent(new GUIDestroyBuildingEvent(id));
	}

	private void _onBuildBuildingOnProvincePressed()
	{
		_buildingsMenu.Visible = true;
	}

	private void _onDevButtonPressed()
	{
		InvokeGUIEvent(new GUIDevProvinceEvent(_currentColonizedProvinceData.Id));
		_setProvinceInfo(_currentColonizedProvinceData);
		_showDevInfoBox();
	}

	private void _showDevInfoBox()
	{
		InvokeGUIEvent(new GUIShowInfoBox(InfoBoxFactory.DevButtonData(_currentColonizedProvinceData)));
	}
	private void _onGoodTransportManagementPressed()
	{
		if (_guestMode)
			return;

		_waterMode = false;

		_routeAdressToTransfer = _currentColonizedProvinceData.SetRoute;
		_transportationRouteToEdit = _currentColonizedProvinceData.HarvestedTransport;
		_goodToTransfer = _currentColonizedProvinceData.Good;
		_isGoodEditable = false;
		_showTransportationMenu();
	}



	#endregion
	
	
	#region Buildings
	
	private PanelContainer _buildingsMenu;
	private GridContainer _possibleBuildingsSpawner;
	
	private Node2D _buildingsHandler;
	private PackedScene BuildingScene;
	private void _buildingsInit()
	{
		_buildingsMenu = GetNode<PanelContainer>("HBoxContainer4/BuildingMenu");
		_buildingsMenu.MouseEntered += () => InvokeGUIEvent(new GUIHideInfoBoxEvent());
		_possibleBuildingsSpawner =
			GetNode<GridContainer>("HBoxContainer4/BuildingMenu/MarginContainer/VBoxContainer/GridContainer");
		BuildingScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIBuilding.tscn");

		foreach (var building in EngineState.MapInfo.Scenario.Buildings)
		{
			var a = BuildingScene.Instantiate();
			a.GetChild<AnimatedTextureRect>(0).SetFrame(building.Id);
			a.GetChild<Label>(1).Text = building.Cost.ToString();
			a.GetChild(0).GetChild<Button>(0).Pressed +=
				() => InvokeGUIEvent(new GUIBuildBuildingEvent(building.Clone()));
			;
			a.GetChild(0).GetChild<Button>(0).MouseEntered += () => _showBuildingInfoBox(building.Id);
			a.GetChild(0).GetChild<Button>(0).MouseExited += () => InvokeGUIEvent(new GUIHideInfoBoxEvent());
			_possibleBuildingsSpawner.AddChild(a);
		}
		
		_buildingsHandler = GetNode<Node2D>("HBoxContainer4/ProvinceWindowSprite/Buildings");

		foreach (var buildingsSlot in _buildingsHandler.GetChildren())
			(buildingsSlot as Button).Pressed += _onBuildBuildingOnProvincePressed;

		var buildingSlots = _buildingsHandler.GetChildren();
		for (var i = 0; i < buildingSlots.Count; i++)
			(buildingSlots[i].GetChild(3).GetChild(0) as Button).Pressed += () => _onDestroyBuildingPressed(i);
	}
	
	private void _showBuildingInfoBox(int id)
	{
		InvokeGUIEvent(new GUIShowInfoBox(InfoBoxFactory.BuildingData(EngineState.MapInfo.Scenario.Buildings[id])));
	}
	

	private void _setBuildingMenuInfo(LandColonizedProvinceData colonizedProvinceData)
	{
		
		for (var i = 0; i < _possibleBuildingsSpawner.GetChildren().Count; i++)
		{
			var building = _possibleBuildingsSpawner.GetChild(i);
			var exists = colonizedProvinceData.Buildings.Exists(g => g.Id == i);
			building.GetChild(0).GetChild<Button>(0).Disabled = exists || !EngineState.MapInfo.Scenario.Countries[colonizedProvinceData.Owner].UnlockedBuildings.Contains(i);
			building.GetChild(0).GetChild<TextureRect>(1).Visible = !EngineState.MapInfo.Scenario
				.Countries[colonizedProvinceData.Owner].UnlockedBuildings.Contains(i);
			building.GetChild<AnimatedTextureRect>(0).SelfModulate =
				exists ? new Color(0.5f, 1.0f, 0.5f) : new Color(1.0f, 1.0f, 1.0f);
		}
	}

	private void _setBuildingsInfo(LandColonizedProvinceData colonizedProvinceData)
	{
		var g = 0;
		foreach (var building in colonizedProvinceData.Buildings)
		{
			(_buildingsHandler.GetChild(g).GetChild(0) as AnimatedSprite2D).Animation = "default";
			(_buildingsHandler.GetChild(g).GetChild(0) as AnimatedSprite2D).Frame = building.Id;
			if (building.IsFinished)
			{
				(_buildingsHandler.GetChild(g).GetChild(0) as AnimatedSprite2D).SelfModulate =
					new Color(1.0f, 1.0f, 1.0f);
				(_buildingsHandler.GetChild(g).GetChild(1) as ProgressBar).Visible = false;
			}
			else
			{
				(_buildingsHandler.GetChild(g).GetChild(0) as AnimatedSprite2D).SelfModulate =
					new Color(0.5f, 0.5f, 0.5f);

				var progressBar = _buildingsHandler.GetChild(g).GetChild(1) as ProgressBar;
				progressBar.Visible = true;
				progressBar.MaxValue = building.TimeToBuild;
				progressBar.Value = building.BuildingTime;
			}

			(_buildingsHandler.GetChild(g).GetChild(2) as Sprite2D).Visible = false;
			(_buildingsHandler.GetChild(g) as Button).Disabled = true;
			(_buildingsHandler.GetChild(g).GetChild(3) as Sprite2D).Visible = !_guestMode;
			(_buildingsHandler.GetChild(g).GetChild(3).GetChild(0) as Button).Disabled = _guestMode;
			g++;
		}

		for (var i = colonizedProvinceData.Buildings.Count; i < 10; i++)
		{
			(_buildingsHandler.GetChild(i).GetChild(0) as AnimatedSprite2D).Animation = "none";
			(_buildingsHandler.GetChild(i).GetChild(0) as AnimatedSprite2D).Frame = 0;
			(_buildingsHandler.GetChild(i).GetChild(1) as ProgressBar).Visible = false;

			(_buildingsHandler.GetChild(i).GetChild(3) as Sprite2D).Visible = false;
			(_buildingsHandler.GetChild(i).GetChild(3).GetChild(0) as Button).Disabled = true;
			if (i > colonizedProvinceData.UnlockedBuildingCount - 1)
			{
				(_buildingsHandler.GetChild(i).GetChild(0) as AnimatedSprite2D).SelfModulate =
					new Color(0.5f, 0.5f, 0.5f);
				(_buildingsHandler.GetChild(i).GetChild(2) as Sprite2D).Visible = true;
				(_buildingsHandler.GetChild(i) as Button).Disabled = true;
			}
			else
			{
				(_buildingsHandler.GetChild(i).GetChild(0) as AnimatedSprite2D).SelfModulate =
					new Color(1.0f, 1.0f, 1.0f);
				(_buildingsHandler.GetChild(i).GetChild(2) as Sprite2D).Visible = false;
				(_buildingsHandler.GetChild(i) as Button).Disabled = _guestMode;
			}
		}
	}

	#endregion


	#region Transportation Menu

	private Button _transportButton;
	private Label _transportLabel;
	private PanelContainer _transportationHandler;
	private HSlider _transportSlider;
	private Label _transportSliderLabel;
	private RouteAdressProvider _routeAdressToTransfer = null;
	private TransportationRoute _transportationRouteToEdit = null;
	private TextureRect _goodEditBox;
	private GUIGoodEditPanel _goodEditBoxPanel;
	private Good _goodToTransfer;
	private bool _isGoodEditable = false;
	private bool _waterMode = false;
	private void _transportationMenuInit()
	{
		_transportButton =
			GetNode<Button>(
				"HBoxContainer4/ProvinceWindowSprite/TransferManagementBox/MarginContainer/VBoxContainer/TransportButton");
		_transportLabel =
			GetNode<Label>(
				"HBoxContainer4/ProvinceWindowSprite/TransferManagementBox/MarginContainer/VBoxContainer/TransferToHarvest");
		_transportationHandler = GetNode<PanelContainer>("HBoxContainer4/ProvinceWindowSprite/TransferManagementBox");
		_transportSlider =
			GetNode<HSlider>(
				"HBoxContainer4/ProvinceWindowSprite/TransferManagementBox/MarginContainer/VBoxContainer/TransportSlider");
		_transportSliderLabel =
			GetNode<Label>(
				"HBoxContainer4/ProvinceWindowSprite/TransferManagementBox/MarginContainer/VBoxContainer/TransportSliderLabel");
		_goodEditBox =
			GetNode<TextureRect>(
				"HBoxContainer4/ProvinceWindowSprite/TransferManagementBox/MarginContainer/VBoxContainer/GoodRect");
		_goodEditBoxPanel = GetNode<GUIGoodEditPanel>("HBoxContainer4/ProvinceWindowSprite/GoodEditPanel");
		_goodEditBoxPanel.Init();

	}

	private void _onChangeGoodTransportationRoutePressed()
	{
		InvokeGUIEvent(new GUIChangeMapType(_waterMode
			? MapTypes.SeaTransportationSelection
			: MapTypes.TransportationSelection));
		InvokeGUIEvent(new GUIGoodTransportChange(_goodToTransfer, _transportSlider.Value, _routeAdressToTransfer,
			_receiveTransportationRoute));
	}

	private void _receiveTransportationRoute(TransportationRoute transportationRoute)
	{
		_transportationRouteToEdit = transportationRoute;
		_showTransportationMenu();
		_setProvinceInfo(_currentColonizedProvinceData);
	}

	private void _onCloseTransportMenuPressed()
	{
		_transportationHandler.Visible = false;
	}

	
	

	private void _showTransportationMenu()
	{
		_transportationHandler.Visible = true;
		_goodEditBox.GetChild<Button>(1).Disabled = !_isGoodEditable;
		if (_transportationRouteToEdit != null)
		{
			_transportSlider.Visible = true;
			_transportSliderLabel.Visible = true;
			_goodEditBox.Visible = true;
			_goodEditBox.GetChild<AnimatedTextureRect>(0).SetFrame(_transportationRouteToEdit.TransportationGood.Id);
			_transportButton.Text = "Change";
			_transportLabel.Text = "Transfering to:" +
								   EngineState.MapInfo.Scenario.Map[_transportationRouteToEdit.ProvinceIdTo].Name;
			_transportSlider.Value = _transportationRouteToEdit.Amount;
			_transportSliderLabel.Text = _transportSlider.Value.ToString("N1");
		}
		else
		{
			_transportButton.Text = "Create New";
			_transportLabel.Text = "Doesn't transfering anywhere";
			_transportSlider.Visible = false;
			_transportSlider.Value = 0;
			_transportSliderLabel.Visible = false;
			_goodEditBox.Visible = false;
		}
	}

	private void _onTransportSliderValueChanged(float value)
	{
		_transportSliderLabel.Text = value.ToString("N1");
		if(_transportationRouteToEdit != null)
			InvokeGUIEvent(new GUIGoodTransportEdit(_transportationRouteToEdit, _transportSlider.Value, _transportationRouteToEdit.TransportationGood));
	}

	private void _onChangeTransportationButtonPressed()
	{
		_goodEditBoxPanel.Visible = true;
	}

	private void _onGoodEditPanelGoodChangePressed(int goodId)
	{
		_goodEditBoxPanel.Visible = false;
		_goodEditBox.GetChild<AnimatedTextureRect>(0).SetFrame(goodId);
		InvokeGUIEvent(new GUIGoodTransportEdit(_transportationRouteToEdit, _transportSlider.Value, EngineState.MapInfo.Scenario.Goods[goodId]));
	}

	#endregion
	

	#region Special Buildings

	
	

	private int _currentTab;

	private TabBar _specialBuildingsTabBar;

	private void _specialBuildingsInit()
	{
		_tradeAndStockInit();
		_factoryInit();
		_militaryTrainingCampInit();
		_dockyardInit();
		_emptyInit();
		_notUnlockedInit();
		_specialBuildingConstructionInit();

	}
	
	private void _closeAllTabs()
	{
		_factoryHandler.Visible = false;
		_tradeAndStockHandler.Visible = false;
		_dockyardHandler.Visible = false;
		_militaryTrainingHandler.Visible = false;
		_emptyHandler.Visible = false;
		_notUnlockedHandler.Visible = false;
		_lockedBuildingHandler.Visible = false;
	}
	private void _onTabContainerTabChanged(int tabId)
	{
		_closeAllTabs();
		_currentTab = tabId;
		_showTab(tabId);
	}

	private void _showTab(int tabId)
	{
		_lockedBuildingHandler.Visible = false;
		switch (_currentColonizedProvinceData.SpecialBuildings[tabId])
		{
			case Factory factory:
				_factoryHandler.Visible = true;
				if(!factory.IsFinished)
					_specialBuildingConstruction(factory.TimeToBuild, factory.BuildingTime);
				_factoryShowData(factory);
				return;
			case Dockyard dockyard:
				_dockyardHandler.Visible = true;
				if(!dockyard.IsFinished)
					_specialBuildingConstruction(dockyard.TimeToBuild, dockyard.BuildingTime);
				_showDockyardData(dockyard);
				return;
			case MilitaryTrainingCamp militaryTrainingCamp:
				_militaryTrainingHandler.Visible = true;
				if(!militaryTrainingCamp.IsFinished)
					_specialBuildingConstruction(militaryTrainingCamp.TimeToBuild, militaryTrainingCamp.BuildingTime);
				_militaryTrainingCampShowData(militaryTrainingCamp);
				return;
			case StockAndTrade stockAndTrade:
				_tradeAndStockHandler.Visible = true;
				if(!stockAndTrade.IsFinished)
					_specialBuildingConstruction(stockAndTrade.TimeToBuild, stockAndTrade.BuildingTime);
				_tradeInStockShowData(stockAndTrade);
				return;
			default:
				if (_currentColonizedProvinceData.Development >= Settings.DevForSpecialBuilding[tabId])
					_showEmptyData();
				else
					_showNotUnlockedData();
				return;
		}
	}

	
	//todo: rewrite with event to map handler
	private void _onSpecialBuildingSelectionPressed(int id)
	{
		InvokeGUIEvent(new GUISpecialBuildingBuild(_currentColonizedProvinceData.Id, id, _currentTab));
		_closeAllTabs();
		_showTab(_currentTab);
	}

	
	#region Factory
	
	private Control _factoryHandler;

	private Button _changeButton;
	private Factory _currentlyShownFactory;
	private Button _deleteButton;
	private TextureRect _outputGood;


	private PackedScene _recipeIngredientBox;
	private GridContainer _recipeIngredientBoxSpawner;


	private PanelContainer _recipePanel;
	private PackedScene _recipeScene;
	private VBoxContainer _recipeSpawner;
	private Button _transportFactoryButton;

	private void _factoryInit()
	{
		_factoryHandler = GetNode<Control>("HBoxContainer4/ProvinceWindowSprite/GuiFactory");

		_recipePanel = _factoryHandler.GetNode<PanelContainer>("PanelContainer2");
		_recipeSpawner =
			_factoryHandler.GetNode<VBoxContainer>("PanelContainer2/MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer");
		_recipeScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIRecipe.tscn");


		var i = 0;
		foreach (var recipe in EngineState.MapInfo.Scenario.Recipes)
		{
			var a = _recipeScene.Instantiate() as GUIRecipe;
			a.SetInfo(recipe);
			var b = i;
			a.GetChild<Button>(1).Pressed += () => ChooseRecipe(b);
			_recipeSpawner.AddChild(a);
			i++;
		}

		_recipeIngredientBox = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIGoodFactoryInfo.tscn");
		_outputGood = _factoryHandler.GetNode<TextureRect>(
			"PanelContainer/MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/OutputGood");
		_recipeIngredientBoxSpawner =
			_factoryHandler.GetNode<GridContainer>(
				"PanelContainer/MarginContainer/VBoxContainer/HBoxContainer2/IngredientsContainer/GridContainer");
		_changeButton =
			_factoryHandler.GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/Button");
		_deleteButton =
			_factoryHandler.GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/Button2");
		_transportFactoryButton =
			_factoryHandler.GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/HBoxContainer2/VBoxContainer/Button3");
	}

	private void _setUnlockedRecipyInfo()
	{
		for (int i = 0; i < EngineState.MapInfo.Scenario.Recipes.Count; i++)
		{
			var recipe = _recipeSpawner.GetChild(i) as GUIRecipe;
			recipe.GetChild<Button>(1).Disabled = !EngineState.MapInfo.Scenario
				.Countries[_currentColonizedProvinceData.Owner].UnlockedRecipies.Contains(i);
			recipe.GetChild<Panel>(2).Visible =!EngineState.MapInfo.Scenario
				.Countries[_currentColonizedProvinceData.Owner].UnlockedRecipies.Contains(i);
		}
	}

	private void _factoryShowData(Factory factory)
	{
		_currentlyShownFactory = factory;
		_changeButton.Visible = !_guestMode;
		foreach (var item in _recipeIngredientBoxSpawner.GetChildren()) item.QueueFree();

		if (factory.Recipe == null)
		{
			_outputGood.GetChild<AnimatedTextureRect>(0).Texture = null;
			_deleteButton.Visible = false;
			_transportFactoryButton.Visible = false;
			return;
		}

		_deleteButton.Visible = !_guestMode;
		_transportFactoryButton.Visible = !_guestMode;
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
		InvokeGUIEvent(new GUISetRecipyInFactory(_currentColonizedProvinceData.Id, _currentTab, id));
		_factoryShowData(_currentlyShownFactory);
		_recipePanel.Visible = false;
	}

	private void _onChangeRecipeButtonPressed()
	{
		_recipePanel.Visible = true;
	}

	private void _onDeleteRecipePressed()
	{
		InvokeGUIEvent(new GUIRemoveRecipeFromFactory(_currentColonizedProvinceData.Id, _currentTab));
		_factoryShowData(_currentlyShownFactory);
	}

	private void _onCloseRecipePanel()
	{
		_recipePanel.Visible = false;
	}

	private void _onFactoryTransportButtonPressed()
	{
		if (_guestMode)
			return;

		_waterMode = false;

		var building = _currentColonizedProvinceData.SpecialBuildings[_currentTab] as Factory;
		_routeAdressToTransfer = building.SetRoute;
		_transportationRouteToEdit =
			building.TransportationRoute;
		_goodToTransfer = building.Recipe.Output;
		_isGoodEditable = false;
		_showTransportationMenu();
	}

	#endregion

	#region Stock And Trade

	private Control _tradeAndStockHandler;

	private GridContainer _transportationContainer;

	public void _tradeAndStockInit()
	{
		_tradeAndStockHandler = GetNode<Control>("HBoxContainer4/ProvinceWindowSprite/GuiStockAndTrade");
		
		
		_transportationContainer =
			_tradeAndStockHandler.GetNode<GridContainer>("PanelContainer/MarginContainer/VBoxContainer/TransportContainer");

		var i = 0;
		foreach (var child in _transportationContainer.GetChildren())
		{
			var b = i;
			child.GetChild(1).GetChild<Button>(1).Pressed += () => _onTransportStockAndTradeButtonPressed(b);
			i++;
		}
	}

	private void _tradeInStockShowData(StockAndTrade stockAndTrade)
	{
		for (var i = 0; i < stockAndTrade.TransportationRoutes.Length; i++)
		{
			var child = _transportationContainer.GetChild(i);
			if (stockAndTrade.TransportationRoutes[i] != null)
			{
				var route = stockAndTrade.TransportationRoutes[i];
				child.GetChild<Label>(0).Visible = true;
				child.GetChild<Label>(0).Text = "-" + route.Amount.ToString("N1") + "t/m";
				child.GetChild(1).GetChild<AnimatedTextureRect>(0).SetFrame(route.TransportationGood.Id);
				child.GetChild<Label>(2).Text = EngineState.MapInfo.Scenario.Map[route.ProvinceIdTo].Name;
			}
			else
			{
				child.GetChild<Label>(0).Visible = false;
				child.GetChild(1).GetChild<AnimatedTextureRect>(0).Texture = null;
				child.GetChild<Label>(2).Text = "nowhere";
			}
		}
	}

	private void _onTransportStockAndTradeButtonPressed(int id)
	{
		if (_guestMode)
			return;

		_waterMode = false;

		var building = _currentColonizedProvinceData.SpecialBuildings[_currentTab] as StockAndTrade;
		building.RouteId = id;
		_routeAdressToTransfer = building.SetRoute;
		_transportationRouteToEdit = building.TransportationRoutes[id];
		_goodToTransfer = EngineState.MapInfo.Scenario.Goods[0];
		_isGoodEditable = true;
		_showTransportationMenu();
	}

	#endregion

	//todo: add guest mode
	#region Dockyard

	private Control _dockyardHandler;
	private HBoxContainer _transportationDockyardContainer;
	private void _dockyardInit()
	{
		_dockyardHandler = GetNode<Control>("HBoxContainer4/ProvinceWindowSprite/GuiDockyard");
		_transportationDockyardContainer =
			_dockyardHandler.GetNode<HBoxContainer>("PanelContainer/MarginContainer/VBoxContainer/TransportationContatiner");

		var i = 0;
		foreach (var child in _transportationDockyardContainer.GetChildren())
		{
			var b = i;
			child.GetChild(1).GetChild<Button>(1).Pressed += () => _onTransportDockyardButtonPressed(b);
			i++;
		}
	}

	private void _onTransportDockyardButtonPressed(int id)
	{
		if (_guestMode)
			return;

		_waterMode = true;

		var building = _currentColonizedProvinceData.SpecialBuildings[_currentTab] as Dockyard;
		building.RouteId = id;
		_routeAdressToTransfer = building.SetRoute;
		_transportationRouteToEdit = building.WaterTransportationRoutes[id];
		_goodToTransfer = EngineState.MapInfo.Scenario.Goods[0];
		_isGoodEditable = true;
		_showTransportationMenu();
	}
	private void _showDockyardData(Dockyard dockyard)
	{
		for (var i = 0; i < dockyard.WaterTransportationRoutes.Length; i++)
		{
			var child = _transportationContainer.GetChild(i);
			if (dockyard.WaterTransportationRoutes[i] != null)
			{
				var route = dockyard.WaterTransportationRoutes[i];
				child.GetChild<Label>(0).Visible = true;
				child.GetChild<Label>(0).Text = "-" + route.Amount.ToString("N1") + "t/m";
				child.GetChild(1).GetChild<AnimatedTextureRect>(0).SetFrame(route.TransportationGood.Id);
				child.GetChild<Label>(2).Text = EngineState.MapInfo.Scenario.Map[route.ProvinceIdTo].Name;
			}
			else
			{
				child.GetChild<Label>(0).Visible = false;
				child.GetChild(1).GetChild<AnimatedTextureRect>(0).Texture = null;
				child.GetChild<Label>(2).Text = "nowhere";
			}
		}
	}

	#endregion

	//todo: add guest mode
	#region Military Training Camp
	
	private Control _militaryTrainingHandler;
	
	private ArmyRegimentTemplate _currentSelectedTemplate;
	private GridContainer _goodContainer;
	private MilitaryTrainingCamp _militaryTrainingCamp;
	private VBoxContainer _queueContainer;

	private PackedScene _queueScene;
	private Label _selectedUnitNameLabel;
	private VBoxContainer _templateContainer;
	private PackedScene _templateScene;
	private Label _trainingTimeLabel;

	private void _militaryTrainingCampInit()
	{
		_militaryTrainingHandler = GetNode<Control>("HBoxContainer4/ProvinceWindowSprite/GuiMilitaryTrainingCamp");
		
		_templateContainer =
			_militaryTrainingHandler.GetNode<VBoxContainer>(
				"PanelContainer/MarginContainer/HBoxContainer/VBoxContainer2/PanelContainer2/ScrollContainer/TemplateContainer");
		_queueContainer =
			_militaryTrainingHandler.GetNode<VBoxContainer>(
				"PanelContainer/MarginContainer/HBoxContainer/VBoxContainer2/PanelContainer/ScrollContainer/QueueContainer");
		_goodContainer =
			_militaryTrainingHandler.GetNode<GridContainer>("PanelContainer/MarginContainer/HBoxContainer/VBoxContainer/GoodContainer");
		_selectedUnitNameLabel =
			_militaryTrainingHandler.GetNode<Label>("PanelContainer/MarginContainer/HBoxContainer/VBoxContainer2/SelectedNameLabel");
		_trainingTimeLabel = _militaryTrainingHandler.GetNode<Label>("PanelContainer/MarginContainer/HBoxContainer/VBoxContainer/TrainingTime");


		_queueScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIQueueRegiment.tscn");
		_templateScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUISelectTemplate.tscn");
	}

	private void _militaryTrainingCampShowData(MilitaryTrainingCamp militaryTrainingCamp)
	{
		_clearQueueInfo();
		_militaryTrainingCamp = militaryTrainingCamp;

		foreach (var armyRegiment in militaryTrainingCamp.TrainingList)
		{
			var a = _queueScene.Instantiate();
			a.GetChild<Label>(0).Text = armyRegiment.Name;
			a.GetChild<ProgressBar>(1).Value = armyRegiment.TimeFromStartOfTheTraining;
			a.GetChild<ProgressBar>(1).MaxValue = armyRegiment.TrainingTime;
			_queueContainer.AddChild(a);
		}
	}

	private void _clearInfo()
	{
		_selectedUnitNameLabel.Text = "Empty";
		_currentSelectedTemplate = null;
		_trainingTimeLabel.Text = "Time: 0";
		for (int i = 0; i < 5; i++)
			_goodContainer.GetChild(i).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
		
	}

	private void _clearQueueInfo()
	{
		foreach (var child in _queueContainer.GetChildren()) child.QueueFree();
	}

	private void _clearTemplateInfo()
	{
		foreach (var child in _templateContainer.GetChildren()) child.QueueFree();
	}

	private void _updateTemplates()
	{
		_clearTemplateInfo();
		foreach (var template in EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates
					 .Where(d => d is ArmyRegimentTemplate))
		{
			var a = _templateScene.Instantiate();
			a.GetChild<Label>(0).Text = template.Name;
			a.GetChild<Button>(1).Pressed += () => _selectUnitTemplate(template.Id);
			_templateContainer.AddChild(a);
		}
	}

	private void _onTrainUnitPressed()
	{
		if (_currentSelectedTemplate != null)
		{
			InvokeGUIEvent(new GUIEnqueArmyRegiment(_currentColonizedProvinceData.Id, _currentTab, _currentSelectedTemplate.Id));
			_militaryTrainingCampShowData(_militaryTrainingCamp);
		}
			
		
	}

	private void _selectUnitTemplate(int unitTemplateId)
	{
		_clearInfo();
		_currentSelectedTemplate =
			EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates[unitTemplateId] as
				ArmyRegimentTemplate;
		_selectedUnitNameLabel.Text = _currentSelectedTemplate.Name;
		_trainingTimeLabel.Text = "Time: " + _currentSelectedTemplate.TrainingTime;
		switch (_currentSelectedTemplate)
		{
			case ArmyInfantryRegimentTemplate template:
			{
				_goodContainer.GetChild(0).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Weapon?.Id ?? -1);
				_goodContainer.GetChild(1).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Helmet?.Id ?? -1);
				_goodContainer.GetChild(2).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Armor?.Id ?? -1);
				_goodContainer.GetChild(3).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Boots?.Id ?? -1);
				_goodContainer.GetChild(4).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Additional?.Id ?? -1);
				return;
			}
			case ArmyCavalryRegimentTemplate template:
			{
				_goodContainer.GetChild(0).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Weapon?.Id ?? -1);
				_goodContainer.GetChild(1).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Horse?.Id ?? -1);
				_goodContainer.GetChild(2).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Helmet?.Id ?? -1);
				_goodContainer.GetChild(3).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Armor?.Id ?? -1);
				_goodContainer.GetChild(4).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Additional?.Id ?? -1);
				return;
			}
			case ArmyArtilleryRegimentTemplate template:
			{
				_goodContainer.GetChild(0).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Weapon?.Id ?? -1);
				_goodContainer.GetChild(1).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Boots?.Id ?? -1);
				_goodContainer.GetChild(2).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Armor?.Id ?? -1);
				_goodContainer.GetChild(3).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Wheel?.Id ?? -1);
				_goodContainer.GetChild(4).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame(template.Additional?.Id ?? -1);
				return;
			}
		}
	}

	#endregion

	#region Empty
	
	private Control _emptyHandler;
	

	private TextureRect _dockyardButton;
	private VBoxContainer _specialBuildingButtons;
	private void _emptyInit()
	{
		
		
		_emptyHandler = GetNode<Control>("HBoxContainer4/ProvinceWindowSprite/EmptySpecialBuilding");
		

		_dockyardButton =
			GetNode<TextureRect>(
				"HBoxContainer4/ProvinceWindowSprite/EmptySpecialBuilding/PanelContainer/MarginContainer/VBoxContainer/GridContainer/Dockyard");
		_specialBuildingButtons =
			GetNode<VBoxContainer>(
				"HBoxContainer4/ProvinceWindowSprite/EmptySpecialBuilding/PanelContainer/MarginContainer/VBoxContainer/GridContainer");

		_specialBuildingsTabBar = GetNode<TabBar>("HBoxContainer4/ProvinceWindowSprite/TabBar");
	}

	private void _showEmptyData()
	{
		_emptyHandler.Visible = true;
		_dockyardButton.Visible =
			_currentColonizedProvinceData.BorderderingProvinces.Any(d =>
				EngineState.MapInfo.Scenario.Map[d] is SeaProvinceData);
		_specialBuildingButtons.Visible = !_guestMode;
	}

	#endregion

	#region NotUnlocked
	
	private Control _notUnlockedHandler;

	private void _notUnlockedInit()
	{
		_notUnlockedHandler = GetNode<Control>("HBoxContainer4/ProvinceWindowSprite/NotUnlockedSpecialBuilding");
	}

	private void _showNotUnlockedData()
	{
		_notUnlockedHandler.Visible = true;
		(_notUnlockedHandler.GetChild(0) as Label).Text =
			"Needed dev to unlock: " + Settings.DevForSpecialBuilding[_currentTab];
	}
	
	#endregion

	#region Special Building Construction

	private Control _lockedBuildingHandler;
	private ProgressBar _lockedSpecialBuildingProgress;

	private void _specialBuildingConstructionInit()
	{
		_lockedBuildingHandler = GetNode<Control>("HBoxContainer4/ProvinceWindowSprite/LockedSpecialBuilding");

		_lockedSpecialBuildingProgress =
			_lockedBuildingHandler.GetNode<ProgressBar>("PanelContainer/MarginContainer/VBoxContainer/ProgressBar");
	}

	private void _specialBuildingConstruction(float maxVal, float curVal)
	{
		_lockedBuildingHandler.Visible = true;
		_lockedSpecialBuildingProgress.Value = curVal;
		_lockedSpecialBuildingProgress.MaxValue = maxVal;
	}

	#endregion

	#endregion
}
