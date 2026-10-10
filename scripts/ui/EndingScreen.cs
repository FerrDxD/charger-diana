using Godot;

/// <summary>
/// Screen shown when an ending is triggered.
/// Displays ending text and provides option to return to menu.
/// </summary>
public partial class EndingScreen : Control
{
	private Label _titleLabel;
	private Label _descriptionLabel;
	private Button _menuButton;
	private EventBus _eventBus;

	public override void _Ready()
	{
		_titleLabel = GetNode<Label>("VBoxContainer/TitleLabel");
		_descriptionLabel = GetNode<Label>("VBoxContainer/DescriptionLabel");
		_menuButton = GetNode<Button>("VBoxContainer/MenuButton");
		_eventBus = GetNode<EventBus>("/root/EventBus");

		_eventBus.EndingTriggered += OnEndingTriggered;
		_menuButton.Pressed += OnMenuButtonPressed;

		Visible = false;
	}

	private void OnEndingTriggered(StringName endingId)
	{
		Visible = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;

		switch ((string)endingId)
		{
			case "A":
				_titleLabel.Text = "Ending A: Berhasil";
				_descriptionLabel.Text = "Diana berhasil mengambil charger dan keluar dari sekolah tanpa tertangkap. Lega.";
				break;
			case "B":
				_titleLabel.Text = "Ending B: Ketahuan OSIS";
				_descriptionLabel.Text = "Diana tertangkap OSIS. \"Cuma gara-gara charger?!\" ceramah mereka mengingatkan Diana bahwa risiko yang diambil terlalu besar untuk hal sepele.";
				break;
			case "C":
				_titleLabel.Text = "Ending C: Tertangkap Kuntilanak";
				_descriptionLabel.Text = "Diana tidak bisa menghindari teror kuntilanak. Kegelapan sekolah malam ini menelan dia sepenuhnya.";
				break;
			default:
				_titleLabel.Text = "Ending Tidak Dikenal";
				_descriptionLabel.Text = "Terjadi kesalahan.";
				break;
		}
	}

	private void OnMenuButtonPressed()
	{
		GetTree().Quit();
	}
}
