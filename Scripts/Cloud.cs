using Godot;
using System;

public class Cloud : Node2D
{
	private float speed = 0;
	private Random rnd = new Random();

	public override void _Ready()
	{
		speed = rnd.Next(3, 9)*(-10);

		var sprite = ((Sprite)GetNode("Sprite"));
		sprite.Texture = ((Texture)GD.Load("res://Assets/Backgrounds/cloud" + rnd.Next(1, 4) + ".png"));

		this.Translate(new Vector2(1500, rnd.Next(1, 360) + rnd.Next(-100,100)));
	}

	public override void _Process(float delta)
	{
		this.Translate(new Vector2((speed*delta), 0));

		if (this.Position.x < -200)
		{
			CallDeferred("free");
		}
	}

	public void RandomizePosition()
	{
		this.Translate(new Vector2(rnd.Next(-1500, 1300), 0));
	}
}
