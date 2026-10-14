using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Audio manager with bus routing and procedural placeholder fallback.
/// Handles 2D/3D SFX, Ambience, Ghost effects, VO with subtitles, and UI sounds.
/// </summary>
public partial class AudioManager : Node
{
	public static AudioManager Instance { get; private set; }

	// Buses
	public const string BusMaster = "Master";
	public const string BusSfx = "SFX";
	public const string BusAmbience = "Ambience";
	public const string BusGhost = "Ghost";
	public const string BusVo = "VO";
	public const string BusUi = "UI";

	// 3D Audio
	[Export] public float SfxMaxDistance { get; set; } = 25f;
	[Export] public float GhostMaxDistance { get; set; } = 30f;

	private AudioStreamPlayer _ambiencePlayer;
	private AudioStreamPlayer _uiPlayer;
	private AudioStreamPlayer _voPlayer;

	// Cached procedural placeholder streams
	private readonly Dictionary<string, AudioStream> _placeholderStreams = new();

	private EventBus _eventBus;

	public override void _Ready()
	{
		Instance = this;
		ProcessMode = ProcessModeEnum.Always;
		_eventBus = GetNode<EventBus>("/root/EventBus");

		EnsureBusesExist();

		_ambiencePlayer = new AudioStreamPlayer { Bus = BusAmbience };
		_uiPlayer = new AudioStreamPlayer { Bus = BusUi };
		_voPlayer = new AudioStreamPlayer { Bus = BusVo };

		AddChild(_ambiencePlayer);
		AddChild(_uiPlayer);
		AddChild(_voPlayer);

		_voPlayer.Finished += OnVoFinished;
	}

	private void EnsureBusesExist()
	{
		string[] buses = { BusSfx, BusAmbience, BusGhost, BusVo, BusUi };
		foreach (var busName in buses)
		{
			if (AudioServer.GetBusIndex(busName) == -1)
			{
				AudioServer.AddBus();
				int index = AudioServer.BusCount - 1;
				AudioServer.SetBusName(index, busName);
				AudioServer.SetBusSend(index, BusMaster);
			}
		}
	}

	/// <summary>Play a sound effect at an optional 3D location, or 2D if null.</summary>
	public void PlaySfx(string soundName, Vector3? position = null, float pitchScale = 1.0f)
	{
		AudioStream stream = LoadOrGenerateStream(soundName, "sfx");

		if (position.HasValue)
		{
			var player3D = new AudioStreamPlayer3D
			{
				Stream = stream,
				Bus = BusSfx,
				PitchScale = pitchScale,
				MaxDistance = SfxMaxDistance
			};
			GetTree().Root.AddChild(player3D);
			player3D.GlobalPosition = position.Value;
			player3D.Play();
			player3D.Finished += () => player3D.QueueFree();
		}
		else
		{
			var player = new AudioStreamPlayer
			{
				Stream = stream,
				Bus = BusSfx,
				PitchScale = pitchScale
			};
			GetTree().Root.AddChild(player);
			player.Play();
			player.Finished += () => player.QueueFree();
		}
	}

	/// <summary>Play ghost sound in 3D with custom volume and pitch.</summary>
	public void PlayGhostSound(string soundName, Vector3 position, float volumeDb = 0f, float pitchScale = 1.0f)
	{
		AudioStream stream = LoadOrGenerateStream(soundName, "ghost");
		var player3D = new AudioStreamPlayer3D
		{
			Stream = stream,
			Bus = BusGhost,
			GlobalPosition = position,
			VolumeDb = volumeDb,
			PitchScale = pitchScale,
			MaxDistance = GhostMaxDistance
		};
		GetTree().Root.AddChild(player3D);
		player3D.Play();
		player3D.Finished += () => player3D.QueueFree();
	}

	/// <summary>Play UI sound effect.</summary>
	public void PlayUi(string soundName)
	{
		_uiPlayer.Stream = LoadOrGenerateStream(soundName, "ui");
		_uiPlayer.Play();
	}

	private Tween _duckingTween;
	private float _normalAmbienceDb = 0f;
	private float _duckedAmbienceDb = -12f;

	public void PlayVo(string voKey, string speaker, string subtitleText, float duration = 3.0f, float pitchScale = 1.0f, string busName = BusVo)
	{
		bool isRadio = busName.StartsWith("HT_");
		var vo = new VoLine 
		{ 
			Id = voKey, 
			Speaker = speaker, 
			SubtitleText = subtitleText, 
			Radio = isRadio,
			Stream = LoadOrGenerateStream(voKey, "vo"),
			Duration = duration
		};
		_voPlayer.PitchScale = pitchScale;
		PlayVo(vo);
	}

	/// <summary>Play voice line from Resource and trigger subtitle with ducking.</summary>
	public void PlayVo(VoLine vo)
	{
		if (vo == null) return;

		string targetBus = BusVo;
		if (vo.Radio)
		{
			string spkr = vo.Speaker?.ToLower();
			if (spkr == "cecillia") targetBus = "HT_Cecillia";
			else if (spkr == "adam") targetBus = "HT_Adam";
			else if (spkr == "ferdi") targetBus = "HT_Ferdi";
		}

		_voPlayer.Bus = AudioServer.GetBusIndex(targetBus) >= 0 ? targetBus : BusVo;

		float duration = vo.Duration > 0 ? vo.Duration : 2.0f;
		if (vo.Stream != null)
		{
			_voPlayer.Stream = vo.Stream;
			_voPlayer.Play();
			ApplyDucking(true);
			float streamLen = (float)vo.Stream.GetLength();
			// If real audio stream exists (length > 1.0s), use it; otherwise fallback to vo.Duration or text length
			if (streamLen > 1.0f)
				duration = streamLen;
			else if (vo.Duration > 0)
				duration = vo.Duration;
			else
				duration = Mathf.Max(2.5f, (vo.SubtitleText?.Length ?? 0) * 0.06f);
		}
		else
		{
			duration = vo.Duration > 0 ? vo.Duration : Mathf.Max(2.5f, (vo.SubtitleText?.Length ?? 0) * 0.06f);
			GetTree().CreateTimer(duration).Timeout += () => ApplyDucking(false);
		}

		_eventBus?.EmitSignal(EventBus.SignalName.SubtitleRequested, vo.Speaker ?? "", vo.SubtitleText ?? "", duration);
	}

	private void OnVoFinished()
	{
		ApplyDucking(false);
	}

	private void ApplyDucking(bool duck)
	{
		int ambBus = AudioServer.GetBusIndex(BusAmbience);
		if (ambBus < 0) return;

		_duckingTween?.Kill();
		_duckingTween = CreateTween();
		
		float targetDb = duck ? _duckedAmbienceDb : _normalAmbienceDb;
		_duckingTween.TweenMethod(Callable.From<float>(v => AudioServer.SetBusVolumeDb(ambBus, v)), 
								  AudioServer.GetBusVolumeDb(ambBus), 
								  targetDb, 
								  0.5f);
	}

	public void PlayStinger(string id)
	{
		AudioStream stream = LoadOrGenerateStream(id, "sfx");
		var player = new AudioStreamPlayer { Stream = stream, Bus = BusSfx };
		GetTree().Root.AddChild(player);
		player.Play();
		player.Finished += () => player.QueueFree();
	}

	/// <summary>Play looping ambience track.</summary>
	public void PlayAmbience(string ambienceName)
	{
		_ambiencePlayer.Stream = LoadOrGenerateStream(ambienceName, "ambience");
		if (!_ambiencePlayer.Playing)
		{
			_ambiencePlayer.Play();
		}
	}

	public void StopAmbience()
	{
		_ambiencePlayer.Stop();
	}

	private AudioStream LoadOrGenerateStream(string name, string category)
	{
		string path = $"res://assets/audio/{category}/{name}.ogg";
		if (ResourceLoader.Exists(path))
		{
			return GD.Load<AudioStream>(path);
		}

		if (_placeholderStreams.TryGetValue(name, out var cached))
			return cached;

		var procedural = GenerateProceduralPlaceholder(name, category);
		_placeholderStreams[name] = procedural;
		return procedural;
	}

	/// <summary>Generate lightweight procedural tone as placeholder so game has audio even without assets.</summary>
	private static AudioStreamWav GenerateProceduralPlaceholder(string name, string category)
	{
		int sampleRate = 22050;
		float duration = category switch
		{
			"ui" => 0.08f,
			"sfx" => 0.25f,
			"ghost" => 0.8f,
			"vo" => 0.4f,
			"ambience" => 1.5f,
			_ => 0.2f
		};

		int totalSamples = (int)(sampleRate * duration);
		byte[] buffer = new byte[totalSamples];

		float freq = category switch
		{
			"ui" => 800f,
			"ghost" => 150f,
			"sfx" => 220f,
			"vo" => 350f,
			_ => 100f
		};

		for (int i = 0; i < totalSamples; i++)
		{
			float t = (float)i / sampleRate;
			float envelope = 1f - (float)i / totalSamples; // simple fade out
			float wave = Mathf.Sin(t * freq * Mathf.Tau);

			if (category == "ghost")
			{
				// eerie modulated vibrato
				wave += 0.5f * Mathf.Sin(t * (freq * 1.5f) * Mathf.Tau);
			}

			byte sampleVal = (byte)(128 + (int)(wave * envelope * 45f));
			buffer[i] = sampleVal;
		}

		var wav = new AudioStreamWav
		{
			Format = AudioStreamWav.FormatEnum.Format8Bits,
			MixRate = sampleRate,
			Stereo = false,
			Data = buffer
		};

		if (category == "ambience")
		{
			wav.LoopMode = AudioStreamWav.LoopModeEnum.Forward;
			wav.LoopEnd = totalSamples;
		}

		return wav;
	}
}
