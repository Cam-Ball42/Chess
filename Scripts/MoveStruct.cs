using Godot;
using System;
using System.Dynamic;


public struct Move
{
    public Move(Vector2 from, Vector2 to, Piece piece)
    {
        From = from;
        To = to;
        Piece = piece;

    }

    public Vector2 From { get; }
    public Vector2 To { get; }
    public Piece Piece { get; }



}