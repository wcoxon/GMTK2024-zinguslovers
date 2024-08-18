using Godot;
using System;

public partial class TreeTrailControl : Button
{
	CharacterBody3d player;
	string[] names;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		names = Text.Split('/');
		Text = names[0];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
