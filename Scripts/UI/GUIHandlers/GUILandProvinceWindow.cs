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
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIPrefabs;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUILandProvinceWindow : GUIHandler
{
	private Node2D _buildingsHandler;
	private PanelContainer _buildingsMenu;

	private LandColonizedProvinceData _currentColonizedProvinceData;


	private int _currentTab;

	private TextureRect _dockyardButton;
	private GUIDockyard _dockyardHandler;

	private Control _emptyHandler;
	//end.

	private GUIFactory _factoryHandler;
	private TextureRect _goodEditBox;
	private GUIGoodEditPanel _goodEditBoxPanel;
	private Good _goodToTransfer;
	private bool _guestMode;


	private bool _isCurrentlyShown;
	private bool _isGoodEditable;
	private GUIMilitaryTrainingCamp _militaryTrainingHandler;
	private Control _notUnlockedHandler;
	private GridContainer _possibleBuildingsSpawner;
	private Label _provinceDev;
	private Button _provinceDevButton;
	private AnimatedSprite2D _provinceFlag;
	private AnimatedSprite2D _provinceGood;

	private Label _provinceName;
	private GUIResources _provinceResources;

	private TextureRect _provinceSprite;

	private RichTextLabel _provinceStats;
	private AnimatedSprite2D _provinceTerrain;
	private RouteAdressProvider _routeAdressToTransfer;
	private VBoxContainer _specialBuildingButtons;

	private TabBar _specialBuildingsTabBar;
	private GUIStockAndTrade _tradeAndStockHandler;
	private PanelContainer _transportationHandler;
	private TransportationRoute _transportationRouteToEdit;

	//transportation zone
	private Button _transportButton;
	private Label _transportLabel;
	private HSlider _transportSlider;
	private Label _transportSliderLabel;
	private bool _waterMode;

	private PackedScene BuildingScene;


	public override void Init()
	{
		_provinceSprite = GetNode<TextureRect>("HBoxContainer4/ProvinceWindowSprite");
		_provinceSprite.MouseEntered += () => InvokeGUIEvent(new GUIHideInfoBoxEvent());
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


		_factoryHandler = GetNode<GUIFactory>("HBoxContainer4/ProvinceWindowSprite/GuiFactory");
		_factoryHandler.Init();

		_militaryTrainingHandler =
			GetNode<GUIMilitaryTrainingCamp>("HBoxContainer4/ProvinceWindowSprite/GuiMilitaryTrainingCamp");
		_militaryTrainingHandler.Init();

		_dockyardHandler = GetNode<GUIDockyard>("HBoxContainer4/ProvinceWindowSprite/GuiDockyard");
		_dockyardHandler.Init();

		_tradeAndStockHandler = GetNode<GUIStockAndTrade>("HBoxContainer4/ProvinceWindowSprite/GuiStockAndTrade");
		_tradeAndStockHandler.Init();

		_emptyHandler = GetNode<Control>("HBoxContainer4/ProvinceWindowSprite/EmptySpecialBuilding");
		_notUnlockedHandler = GetNode<Control>("HBoxContainer4/ProvinceWindowSprite/NotUnlockedSpecialBuilding");

		_dockyardButton =
			GetNode<TextureRect>(
				"HBoxContainer4/ProvinceWindowSprite/EmptySpecialBuilding/PanelContainer/MarginContainer/VBoxContainer/GridContainer/Dockyard");
		_specialBuildingButtons =
			GetNode<VBoxContainer>(
				"HBoxContainer4/ProvinceWindowSprite/EmptySpecialBuilding/PanelContainer/MarginContainer/VBoxContainer/GridContainer");

		_specialBuildingsTabBar = GetNode<TabBar>("HBoxContainer4/ProvinceWindowSprite/TabBar");

		_buildingsHandler = GetNode<Node2D>("HBoxContainer4/ProvinceWindowSprite/Buildings");

		foreach (var buildingsSlot in _buildingsHandler.GetChildren())
			(buildingsSlot as Button).Pressed += _onBuildBuildingOnProvincePressed;

		var buildingSlots = _buildingsHandler.GetChildren();
		for (var i = 0; i < buildingSlots.Count; i++)
			(buildingSlots[i].GetChild(3).GetChild(0) as Button).Pressed += () => _onDestroyBuildingPressed(i);
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
				_militaryTrainingHandler.UpdateTemplates();
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

		_showTab(_currentTab);
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
		var requirments = Settings.ResourceAndCostRequirmentsToDev(_currentColonizedProvinceData.Development);
		if (EngineState.MapInfo.Scenario.Countries[_currentColonizedProvinceData.Owner].Money - requirments.Key >= 0f &&
			Good.CheckIfMeetsRequirements(_currentColonizedProvinceData.Resources, requirments.Value))
		{
			EngineState.MapInfo.Scenario.Countries[_currentColonizedProvinceData.Owner].Money -= requirments.Key;
			_currentColonizedProvinceData.Resources =
				Good.DecreaseGoodsByGoods(_currentColonizedProvinceData.Resources, requirments.Value);
			_currentColonizedProvinceData.Development++;
			_setProvinceInfo(_currentColonizedProvinceData);
			_showDevInfoBox();
		}
	}

	private void _showDevInfoBox()
	{
		InvokeGUIEvent(new GUIShowInfoBox(InfoBoxFactory.DevButtonData(_currentColonizedProvinceData)));
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
			building.GetChild(0).GetChild<Button>(0).Disabled = exists;
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

	private void _onTabContainerTabChanged(int tabId)
	{
		_closeAllTabs();
		_currentTab = tabId;
		_showTab(tabId);
	}

	private void _showTab(int tabId)
	{
		switch (_currentColonizedProvinceData.SpecialBuildings[tabId])
		{
			case Factory factory:
				_factoryHandler.Visible = true;
				_factoryHandler.ShowData(factory, _guestMode);
				return;
			case Dockyard dockyard:
				_dockyardHandler.Visible = true;
				_dockyardHandler.ShowData(dockyard, _guestMode);
				return;
			case MilitaryTrainingCamp militaryTrainingCamp:
				_militaryTrainingHandler.Visible = true;
				_militaryTrainingHandler.ShowInfo(militaryTrainingCamp);
				return;
			case StockAndTrade stockAndTrade:
				_tradeAndStockHandler.Visible = true;
				_tradeAndStockHandler.ShowData(stockAndTrade, _guestMode);
				return;
			default:
				if (_currentColonizedProvinceData.Development >= Settings.DevForSpecialBuilding[tabId])
				{
					_emptyHandler.Visible = true;
					_dockyardButton.Visible =
						_currentColonizedProvinceData.BorderderingProvinces.Any(d =>
							EngineState.MapInfo.Scenario.Map[d] is SeaProvinceData);
					_specialBuildingButtons.Visible = !_guestMode;
				}
				else
				{
					_notUnlockedHandler.Visible = true;
					(_notUnlockedHandler.GetChild(0) as Label).Text =
						"Needed dev to unlock: " + Settings.DevForSpecialBuilding[tabId];
				}

				return;
		}
	}

	private void _onSpecialBuildingSelectionPressed(int id)
	{
		switch (id)
		{
			case 0:
				_currentColonizedProvinceData.SpecialBuildings[_currentTab] =
					new StockAndTrade(0, false, 100, StockAndTrade.DefaultRoutes());
				break;
			case 1:
				_currentColonizedProvinceData.SpecialBuildings[_currentTab] =
					new Factory(null, 0, false, 0.1f, 100, null);
				break;
			case 2:
				_currentColonizedProvinceData.SpecialBuildings[_currentTab] =
					new Dockyard(0, false, 100, Dockyard.DefaultWaterTransportationRoutes());
				break;
			case 3:
				_currentColonizedProvinceData.SpecialBuildings[_currentTab] =
					new MilitaryTrainingCamp(0, false, 100, new Queue<ArmyRegiment>());
				break;
			default:
				return;
		}

		_closeAllTabs();
		_showTab(_currentTab);
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

	private void _onGuiFactoryTrasportationRouteMenuPressed()
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

	private void _onGuiStockAndTradeTrasportationRouteMenuPressed(int id)
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

	private void _onGuiDockyardTrasportationRouteMenuPressed(int id)
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
		if (_transportationRouteToEdit != null)
			_transportationRouteToEdit.Amount = _transportSlider.Value;
	}

	private void _onChangeTransportationButtonPressed()
	{
		_goodEditBoxPanel.Visible = true;
	}

	private void _onGoodEditPanelGoodChangePressed(int goodId)
	{
		_goodEditBoxPanel.Visible = false;
		_goodEditBox.GetChild<AnimatedTextureRect>(0).SetFrame(goodId);
		_transportationRouteToEdit.TransportationGood = EngineState.MapInfo.Scenario.Goods[goodId];
	}


	private void _closeAllTabs()
	{
		_factoryHandler.Visible = false;
		_tradeAndStockHandler.Visible = false;
		_dockyardHandler.Visible = false;
		_militaryTrainingHandler.Visible = false;
		_emptyHandler.Visible = false;
		_notUnlockedHandler.Visible = false;
	}
}
