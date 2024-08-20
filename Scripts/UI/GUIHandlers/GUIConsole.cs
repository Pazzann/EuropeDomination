using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;


namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIConsole : GUIHandler
{
	private Expression _expression;
	private RichTextLabel _commandLabel;
	private LineEdit _inputLabel;

	private HashSet<string> _history =  new HashSet<string>();
	private int _scrollingIndex = -1;

	public override void Init()
	{
		_commandLabel = GetNode<RichTextLabel>("Container/VContainer/VerticalText");
		_commandLabel.BbcodeEnabled = true;
		_inputLabel = GetNode<LineEdit>("Container/VContainer/InputText");
		_expression = new Expression();
		
	}

	

	public override void InputHandle(InputEvent @event)
	{
		if (@event.IsActionPressed("open_close_console"))
		{
			Visible = !Visible;
		}

		if (@event.IsActionPressed("arrow_up")&& _inputLabel.FocusMode == FocusModeEnum.Click)
		{
			//TODO
			//_inputLabel.Text = _history.[_history.Count-1]
		}
		if (@event.IsActionPressed("arrow_down")&& _inputLabel.FocusMode == FocusModeEnum.Click)
		{
			//TODO
			
			//_inputLabel.Text = _history.[_history.Count-1]
		}
		if (@event.IsActionPressed("tab") && _inputLabel.FocusMode == FocusModeEnum.Click)
		{
			//TODO
			//_inputLabel.Text = _history.[_history.Count-1]
		}
	}

	private void _onInputTextSubmitted(string name)
	{
		_handleConsoleInput();
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		return;
	}

	private void _handleConsoleInput()
	{
		_commandLabel.Text += "\n" + "> " + _inputLabel.Text;
		Error error = _expression.Parse(_inputLabel.Text);
		if (error != Error.Ok)
		{
			_commandLabel.Text += "\n" + "[b][color=red]" + _expression.GetErrorText() + "[/color][/b]";
			return;
		}
		_history.Add(_inputLabel.Text);
		_inputLabel.Text = "";


		Variant result = _expression.Execute(null, this, false);
		if (!_expression.HasExecuteFailed())
		{
			_commandLabel.Text += "\n" + result.ToString();
		}
		else
		{
			_commandLabel.Text += "\n" + "[b][color=red]" + _expression.GetErrorText()  + "[/color][/b]";
		}
		
	}
	
	public void Clear()
	{
		_commandLabel.Text = "";
		_commandLabel.Text += "\n" + "[color=green]cleared the console[/color]";
	}

	public void DebugMode()
	{
		EngineState.DebugMode = !EngineState.DebugMode;
		_commandLabel.Text += "\n" + "[i][color=blue]" + ((EngineState.DebugMode)? "Enabled" : "Disabled") +" debug mode[/color][/i]";
	}

	public void SwitchToCountry(int id)
	{
		if (EngineState.MapInfo.Scenario.Countries.ContainsKey(id))
		{
			EngineState.PlayerCountryId = id;
			InvokeGUIEvent(new GUISwitchCountry());
			_commandLabel.Text += "\n" + "[color=green]Successfully switched[/color]";
		}
		else
		{
			_commandLabel.Text += "\n" + "[b][color=red]This id doesn't exist[/color][/b]";
		}
	} 
}
