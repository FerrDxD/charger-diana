using Godot;
using System;
using System.Threading.Tasks;

public enum GameFlow { Boot, ContentWarning, MainMenu, Prolog, Gameplay, EndingA, EndingB, EndingC, Credits }

public partial class SceneManager : Node
{
    private Node _screenFader;
    public GameFlow CurrentFlow { get; private set; } = GameFlow.Boot;

    [Signal]
    public delegate void FlowChangedEventHandler(int flow); // Using int since custom enums in signals can be tricky in Godot C# without wrappers

    public override void _Ready()
    {
        // Try to instantiate ScreenFader if it doesn't exist
        var faderScene = GD.Load<PackedScene>("res://scenes/ui/components/ScreenFader.tscn");
        if (faderScene != null)
        {
            _screenFader = faderScene.Instantiate();
            AddChild(_screenFader);
        }
    }

    public async void GoTo(GameFlow target)
    {
        var fader = _screenFader as ScreenFader;

        if (fader != null)
        {
            await fader.FadeIn(0.5f);
        }

        CurrentFlow = target;
        EmitSignal(SignalName.FlowChanged, (int)target);

        string scenePath = GetScenePathForFlow(target);
        if (!string.IsNullOrEmpty(scenePath))
        {
            GetTree().ChangeSceneToFile(scenePath);
        }

        if (fader != null)
        {
            await fader.FadeOut(0.5f);
        }
    }

    public async void GoToScene(string scenePath)
    {
        var fader = _screenFader as ScreenFader;

        if (fader != null)
        {
            await fader.FadeIn(0.5f);
        }

        if (!string.IsNullOrEmpty(scenePath))
        {
            GetTree().ChangeSceneToFile(scenePath);
        }

        if (fader != null)
        {
            await fader.FadeOut(0.5f);
        }
    }

    private string GetScenePathForFlow(GameFlow flow)
    {
        switch (flow)
        {
            case GameFlow.ContentWarning: return "res://scenes/boot/ContentWarning.tscn";
            case GameFlow.MainMenu: return "res://scenes/menu/MainMenu.tscn";
            case GameFlow.Prolog: return "res://scenes/prolog/PrologRoom.tscn";
            case GameFlow.Gameplay: return "res://scenes/levels/Chapter1.tscn";
            case GameFlow.EndingA: return "res://scenes/endings/EndingA.tscn";
            case GameFlow.EndingB: return "res://scenes/endings/EndingB.tscn";
            case GameFlow.EndingC: return "res://scenes/endings/GameOver.tscn";
            case GameFlow.Credits: return "res://scenes/ui/Credits.tscn";
            default: return "";
        }
    }

    public void StartNewGame()
    {
        GoTo(GameFlow.Prolog);
    }

    public void ContinueGame()
    {
        GoTo(GameFlow.Gameplay);
    }
}
