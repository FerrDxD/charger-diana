using Godot;

/// <summary>
/// Switch for power panel puzzle (P2).
/// Toggle on/off with noise emission.
/// Used in sequence puzzle where correct order must be pressed.
/// </summary>
public partial class Switch : InteractableBase
{
	[Export] public int SwitchIndex { get; set; }
	[Export] public PowerPanel ParentPanel { get; set; }

	private bool _isOn;
	private MeshInstance3D _mesh;
	private static readonly StandardMaterial3D OnMaterial = new() { AlbedoColor = Colors.Green };
	private static readonly StandardMaterial3D OffMaterial = new() { AlbedoColor = Colors.Red };

	public override void _Ready()
	{
		base._Ready();
		PromptText = "Tekan saklar";

		_mesh = GetNodeOrNull<MeshInstance3D>("MeshInstance3D");

		if (ParentPanel == null)
		{
			ParentPanel = GetParent() as PowerPanel ?? GetParent()?.GetParent() as PowerPanel;
		}

		UpdateVisual();
	}

	public override void Interact(Node3D player)
	{
		base.Interact(player);

		_isOn = !_isOn;
		UpdateVisual();
		AudioManager.Instance?.PlaySfx("switch_click", GlobalPosition);

		ParentPanel?.OnSwitchToggled(this);
	}

	public void SetState(bool on)
	{
		_isOn = on;
		UpdateVisual();
	}

	private void UpdateVisual()
	{
		if (_mesh != null)
		{
			_mesh.MaterialOverride = _isOn ? OnMaterial : OffMaterial;
		}
	}
}
