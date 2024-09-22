using Godot;
using System;

public partial class InputHandler : Node
{

    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton)
		{


            if (mouseButton.Pressed)
            {
                InputState.IsClicking = true;
                InputState.SetCurMouseClickPos(Tools.GetMousePosition(this));
                
            }
            else
            {
                InputState.IsClicking = false;
                InputState.SetLastMouseClickPos(Tools.GetMousePosition(this));
            }

            if (InputState.CurrentMouseClickPosition != InputState.LastMouseClickPosition && mouseButton.Pressed)
            {
                InputState.IsDragging = true;
                
            }
            else
            {
                InputState.IsDragging = false;
            }
                
		}

        if (@event is InputEventKey key)
        {
            if (key.Pressed)
            {
                if (key.Keycode == Key.Right)
                {
                    InputState.RightPressed = true;
                }
                if (key.Keycode == Key.Left)
                {
                    InputState.LeftPressed = true;
                }
            }

            if (!key.Pressed)
            {
                if (key.Keycode == Key.Right)
                {
                    InputState.RightPressed = false;
                }
                if (key.Keycode == Key.Left)
                {
                    InputState.LeftPressed = false;
                }
            }
	    }
    }

}
