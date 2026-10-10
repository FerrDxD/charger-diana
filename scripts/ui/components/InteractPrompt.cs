using Godot;

/// <summary>
/// Small interaction prompt below reticle following DESIGN.md.
/// VT323 font, bone text, 2px dark outline.
/// </summary>
public partial class InteractPrompt : Label
{
	public override void _Ready()
	{
		Visible = false;
	}

	/// <summary>Set prompt text (e.g. "[E] Buka"). Empty hides it.</summary>
	public void SetPrompt(string text)
	{
		Text = text ?? "";
		Visible = !string.IsNullOrEmpty(Text);
	}
}
