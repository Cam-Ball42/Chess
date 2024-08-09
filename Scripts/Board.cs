using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using System.Runtime.ExceptionServices;
using System.Security.Cryptography;

public partial class Board : Node
{
	// Board is denoted by file, rank eg 0,0 is a1
    public Dictionary<Vector2, Piece> PreviousGameState = new Dictionary<Vector2, Piece>();
    public Dictionary<Vector2, Piece> CurrentGameState = new Dictionary<Vector2, Piece>();
	
	string CurrentTurn = "W";

	public bool GameFinished = false;

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


	public bool MovePiece(Vector2 from, Vector2 to)
	{
		Vector2[] CurrentMobility = GetPieceMobility(CurrentGameState[from]);
		bool moved = false;

		if (CurrentGameState.ContainsKey(from) && CurrentGameState.ContainsKey(to))
		{
			for (int i = 0; i < CurrentMobility.Length; i++)
			{
				if (CurrentMobility[i] == to)
				{
					Piece piece = CurrentGameState[from];
					CurrentGameState[from] = new Piece(true);
					CurrentGameState[to] = new Piece(true);
					CurrentGameState[to] = piece;
					piece.SetPiecePosition(new Vector2(to.X, to.Y));
					moved = true;
				}
			
			 	else { Console.WriteLine("Invalid Move"); }

			}
			
		}
		EvaluateBoardMobility();
		return moved;
	}

	public bool IsPieceMove(Vector2 from, Vector2 to, string PieceType)
	{
		for(int i = 0; i < Moves.GetMoves(PieceType).Length; i++)
		{
			if (from + Moves.GetMoves(PieceType)[i] == to)
			{
				return true;
			}
		}

		return false;
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
		Vector2[] moves;
		if (piece.PieceColor == "B")
		{
			moves = Moves.FlipToBlack(Moves.GetMoves(piece.PieceType));
		} else { moves = Moves.GetMoves(piece.PieceType);}
		
		bool isChecking = false;
		List<Vector2> blockingPieces = new List<Vector2>();
		List<Vector2> attackingCells = new List<Vector2>();
		List<Vector2> mobility = new List<Vector2>();
		int directions = Moves.GetPieceDirections(piece.PieceType);
		int j = 0;
		
	
		for (int i = 0; i < moves.Length; i++)
		{
			
				Vector2 newPosition = piece.GetPiecePosition() + moves[i];
				if (CurrentGameState.ContainsKey(newPosition))
				{
					
					if (CurrentGameState[newPosition].IsEmpty)
					{
						mobility.Add(newPosition);
						
						
					}
					else
					{
						if (CurrentGameState[newPosition].PieceType == "K")
						{
							isChecking = true;
						}
						if (CurrentGameState[newPosition].PieceColor != piece.PieceColor)
						{
							mobility.Add(newPosition);
							attackingCells.Add(newPosition);
							
						}
						else {
						blockingPieces.Add(newPosition);
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
		piece.IsChecking = isChecking;
		return piece.CurrentMobility;
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
						Fen += GameState[Position].PieceColor == "W" ? GameState[Position].PieceType.ToLower() : GameState[Position].PieceType;
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
