using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

public partial class Game : Node2D
{



	public int WindowSize = 640;


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

		Board = new Board(CurrentFen);
		Debug = new Debug(WindowSize);
		Player = new Player(Board, "W");
		BotPlayer = new Player(Board, "B");
		InputStateEvaluator = new InputStateEvaluator(Player);



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
			}
		}


		Console.WriteLine(Board.GetFenFromState(Board.CurrentGameState));

		foreach (KeyValuePair<Vector2, Piece> piece in Board.CurrentGameState)
		{
			Console.WriteLine(piece.Value.ToString());
		}

		Debug.TrackMousePosition();

		StartGame();
	}


	public override async void _Process(double delta)
	{
		if (Board.GameFinished == false)
		{
			if (CurrentPlayState == PlayState.Playing)
			{
				Debug.UpdateTrackedObjects();
				InputStateEvaluator.EvaluateInputState(this, Board);
				Canvas.CurrentBoard = Board;
				if (Player.Turn == false)
				{
					//await ToSignal(GetTree().CreateTimer(1f), "timeout");
					BotPlayer.MakeBotMove(Board);
					Player.Turn = true;
				}

			}
		}
	}



	public void StartGame()
	{
		CurrentPlayState = PlayState.Playing;
		Player.IsPlaying = true;
		Player.Turn = true;

	}










}
