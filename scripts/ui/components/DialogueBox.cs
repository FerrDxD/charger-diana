using Godot;

/// <summary>
/// Pixel dialogue box component following DESIGN.md.
/// 2px bone border over ink background, speaker name on top (colored by token), VT323 text.
/// </summary>
public partial class DialogueBox : Panel
{
	private const string HexDiana = "#3E7CB1";
	private const string HexOsis = "#D9A520";

	private Label _speakerLabel;
	private Label _dialogueLabel;

	public override void _Ready()
	{
		_speakerLabel = GetNode<Label>("MarginContainer/VBoxContainer/SpeakerLabel");
		_dialogueLabel = GetNode<Label>("MarginContainer/VBoxContainer/DialogueLabel");
	}

	/// <summary>Set speaker and dialogue text with appropriate speaker token color.</summary>
	public void SetDialogue(string speaker, string dialogue)
	{
		if (_speakerLabel == null)
		{
			_speakerLabel = GetNode<Label>("MarginContainer/VBoxContainer/SpeakerLabel");
		}
		if (_dialogueLabel == null)
		{
			_dialogueLabel = GetNode<Label>("MarginContainer/VBoxContainer/DialogueLabel");
		}

		_speakerLabel.Text = string.IsNullOrEmpty(speaker) ? "" : $"{speaker}:";
		_dialogueLabel.Text = dialogue ?? "";

		string hexColor = GetSpeakerColorHex(speaker);
		_speakerLabel.AddThemeColorOverride("font_color", Color.FromHtml(hexColor));
	}

	private static string GetSpeakerColorHex(string speaker)
	{
		if (string.IsNullOrEmpty(speaker)) return "#D8D3C4";
		string lower = speaker.Trim().ToLowerInvariant();
		if (lower.Contains("diana"))
		{
			return HexDiana;
		}
		return HexOsis;
	}
}
