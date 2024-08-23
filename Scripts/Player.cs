using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Object
{

    public  bool IsPlaying {get; set;}
    public  bool IsHoldingPiece {get; set;} = false;
    public  bool Turn {get; set;} = false;
    public  Piece HeldPiece {get; set;}
    public  List<Piece> Pieces = new List<Piece>();
    public  string Color {get; set;}

    public Player Opponent {get; set;}
    public bool IsInCheck {get; set;} = false;
    

    public Player(Board board, string Color)
    {
        foreach (KeyValuePair<Vector2, Piece> entry in board.CurrentGameState)
        {
            if (entry.Value.Color == Color)
            {
                this.Pieces.Add(entry.Value);
                entry.Value.Player = this;
            }
        }
       
    }

    public void MakeMove(Vector2 from, Vector2 to, Board Board)
    {
        if (Board.MovePiece(from, to, this) == true)
        {
            Turn = false;
        }
    }

    public void MakeBotMove(Board Board)
    {
        Random random = new Random();
        List<Piece> mobilePieces = new List<Piece>();
        Piece SelectedPiece = null;
        int RandomMobilePiece;
        int RandomMove;
        Vector2 SelectedMove;
        
        foreach (Piece piece in Pieces)
        {
            if (piece.CurrentMobility.Length > 0)
            {
                mobilePieces.Add(piece);
            }
            if ( IsInCheck == true)
            {
                if (piece.Type == "K")
                {
                    SelectedPiece = piece;
                }
            }
        }
        if ( IsInCheck == true)
        {
            if (SelectedPiece != null)
            {
                RandomMove = random.Next(0, SelectedPiece.CurrentMobility.Length);
                SelectedMove = SelectedPiece.CurrentMobility[RandomMove];
                MakeMove(SelectedPiece.GetPiecePosition(), SelectedMove, Board);
                IsInCheck = false;
                return;
            }
        }
        RandomMobilePiece = random.Next(0, mobilePieces.Count);
        SelectedPiece = mobilePieces[RandomMobilePiece];
        RandomMove = random.Next(0, SelectedPiece.CurrentMobility.Length);
        SelectedMove = SelectedPiece.CurrentMobility[RandomMove];
        MakeMove(SelectedPiece.GetPiecePosition(), SelectedMove, Board);
        
    }

    public Vector2[] GetAttackingCells(Board Board)
    {
        List<Vector2> attackingCells = new List<Vector2>();
        foreach (Piece piece in Pieces)
        {
            foreach (Vector2 cell in piece.CurrentMobility)
            {
                attackingCells.Add(cell);
            }
        }
        return attackingCells.ToArray();
    }

}
