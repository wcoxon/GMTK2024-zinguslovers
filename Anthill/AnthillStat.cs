using Godot;
using System;

public partial class AnthillStat : Node
{
	[Export] public double baseValue;
	[Export] public double incrementValue;
	[Export] public uint level = 1;
	[Export] public double baseCost;
	[Export] public double incrementCost;

	public double GetValue() {
		return baseValue + incrementValue * level;
	}

	public double GetCost() {
		return baseCost + incrementCost * level;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
