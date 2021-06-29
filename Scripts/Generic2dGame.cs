using Godot;
using System;

public class Generic2dGame : Node
{
	private enum GameState
	{
		NotInGame,
		Playing,
		Paused
	};

	public enum Scenes
	{
		Unknown,
		Titlescreen,
		CutsceneIntro,
		Level1,
		//Level2,
		//Level3,
		CreditsScreen,
		Gameover
	};
	
	public enum Difficulty
	{
		Easy,
		Medium,
		Hard,
		Expert
	};

	public const int ScreenWidth = 1280;
	public const int ScreenHeight = 720;

	public readonly Vector2 MoneyBagLocation = new Vector2(1200, 56);

	public int PlayerScore = 0;
	public int PlayerMaxHealth = 3;
	public int PlayerHealth = 3;
	public int LeftArmDamage = 1;
	public int RightArmDamage = 1;
	
	// Difficulty related settings
	public Difficulty DifficultyLevel = Difficulty.Medium;
	public ulong EnemySpawnRate = 3;
	public float PlayerMaxRotationSpeed = 2.0f;
	
	public float Level1Duration = 60.0f;
	public ulong Level1EnemyAddSpeed = 3;
	public bool Level1Complete = false;
	
	public float Boss1ShootStartingTimer = 4.0f;
	public float Boss1ShootMinimumTime = 2.0f;
	public float Boss1ShrinkSpeed = 4.0f;
	public float Boss1MoveSpeed = 30.0f;
	public float Boss1ProjectileSpeed = 0.18f;
	public int Boss1Health = 7;

	// Data to persist
	private const string saveFile = "user://saveFile.save";
	public int HighestScore = 0;
	public bool FirstTimePlaying = true;

	public Node CurrentSceneFile { get; set; }

	public override void _Ready()
	{
		//RestorePersistedData();

		Viewport root = GetTree().Root;
		CurrentSceneFile = root.GetChild(root.GetChildCount() - 1);
	}

	public override void _Notification(int notification)
	{
		// Save on quit. Note that you can call `DataManager.Save()` whenever you want
		if (notification == MainLoop.NotificationWmQuitRequest)
		{
			StorePersistedData();
			GetTree().Quit();
		}
	}

	public void StorePersistedData()
	{
		var file = new File();

		var fileExists = file.FileExists(saveFile);

		if (!fileExists)
		{
			file.Open(saveFile, File.ModeFlags.Write);
		}
		else
		{
			file.Open(saveFile, File.ModeFlags.ReadWrite);
		}

		file.StoreVar(FirstTimePlaying);
		file.StoreVar(HighestScore);

		file.Close();
	}

	public void RestorePersistedData()
	{
		var file = new File();

		var ifExists = file.FileExists(saveFile);

		if (file.FileExists(saveFile))
		{
			file.Open(saveFile, File.ModeFlags.Read);

			FirstTimePlaying = (bool)file.GetVar();
			HighestScore = (int)file.GetVar();

			file.Close();
		}
	}
	
	public void SetDifficulty(Difficulty _diff)
	{
		switch (_diff)
		{
			case Difficulty.Easy:
				DifficultyLevel = Difficulty.Easy;
				EnemySpawnRate = 2;
				PlayerMaxRotationSpeed = 3.0f;
				
				Level1Duration = 45.0f;
				Level1EnemyAddSpeed = 4;
				
				Boss1Health = 4;
				Boss1ShootStartingTimer = 5.0f;
				Boss1ShootMinimumTime = 3.0f;
				Boss1ProjectileSpeed = 0.13f;
				Boss1MoveSpeed = 30.0f;
				Boss1ShrinkSpeed = 3.0f;
				break;
				
			case Difficulty.Medium:
				DifficultyLevel = Difficulty.Medium;
				EnemySpawnRate = 3;
				PlayerMaxRotationSpeed = 2.0f;
				
				Level1Duration = 60.0f;
				Level1EnemyAddSpeed = 3;
				
				Boss1Health = 7;
				Boss1ShootStartingTimer = 4.0f;
				Boss1ShootMinimumTime = 2.0f;
				Boss1ProjectileSpeed = 0.20f;
				Boss1MoveSpeed = 35.0f;
				Boss1ShrinkSpeed = 4.0f;
				break;
				
			case Difficulty.Hard:
				DifficultyLevel = Difficulty.Hard;
				EnemySpawnRate = 4;
				PlayerMaxRotationSpeed = 1.5f;
				
				Level1EnemyAddSpeed = 2;
				Level1Duration = 90.0f;
				
				Boss1Health = 10;
				Boss1ShootStartingTimer = 3.0f;
				Boss1ShootMinimumTime = 0.5f;
				Boss1ProjectileSpeed = 0.30f;
				Boss1MoveSpeed = 40.0f;
				Boss1ShrinkSpeed = 6.0f;
				break;
				
			case Difficulty.Expert:
				// FIXME - implement this setting
				//         Maybe give the player 1 health?
				break;
		}
	}

	public void GotoScene(Scenes nextScene)
	{
		// This function will usually be called from a signal callback,
		// or some other function from the current scene.
		// Deleting the current scene at this point is
		// a bad idea, because it may still be executing code.
		// This will result in a crash or unexpected behavior.

		// The solution is to defer the load to a later time, when
		// we can be sure that no code from the current scene is running:
		CallDeferred(nameof(DeferredGotoScene), "res://Scenes/" + nextScene.ToString() + ".tscn");
	}

	public void DeferredGotoScene(string path)
	{
		// It is now safe to remove the current scene
		CurrentSceneFile.Free();

		// Load a new scene.
		var nextScene = (PackedScene)GD.Load(path);

		// Instance the new scene.
		CurrentSceneFile = nextScene.Instance();

		// Add it to the active scene, as child of root.
		GetTree().Root.AddChild(CurrentSceneFile);

		// Optionally, to make it compatible with the SceneTree.change_scene() API.
		GetTree().CurrentScene = CurrentSceneFile;
	}


}
