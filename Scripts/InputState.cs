using Godot;
using System;

public static class InputState : Object
{
    public static Godot.Vector2 LastMouseClickPosition ;
    public static Godot.Vector2 CurrentMouseClickPosition ;

    public static Godot.Vector2 LastCellClickPosition;
    public static Godot.Vector2 CurrentCellClickPosition;

    public static bool IsClicking  {get; set;}= false;
    public static bool IsDragging {get; set;} = false;



    public static void SetCurMouseClickPos(Godot.Vector2 pos)
    {
        InputState.CurrentMouseClickPosition = pos;
        InputState.CurrentCellClickPosition = Tools.PositionToCell(pos);
    }

    public static void SetLastMouseClickPos(Godot.Vector2 pos)
    {
        InputState.LastMouseClickPosition = pos;
        InputState.LastCellClickPosition = Tools.PositionToCell(pos);
    }

    public static Godot.Vector2 GetCurCellClickPos()
    {
        return InputState.CurrentCellClickPosition;
    }

    public static Godot.Vector2 GetLastCellClickPos()
    {
        return InputState.LastCellClickPosition;
    }

    public static Godot.Vector2 GetCurMouseClickPos()
    {
        return InputState.CurrentMouseClickPosition;
    }




}