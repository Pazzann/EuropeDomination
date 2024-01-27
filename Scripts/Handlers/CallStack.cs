using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Handlers;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;

namespace EuropeDominationDemo.Scripts;

public class CallStack
{
    private List<GameHandler> _gameHandlers;
    private Scenario _scenario;
    
    private int _previousMonth;
    private int _previousYear;
    
    public CallStack(List<GameHandler> gameHandlers)
    {
        _gameHandlers = gameHandlers;
    }
    
    
    public void Init(Scenario scenario)
    {
        _scenario = scenario;
        foreach (var handler in _gameHandlers)
        {
            handler.Init(scenario);
        }
    }

    public void InputHandle(InputEvent @event)
    {
        foreach (var handler in _gameHandlers)
        {
            handler.InputHandle(@event);
        }
    }

    public void TimeTick()
    {
        _scenario.Date = _scenario.Date.Add(_scenario.Ts);
        
        foreach (var handler in _gameHandlers)
        {
            handler.TimeHandler.DayTick();
        }
        
        if (_previousMonth != _scenario.Date.Month)
        {
            foreach (var handler in _gameHandlers)
            {
                handler.TimeHandler.MonthTick();
            }
            
            _previousMonth = _scenario.Date.Month;
        }
        
        if (_previousYear != _scenario.Date.Year)
        {
            foreach (var handler in _gameHandlers)
            {
                handler.TimeHandler.YearTick();
            }
            
            _previousYear = _scenario.Date.Year;
        }
    }

    public void ViewModUpdate(MapTypes mapTypes, float zoom)
    {
        foreach (var handler in _gameHandlers)
        {
            handler.ViewModUpdate(mapTypes, zoom);
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