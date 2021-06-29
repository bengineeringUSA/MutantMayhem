using Godot;
using System;

public class IEnemy : Node2D
{
	public enum Side
	{
		Top = 0,
		Left,
		Right,
		Bottom
	};

	public Guid NodeGuid = Guid.NewGuid();
	public Side EntranceSide = Side.Top;
	public int StartingX = 0;
	public int StartingY = 0;
	public Random Rnd = new Random();
	public int Health = 0;
	public int TotalHealth = 0;
	public float HitCoolDownTimer = 0.0f;
	public float HitAnimationTimer = 0.0f;
	public int DamageToTake = 0;
	public Area2D Area;
	
	public virtual void Hit(int damage, string guid)
	{
		if (guid == NodeGuid.ToString())
		{
			if (HitCoolDownTimer <= 0.0f)
			{
				Health -= damage;

				if (Health > 0)
				{
					HitAnimationTimer = 0.2f;
					HitCoolDownTimer = 0.5f;
					DamageToTake = damage;
					this.GetNode<AudioStreamPlayer2D>("HitSound").Play();
				}
				else
				{
					var explosion = (PackedScene)ResourceLoader.Load("res://Components/Explosion.tscn");
					Node2D explosionInstance = (Node2D)explosion.Instance();
					var position = this.GlobalPosition;
					explosionInstance.Position = position;
					this.GetParent().AddChild(explosionInstance);

					var coin = (PackedScene)ResourceLoader.Load("res://Components/Coin.tscn");
					Coin coinInstance = (Coin)coin.Instance();
					coinInstance.Position = position;

					var hud = this.GetParent().GetNode("HUD");

					if (Rnd.NextDouble() < 0.90)
					{
						coinInstance.SetValue(1);
						((HUD)hud).AddCoin(1);
					}
					else
					{
						coinInstance.SetValue(5);
						((HUD)hud).AddCoin(5);
					}

					this.GetParent().AddChild(coinInstance);

					CallDeferred("free");
				}
			}
		}
	}
	
	public void Destroy()
	{
		var explosion = (PackedScene)ResourceLoader.Load("res://Components/Explosion.tscn");
		Node2D explosionInstance = (Node2D)explosion.Instance();
		var position = this.GlobalPosition;
		explosionInstance.Position = position;
		this.GetParent().AddChild(explosionInstance);
		
		CallDeferred("free");
	}
}
