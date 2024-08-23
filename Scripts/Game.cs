using Godot;
using System;
using System.Collections.Generic;

using System.Runtime.ExceptionServices;

public partial class Game : Node2D
{
	public AudioStreamPlayer2D AudioStreamPlayer;

	public int WindowSize = 512;


	public string CurrentFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";

	public Debug Debug;

	public Canvas Canvas;
	public Board Board;
	public InputHandler InputHandler = new InputHandler();
	public InputStateEvaluator InputStateEvaluator;
	public PlayState CurrentPlayState { get; set; }
	public enum PlayState
	{
		Playing,
		Paused,
		InMenu,
		InSettings

	}
	public Player Player;
	public Player BotPlayer;

	public Timer MatchTimer = new Timer();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DisplayServer.WindowSetSize(new Vector2I(WindowSize, WindowSize));
		Tools.WindowSize = WindowSize;
		AudioStreamPlayer = GetNode("AudioStreamPlayer2D") as AudioStreamPlayer2D;

		Board = new Board(CurrentFen);
		Debug = new Debug(WindowSize);

		Player = new Player(Board, "W");
		BotPlayer = new Player(Board, "B");
		Player.Opponent = BotPlayer;
		BotPlayer.Opponent = Player;

		InputStateEvaluator = new InputStateEvaluator(Player, Board);

		AddChild(InputHandler);
		AddChild(InputStateEvaluator);
		AddChild(Debug);

		AddChild(Board);
		PackedScene packedScene = GD.Load<PackedScene>("res://canvas.tscn");
		if (packedScene != null)
		{
			Canvas = packedScene.Instantiate() as Canvas;
			if (Canvas != null)
			{

				AddChild(Canvas);
				Canvas.CurrentBoard = Board;
				Canvas.Player = Player;
				Canvas.ConnectSignals();
			}
		}

		Board.MoveMade += PlayMoveSound;

		Debug.TrackMousePosition();

		StartGame();
		GameLoop();
	}


	public override async void _Process(double delta)
	{
		if (Board.GameFinished == false)
		{
			if (CurrentPlayState == PlayState.Playing)
			{
				Debug.UpdateTrackedObjects();
				InputStateEvaluator.EvaluateInputState(this, Board);
				

			}
		}
	}



	public void StartGame()
	{
		CurrentPlayState = PlayState.Playing;
		Player.IsPlaying = true;
		Player.Turn = true;

	}

	public Board GetBoard()
	{
		return Board;
	}

	public void PlayMoveSound(Vector2 from, Vector2 to , Piece piece)
	{
		AudioStreamPlayer.Play();
	}
	
	public async void GameLoop()
	{
		while(Board.GameFinished == false & CurrentPlayState == PlayState.Playing)
		{
			if (Player.Turn == false)
			{
				await ToSignal(GetTree().CreateTimer(1f), "timeout");
				Board.EvaluateBoardMobility();
				BotPlayer.MakeBotMove(Board);
				Player.Turn = true;
			}

			await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
		}
	}










}
