using Godot;
using System;

public class Explosion : AnimatedSprite
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Play();
		this.GetNode<AudioStreamPlayer2D>("ExplosionSound").Play();
	}

	private void _on_AnimatedSprite_animation_finished()
	{
		this.Visible = false;
		CallDeferred("free");
	}
}
