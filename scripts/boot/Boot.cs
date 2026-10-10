using Godot;
using System;
using System.Threading.Tasks;

public partial class Boot : Control
{
    public override async void _Ready()
    {
        // Wait for 3 seconds or input
        await Task.Delay(3000);

        GoToWarning();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
        {
            GoToWarning();
        }
        else if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            GoToWarning();
        }
    }

    private bool _transitioned = false;

    private void GoToWarning()
    {
        if (_transitioned) return;
        _transitioned = true;
        
        if (GetNodeOrNull("/root/SceneManager") != null)
        {
            GetNode("/root/SceneManager").Call("GoTo", (int)GameFlow.ContentWarning);
        }
        else
        {
            GetTree().ChangeSceneToFile("res://scenes/boot/ContentWarning.tscn");
        }
    }
}
