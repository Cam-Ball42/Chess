using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


public partial class Canvas : Node2D
{

    Vector2 CellSize;
    public Board CurrentBoard {get; set;}

    Font DefaultFont;

    public Player Player {get; set;}

    private bool IsAnimating {get;set;} = false;
    private Move CurrentPieceMove {get; set;}
    private Vector2 CurrentAnimationPos;

    private Tween AnimTween;
    

    public override void _Ready()
    {
    

        CellSize = new Vector2(GetViewportRect().Size.X / 8, GetViewportRect().Size.Y / 8);
        DefaultFont = ThemeDB.FallbackFont;
        
    }
    

    public void ConnectSignals()
    {
        CurrentBoard.MoveMade += StartMoveAnimation;
    }

    public override void _Process(double delta)
    {
         QueueRedraw();
    }

    public override async void _Draw()
    {
        
        DrawBoard();
        DrawPieces();
        //DrawAllAttackingCells();
        if (Player.IsHoldingPiece && Player.HeldPiece.CurrentMobility != null)
        {
            CurrentBoard.GetPieceMobility(Player.HeldPiece);
            DrawLegalMoves(Player.HeldPiece);
            //DrawBlockingPieces(Player.HeldPiece);
            //DrawAttackingCells(Player.HeldPiece);
            
            DrawHeldPiece();
        }
        if (IsAnimating == true)
        {
            if ( AnimTween != null)
            {
                AnimTween.Kill();
                AnimTween = CreateTween();
            } else { AnimTween = CreateTween(); }
            AnimTween.TweenProperty(this, "CurrentAnimationPos", CurrentPieceMove.To, 0.1);
            DrawTexture(CurrentPieceMove.Piece.AtlasTex, CurrentAnimationPos * CellSize);
            if((CurrentPieceMove.To - CurrentAnimationPos).Length() < 0.1f)
            {
                IsAnimating = false;
            }
        }
        //DrawAllAttackingCells();
        
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

                //DrawString(DefaultFont, new Vector2(i * CellSize.X + CellSize.X / 2, j * CellSize.Y + CellSize.Y / 2), $"{i}{j}");
                
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

    public void StartMoveAnimation(Vector2 from, Vector2 to, Piece piece)
    {
        CurrentPieceMove = new Move(from, to, piece);
        CurrentAnimationPos = from;
        IsAnimating = true;
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

    private void DrawAllAttackingCells()
    {
        foreach (KeyValuePair<Vector2, Piece> piece in CurrentBoard.CurrentGameState)
        {
            if (!piece.Value.IsEmpty && piece.Value.AttackingCells != null)
            {
                
                foreach (Vector2 cell in piece.Value.AttackingCells)
                {
                    if(piece.Value.Color == "W")
                    {
                        HighlightCell(cell, new Color(1, 0, 0, (float)0.5));
                    }
                   else
                   {
                       //HighlightCell(cell, new Color(1, 1, 0, (float)0.5));
                   }
                }
                
            }
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
