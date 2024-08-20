using System.Collections.Generic;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;

namespace EuropeDominationDemo.Scripts.Handlers;

public class CallMulticaster
{
    private readonly List<GameHandler> _gameHandlers;


    private int _previousMonth;
    private int _previousYear;

    public CallMulticaster(List<GameHandler> gameHandlers)
    {
        _gameHandlers = gameHandlers;
    }


    public void Init()
    {
        foreach (var handler in _gameHandlers)
        {
            handler.Init();
        }
    }

    public void InputHandle(InputEvent @event, int tileId)
    {
        foreach (var handler in _gameHandlers)
        {
            if (handler.InputHandle(@event, tileId))
                return;
        }
    }

    public void TimeTick()
    {
        EngineState.MapInfo.Scenario.Date = EngineState.MapInfo.Scenario.Date.Add(EngineState.MapInfo.Scenario.Ts);

        foreach (var handler in _gameHandlers)
        {
            handler.DayTick();
        }

        if (_previousMonth != EngineState.MapInfo.Scenario.Date.Month)
        {
            foreach (var handler in _gameHandlers)
            {
                handler.MonthTick();
            }

            _previousMonth = EngineState.MapInfo.Scenario.Date.Month;
        }

        if (_previousYear != EngineState.MapInfo.Scenario.Date.Year)
        {
            foreach (var handler in _gameHandlers)
            {
                handler.YearTick();
            }

            _previousYear = EngineState.MapInfo.Scenario.Date.Year;
        }
    }

    public void ViewModUpdate(float zoom)
    {
        foreach (var handler in _gameHandlers)
        {
            handler.ViewModUpdate(zoom);
        }
    }

    public void GUIInteractionHandler(GUIEvent @event)
    {
        foreach (var handler in _gameHandlers)
        {
            handler.GUIInteractionHandler(@event);
        }
    }
}