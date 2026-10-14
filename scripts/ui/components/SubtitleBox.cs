using Godot;

/// <summary>
/// Subtitle box component following DESIGN.md.
/// VT323 font, bone text, 2px dark outline, speaker name colored by token:
/// Diana = battery_ok (#3E7CB1), OSIS (Cecillia/Adam/Ferdi) = battery_low (#D9A520).
/// No background panel.
/// </summary>
public partial class SubtitleBox : RichTextLabel
{
	private const string HexDiana = "#3E7CB1";
	private const string HexOsis = "#D9A520";
	private const string HexBone = "#D8D3C4";

	private int _currentSubtitleToken = 0;

	public override void _Ready()
	{
		BbcodeEnabled = true;
		FitContent = true;
		ScrollActive = false;
		Text = "";
		Visible = false;
		MouseFilter = MouseFilterEnum.Ignore;
		ZIndex = 20;
	}

	/// <summary>Display a subtitle line for the given duration in seconds.</summary>
	public void ShowSubtitle(string speaker, string text, float duration)
	{
		if (string.IsNullOrEmpty(text))
		{
			ClearSubtitle();
			return;
		}

		string formattedText;
		if (string.IsNullOrEmpty(speaker))
		{
			formattedText = $"[center][color={HexBone}]{text}[/color][/center]";
		}
		else
		{
			string speakerColorHex = GetSpeakerColorHex(speaker);
			formattedText = $"[center][color={speakerColorHex}]{speaker}:[/color] [color={HexBone}]{text}[/color][/center]";
		}

		Text = formattedText;
		Visible = true;

		int token = ++_currentSubtitleToken;
		if (duration > 0f)
		{
			GetTree().CreateTimer(duration).Timeout += () =>
			{
				if (_currentSubtitleToken == token)
				{
					ClearSubtitle();
				}
			};
		}
	}

	/// <summary>Immediately clear and hide subtitle.</summary>
	public void ClearSubtitle()
	{
		_currentSubtitleToken++;
		Text = "";
		Visible = false;
	}

	private static string GetSpeakerColorHex(string speaker)
	{
		string lower = speaker.Trim().ToLowerInvariant();
		if (lower.Contains("diana"))
		{
			return HexDiana;
		}

		// Cecillia, Adam, Ferdi, OSIS
		return HexOsis;
	}
}
