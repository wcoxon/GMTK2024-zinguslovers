using Godot;
using System;
public partial class Grass : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		Rotate(Vector3.Up,2*Mathf.Pi*rng.Randf());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
