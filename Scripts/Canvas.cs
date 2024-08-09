using Godot;
using System;
using System.Collections.Generic;
using System.Data;

public partial class Canvas : Node2D
{

    Vector2 CellSize;
    public Board CurrentBoard {get; set;}

    Font DefaultFont;

    public Player Player {get; set;}

    

    public override void _Ready()
    {
    

        CellSize = new Vector2(GetViewportRect().Size.X / 8, GetViewportRect().Size.Y / 8);
        DefaultFont = ThemeDB.FallbackFont;
        
    }

    public override void _Process(double delta)
    {
         QueueRedraw();
    }

    public override void _Draw()
    {
        
        DrawBoard();
        DrawPieces();
        if (Player.IsHoldingPiece && Player.HeldPiece.CurrentMobility != null)
        {
            
            DrawLegalMoves(Player.HeldPiece);
            DrawBlockingPieces(Player.HeldPiece);
            DrawAttackingCells(Player.HeldPiece);
            DrawHeldPiece();
        }
        
    }

    private void DrawBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if ((i + j) % 2 == 0)
                {
                    DrawRect(new Rect2(i * CellSize.X, j  * CellSize.Y, CellSize.X, CellSize.Y), Colors.Tan, true);
                }
                else
                {
                    DrawRect(new Rect2(i * CellSize.X, j  * CellSize.Y, CellSize.X, CellSize.Y), Colors.Beige, true);
                }

                DrawString(DefaultFont, new Vector2(i * CellSize.X + CellSize.X / 2, j * CellSize.Y + CellSize.Y / 2), $"{(i)}{j}");
                
            }
        }
    }

    private void DrawPieces()
    {
        foreach (KeyValuePair<Vector2, Piece> piece in CurrentBoard.CurrentGameState)
        {
            if (!piece.Value.IsEmpty)
            {
                
                DrawTexture(piece.Value.AtlasTex, piece.Value.GetPiecePosition() * CellSize);
                
                
            }
        }
       
    }

    private void DrawHeldPiece()
    {
        if (Player.IsHoldingPiece)
        {
            DrawTexture(Player.HeldPiece.AtlasTex, Tools.GetMousePosition(this));
        }
    }

    private void HighlightCell(Vector2 cell, Color color)
    {
        DrawRect(new Rect2(cell * CellSize, CellSize), color, true);
    }

    private void DrawLegalMoves(Piece piece)
    {
        if (piece.CurrentMobility == null) {return;}
        foreach (Vector2 move in piece.CurrentMobility)
        {
            HighlightCell(move, new Color(0, 0, 1, (float)0.5));
        }
    }

    private void DrawBlockingPieces(Piece piece)
    {
        if (piece.BlockingCells == null) {return;}
        foreach (Vector2 blockingPiece in piece.BlockingCells)
        {
            HighlightCell(blockingPiece, new Color(1, 0, 0, (float)0.5));
        }
    }

    private void DrawAttackingCells(Piece piece)
    {
        if (piece.AttackingCells == null) {return;}
        foreach (Vector2 attackingCells in piece.AttackingCells)
        {
            HighlightCell(attackingCells, new Color(1, 1, 0, (float)0.5));
        }
    }

    











    public void SetGameState(Dictionary<Vector2, Piece> gameState)
    {
        CurrentBoard.CurrentGameState = gameState;
    }

    public Dictionary<Vector2, Piece> GetGameState()
    {
        return CurrentBoard.CurrentGameState;
    }
}
