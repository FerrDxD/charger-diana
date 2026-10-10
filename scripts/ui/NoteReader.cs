using Godot;

/// <summary>
/// Note reader for viewing collected documents.
/// Shows paper-styled document with Special Elite font.
/// Following DESIGN.md specifications.
/// </summary>
public partial class NoteReader : Control
{
	private Label _titleLabel;
	private RichTextLabel _contentLabel;
	private Label _closePrompt;
	private EventBus _eventBus;

	public override void _Ready()
	{
		_titleLabel = GetNode<Label>("TopBar/TitleLabel");
		_contentLabel = GetNode<RichTextLabel>("CenterContainer/PaperContainer/ContentLabel");
		_closePrompt = GetNode<Label>("Footer/ClosePrompt");
		_eventBus = GetNode<EventBus>("/root/EventBus");

		_eventBus.NoteCollected += OnNoteCollected;

		Visible = false;
	}

	public override void _Input(InputEvent @event)
	{
		if (Visible && @event.IsActionPressed("interact"))
		{
			Close();
		}
	}

	public void ShowNote(StringName noteId, string title, string content)
	{
		_titleLabel.Text = $"> DOKUMEN TEMUAN: {title.ToUpper()}";
		_contentLabel.Text = content;

		Visible = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		GetTree().Paused = true;
	}

	private void Close()
	{
		Visible = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		GetTree().Paused = false;
	}

	private void OnNoteCollected(StringName noteId)
	{
		// Find note data and show it
		// Ponytail: implement proper note data lookup from resources
		GD.Print($"Note collected: {noteId}");
	}
}
