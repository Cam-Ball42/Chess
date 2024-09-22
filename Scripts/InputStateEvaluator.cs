using Godot;
using System;

public partial class InputStateEvaluator : Node
{
    public Player HumanPlayer {get; set;}
    public Board Board {get;set;}

    public InputStateEvaluator(Player player, Board Board)
    {
        HumanPlayer = player;
    }
    public void EvaluateInputState(Game Game, Board Board)
    {
        if (Game.CurrentPlayState == Game.PlayState.Playing)
        {
            if (InputState.IsClicking == true && HumanPlayer.IsHoldingPiece == false && HumanPlayer.Turn == true)
            {
                if(Board.CurrentGameState.ContainsKey(InputState.GetCurCellClickPos()) && Board.CurrentGameState[InputState.GetCurCellClickPos()].Type != null)
                {
                    HumanPlayer.HeldPiece = Board.CurrentGameState[InputState.GetCurCellClickPos()];
                    HumanPlayer.IsHoldingPiece = true;

                    //PrintState();

                }

                
            }
            else if (InputState.IsClicking == false)
            {
                if (HumanPlayer.IsHoldingPiece == true)
                {
                    if (Board.CurrentGameState.ContainsKey(InputState.GetCurCellClickPos()))
                    {

                        HumanPlayer.MakeMove(HumanPlayer.HeldPiece.GetPiecePosition(), Tools.GetCellFromMouse(this), Board);
                        HumanPlayer.IsHoldingPiece = false;
                        HumanPlayer.HeldPiece = null;
                        

                        //PrintState();
                    }

                    
                }
            }
            
        }

        if (InputState.LeftPressed == true)
        {
            HumanPlayer.Turn = !HumanPlayer.Turn;
            Board.UndoMove();
            InputState.LeftPressed = false;

        }
        
    }

    public void PrintState()    
    {
        Console.WriteLine("IsClicking: " + InputState.IsClicking);
        Console.WriteLine("IsDragging: " + InputState.IsDragging);
        Console.WriteLine("IsHoldingPiece: " + HumanPlayer.IsHoldingPiece);
        Console.WriteLine("HeldPiece: " + HumanPlayer.HeldPiece);
    }
}
