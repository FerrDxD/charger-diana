using Godot;

/// <summary>
/// Worn note sheet component following DESIGN.md.
/// Paper bone background (#D8D3C4), slight rotation, tape at corner, Special Elite typewriter font.
/// </summary>
public partial class NoteSheet : Panel
{
	private RichTextLabel _contentLabel;

	public override void _Ready()
	{
		_contentLabel = GetNode<RichTextLabel>("Content");
	}

	/// <summary>Set note content text.</summary>
	public void SetContent(string bbcodeText)
	{
		if (_contentLabel == null)
		{
			_contentLabel = GetNode<RichTextLabel>("Content");
		}
		_contentLabel.Text = bbcodeText ?? "";
	}
}
