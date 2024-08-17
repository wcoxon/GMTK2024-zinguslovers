using Godot;
using System;


public partial class Anthill : Node3D
{
	public enum Stat
	{
		AntSpeed, AntCarryCapacity, NewAnt, AntBreedings
	}

	[Export] public double leafMass;
	public RandomNumberGenerator rng;

	// Setup
	[Export] public PackedScene antScene;
	[Export] public Vector3 antSpawningPos;

	[Export] public Path3D[] paths;
	[Export] public Tree[] targetTrees;

	[Export] public uint numAnts = 1;

	private double nextAnt;

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
		if (stat == Stat.AntBreedings) {
			nextAnt = Math.Min(nextAnt, GetStat(Stat.AntBreedings).GetValue());
		}
	}

	public void Deliver(double mass)
	{
		leafMass += mass;
	}
	private void SpawnAnt()
	{
		numAnts++;
		antBehaviour instance = antScene.Instantiate<antBehaviour>();
		instance.Position = antSpawningPos;
		instance.anthill = this;

		int targetIndex = rng.RandiRange(0,targetTrees.Length-1);

		instance.targetTree = targetTrees[targetIndex];
		paths[targetIndex].AddChild(instance);
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rng = new RandomNumberGenerator();
		nextAnt = GetStat(Stat.AntBreedings).GetValue();

	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		nextAnt -= delta;
		if (nextAnt < 0) {
			SpawnAnt();
			nextAnt = GetStat(Stat.AntBreedings).GetValue();
		}
	}
}
