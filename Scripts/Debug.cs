using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;

public partial class Debug : Node
{
    int WindowSize {get; set;}
    
    public Dictionary<object, string> TrackedObjects = new Dictionary<object, string>();

    public VBoxContainer DebugContainer;

    public Debug(int windowSize)
    {
        WindowSize = windowSize;
    }

    public override void _Ready()
    {
        DebugContainer = new VBoxContainer();
        AddChild(DebugContainer);
        DebugContainer.GlobalPosition = new Godot.Vector2(WindowSize, 10);
    }
    

    public void AddTrackedObject(object obj, string propertyName)
    {
        
        

        TrackedObjects.TryAdd(obj, propertyName);
        Label NewLabel = new Label
        {
            Text = $"{propertyName}: {obj}"
        };
        DebugContainer.AddChild(NewLabel);

    }

    public void TrackMousePosition()
    {
        AddTrackedObject(Tools.GetMousePosition(this), "Mouse Position");
        AddTrackedObject(Tools.GetCellFromMouse(this), "Cell");
        AddTrackedObject(Tools.GetVCellFromMouse(this), "VCell");
    }

    public void UpdateTrackedObjects()
    {

        


        foreach (KeyValuePair<object, string> trackedObject in TrackedObjects)
        {
            PropertyInfo[] props = trackedObject.Key.GetType().GetProperties();
            for (int i = 0; i < DebugContainer.GetChildren().Count; i++)
            {
                var child = DebugContainer.GetChild(i);
                if (child is Label label && label.Text.Contains(trackedObject.Value.ToString()))
                {
                    label.Text = $"{trackedObject.Value}: {trackedObject.Key}";
                }
                
            }
        }
    }
}
