using Godot;
using System;

public class LevelControls : Node2D
{
	[Signal]
	public delegate void On_LeftButton_pressed();

	[Signal]
	public delegate void On_LeftButton_released();

	[Signal]
	public delegate void On_RightButton_pressed();

	[Signal]
	public delegate void On_RightButton_released();

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_left"))
		{
			_on_LeftButton_pressed();
		}

		if (Input.IsActionJustReleased("ui_left"))
		{
			_on_LeftButton_released();
		}

		if (Input.IsActionJustPressed("ui_right"))
		{
			_on_RightButton_pressed();
		}

		if (Input.IsActionJustReleased("ui_right"))
		{
			_on_RightButton_released();
		}
	}

	public void _on_LeftButton_pressed()
	{
		EmitSignal(nameof(On_LeftButton_pressed));
	}

	public void _on_LeftButton_released()
	{
		EmitSignal(nameof(On_LeftButton_released));
	}

	public void _on_RightButton_pressed()
	{
		EmitSignal(nameof(On_RightButton_pressed));
	}

	public void _on_RightButton_released()
	{
		EmitSignal(nameof(On_RightButton_released));
	}
}
