using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;

public partial class Board : Node
{
	// Board is denoted by file, rank eg 0,0 is a1
    public Dictionary<Vector2, Piece> PreviousGameState = new Dictionary<Vector2, Piece>();
    public Dictionary<Vector2, Piece> CurrentGameState = new Dictionary<Vector2, Piece>();
	
	string CurrentTurn = "W";

	public bool GameFinished = false;

	[Signal]
	public delegate void MoveMadeEventHandler(Vector2 from, Vector2 to, Piece piece);

	public Board(string Fen)
	{
		CurrentGameState = GetStateFromFen(Fen);

	}

	public Board(Dictionary<Vector2, Piece> GameState)
	{
		CurrentGameState = GameState;
	}

	public override void _Ready()
	{
		PreviousGameState = CurrentGameState;
		EvaluateBoardMobility();

	}



	public bool MovePiece(Vector2 from, Vector2 to, Player player)
	{
		EvaluateBoardMobility();
		Vector2[] CurrentMobility = CurrentGameState[from].CurrentMobility;
		bool moved = false;

		if (CurrentGameState.ContainsKey(from) && CurrentGameState.ContainsKey(to))
		{
			for (int i = 0; i < CurrentMobility.Length; i++)
			{
				if (CurrentMobility[i] == to)
				{
					Piece piece = CurrentGameState[from];
					CurrentGameState[from] = new Piece(true);
					if (CurrentGameState[to].IsEmpty != true)
					{
						player.Opponent.Pieces.Remove(CurrentGameState[to]);
					}
					CurrentGameState[to] = new Piece(true);
					CurrentGameState[to] = piece;
					piece.SetPiecePosition(new Vector2(to.X, to.Y));
					piece.HasMoved = true;
					moved = true;
					EmitSignal(SignalName.MoveMade, from, to, piece);
					EvaluateBoardMobility();
					return moved;
				}
			 	
			}
			
			}	else { Console.WriteLine("Invalid Move"); }
		EvaluateBoardMobility();
		return moved;
	}

	public void UndoMove()
	{
		CurrentGameState = PreviousGameState;
	}

	public void EvaluateBoardMobility()
	{
		foreach (KeyValuePair<Vector2, Piece> piece in CurrentGameState)
		{
			if (!piece.Value.IsEmpty)
			{
				GetPieceMobility(piece.Value);
			}
		}
	}

	public Vector2[] GetPieceMobility(Piece piece)
	{
		if(piece.IsEmpty) {return null;}
		else if (piece.Type == "P") 
			{ 
				return GetPawnMobility(piece);
			}
		else if (piece.Type == "K")
			{
				return GetKingMobility(piece);
			}
		
		Vector2[] moves;
		moves = Moves.GetMoves(piece.Type);

		if (piece.Color == "B" && piece.Type != "P")
		{
			moves = Moves.FlipToBlack(moves);
		} 

		List<Vector2> blockingPieces = new List<Vector2>();
		List<Vector2> attackingCells = new List<Vector2>();
		List<Vector2> mobility = new List<Vector2>();

		int directions = Moves.GetPieceDirections(piece.Type);
		int j = 0;
		
		for (int i = 0; i < moves.Length; i++)
		{
			
			Vector2 newPosition = piece.GetPiecePosition() + moves[i];

			if (CurrentGameState.ContainsKey(newPosition))
			{
				
				if (CurrentGameState[newPosition].IsEmpty)
				{
					mobility.Add(newPosition);
					attackingCells.Add(newPosition);
				}
				else if (CurrentGameState[newPosition].Color != piece.Color && CurrentGameState[newPosition].Type == "K")
				{
					attackingCells.Add(newPosition);
					piece.IsChecking = true;
					piece.Player.Opponent.IsInCheck = true;
				}
				else
				{
					if (CurrentGameState[newPosition].Color != piece.Color && CurrentGameState[newPosition].Type!= "K")
					{
						mobility.Add(newPosition);
						attackingCells.Add(newPosition);
					}
					else 
					{
					blockingPieces.Add(newPosition);
					attackingCells.Add(newPosition);
					}
					i = i + Math.Clamp(directions - j , 0, directions) ;
					j = 0;
					
				}
				if(j == Math.Clamp(directions , 0, directions) ) {j=0;} else {j++;}
			} else {j = 0;}
				
		}
		piece.CurrentMobility = mobility.ToArray();
		piece.BlockingCells = blockingPieces.ToArray();
		piece.AttackingCells = attackingCells.ToArray();
		return piece.CurrentMobility;
	}

	public Vector2[] GetPawnMobility(Piece piece)
	{
		Vector2[] moves = Moves.GetMoves(piece.Type);

		List<Vector2> blockingPieces = new List<Vector2>();
		List<Vector2> attackingCells = new List<Vector2>();
		List<Vector2> mobility = new List<Vector2>();

		if (piece.HasMoved == false)
			{
				moves = Moves.GetPawnFirstMove();
			}

			Vector2[] Attacks;
			if (piece.Color == "W"){ Attacks = Moves.GetWPawnAttackMoves();} 
			else { Attacks = Moves.GetBPawnAttackMoves(); 
					moves = Moves.FlipToBlack(moves);
			}

			List<Vector2> newMoves = new List<Vector2>();

			for (int i = 0; i < Attacks.Length; i++)
			{
				Vector2 newPosition = piece.GetPiecePosition() + Attacks[i];
				if (CurrentGameState.ContainsKey(newPosition))
				{
					if (!CurrentGameState[newPosition].IsEmpty && CurrentGameState[newPosition].Color != piece.Color && CurrentGameState[newPosition].Type != "K")
					{
						mobility.Add(newPosition);
						attackingCells.Add(newPosition);
						newMoves.Add(newPosition);
					}
				}
			}

			for (int i = 0; i < moves.Length; i++)
			{
				Vector2 newPosition = piece.GetPiecePosition() + moves[i];
				if (CurrentGameState.ContainsKey(newPosition))
				{
					if (CurrentGameState[newPosition].IsEmpty)
					{
						mobility.Add(newPosition);
						attackingCells.Add(newPosition);
						newMoves.Add(newPosition);
					}
					else
					{
						blockingPieces.Add(newPosition);
						break;
					}
				}
			}
			
			
		piece.CurrentMobility = mobility.ToArray();
		piece.BlockingCells = blockingPieces.ToArray();
		piece.AttackingCells = attackingCells.ToArray();
		
		return newMoves.ToArray();
	}

	public Vector2[] GetKingMobility(Piece piece)
	{
		Vector2[] moves = Moves.GetMoves(piece.Type);
		List<Vector2> mobility = new List<Vector2>();
		List<Vector2> blockingPieces = new List<Vector2>();
		List<Vector2> attackingCells = new List<Vector2>();

		for (int i =0; i < moves.Length; i++)
		{
			Vector2 newPosition = piece.GetPiecePosition() + moves[i];
			if (CurrentGameState.ContainsKey(newPosition))
			{
				if (CurrentGameState[newPosition].IsEmpty && piece.Player.Opponent.GetAttackingCells(this).Contains(newPosition) == false)
				{
					mobility.Add(newPosition);
					attackingCells.Add(newPosition);
				}
				else if (CurrentGameState[newPosition].Color != piece.Color && piece.Player.Opponent.GetAttackingCells(this).Contains(newPosition) == false)
				{
					mobility.Add(newPosition);
					attackingCells.Add(newPosition);
				}
				else
				{
					blockingPieces.Add(newPosition);
				}
			}
		}

		piece.CurrentMobility = mobility.ToArray();
		piece.BlockingCells = blockingPieces.ToArray();
		piece.AttackingCells = attackingCells.ToArray();
		return mobility.ToArray();

	}



	public Dictionary<Vector2, Piece> GetStateFromFen(string Fen)
	{
		Dictionary<Vector2, Piece> NewGameState = new Dictionary<Vector2, Piece>();
		string[] Rows = Fen.Split("/");
	
		for (int i = 0; i < Rows.Length; i++)
		{
			string Row = Rows[i];
			int column = 0; 
	
			for (int j = 0; j < Row.Length; j++)
			{
				char Piece = Row[j];
	
				if (char.IsNumber(Piece))
				{
					int emptySquares = (int)char.GetNumericValue(Piece);
					for (int k = 0; k < emptySquares; k++)
					{
						NewGameState.Add(new Vector2(column, i), new Piece(true));
						column++;
					}
				}
				else
				{
					string Color = char.IsLower(Piece) ? "B" : "W";
					NewGameState.Add(new Vector2 (column, i), new Piece(Piece.ToString().ToUpper(), Color, $"{column}{i}"));
					column++;
				}
			}
		}
	
		return NewGameState;
	}

	public string GetFenFromState(Dictionary<Vector2, Piece> GameState)
	{
		string Fen = "";
		int emptySquares = 0;
	
		for (int i = 0; i < 8; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				Vector2 Position = new Vector2(j, i);
				if (GameState.ContainsKey(Position))
				{
					if (GameState[Position].IsEmpty)
					{
						emptySquares++;
					}
					else
					{
						if (emptySquares > 0)
						{
							Fen += emptySquares.ToString();
							emptySquares = 0;
						}
						Fen += GameState[Position].Color == "W" ? GameState[Position].Type.ToLower() : GameState[Position].Type;
					}
				}
			}
	
			if (emptySquares > 0)
			{
				Fen += emptySquares.ToString();
				emptySquares = 0;
			}
	
			if (i < 7)
			{
				Fen += "/";
			}
		}

		return Fen;
	}
}
