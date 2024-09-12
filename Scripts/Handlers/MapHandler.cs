using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using EuropeDominationDemo.Scripts.Text;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToEngine;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;
using ToGuiHideProvinceDataEvent = EuropeDominationDemo.Scripts.UI.Events.ToGUI.ToGuiHideProvinceDataEvent;

namespace EuropeDominationDemo.Scripts.Handlers;

public partial class MapHandler : GameHandler
{
    private PackedScene _capitalScene;
    private Node2D _capitalSpawner;
    private Node2D _countryTextSpawner;

    private PackedScene _devScene;
    private Node2D _devSpawner;
    private Dictionary<int, AnimatedSprite2D> _devStorage = new ();
    
    private ShaderMaterial _fogMaterial;
    private Sprite2D _fogSprite;

    private PackedScene _goodsScene;
    private Node2D _goodsSpawner;

    private Good _goodToTransport;

    private float _lastClickTimestamp;
    private ShaderMaterial _mapMaterial;
    private Sprite2D _mapSprite;
    private Node2D _provinceTextSpawner;


    private PackedScene _textScene;

    private PackedScene _transportArrowScene;
    private Node2D _transportArrowSpawner;
    private double _transportationAmount;
    private NewTransportationRouteReciever _uiReciever;
    private RouteAdressProvider _whereToAddRoute;


    public override void Init()
    {
        _mapSprite = GetNode<Sprite2D>("./Map");
        _mapMaterial = _mapSprite.Material as ShaderMaterial;
        _fogSprite = GetNode<Sprite2D>("Map3");
        _fogMaterial = _fogSprite.Material as ShaderMaterial;
        _fogMaterial.SetShaderParameter("timescale", 0.01f);


        _textScene = (PackedScene)GD.Load("res://Prefabs/Text.tscn");
        _countryTextSpawner = GetNode<Node2D>("./CountryTextHandler");
        _provinceTextSpawner = GetNode<Node2D>("./ProvinceTextHandler");

        _goodsScene = (PackedScene)GD.Load("res://Prefabs/Good.tscn");
        _goodsSpawner = GetNode<Node2D>("./GoodsHandler");

        _devScene = (PackedScene)GD.Load("res://Prefabs/Development.tscn");
        _devSpawner = GetNode<Node2D>("./DevHandler");

        _capitalScene = GD.Load<PackedScene>("res://Prefabs/GUI/Capital.tscn");
        _capitalSpawner = GetNode<Node2D>("./CapitalHandler");

        _transportArrowScene = (PackedScene)GD.Load("res://Prefabs/TransportArrow.tscn");
        _transportArrowSpawner = GetNode<Node2D>("TransportArrowHandler");


        _mapMaterial.SetShaderParameter("colors", EngineState.MapInfo.MapColors);
        _mapMaterial.SetShaderParameter("selectedID", -1);

        _updateCountryText();
        _updateProvinceText();
        _addGoods();
        _addCapitals();
        _addDev();
        _setFogOfWar();

        _goodsSpawner.Visible = false;
        _devSpawner.Visible = false;
        _transportArrowSpawner.Visible = false;
    }

    public override bool InputHandle(InputEvent @event, int tileId)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Right, Pressed: false })
            if (EngineState.MapInfo.Scenario.Map[tileId] is LandColonizedProvinceData countryProvince)
            {
                if (countryProvince.Owner == EngineState.PlayerCountryId)
                    InvokeToGUIEvent(new ToGUIShowCountryWindowEvent());
                else
                    InvokeToGUIEvent(
                        new ToGUIShowDiplomacyWindow(EngineState.MapInfo.Scenario.Countries[countryProvince.Owner]));
            }

        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
        {
            _lastClickTimestamp = Time.GetTicksMsec() / 1000f;
            return false;
        }

        if (@event is not InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false }) return false;
        if (Time.GetTicksMsec() / 1000f - _lastClickTimestamp > 0.2f) return false;


        if ((EngineState.MapInfo.CurrentMapMode == MapTypes.SeaTransportationSelection ||
             EngineState.MapInfo.CurrentMapMode == MapTypes.TransportationSelection) &&
            EngineState.MapInfo.MapColors[tileId] != MapDefaultColors.Unselectable)
        {
            if (EngineState.MapInfo.MapColors[tileId] == MapDefaultColors.Selectable)
            {
                TransportationRoute route;
                if (EngineState.MapInfo.CurrentMapMode == MapTypes.TransportationSelection)
                    route = new TransportationRoute(tileId, EngineState.MapInfo.CurrentSelectedProvinceId,
                        _goodToTransport, _transportationAmount);
                else
                    route = new WaterTransportationRoute(tileId, EngineState.MapInfo.CurrentSelectedProvinceId,
                        _goodToTransport, _transportationAmount);
                _whereToAddRoute.Invoke(route);
                _uiReciever.Invoke(route);
            }
            else if (EngineState.MapInfo.MapColors[tileId] == MapDefaultColors.OwnProvince)
            {
                _whereToAddRoute = null;
            }

            EngineState.MapInfo.CurrentMapMode = MapTypes.Political;
            InvokeToEngineEvent(new ToEngineViewModUpdate());
            InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(
                EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId] as
                    LandColonizedProvinceData));
            return true;
        }

        if (tileId == EngineState.MapInfo.CurrentSelectedProvinceId || tileId < 0)
        {
            DeselectProvince();
            EngineState.MapInfo.CurrentSelectedProvinceId = -2;
            return false;
        }

        EngineState.MapInfo.CurrentSelectedProvinceId = tileId;
        if (EngineState.MapInfo.Scenario.Map[tileId] is WastelandProvinceData)
            return false;
        _mapMaterial.SetShaderParameter("selectedID", tileId);
        if (EngineState.MapInfo.Scenario.Map[tileId] is LandColonizedProvinceData data)
            InvokeToGUIEvent(new ToGuiShowLandProvinceDataEvent(data));
        if (EngineState.MapInfo.Scenario.Map[tileId] is UncolonizedProvinceData uncolonizedProvinceData)
        {
            bool isPossibleToColonize = !(uncolonizedProvinceData.CurrentlyColonizedByCountry != null);
            
            //Some Crazy condition
            if (!EngineState.MapInfo.MapProvinces(ProvinceTypes.CurrentCountryProvincesAndBordering).Contains(uncolonizedProvinceData))
                if (!EngineState.MapInfo.MapProvinces(ProvinceTypes.CurrentCountryProvinces)
                        .Where(d => (d as LandColonizedProvinceData).SpecialBuildings.Any(b => b is Dockyard))
                        .Any(d =>
                        {
                            var searchPr = EngineState.MapInfo.MapProvinces(ProvinceTypes.SeaProvinces).ToList();
                            searchPr.Add(d);
                            searchPr.Add(uncolonizedProvinceData);
                            return PathFinder.CheckConnectionFromAToB(d.Id, uncolonizedProvinceData.Id,
                                       searchPr.ToArray()) &&
                                   PathFinder.FindPathFromAToB(d.Id, uncolonizedProvinceData.Id, searchPr.ToArray()
                                   ).Length < 6;
                        }))
                    isPossibleToColonize = false;
            InvokeToGUIEvent(new ToGUIShowUncolonizedProvinceData(uncolonizedProvinceData, isPossibleToColonize));

        }
        return false;
    }

    private void _setFogOfWar()
    {
        _fogMaterial.SetShaderParameter("colors", EngineState.MapInfo.VisionZone);
    }

    public override void ViewModUpdate(float zoom)
    {
        switch (EngineState.MapInfo.CurrentMapMode)
        {
            case MapTypes.Political:
            {
                _goodsSpawner.Visible = false;

                if (zoom < 3.0f)
                {
                    _fogSprite.Visible = false;
                    _countryTextSpawner.Visible = true;
                    _provinceTextSpawner.Visible = false;
                    _devSpawner.Visible = false;
                    _transportArrowSpawner.Visible = false;
                }
                else
                {
                    _fogSprite.Visible = true;
                    _countryTextSpawner.Visible = false;
                    _provinceTextSpawner.Visible = true;
                    _devSpawner.Visible = true;
                    _transportArrowSpawner.Visible = false;

                    _addTransportArrows();
                }

                break;
            }
            case MapTypes.Goods:
            {
                _fogSprite.Visible = false;
                _goodsSpawner.Visible = true;
                _devSpawner.Visible = false;
                _countryTextSpawner.Visible = false;
                _provinceTextSpawner.Visible = false;
                _transportArrowSpawner.Visible = false;
                break;
            }
            case MapTypes.Terrain:
            {
                _fogSprite.Visible = false;
                _goodsSpawner.Visible = false;
                _countryTextSpawner.Visible = false;
                _provinceTextSpawner.Visible = false;
                _devSpawner.Visible = false;
                _transportArrowSpawner.Visible = false;
                break;
            }
            case MapTypes.Trade: //TODO: Finish
                _fogSprite.Visible = false;
                _goodsSpawner.Visible = false;
                _countryTextSpawner.Visible = false;
                _provinceTextSpawner.Visible = false;
                _devSpawner.Visible = false;
                _transportArrowSpawner.Visible = true;
                break;
            case MapTypes.Development:
                break;
            case MapTypes.Factories:
                break;
            case MapTypes.TransportationSelection:
                _fogSprite.Visible = false;
                _goodsSpawner.Visible = false;
                _countryTextSpawner.Visible = false;
                _provinceTextSpawner.Visible = false;
                _devSpawner.Visible = false;
                _transportArrowSpawner.Visible = false;
                break;
            case MapTypes.SeaTransportationSelection:
                _fogSprite.Visible = false;
                _goodsSpawner.Visible = false;
                _countryTextSpawner.Visible = false;
                _provinceTextSpawner.Visible = false;
                _devSpawner.Visible = false;
                _transportArrowSpawner.Visible = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _mapMaterial.SetShaderParameter("colors", EngineState.MapInfo.MapColors);
        _mapMaterial.SetShaderParameter("viewMod", zoom < 3.0f ? 1 : 0);
    }

    public override void GUIInteractionHandler(GUIEvent @event)
    {
        if (EngineState.MapInfo.CurrentSelectedProvinceId < 0)
            return;
        if (EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId] is UncolonizedProvinceData
            data2)
        {
            switch (@event)
            {
                
                case GUIColonizeProvince e:
                    var country = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId];
                    if (country.Money > Settings.InitialMoneyCostColony &&
                        country.Manpower >= Settings.InitialManpowerCostColony)
                    {
                        country.Money -= Settings.InitialMoneyCostColony;
                        country.Manpower -= Settings.InitialManpowerCostColony;
                        (EngineState.MapInfo.Scenario.Map[e.ProvinceId] as UncolonizedProvinceData).CurrentlyColonizedByCountry =
                            EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId];   
                        InvokeToGUIEvent(new ToGUIShowUncolonizedProvinceData((EngineState.MapInfo.Scenario.Map[e.ProvinceId] as UncolonizedProvinceData), false));
                        InvokeToGUIEvent(new ToGUIUpdateCountryInfo());
                    }
                    return;
            }
        }
        if (EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId] is LandColonizedProvinceData
            data)
            switch (@event)
            {
                case GUIBuildBuildingEvent e:
                    var province =
                        (LandColonizedProvinceData)EngineState.MapInfo.Scenario.Map[
                            EngineState.MapInfo.CurrentSelectedProvinceId];
                    if (EngineState.PlayerCountryId == province.Owner &&
                        EngineState.MapInfo.Scenario.Countries[province.Owner].Money - e.NewBuilding.Cost >= 0 &&
                        Good.CheckIfMeetsRequirements(province.Resources, e.NewBuilding.ResourceCost))
                    {
                        EngineState.MapInfo.Scenario.Countries[province.Owner].Money -= e.NewBuilding.Cost;
                        province.Resources = Good.DecreaseGoodsByGoods(province.Resources, e.NewBuilding.ResourceCost);
                        data.Buildings.Add(e.NewBuilding);
                        InvokeToGUIEvent(
                            new ToGUIUpdateLandProvinceDataEvent(
                                (LandColonizedProvinceData)EngineState.MapInfo.Scenario.Map[
                                    EngineState.MapInfo.CurrentSelectedProvinceId]));
                        InvokeToGUIEvent(new ToGUIUpdateCountryInfo());
                    }

                    return;
                case GUIDestroyBuildingEvent e:
                    var province2 =
                        (LandColonizedProvinceData)EngineState.MapInfo.Scenario.Map[
                            EngineState.MapInfo.CurrentSelectedProvinceId];
                    if (EngineState.PlayerCountryId == province2.Owner)
                    {
                        data.Buildings.RemoveAt(e.DestroyedId);
                        InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(province2));
                    }

                    return;
                case GUIGoodTransportChange e:
                    _goodToTransport = e.GoodToTransport;
                    _transportationAmount = e.Amount;
                    _whereToAddRoute = e.RouteAdress;
                    _uiReciever = e.NewTransportationRouteReciever;
                    return;
                default:
                    return;
            }
    }


    public void DeselectProvince()
    {
        InvokeToGUIEvent(new ToGuiHideProvinceDataEvent());
        EngineState.MapInfo.CurrentSelectedProvinceId = -1;
        _mapMaterial.SetShaderParameter("selectedID", EngineState.MapInfo.CurrentSelectedProvinceId);
    }

    private void _clearCountryText()
    {
        foreach (var text in _countryTextSpawner.GetChildren())
            text.Free();
    }

    private void _clearProvinceText()
    {
        foreach (var text in _provinceTextSpawner.GetChildren())
            text.Free();
    }

    private void _updateProvinceText()
    {
        _clearProvinceText();

        var mapContours = new MapContours(EngineState.MapInfo, EngineState.MapInfo.Scenario.MapTexture);
        var allLabels = new ConcurrentBag<CurvedText>();

        Parallel.ForEach(EngineState.MapInfo.Scenario.Map, province =>
        {
            var labels = new TextSolver2(mapContours.Provinces, province.Id, province.Name, 0.5f).FitText();

            foreach (var label in labels)
                allLabels.Add(label);
        });

        foreach (var label in allLabels)
        {
            var node = _textScene.Instantiate() as CurvedLabel;
            node!.Text = label;
            _provinceTextSpawner.AddChild(node);
        }
    }

    private void _updateCountryText()
    {
        _clearCountryText();
        var mapContours = new MapContours(EngineState.MapInfo, EngineState.MapInfo.Scenario.MapTexture);
        var allLabels = new ConcurrentBag<CurvedText>();

        Parallel.ForEach(EngineState.MapInfo.Scenario.Countries.Values, country =>
        {
            var labels = new TextSolver2(mapContours.States, country.Id, country.Name, 0.5f).FitText();

            foreach (var label in labels)
                allLabels.Add(label);
        });

        foreach (var label in allLabels)
        {
            var node = _textScene.Instantiate() as CurvedLabel;
            node!.Text = label;
            _countryTextSpawner.AddChild(node);
        }
    }

    private void _addCapitals()
    {
        foreach (var data in EngineState.MapInfo.Scenario.Countries)
        {
            var obj = _capitalScene.Instantiate() as Node2D;
            obj.Position = EngineState.MapInfo.Scenario.Map[data.Value.CapitalId].CenterOfWeight;
            _capitalSpawner.AddChild(obj);
        }
    }

    private void _addGoods()
    {
        foreach (var data in EngineState.MapInfo.MapProvinces(ProvinceTypes.LandProvinces))
        {
            var obj = _goodsScene.Instantiate() as AnimatedSprite2D;
            obj.Frame = ((LandProvinceData)data).Good.Id;
            obj.Position = ((LandProvinceData)data).CenterOfWeight;
            _goodsSpawner.AddChild(obj);
        }
    }

    private void _addDev()
    {
        foreach (var data in EngineState.MapInfo.MapProvinces(ProvinceTypes.ColonizedProvinces))
        {
            var obj = _devScene.Instantiate() as AnimatedSprite2D;
            obj.Frame = ((LandColonizedProvinceData)data).Development - 1;
            obj.Position = ((LandColonizedProvinceData)data).CenterOfWeight;
            _devSpawner.AddChild(obj);
            _devStorage.Add(data.Id, obj);
        }
    }


    private void _drawArrow(TransportationRoute route)
    {
        var arrow = _transportArrowScene.Instantiate() as Node2D;
        var map = EngineState.MapInfo.Scenario.Map;

        arrow.GlobalPosition = MathUtils.VectorCenter(map[route.ProvinceIdFrom].CenterOfWeight,
            map[route.ProvinceIdTo].CenterOfWeight);

        var arrowScale = (map[route.ProvinceIdTo].CenterOfWeight - map[route.ProvinceIdFrom].CenterOfWeight).Length() /
                         arrow.GetChild<Sprite2D>(0).Texture.GetSize().X;

        arrow.Scale = new Vector2(arrowScale, arrowScale);

        arrow.LookAt(map[route.ProvinceIdFrom].CenterOfWeight);

        _transportArrowSpawner.AddChild(arrow);
    }


    private void _addTransportArrows()
    {
        var children = _transportArrowSpawner.GetChildren();
        foreach (var child in children)
        {
            _transportArrowSpawner.RemoveChild(child);
            child.QueueFree();
        }

        var drawnArrows = new HashSet<Tuple<int, int>>();
        foreach (LandColonizedProvinceData data in EngineState.MapInfo.MapProvinces(ProvinceTypes.ColonizedProvinces))
        {
            if (data.HarvestedTransport != null)
            {
                if (drawnArrows.Contains(new Tuple<int, int>(data.HarvestedTransport.ProvinceIdFrom,
                        data.HarvestedTransport.ProvinceIdTo)))
                {
                    // TODO: Add [multiple] icons to transport arrow
                }
                else
                {
                    _drawArrow(data.HarvestedTransport);
                    drawnArrows.Add(new Tuple<int, int>(data.HarvestedTransport.ProvinceIdFrom,
                        data.HarvestedTransport.ProvinceIdTo));
                }
            }

            foreach (var building in data.SpecialBuildings.Where(b => b != null))
            {
                if (building is Factory factory && factory.TransportationRoute != null)
                {
                    if (drawnArrows.Contains(new Tuple<int, int>(factory.TransportationRoute.ProvinceIdFrom,
                            factory.TransportationRoute.ProvinceIdTo)))
                    {
                        // TODO: Add [multiple] icons to transport arrow
                    }
                    else
                    {
                        _drawArrow(factory.TransportationRoute);
                        drawnArrows.Add(new Tuple<int, int>(factory.TransportationRoute.ProvinceIdFrom,
                            factory.TransportationRoute.ProvinceIdTo));
                    }
                }

                if (building is StockAndTrade stockAndTrade)
                    foreach (var route in stockAndTrade.TransportationRoutes)
                        if (route != null)
                        {
                            if (drawnArrows.Contains(new Tuple<int, int>(route.ProvinceIdFrom,
                                    route.ProvinceIdTo)))
                            {
                                // TODO: Add [multiple] icons to transport arrow
                            }
                            else
                            {
                                _drawArrow(route);
                                drawnArrows.Add(new Tuple<int, int>(route.ProvinceIdFrom,
                                    route.ProvinceIdTo));
                            }
                        }
            }

            // If any new routes get added, put them here
        }
    }

    private void _updateMap()
    {
        InvokeToEngineEvent(new ToEngineViewModUpdate());
        _updateCountryText();
        _setFogOfWar();
    }


    public override void DayTick()
    {
        foreach (LandColonizedProvinceData data in EngineState.MapInfo.MapProvinces(ProvinceTypes.ColonizedProvinces))
        {
            foreach (var building in data.Buildings.Where(building => !building.IsFinished))
            {
                building.BuildingTime++;
                if (building.BuildingTime == building.TimeToBuild) building.IsFinished = true;
            }

            foreach (var specialBuilding in data.SpecialBuildings.Where(d => d != null))
                if (specialBuilding is MilitaryTrainingCamp camp)
                    if (camp.TrainingList.Count > 0 && camp.TrainingList.Peek().DayTick())
                    {
                        var a = camp.TrainingList.Dequeue();
                        a.IsFinished = true;
                        var b = new ArmyUnitData("test", data.Owner, data.Id, Modifiers.DefaultModifiers(),
                            new List<ArmyRegiment> { a }, null, new List<KeyValuePair<int, int>>(), 0,
                            UnitStates.Standing);
                        EngineState.MapInfo.Scenario.Countries[data.Owner].Units.Add(b);
                        InvokeToEngineEvent(new ToEngineAddArmyUnitEvent(b));
                    }
        }

        if (EngineState.MapInfo.CurrentSelectedProvinceId > -1 &&
            EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId] is LandColonizedProvinceData
                landData) InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(landData));
    }


    public override void MonthTick()
    {
        foreach (var country in EngineState.MapInfo.Scenario.Countries)
        {
            country.Value.Money -= Settings.MoneyConsumptionPerMonthColony(EngineState.MapInfo
                .MapProvinces(ProvinceTypes.UncolonizedProvinces).Count(d =>
                    (d as UncolonizedProvinceData).CurrentlyColonizedByCountry != null &&
                    (d as UncolonizedProvinceData).CurrentlyColonizedByCountry.Id ==
                    country.Key));
        }
        foreach (UncolonizedProvinceData data in EngineState.MapInfo.MapProvinces(ProvinceTypes.UncolonizedProvinces))
        {
            if (data.CurrentlyColonizedByCountry != null)
            {
                
                data.SettlersCombined += data.ColonyGrowth;

                if (data.SettlersCombined >= data.SettlersNeeded)
                {
                    EngineState.MapInfo.Scenario.Map[data.Id] = data.ConvertToLandProvince();
                    if(EngineState.MapInfo.CurrentSelectedProvinceId == data.Id)InvokeToGUIEvent(new ToGuiShowLandProvinceDataEvent(EngineState.MapInfo.Scenario.Map[data.Id] as LandColonizedProvinceData));
                    _updateMap();
                    var obj = _devScene.Instantiate() as AnimatedSprite2D;
                    obj.Frame = (EngineState.MapInfo.Scenario.Map[data.Id] as LandColonizedProvinceData).Development - 1;
                    obj.Position = (EngineState.MapInfo.Scenario.Map[data.Id] as LandColonizedProvinceData).CenterOfWeight;
                    _devSpawner.AddChild(obj);
                    _devStorage.Add(data.Id, obj);
                }
                else
                {
                    if (EngineState.MapInfo.CurrentSelectedProvinceId == data.Id) 
                        InvokeToGUIEvent(new ToGUIShowUncolonizedProvinceData(data, false));
                }
                
                
            }
        }
        
        //goodgenerationandtransportation and tax income
        foreach (LandColonizedProvinceData data in EngineState.MapInfo.MapProvinces(ProvinceTypes.ColonizedProvinces))
        {
            EngineState.MapInfo.Scenario.Countries[data.Owner].Money += data.TaxIncome;
            EngineState.MapInfo.Scenario.Countries[data.Owner].Manpower += (int)data.ManpowerGrowth;


            data.Resources[data.Good.Id] += data.ProductionRate;
            if (data.HarvestedTransport != null)
            {
                var diff = data.Resources[data.Good.Id] -
                           Mathf.Max(data.Resources[data.Good.Id] - data.HarvestedTransport.Amount, 0);
                data.Resources[data.Good.Id] -= diff;
                (EngineState.MapInfo.Scenario.Map[data.HarvestedTransport.ProvinceIdTo] as LandColonizedProvinceData)
                    .Resources[data.HarvestedTransport.TransportationGood.Id] += diff;
            }
        }

        //factorystage
        foreach (LandColonizedProvinceData data in EngineState.MapInfo.MapProvinces(ProvinceTypes.ColonizedProvinces))
        foreach (var building in data.SpecialBuildings.Where(b => b != null))
        {
            if (building is Factory factory && factory.Recipe != null)
            {
                if (factory.Recipe.Ingredients.Where(ingredient =>
                            data.Resources[ingredient.Key.Id] - ingredient.Value * factory.ProductionRate < 0).ToArray()
                        .Length <= 0)
                {
                    foreach (var ingredient in factory.Recipe.Ingredients)
                        data.Resources[ingredient.Key.Id] -= ingredient.Value * factory.ProductionRate;

                    data.Resources[factory.Recipe.Output.Id] += factory.ProductionRate;
                    factory.ProductionRate = Mathf.Min(factory.ProductionRate + factory.ProductionGrowthRate,
                        factory.MaxProductionRate);
                }
                else
                {
                    factory.ProductionRate = Mathf.Max(factory.ProductionRate - factory.ProductionGrowthRate, 0.1f);
                }

                if (factory.TransportationRoute != null)
                {
                    var diff = data.Resources[factory.Recipe.Output.Id] -
                               Mathf.Max(data.Resources[factory.Recipe.Output.Id] - factory.TransportationRoute.Amount,
                                   0);
                    data.Resources[factory.Recipe.Output.Id] -= diff;
                    (EngineState.MapInfo.Scenario.Map[factory.TransportationRoute.ProvinceIdTo] as
                        LandColonizedProvinceData).Resources[factory.Recipe.Output.Id] += diff;
                }
            }

            if (building is StockAndTrade stockAndTrade)
                foreach (var route in stockAndTrade.TransportationRoutes)
                    if (route != null)
                    {
                        var diff = data.Resources[route.TransportationGood.Id] -
                                   Mathf.Max(data.Resources[route.TransportationGood.Id] - route.Amount, 0);
                        data.Resources[route.TransportationGood.Id] -= diff;
                        (EngineState.MapInfo.Scenario.Map[route.ProvinceIdTo] as LandColonizedProvinceData).Resources[
                            route.TransportationGood.Id] += diff;
                    }

            if (building is Dockyard dockyard)
                foreach (var route in dockyard.WaterTransportationRoutes)
                    if (route != null)
                    {
                        var diff = data.Resources[route.TransportationGood.Id] -
                                   Mathf.Max(data.Resources[route.TransportationGood.Id] - route.Amount, 0);
                        data.Resources[route.TransportationGood.Id] -= diff;
                        (EngineState.MapInfo.Scenario.Map[route.ProvinceIdTo] as LandColonizedProvinceData).Resources[
                            route.TransportationGood.Id] += diff;
                    }
        }

        InvokeToGUIEvent(new ToGUIUpdateCountryInfo());

        if (EngineState.MapInfo.CurrentSelectedProvinceId > -1 &&
            EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId] is LandColonizedProvinceData
                landData) InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(landData));
    }

    public override void YearTick()
    {
    }
}