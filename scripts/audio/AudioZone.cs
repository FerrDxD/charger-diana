using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class AudioZone : Area3D
{
    [Export] public Godot.Collections.Array<AudioStream> AmbienceLayers { get; set; } = new Godot.Collections.Array<AudioStream>();
    
    private List<AudioStreamPlayer> _players = new List<AudioStreamPlayer>();
    private Tween _fadeTween;
    private bool _isActive = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        
        foreach (var stream in AmbienceLayers)
        {
            if (stream == null) continue;
            var player = new AudioStreamPlayer();
            player.Stream = stream;
            player.Bus = "Ambience";
            player.VolumeDb = -80f; // Start muted
            AddChild(player);
            _players.Add(player);
        }
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body.IsInGroup("player") && !_isActive)
        {
            _isActive = true;
            FadeIn();
        }
    }

    private void OnBodyExited(Node3D body)
    {
        if (body.IsInGroup("player") && _isActive)
        {
            _isActive = false;
            FadeOut();
        }
    }

    private void FadeIn()
    {
        _fadeTween?.Kill();
        _fadeTween = CreateTween();
        _fadeTween.SetParallel(true);
        
        foreach (var player in _players)
        {
            if (!player.Playing) player.Play();
            _fadeTween.TweenProperty(player, "volume_db", 0f, 2.0f);
        }
    }

    private void FadeOut()
    {
        _fadeTween?.Kill();
        _fadeTween = CreateTween();
        _fadeTween.SetParallel(true);
        
        foreach (var player in _players)
        {
            _fadeTween.TweenProperty(player, "volume_db", -80f, 2.0f);
        }
        
        _fadeTween.Chain().TweenCallback(Callable.From(() => {
            foreach (var player in _players)
            {
                player.Stop();
            }
        }));
    }
}
