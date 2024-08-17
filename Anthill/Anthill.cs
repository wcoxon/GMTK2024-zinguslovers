using Godot;
using System;


public partial class Anthill : Node3D
{
	public enum Stat
	{
		AntSpeed, AntCarryCapacity, NewAnt
	}

	[Export] public double leafMass;

	// Setup
	[Export] public PackedScene antScene;
	[Export] public Vector3 antSpawningPos;

	[Export] public Path3D[] paths;

	public AnthillStat GetStat(Stat stat) {
		return GetNode<AnthillStat>(stat.ToString());
	}
	
	public void Upgrade(Stat stat) {
		AnthillStat obj = GetStat(stat);
		if (leafMass >= obj.GetCost()) {
			leafMass -= obj.GetCost();
			obj.level++;
		}
		if (stat == Stat.NewAnt) {
			SpawnAnt();
		}
	}

	public void Deliver(double mass)
	{
		leafMass += mass;
	}
	private void SpawnAnt()
	{
		antBehaviour instance = antScene.Instantiate<antBehaviour>();
		instance.Position = antSpawningPos;
		instance.anthill = this;
		paths[0].AddChild(instance);
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
