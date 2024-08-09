using Godot;
using System;
using System.Numerics;

public static class Tools : Object
{


    public static int WindowSize { get;  set; }

    


    

    public static Godot.Vector2 PositionToCell(Godot.Vector2 position)
	{
		int x = (int)position.X / (WindowSize / 8);
		int y = (int)position.Y / (WindowSize / 8);

		return new Godot.Vector2 (x, y);	
	}

    public static Godot.Vector2 GetMousePosition(Node node)
	{
		Godot.Vector2 mousePosition = node.GetViewport().GetMousePosition();
		return mousePosition; 
	}

	public static Godot.Vector2 GetCellFromMouse(Node node)
	{
		return PositionToCell(GetMousePosition(node));
	}

	public static Godot.Vector2 GetVCellFromMouse(Node node)
	{
		return new Godot.Vector2(GetMousePosition(node).Y / (WindowSize / 8), GetMousePosition(node).X / (WindowSize / 8));
	}


}
