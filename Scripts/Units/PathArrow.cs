using Godot;

namespace EuropeDominationDemo.Scripts.Units;

public partial class PathArrow : Node2D
{
	private ProgressBar _progressBar;
	
	public override void _Ready()
	{
		_progressBar = GetChild(0).GetChild(0) as ProgressBar;
	}

	public void Setup(int length)
	{
		_progressBar.MaxValue = length;
	}
	public double Value
	{
		get => _progressBar.Value;
		set => _progressBar.Value = value;
	}

	public bool AddDay()
	{
		_progressBar.Value++;
		return _progressBar.Value >= _progressBar.MaxValue;
	}
}