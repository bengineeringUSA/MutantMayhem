using Godot;
using System;

public class Gameover : Node2D
{
	private Generic2dGame game;

	public override void _Ready()
	{
		game = (Generic2dGame)GetNode("/root/Generic2dGame");
		
		if (game.Level1Complete == true)
		{
			GetNode<AudioStreamPlayer>("WinMusic").Playing = true;
			GetNode<Node2D>("Credits").Visible = true;
			GetNode<Node2D>("HighScore").Visible = false;
		}
		else
		{
			GetNode<AudioStreamPlayer>("LoseMusic").Playing = true;
			GetNode<Node2D>("Credits").Visible = false;
			GetNode<Node2D>("HighScore").Visible = true;
		}
		
		((RichTextLabel)GetNode("HighScore/ScoreText")).BbcodeText = $"Final Score:  {game.PlayerScore}";
		((RichTextLabel)GetNode("HighScore/HighScoreText")).BbcodeText = $"High Score:  {game.HighestScore}";
	}
	
	public override void _Process(float delta)
	{
		if (game.Level1Complete == true)
		{
			var credits = GetNode<Node2D>("Credits/CreditsTextNode");
			
			if (credits.Position.y > -2700)
			{
				credits.Translate(new Vector2(0, (-50.0f)*delta));
			}
			else
			{
				GetNode<Node2D>("Credits").Visible = false;
				GetNode<Node2D>("HighScore").Visible = true;
			}
		}
		
		if (Input.IsActionJustReleased("ui_cancel") || Input.IsActionJustPressed("ui_accept") )
		{
			_on_TextureButton_pressed();
		}
	}

	private void _on_TextureButton_pressed()
	{
		GetNode<AudioStreamPlayer>("ButtonSound").Play();
	}

	private void _on_ButtonSound_finished()
	{
		var game = (Generic2dGame)GetNode("/root/Generic2dGame");
		game.GotoScene(Generic2dGame.Scenes.Titlescreen);
	}
}
