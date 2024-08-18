using Godot;
using System;

public partial class AnthillTrailControl : Button
{
	Player player;
	string[] names;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = (Owner as AnthillUI).player;
		names = Text.Split('/');
		Text = names[0];
		Disabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
