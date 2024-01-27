using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Handlers;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;

namespace EuropeDominationDemo.Scripts;

public class CallStack
{
    private List<GameHandler> _gameHandlers;
    private MapData _mapData;
    
    private int _previousMonth;
    private int _previousYear;
    
    public CallStack(List<GameHandler> gameHandlers)
    {
        _gameHandlers = gameHandlers;
    }
    
    
    public void Init(MapData mapData)
    {
        _mapData = mapData;
        foreach (var handler in _gameHandlers)
        {
            handler.Init(mapData);
        }
    }

    public void InputHandle(InputEvent @event, int tileId)
    {
        foreach (var handler in _gameHandlers)
        {
            handler.InputHandle(@event, tileId);
        }
    }

    public void TimeTick()
    {
        _mapData.Scenario.Date = _mapData.Scenario.Date.Add(_mapData.Scenario.Ts);
        
        foreach (var handler in _gameHandlers)
        {
            if (handler.TimeHandler != null)
                handler.TimeHandler.DayTick();
        }
        
        if (_previousMonth != _mapData.Scenario.Date.Month)
        {
            foreach (var handler in _gameHandlers)
            {
                if (handler.TimeHandler != null)
                    handler.TimeHandler.MonthTick();
            }
            
            _previousMonth = _mapData.Scenario.Date.Month;
        }
        
        if (_previousYear != _mapData.Scenario.Date.Year)
        {
            foreach (var handler in _gameHandlers)
            {
                if (handler.TimeHandler != null)
                    handler.TimeHandler.YearTick();
            }
            
            _previousYear = _mapData.Scenario.Date.Year;
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