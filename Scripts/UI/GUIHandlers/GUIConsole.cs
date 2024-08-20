using System;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;


namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIConsole : GUIHandler
{
    private Expression _expression;
    private RichTextLabel _commandLabel;
    private LineEdit _inputLabel;

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
            _commandLabel.Text += "\n" + _expression.GetErrorText();
            return;
        }
        _inputLabel.Text = "";


        Variant result = _expression.Execute(null, this, false);
        if (!_expression.HasExecuteFailed())
        {
            _commandLabel.Text += "\n" + result.ToString();
        }
        else
        {
            _commandLabel.Text += "\n" + _expression.GetErrorText();
        }
    }
    
    public void Clear()
    {
        _commandLabel.Text = "";
        _commandLabel.Text += "\n" + "[color=green]cleared the console[/color]";
    }

    public void DebugMode()
    {
        //TODO
    }
}