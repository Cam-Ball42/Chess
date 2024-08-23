using Godot;
using System;



public static class Moves : Object
{

    //As Gameboard is denoted by file, rank eg 3,2 is c4 and need to be stored as Vector2(3,2)
    public static Vector2[] Pawn = new Vector2[] {new Vector2(0,-1)};
    public static Vector2[] PawnFirstMove = new Vector2[] {new Vector2(0,-1), new Vector2(0,-2)} ;
    public static Vector2[] WPawnAttacking = new Vector2[] {new Vector2(1,-1), new Vector2(-1,-1)};  
    public static Vector2[] BPawnAttacking = new Vector2[] {new Vector2(-1, 1), new Vector2(1, 1)} ;
    public static Vector2[] Knight = new Vector2[] {new Vector2(1,2), new Vector2(-1,2), new Vector2(1,-2), new Vector2(-1,-2),
             new Vector2(2,1), new Vector2(-2,1), new Vector2(2,-1), new Vector2(-2,-1)
             };
           

    public static Vector2[] Bishop = new Vector2[]{
        new Vector2(1,1), new Vector2(2,2), new Vector2(3,3), new Vector2(4,4), new Vector2(5,5), new Vector2(6,6), new Vector2(7,7),
        new Vector2(-1,1), new Vector2(-2,2), new Vector2(-3,3), new Vector2(-4,4), new Vector2(-5,5), new Vector2(-6,6), new Vector2(-7,7),
        new Vector2(1,-1), new Vector2(2,-2), new Vector2(3,-3), new Vector2(4,-4), new Vector2(5,-5), new Vector2(6,-6), new Vector2(7,-7),
        new Vector2(-1,-1), new Vector2(-2,-2), new Vector2(-3,-3), new Vector2(-4,-4), new Vector2(-5,-5), new Vector2(-6,-6), new Vector2(-7,-7)
        };
    
    public static Vector2[] Rook = new Vector2[] {
        new Vector2(1,0), new Vector2(2,0), new Vector2(3,0), new Vector2(4,0), new Vector2(5,0), new Vector2(6,0), new Vector2(7,0),
        new Vector2(-1,0), new Vector2(-2,0), new Vector2(-3,0), new Vector2(-4,0), new Vector2(-5,0), new Vector2(-6,0), new Vector2(-7,0),
        new Vector2(0,1), new Vector2(0,2), new Vector2(0,3), new Vector2(0,4), new Vector2(0,5), new Vector2(0,6), new Vector2(0,7),
        new Vector2(0,-1), new Vector2(0,-2), new Vector2(0,-3), new Vector2(0,-4), new Vector2(0,-5), new Vector2(0,-6), new Vector2(0,-7)
             };

    public static Vector2[] Queen =  new Vector2[] {
        //Up
        new Vector2(0, -1), new Vector2(0, -2), new Vector2(0, -3), new Vector2(0, -4), new Vector2(0, -5), new Vector2(0, -6), new Vector2(0, -7),
        //DiagRightUp
        new Vector2(1, -1), new Vector2(2, -2), new Vector2(3, -3), new Vector2(4, -4), new Vector2(5, -5), new Vector2(6, -6), new Vector2(7, -7),
        //Right
        new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0), new Vector2(4, 0), new Vector2(5, 0), new Vector2(6, 0), new Vector2(7, 0),
        //DiagRightDown
        new Vector2(1, 1), new Vector2(2, 2), new Vector2(3, 3), new Vector2(4, 4), new Vector2(5, 5), new Vector2(6, 6), new Vector2(7, 7),
        //Down
        new Vector2(0, 1), new Vector2(0, 2), new Vector2(0, 3), new Vector2(0, 4), new Vector2(0, 5), new Vector2(0, 6), new Vector2(0, 7),
        //DiagLeftDown
        new Vector2(-1, 1), new Vector2(-2, 2), new Vector2(-3, 3), new Vector2(-4, 4), new Vector2(-5, 5), new Vector2(-6, 6), new Vector2(-7, 7),
        //Left
        new Vector2(-1, 0), new Vector2(-2, 0), new Vector2(-3, 0), new Vector2(-4, 0), new Vector2(-5, 0), new Vector2(-6, 0), new Vector2(-7, 0),
        //DiagLeftUp
        new Vector2(-1, -1), new Vector2(-2, -2), new Vector2(-3, -3), new Vector2(-4, -4), new Vector2(-5, -5), new Vector2(-6, -6), new Vector2(-7, -7)
    };  

    public static Vector2[] King = new Vector2[] {new Vector2(1,1), new Vector2(-1,1), new Vector2(1,-1), new Vector2(-1,-1),
             new Vector2(1,0), new Vector2(-1,0), new Vector2(0,1), new Vector2(0,-1)};

    public static Vector2[] GetMoves(string pieceType)
    {
        switch (pieceType)
        {
            case "P":
                return Pawn;
            case "N":
                return Knight;
            
            case "B":
                return Bishop;
            
            case "R":
                return Rook;
            
            case "Q":
                return Queen;
            
            case "K":
                return King;
            default:
                return null;
        }
    }

    public static int GetPieceDirections(string pieceType)
    {
        switch (pieceType)
        {
            case "P":
                return 0;
            case "N":
                return 0;
            case "B":
                return 6;
            case "R":
                return 6;
            case "Q":
                return 6;

            case "K":
                return 0;

            default:
                return 0;
        }
    }

    public static Vector2[] FlipToBlack(Vector2[] moves)
    {
        Vector2[] newMoves = new Vector2[moves.Length];
        for (int i = 0; i < moves.Length; i++)
        {
            newMoves[i] = new Vector2(moves[i].X, -moves[i].Y);
        }
        return newMoves;
    }

    public static Vector2[] GetPawnFirstMove()
    {
        return PawnFirstMove;
    }

    public static Vector2[] GetWPawnAttackMoves()
    {
        return WPawnAttacking;
    }

    public static Vector2[] GetBPawnAttackMoves()
    {
        return BPawnAttacking;
    }
}
