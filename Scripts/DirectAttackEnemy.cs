using Godot;
using System;

public class DirectAttackEnemy : IEnemy
{
	private Generic2dGame game;
	
	private float rotationSpeed = 0.0f;

	// TODO - This won't be true on levels where we're not flying.
	private Vector2 playerPosition = new Vector2(640, 360);
	private Vector2 positionDifference;
	private float speed = 0.25f;

	public override void _Ready()
	{
		game = (Generic2dGame)GetNode("/root/Generic2dGame");
		
		EntranceSide = ((Side)Rnd.Next(0, 4));

		switch (EntranceSide)
		{
			case Side.Top:
				StartingX = Rnd.Next(0, Generic2dGame.ScreenWidth);
				StartingY = -80;
				break;

			case Side.Left:;
				StartingX = -80;
				StartingY = Rnd.Next(0, Generic2dGame.ScreenHeight);
				break;

			case Side.Right:
				StartingX = Generic2dGame.ScreenWidth + 80;
				StartingY = Rnd.Next(0, Generic2dGame.ScreenHeight);
				break;

			case Side.Bottom:
				StartingX = Rnd.Next(0, Generic2dGame.ScreenWidth);
				StartingY = Generic2dGame.ScreenHeight + 80;
				break;
		}

		Position = new Vector2(StartingX, StartingY);
		positionDifference = playerPosition - Position;

		var sprite = this.GetNode("Sprite");
		Area = ((Area2D)sprite.GetNode("Area2D"));

		var player = this.GetParent().GetNode("Player");
		if (player != null)
		{
			player.Connect("Hit", this, nameof(Hit));
		}
	}

	public override void _Process(float delta)
	{
		base.Translate(positionDifference * speed * delta);
		base.Rotate(rotationSpeed*delta);

		if (HitCoolDownTimer > 0.0f)
		{
			HitCoolDownTimer -= delta;
		}
		else
		{
			if ((Area.GetOverlappingAreas().Count) > 0 && (DamageToTake > 0))
			{
				Hit(DamageToTake, NodeGuid.ToString());
			}
		}

		if (HitAnimationTimer > 0.0f)
		{
			var sprite = this.GetNode<AnimatedSprite>("Sprite");
			sprite.SelfModulate = new Color(0.5f - HitAnimationTimer, 0.5f - HitAnimationTimer, 0.5f - HitAnimationTimer);

			HitAnimationTimer -= delta;
		}
		else
		{
			HitAnimationTimer = 0.0f;
			var sprite = this.GetNode<AnimatedSprite>("Sprite");
			sprite.SelfModulate = new Color(1, 1, 1);
		}
	}

	public void SetStartingPosition(Vector2 startingPosition)
	{
		this.Position = startingPosition; 
		positionDifference = playerPosition - Position;
	}

	public void SetVariety(int variety)
	{
		switch (variety)
		{
			case 1:
				var aSprite = ((AnimatedSprite)this.GetNode("Sprite"));
				var image1 = ((Texture)GD.Load("res://Assets/Enemies/Bomb1.png"));

				aSprite.Frames = new SpriteFrames();
				aSprite.Frames.AddAnimation("default");
				aSprite.Frames.AddFrame("default", image1, 0);
				aSprite.Frames.SetAnimationLoop("default", true);
				aSprite.Frames.SetAnimationSpeed("default", 5.0f);

				rotationSpeed = ((float)Rnd.Next(-50,50))*0.1f;
				Health = 1;
				break;

			case 2:
				aSprite = ((AnimatedSprite)this.GetNode("Sprite"));
				image1 = ((Texture)GD.Load("res://Assets/Enemies/Potion1.png"));

				aSprite.Frames = new SpriteFrames();
				aSprite.Frames.AddAnimation("default");
				aSprite.Frames.AddFrame("default", image1, 0);
				aSprite.Frames.SetAnimationLoop("default", true);
				aSprite.Frames.SetAnimationSpeed("default", 5.0f);

				rotationSpeed = ((float)Rnd.Next(-50,50))*0.1f;
				Health = 2;
				speed = 0.22f;
				break;

			case 3:
				aSprite = ((AnimatedSprite)this.GetNode("Sprite"));
				image1 = ((Texture)GD.Load("res://Assets/Enemies/Flyingfrog_1.png"));
				var image2 = ((Texture)GD.Load("res://Assets/Enemies/Flyingfrog_2.png"));
				var image3 = ((Texture)GD.Load("res://Assets/Enemies/Flyingfrog_3.png"));
				var image4 = ((Texture)GD.Load("res://Assets/Enemies/Flyingfrog_4.png"));

				aSprite.Frames = new SpriteFrames();
				aSprite.Frames.AddAnimation("default");
				aSprite.Frames.AddFrame("default", image1, 0);
				aSprite.Frames.AddFrame("default", image2, 1);
				aSprite.Frames.AddFrame("default", image3, 2);
				aSprite.Frames.AddFrame("default", image4, 3);
				aSprite.Frames.SetAnimationLoop("default", true);
				aSprite.Frames.SetAnimationSpeed("default", 30.0f);
				aSprite.Playing = true;

				Health = 1;
				speed = game.Boss1ProjectileSpeed;
				break;

			default:
				((Sprite)this.GetNode("Sprite")).Texture = ((Texture)GD.Load("res://Assets/Enemies/Bomb1.png"));
				rotationSpeed = ((float)Rnd.Next(-50,50))*0.1f;
				Health = 1;
				break;
		}
	}
}
