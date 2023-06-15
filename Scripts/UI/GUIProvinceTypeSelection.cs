using Godot;
using System;

public partial class GUIProvinceTypeSelection : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _on_nothing_pressed()
	{
		// Replace with function body.
	}


	private void _on_production_line_pressed()
	{
		// Replace with function body.
	}


	private void _on_stock_pressed()
	{
		// Replace with function body.
	}


	private void _on_trade_center_pressed()
	{
		// Replace with function body.
	}


	private void _on_province_type_selection_pressed()
	{
		(GetChild(2) as ScrollContainer).Visible = !(GetChild(2) as ScrollContainer).Visible;
	}

}


