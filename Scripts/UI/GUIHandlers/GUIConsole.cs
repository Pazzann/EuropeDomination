using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIConsole : GUIHandler
{
	private RichTextLabel _commandLabel;

	private readonly List<string> _commandList = new()
		{ "SwitchToCountry(0)", "Clear()", "DebugMode()", "GoToProvince(0)" };

	private Expression _expression;


	private readonly HashSet<string> _history = new();
	private LineEdit _inputLabel;
	private string _lastResult = "";
	private string _lastStroke = "";
	private int _scrollingIndex;
	private int _tabScrollingIndex;

	public override void Init()
	{
		_commandLabel = GetNode<RichTextLabel>("Container/VContainer/VerticalText");
		_commandLabel.BbcodeEnabled = true;
		_inputLabel = GetNode<LineEdit>("Container/VContainer/InputText");
		_expression = new Expression();
	}


	public override void InputHandle(InputEvent @event)
	{
		if (Input.IsActionJustReleased("open_close_console"))
		{
			Visible = !Visible;
			_inputLabel.Text = "";
		}

		if (Input.IsActionJustReleased("arrow_up") && Visible)
		{
			var history = _history.ToArray();
			if (history.Length == 0)
				return;
			if (history.Length > Mathf.Abs(_scrollingIndex))
				_scrollingIndex += -1;
			_inputLabel.Text = history[history.Length + _scrollingIndex];
		}

		if (Input.IsActionJustReleased("arrow_down") && Visible)
		{
			var history = _history.ToArray();
			if (history.Length == 0)
				return;
			if (_scrollingIndex == 0)
				_scrollingIndex = -1;
			if (-1 > _scrollingIndex)
				_scrollingIndex += 1;
			_inputLabel.Text = history[history.Length + _scrollingIndex];
		}

		if (Input.IsActionJustReleased("tab") && Visible)
		{
			var arr = _commandList.ToArray().Where(s => s.Contains(_lastStroke)).ToArray();
			if (arr.Length == 0)
				return;
			_inputLabel.Text = arr[_tabScrollingIndex];
			_lastResult = arr[_tabScrollingIndex];
			if (arr.Length - 1 > _tabScrollingIndex)
				_tabScrollingIndex++;
			else if (_tabScrollingIndex == arr.Length - 1)
				_tabScrollingIndex = 0;
		}
	}


	private void _onInputTextSubmitted(string name)
	{
		_handleConsoleInput();
	}

	private void _onInputTextChanged(string name)
	{
		if (_inputLabel.Text == _lastResult)
			return;
		_lastStroke = _inputLabel.Text;
		_tabScrollingIndex = 0;
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
	}

	private void _handleConsoleInput()
	{
		_commandLabel.Text += "\n" + "> " + _inputLabel.Text;
		var error = _expression.Parse(_inputLabel.Text);
		if (error != Error.Ok)
		{
			_commandLabel.Text += "\n" + "[b][color=red]" + _expression.GetErrorText() + "[/color][/b]";
			return;
		}

		_history.Add(_inputLabel.Text);
		_inputLabel.Text = "";
		_scrollingIndex = 0;
		_tabScrollingIndex = 0;

		var result = _expression.Execute(null, this, false);
		if (!_expression.HasExecuteFailed())
			_commandLabel.Text += "\n" + result;
		else
			_commandLabel.Text += "\n" + "[b][color=red]" + _expression.GetErrorText() + "[/color][/b]";
	}

	public void Clear()
	{
		_commandLabel.Text = "";
		_commandLabel.Text += "\n" + "[color=green]cleared the console[/color]";
	}

	public void DebugMode()
	{
		EngineState.DebugMode = !EngineState.DebugMode;
		_commandLabel.Text += "\n" + "[i][color=blue]" + (EngineState.DebugMode ? "Enabled" : "Disabled") +
							  " debug mode[/color][/i]";
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

	public void GoToProvince(int id)
	{
		if (EngineState.MapInfo.Scenario.Map.Length > id)
		{
			InvokeGUIEvent(new GUIGoToProvince(id));
			_commandLabel.Text += "\n" + "[color=green]Successfully moved[/color]";
		}
		else
		{
			_commandLabel.Text += "\n" + "[b][color=red]This id doesn't exist[/color][/b]";
		}
	}
}
