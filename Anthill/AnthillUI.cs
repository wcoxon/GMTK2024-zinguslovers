using Godot;
using System;

public partial class AnthillUI : Panel
{
	[Export] public Anthill anthill;
	[Export] public Player player;

	private Label _infolabel;
	private string _labelformat;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_infolabel = GetNode<Label>("MarginContainer/VBoxContainer/Label");
		_labelformat = _infolabel.Text;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_infolabel.Text = string.Format(_labelformat, anthill.leafMass);
	}
}