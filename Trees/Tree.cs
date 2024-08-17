using Godot;
using System;
using System.Diagnostics;

public partial class Tree : Node
{
	public int Leaves; //how many leaves are on the tree

	[Export]
	public int MaxLeaves; //maximum leaves on a tree

	[Export]
	public int RegenerationRate; //per second

	Timer regenTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		regenTimer = GetNode<Timer>("RegenTimer");

		regenTimer.Paused = false;

		Leaves = 0;

		regenTimer.Timeout += OnLeafTimerTimeout;
	}

	public void OnLeafTimerTimeout(){
		Leaves += RegenerationRate;

		if (Leaves > MaxLeaves){
			Leaves = MaxLeaves;
		}

		Debug.WriteLine(Leaves);
	}



	// // Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
