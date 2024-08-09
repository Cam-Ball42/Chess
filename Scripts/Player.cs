using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Object
{

    public  bool IsPlaying {get; set;}
    public  bool IsHoldingPiece {get; set;} = false;
    public  bool Turn {get; set;} = false;
    public  Piece HeldPiece {get; set;}
    public  Piece[] Pieces {get; set;}
    public  string Color {get; set;}
    public Board Board {get; set;}

    public Player(Board board, string Color)
    {
        this.Board = board;
        List<Piece> pieces = new List<Piece>();
        foreach (KeyValuePair<Vector2, Piece> entry in board.CurrentGameState)
        {
            if (entry.Value.PieceColor == Color)
            {
                pieces.Add(entry.Value);
            }
        }
        Pieces = pieces.ToArray();
    }

    public void MakeMove(Vector2 from, Vector2 to)
    {
        if (Board.MovePiece(from, to) == true)
        {
            Turn = false;
        }
    }

    public void MakeBotMove(Board CurrentBoard)
    {
        Random random = new Random();
        List<Piece> mobilePieces = new List<Piece>();
        foreach (Piece piece in Pieces)
        {
            if (piece.CurrentMobility.Length != 0)
            {
                mobilePieces.Add(piece);
            }
        }
        int randomMobilePiece = random.Next(0, mobilePieces.Count);
        Piece selectedPiece = mobilePieces[randomMobilePiece];
        int randomMove = random.Next(0, selectedPiece.CurrentMobility.Length);
        Vector2 selectedMove = selectedPiece.CurrentMobility[randomMove];
        MakeMove(selectedPiece.GetPiecePosition(), selectedMove);
        
    }

}
