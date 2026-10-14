using Godot;
using System;
using System.Threading.Tasks;

public enum GameFlow { Boot, ContentWarning, MainMenu, Prolog, Gameplay, EndingA, EndingB, EndingC, Credits }

public partial class SceneManager : Node
{
    private ScreenFader _screenFader;
    public GameFlow CurrentFlow { get; private set; } = GameFlow.Boot;

    [Signal]
    public delegate void FlowChangedEventHandler(int flow);

    public override void _Ready()
    {
        var faderScene = GD.Load<PackedScene>("res://scenes/ui/components/ScreenFader.tscn");
        if (faderScene != null)
        {
            var inst = faderScene.Instantiate();
            _screenFader = inst as ScreenFader;
            if (_screenFader != null)
                AddChild(_screenFader);
            else
                AddChild(inst);
        }
        // Ensure we survive scene changes (autoload already does, but keep fader alive)
        ProcessMode = ProcessModeEnum.Always;
    }

    public void GoTo(GameFlow target)
    {
        // Fire-and-forget with error logging — async void loses exceptions otherwise
        _ = GoToAsync(target);
    }

    private async Task GoToAsync(GameFlow target)
    {
        try
        {
            if (_screenFader != null)
                await _screenFader.FadeIn(0.5f);

            CurrentFlow = target;
            EmitSignal(SignalName.FlowChanged, (int)target);

            string scenePath = GetScenePathForFlow(target);
            if (!string.IsNullOrEmpty(scenePath))
            {
                var err = GetTree().ChangeSceneToFile(scenePath);
                if (err != Error.Ok)
                    GD.PushError($"[SceneManager] ChangeSceneToFile failed: {scenePath} err={err}");
                else
                    GD.Print($"[SceneManager] -> {target} : {scenePath}");
            }
            // FadeOut must run in new scene tree — defer one frame so new scene's _Ready runs
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
            if (_screenFader != null)
                await _screenFader.FadeOut(0.5f);
        }
        catch (Exception ex)
        {
            GD.PushError($"[SceneManager] GoTo({target}) failed: {ex}");
        }
    }

    public void GoToScene(string scenePath)
    {
        _ = GoToSceneAsync(scenePath);
    }

    private async Task GoToSceneAsync(string scenePath)
    {
        try
        {
            if (_screenFader != null)
                await _screenFader.FadeIn(0.5f);

            if (!string.IsNullOrEmpty(scenePath))
            {
                var err = GetTree().ChangeSceneToFile(scenePath);
                if (err != Error.Ok)
                    GD.PushError($"[SceneManager] ChangeSceneToFile failed: {scenePath} err={err}");
                else
                    GD.Print($"[SceneManager] -> scene {scenePath}");
            }
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
            if (_screenFader != null)
                await _screenFader.FadeOut(0.5f);
        }
        catch (Exception ex)
        {
            GD.PushError($"[SceneManager] GoToScene({scenePath}) failed: {ex}");
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
        GD.Print("[SceneManager] StartNewGame -> Prolog");
        GoTo(GameFlow.Prolog);
    }

    public void ContinueGame()
    {
        GoTo(GameFlow.Gameplay);
    }
}
