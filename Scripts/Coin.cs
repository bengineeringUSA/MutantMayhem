using Godot;
using System;

public class Coin : Node2D
{
	private Generic2dGame game;
	private float moveSpeed = 0f;
	private Vector2 positionDifference;
	private Vector2 moneyBagPosition;
	private bool hasHitMoneyBag = false;

	public override void _Ready()
	{
		game = (Generic2dGame)GetNode("/root/Generic2dGame");
		moneyBagPosition = game.MoneyBagLocation;
		positionDifference = moneyBagPosition - this.Position;
	}

	public override void _Process(float delta)
	{
		if (!hasHitMoneyBag)
		{
			this.Translate(positionDifference * moveSpeed * delta);
			moveSpeed += 0.1f;
		}
	}

	private void _on_Area2D_area_entered(object area)
	{
		this.GetNode<AudioStreamPlayer2D>("CoinSound").Play();
		moveSpeed = 0;
		hasHitMoneyBag = true;
		this.GetNode<Sprite>("Sprite").Visible = false;
	}

	private void _on_CoinSound_finished()
	{
		CallDeferred("free");
	}

	public void SetValue(int value)
	{
		var sprite = ((Sprite)this.GetNode("Sprite"));

		if (value == 5)
		{
			sprite.Texture = ((Texture)GD.Load("res://Assets/Misc/Collectibles/Coin1.png"));
		}
		else
		{
			sprite.Texture = ((Texture)GD.Load("res://Assets/Misc/Collectibles/Coin2.png"));
		}
	}

	public void DeferredGotoScene()
	{
		this.GetParent().GetParent().QueueFree();
	}
}
