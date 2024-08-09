using Godot;
using System;
using System.Numerics;

public partial class Piece : Node
{
    
    public AtlasTexture AtlasTex  { get; set; }
    public string PieceType { get; set; }
    public string PieceColor {get; set;}
    Godot.Vector2 PiecePosition;
    public bool IsAlive { get; set; } 
    public bool IsEmpty { get; set; }
    public bool IsChecking { get; set; }
    public Godot.Vector2[] CurrentMobility { get; set; }

    public Godot.Vector2[] BlockingCells { get; set; } 
    public Godot.Vector2[] AttackingCells { get; set; }

    public Piece(string pieceType, string pieceColor, string position)
    {
        PieceType = pieceType;
        PieceColor = pieceColor;
        IsAlive = true;
        IsEmpty = false;

        AtlasTex = new AtlasTexture();
        DetermineAtlasRegion(PieceType);
        
        PiecePosition = new Godot.Vector2((float)char.GetNumericValue(position[0]), (float)char.GetNumericValue(position[1]));
              
    }

    public Piece(bool isEmpty)
    {
        this.IsEmpty = isEmpty;
    }

    public override void _Ready()
    {
       
        
    }

    public override void _Process(double delta)
    {
        
    }

    public override string ToString()
    {
        if (!IsEmpty) {return PieceColor + PieceType;}
        else {return "Empty";}
    }

    public Godot.Vector2 GetPiecePosition()
    {
        return PiecePosition;
    }

    public void SetPiecePosition(Godot.Vector2 position)
    {
        PiecePosition = position;
    }

    public string GetStrPiecePosition()
    {
        return PiecePosition.X.ToString() + PiecePosition.Y.ToString();
    }

    public void DetermineAtlasRegion(string pieceType)
    {

        AtlasTex.Atlas = ResourceLoader.Load<CompressedTexture2D>("res://ChessPiecesArray.png");
        int YStart = 0;
        if (PieceColor == "W")
        {
            YStart = 60;
        }
        
        string TypeCheck = pieceType.ToUpper();

        switch (TypeCheck)
        {
            case "Q":
                AtlasTex.Region = new Rect2(0, YStart, 60, 60);
                break;
            case "K":
                AtlasTex.Region = new Rect2(60, YStart, 60, 60);
                break;
            case "R":
                AtlasTex.Region = new Rect2(120, YStart, 60, 60);
                break;
            case "N":
                AtlasTex.Region = new Rect2(180, YStart, 60, 60);
                break;
            case "B":
                AtlasTex.Region = new Rect2(240, YStart, 60, 60);
                break;
            case "P":
                AtlasTex.Region = new Rect2(300, YStart, 60, 60);
                break;
            case " ":
                break;
        }

    }
}
