using System.Linq;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
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

	private Control _provinceTypeSelection;
	private AnimatedSprite2D _provinceTypeSprite;
	private Control _provinceTypeSelectionMenu;

	private Node2D _buildingsHandler;

	private ProvinceData _currentProvinceData;


	private bool _isCurrentlyShown = false;

	
	public override void Init()
	{
		_buildingsMenu = GetNode<Sprite2D>("HBoxContainer4/BuildingMenu");
		_possibleBuildings = GetNode<Control>("HBoxContainer4/BuildingMenu/PossibleBuildings");

		_provinceName = GetNode<Label>("HBoxContainer4/ProvinceWindowSprite/ProvinceName");
		_provinceFlag = GetNode<AnimatedSprite2D>("HBoxContainer4/ProvinceWindowSprite/Flag");
		_provinceGood = GetNode<AnimatedSprite2D>("HBoxContainer4/ProvinceWindowSprite/Good");
		_provinceTerrain = GetNode<AnimatedSprite2D>("HBoxContainer4/ProvinceWindowSprite/Terrain");
		_provinceResources = GetNode<GUIResources>("HBoxContainer4/ProvinceWindowSprite/ResourcesContainer/Control");

		_provinceTypeSelection = GetNode<Control>("HBoxContainer4/ProvinceWindowSprite/EmptySpecialBuilding/ProvinceTypeSelection");
		_provinceTypeSprite = _provinceTypeSelection.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_provinceTypeSelectionMenu = _provinceTypeSelection.GetNode<Control>("ScrollContainer");

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
				_showProvinceWindow();
				_setProvinceInfo(e.ShowProvinceData);
				return;
			case ToGUIUpdateLandProvinceDataEvent e:
				//_buildingsMenu.Visible = false;
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
		
		_currentProvinceData = provinceData;
		
		_provinceName.Text = provinceData.Name;
		_provinceFlag.Frame = provinceData.Owner;
		_provinceGood.Frame = (int)provinceData.Good;
		_provinceTerrain.Frame = (int)provinceData.Terrain;
		_provinceResources.DrawResources(provinceData);
		
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
	
	private void _onBuildBuildingOnMenuPressed(Building building)
	{
		InvokeGUIEvent(new GUIBuildBuildingEvent(building));
	}

	private void _onProvinceTypeSelection()
	{
		_provinceTypeSelectionMenu.Visible = !_provinceTypeSelectionMenu.Visible;
	}

}
