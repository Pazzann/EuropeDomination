using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUILandProvinceWindow : GUIHandler
{
	private bool _guestMode;
	private Sprite2D _buildingsMenu;
	private Control _possibleBuildings;

	private Label _provinceName;
	private AnimatedSprite2D _provinceFlag;
	private AnimatedSprite2D _provinceGood;
	private AnimatedSprite2D _provinceTerrain;
	private GUIResources _provinceResources;
	private Label _provinceDev;
	

	private Node2D _buildingsHandler;

	//transportation zone
	private Button _transportButton;
	private Label _transportLabel;
	private PanelContainer _transportationHandler;
	private HSlider _transportSlider;
	private Label _transportSliderLabel;
	private RouteAdressProvider _routeAdressToTransfer = null;
	private TransportationRoute _transportationRouteToEdit = null;
	private TextureRect _goodEditBox;
	private GUIGoodEditPanel _goodEditBoxPanel;
	//end.

	private GUIFactory _factoryHandler;
	private GUIStockAndTrade _tradeAndStockHandler;
	private GUIDockyard _dockyardHandler;
	private GUIMilitaryTrainingCamp _militaryTrainingHandler;
	private Control _emptyHandler;
	private Control _notUnlockedHandler;
	

	private int _currentTab;

	private TabBar _specialBuildingsTabBar;

	private LandProvinceData _currentProvinceData;


	private bool _isCurrentlyShown = false;

	
	public override void Init()
	{
		_buildingsMenu = GetNode<Sprite2D>("HBoxContainer4/BuildingMenu");
		_possibleBuildings = GetNode<Control>("HBoxContainer4/BuildingMenu/PossibleBuildings");

		_provinceName = GetNode<Label>("HBoxContainer4/ProvinceWindowSprite/ProvinceName");
		_provinceFlag = GetNode<AnimatedSprite2D>("HBoxContainer4/ProvinceWindowSprite/Flag");
		_provinceGood = GetNode<AnimatedSprite2D>("HBoxContainer4/ProvinceWindowSprite/Good");
		_provinceTerrain = GetNode<AnimatedSprite2D>("HBoxContainer4/ProvinceWindowSprite/Terrain");
		_provinceDev = GetNode<Label>("HBoxContainer4/ProvinceWindowSprite/Dev");
		
		_provinceResources = GetNode<GUIResources>("HBoxContainer4/ProvinceWindowSprite/ResourcesContainer/Control");
		_provinceResources.Init();

		_transportButton =
			GetNode<Button>(
				"HBoxContainer4/ProvinceWindowSprite/TransferManagementBox/MarginContainer/VBoxContainer/TransportButton");
		_transportLabel = GetNode<Label>("HBoxContainer4/ProvinceWindowSprite/TransferManagementBox/MarginContainer/VBoxContainer/TransferToHarvest");
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
		_militaryTrainingHandler = GetNode<GUIMilitaryTrainingCamp>("HBoxContainer4/ProvinceWindowSprite/GuiMilitaryTrainingCamp");
		_dockyardHandler = GetNode<GUIDockyard>("HBoxContainer4/ProvinceWindowSprite/GuiDockyard");
		_tradeAndStockHandler = GetNode<GUIStockAndTrade>("HBoxContainer4/ProvinceWindowSprite/GuiStockAndTrade");
		_emptyHandler = GetNode<Control>("HBoxContainer4/ProvinceWindowSprite/EmptySpecialBuilding");
		_notUnlockedHandler = GetNode<Control>("HBoxContainer4/ProvinceWindowSprite/NotUnlockedSpecialBuilding");

		_specialBuildingsTabBar = GetNode<TabBar>("HBoxContainer4/ProvinceWindowSprite/TabBar");

		_buildingsHandler = GetNode<Node2D>("HBoxContainer4/ProvinceWindowSprite/Buildings");

		foreach (var buildingsSlot in _buildingsHandler.GetChildren())
		{
			(buildingsSlot as Button).Pressed += _onBuildBuildingOnProvincePressed;
		}

		var buildingSlots = _buildingsHandler.GetChildren();
		for (var i = 0; i < buildingSlots.Count; i++)
		{
			(buildingSlots[i].GetChild(3).GetChild(0) as Button).Pressed += () => _onDestroyBuildingPressed(i);
		}

		_possibleBuildings.GetNode<Button>("./Workshop/BuildButton").Pressed += () => _onBuildBuildingOnMenuPressed(new Workshop());
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGuiHideProvinceDataEvent:
				_hideProvinceWindow();
				return;
			case ToGuiShowLandProvinceDataEvent e:
				_transportationHandler.Visible = false;
				_showProvinceWindow();
				_currentProvinceData = e.ShowProvinceData;
				_closeAllTabs();
				_specialBuildingsTabBar.CurrentTab = 0;
				_currentTab = 0;
				_showTab(_currentTab);
				_setProvinceInfo(e.ShowProvinceData);
				return;
			case ToGUIUpdateLandProvinceDataEvent e:
				_currentProvinceData = e.UpdateProvinceData;
				_setProvinceInfo(e.UpdateProvinceData);
				return;
			default:
				return;
		}
	}

	public override void InputHandle(InputEvent @event)
	{
		
	}

	private void _setProvinceInfo(LandProvinceData provinceData)
	{
		_guestMode = provinceData.Owner != EngineState.PlayerCountryId;
		
		
		_provinceName.Text = provinceData.Name;
		_provinceFlag.Frame = provinceData.Owner;
		_provinceGood.Frame = (int)provinceData.Good;
		_provinceTerrain.Frame = (int)provinceData.Terrain;
		_provinceResources.DrawResources(provinceData);
		_provinceDev.Text = provinceData.Development.ToString();
		
		if(_transportationHandler.Visible)
			_showTransportationMenu();
		
		_setBuildingMenuInfo(provinceData);
		
		_setBuildingsInfo(provinceData);
	}
	
	private void _showProvinceWindow()
	{
		if(_isCurrentlyShown) return;
		Tween tween = GetTree().CreateTween(); 
		tween.TweenProperty(this, "position",new Vector2(570.0f, 0.0f) , 0.4f);
		_isCurrentlyShown = true;
	}

	private void _hideProvinceWindow()
	{
		if(!_isCurrentlyShown) return;
		_buildingsMenu.Visible = false;
		_transportationHandler.Visible = false;
		Tween tween = GetTree().CreateTween(); 
		tween.TweenProperty(this, "position",new Vector2(0.0f, 0.0f) , 0.4f);
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

	private void _setBuildingMenuInfo(LandProvinceData provinceData)
	{
		//TODO: REWRITE
		
		
		var workshopSprite = _possibleBuildings.GetChild(0) as AnimatedSprite2D;
		(workshopSprite.GetChild(0) as Label).Text = Building.Buildings[1].Cost.ToString();
		if (provinceData.Buildings.Exists(i => i.ID == 1))
		{
			workshopSprite.SelfModulate = new Color(0.5f, 1.0f, 0.5f);
			(workshopSprite.GetChild(1) as Button).Disabled = true;
		}
		else
		{
			workshopSprite.SelfModulate = new Color(1.0f, 1.0f, 1.0f);
			(workshopSprite.GetChild(1) as Button).Disabled = false;
		}
	}

	private void _setBuildingsInfo(LandProvinceData provinceData)
	{
		int g = 0;
		foreach (var building in provinceData.Buildings)
		{
			if (building.IsFinished)
			{
				(_buildingsHandler.GetChild(g).GetChild(0) as AnimatedSprite2D).SelfModulate = new Color(1.0f, 1.0f, 1.0f);
				(_buildingsHandler.GetChild(g).GetChild(0) as AnimatedSprite2D).Frame = building.ID;
				(_buildingsHandler.GetChild(g).GetChild(1) as ProgressBar).Visible = false;
			}
			else
			{
				(_buildingsHandler.GetChild(g).GetChild(0) as AnimatedSprite2D).SelfModulate = new Color(0.5f, 0.5f, 0.5f);
				(_buildingsHandler.GetChild(g).GetChild(0) as AnimatedSprite2D).Frame = building.ID;
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

		for (var i = provinceData.Buildings.Count; i < 10; i++)
		{
			
			(_buildingsHandler.GetChild(i).GetChild(0) as AnimatedSprite2D).Frame = 0;
			(_buildingsHandler.GetChild(i).GetChild(1) as ProgressBar).Visible = false;
			
			(_buildingsHandler.GetChild(i).GetChild(3) as Sprite2D).Visible = false;
			(_buildingsHandler.GetChild(i).GetChild(3).GetChild(0) as Button).Disabled = true;
			if (i > provinceData.UnlockedBuildingCount - 1)
			{
				(_buildingsHandler.GetChild(i).GetChild(0) as AnimatedSprite2D).SelfModulate = new Color(0.5f, 0.5f, 0.5f);
				(_buildingsHandler.GetChild(i).GetChild(2) as Sprite2D).Visible = true;
				(_buildingsHandler.GetChild(i) as Button).Disabled = true;
			}
			else
			{
				(_buildingsHandler.GetChild(i).GetChild(0) as AnimatedSprite2D).SelfModulate = new Color(1.0f, 1.0f, 1.0f);
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
		switch (_currentProvinceData.SpecialBuildings[tabId])
		{
			case Factory:
				_factoryHandler.Visible = true;
				return;
			case Dockyard:
				_dockyardHandler.Visible = true;
				return;
			case MilitaryTrainingCamp:
				_militaryTrainingHandler.Visible = true;
				return;
			case StockAndTrade:
				_tradeAndStockHandler.Visible = true;
				return;
			default:
				if (_currentProvinceData.Development >= 10 + 10 * tabId) 
					_emptyHandler.Visible = true;

				else
				{
					_notUnlockedHandler.Visible = true;
					(_notUnlockedHandler.GetChild(0) as Label).Text = "Needed dev to unlock: " + (10 + 10 * tabId).ToString();
				}
				return;
		}
	}

	private void _onSpecialBuildingSelectionPressed(int id)
	{
		switch (id)
		{
			case 0:
				_currentProvinceData.SpecialBuildings[_currentTab] = new StockAndTrade(0, false);
				break;
			case 1:
				_currentProvinceData.SpecialBuildings[_currentTab] = new Factory(null,0, false, 0.2f);
				break;
			case 2:
				_currentProvinceData.SpecialBuildings[_currentTab] = new Dockyard(0, false);
				break;
			case 3:
				_currentProvinceData.SpecialBuildings[_currentTab] = new MilitaryTrainingCamp(0, false);
				break;
			default:
				return;
		}
		_closeAllTabs();
		_showTab(_currentTab);
	}

	private void _onChangeGoodTransportationRoutePressed()
	{
		InvokeGUIEvent(new GUIChangeMapType(MapTypes.TransportationSelection));
		InvokeGUIEvent(new GUIGoodTransportChange(_currentProvinceData.Good, _transportSlider.Value, _routeAdressToTransfer, _receiveTransportationRoute));
	}

	private void _receiveTransportationRoute(TransportationRoute transportationRoute)
	{
		_transportationRouteToEdit = transportationRoute;
		_showTransportationMenu();
	}

	private void _onCloseTransportMenuPressed()
	{
		_transportationHandler.Visible = false;
	}

	private void _onGoodTransportManagementPressed()
	{
		_routeAdressToTransfer = _currentProvinceData.SetRoute;
		_transportationRouteToEdit = _currentProvinceData.HarvestedTransport;
		_showTransportationMenu();
	}

	private void _showTransportationMenu(bool isGoodEditable = false)
	{
		_transportationHandler.Visible = true;
		_goodEditBox.GetChild<Button>(1).Disabled = !isGoodEditable;
		if (_transportationRouteToEdit != null)
		{
			_transportSlider.Visible = true;
			_transportSliderLabel.Visible = true;
			_goodEditBox.Visible = true;
			_goodEditBox.GetChild<AnimatedTextureRect>(0).SetFrame((int)_transportationRouteToEdit.TransportationGood);
			_transportButton.Text = "Change";
			_transportLabel.Text = "Transfering to:" + EngineState.MapInfo.Scenario.Map[_transportationRouteToEdit.ProvinceIdTo].Name;
			_transportSlider.Value = _transportationRouteToEdit.Amount;
			_transportSliderLabel.Text = _transportSlider.Value.ToString("N1"); 
			
		}
		else
		{
			_transportButton.Text = "Create New";
			_transportLabel.Text = "Doesn't transfering anywhere";
			_transportSlider.Visible = false;
			_transportSliderLabel.Visible = false;
			_goodEditBox.Visible = false;
		}
	}
	
	private void _onTransportSliderValueChanged(float value)
	{
		_transportSliderLabel.Text = value.ToString("N1");
		if(_transportationRouteToEdit != null)
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
		_transportationRouteToEdit.TransportationGood = (Good)goodId;
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

	
	private void _onBuildBuildingOnMenuPressed(Building building)
	{
		InvokeGUIEvent(new GUIBuildBuildingEvent(building));
	}
	

}
