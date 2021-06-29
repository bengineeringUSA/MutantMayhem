using Godot;
using System;

public class Titlescreen : Node
{
	private Generic2dGame game;

	private float scaleFactor = 1f;
	private bool startGameNext = false;

	public override void _Ready()
	{
		game = (Generic2dGame)GetNode("/root/Generic2dGame");
	}

	public override void _Process(float delta)
	{
		var buildType = ProjectSettings.GetSetting("application/config/name/BuildType");
		var versionMajor = ProjectSettings.GetSetting("application/config/name/VersionMajor");
		var versionMinor = ProjectSettings.GetSetting("application/config/name/VersionMinor");
		var versionBuild = ProjectSettings.GetSetting("application/config/name/VersionBuild");
		((RichTextLabel)GetNode("VersionInfo")).BbcodeText = $"{buildType}:  v{versionMajor}.{versionMinor}.{versionBuild}";
		
		var foreground = (Sprite)GetNode("Foreground");
		scaleFactor += 0.1f;
		var factor = ((float)Math.Sin(((double)scaleFactor)));

		foreground.ApplyScale(new Vector2(1 + (factor/200), 1 + (factor/200)));

		if ( Input.IsActionJustPressed("ui_accept") )
		{
			_on_StartButton_pressed();
		}
		
		if ( Input.IsActionJustPressed("ui_cancel") )
		{
			_on_QuitButton_pressed();
		}
		
		if ( Input.IsActionJustReleased("ui_up") )
		{
			switch (game.DifficultyLevel)
			{
				case Generic2dGame.Difficulty.Easy:
					_on_HardButton_pressed();
					break;
					
				case Generic2dGame.Difficulty.Medium:
					_on_EasyButton_pressed();
					break;
					
				case Generic2dGame.Difficulty.Hard:
					_on_MediumButton_pressed();
					break;
					
				case Generic2dGame.Difficulty.Expert:
					break;
			}
		}
		
		if ( Input.IsActionJustReleased("ui_down") )
		{
			switch (game.DifficultyLevel)
			{
				case Generic2dGame.Difficulty.Easy:
					_on_MediumButton_pressed();
					break;
					
				case Generic2dGame.Difficulty.Medium:
					_on_HardButton_pressed();
					break;
					
				case Generic2dGame.Difficulty.Hard:
					_on_EasyButton_pressed();
					break;
					
				case Generic2dGame.Difficulty.Expert:
					break;
			}
		}
	}

	private void _on_QuitButton_pressed()
	{
		this.GetNode<AudioStreamPlayer2D>("ButtonSound").Play();
		GetTree().Quit();
	}

	private void _on_StartButton_pressed()
	{
		startGameNext = true;
		this.GetNode<AudioStreamPlayer2D>("ButtonSound").Play();
	}

	private void _on_ButtonSound_finished()
	{
		if (startGameNext == true)
		{
			game.GotoScene(Generic2dGame.Scenes.CutsceneIntro);
		}
	}
	
	private void _on_EasyButton_pressed()
	{
		game.SetDifficulty(Generic2dGame.Difficulty.Easy);
		this.GetNode<TextureButton>("EasyButton").Pressed = true;
		this.GetNode<TextureButton>("MediumButton").Pressed = false;
		this.GetNode<TextureButton>("HardButton").Pressed = false;
	}

	private void _on_MediumButton_pressed()
	{
		game.SetDifficulty(Generic2dGame.Difficulty.Medium);
		this.GetNode<TextureButton>("EasyButton").Pressed = false;
		this.GetNode<TextureButton>("MediumButton").Pressed = true;
		this.GetNode<TextureButton>("HardButton").Pressed = false;
	}

	private void _on_HardButton_pressed()
	{
		game.SetDifficulty(Generic2dGame.Difficulty.Hard);
		this.GetNode<TextureButton>("EasyButton").Pressed = false;
		this.GetNode<TextureButton>("MediumButton").Pressed = false;
		this.GetNode<TextureButton>("HardButton").Pressed = true;
	}
}
