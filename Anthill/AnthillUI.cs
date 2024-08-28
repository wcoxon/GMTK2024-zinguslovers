using Godot;
using System;

public partial class AnthillUI : Panel
{
	[Export] public Anthill anthill;
	[Export] public Player player;

	private Label _infolabel;
	private string _labelformat;

	private Button _depositbutton;
	private string _depositformat;

	public bool hidden = false;


	public void UpdateText(){
		_infolabel.Text = string.Format(_labelformat, anthill.leafMass);
		_depositbutton.Text = string.Format(_depositformat, player.getCargo());
	}
	public void DepositLeaves(){
		player.DeliverTo(anthill);
	}

	public void upgradePlayerSpeed(){
		player.upgradeSpeed();
	}
	public void UpdatePlayerColour(Color colour){
		player.updateColour(colour);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_infolabel = GetNode<Label>("MarginContainer/VBoxContainer/Label");
		_labelformat = _infolabel.Text;

		_depositbutton = GetNode<Button>("MarginContainer/VBoxContainer/DepositButton");
		_depositformat = _depositbutton.Text;

		EventManager.instance.BeginEndingCutscene += () => {
			Hide();
			hidden = true;
		};

		EventManager.instance.PostEndingCutscene += () => {
			hidden = false;
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateText();

	}
}