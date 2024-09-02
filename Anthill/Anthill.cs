using Godot;
using System;
using Godot.Collections;
using System.Diagnostics;


public partial class Anthill : Node3D
{
	public enum Stat
	{
		AntSpeed, AntCarryCapacity, NewAnt, AntBreedings,
		PlayerSpeed, PlayerJump, CollectableValue
	}

	[Export] public double leafMass;

	// Setup
	[Export] public PackedScene antScene;
	[Export] public Vector3 antSpawningPos;
	[Export] public Array<Tree> targetTrees;

	[Export] public uint numAnts = 1;
	
	private TutorialUI tutorialUI;

	[Export] public int GoalAntNumber = 5000;

	private double nextAnt;


	public AnthillStat GetStat(Stat stat) {
		return GetNode<AnthillStat>(stat.ToString());
	}

	public void removeTrail(Tree target){
		if(!targetTrees.Contains(target)) return;
		targetTrees.Remove(target);
		target.pathTrail.QueueFree();
		target.pathTrail=null;
	}
	
	public void Upgrade(Stat stat) {

		tutorialUI.completedHint(TutorialUI.Hint.upgrades);

		AnthillStat upgrade = GetStat(stat);
		leafMass -= upgrade.GetCost();
		
		upgrade.Upgrade();

		switch(stat){
			case Stat.NewAnt:
				for(int x = 0; x < 50; x++) SpawnAnt();
				break;
			case Stat.AntBreedings:
				nextAnt = Math.Min(nextAnt, 60f/GetStat(Stat.AntBreedings).GetValue());
				break;
				
			
		}
	}

	public void Deliver(double mass)
	{
		leafMass += mass;
	}
	
	private void SpawnAnt()
	{
		numAnts++;

		var scene = (PackedScene)GD.Load("res://Worker/worker.tscn");
		var instance = scene.Instantiate();

		instance.Set("position",antSpawningPos);
		instance.Set("anthill", this);

		//random variation happens in the GDscript worker's ready function at the moment

		//set it on a trail
		instance.Call("chooseTarget");

		if	(numAnts == GoalAntNumber){
			EventManager.instance.EmitSignal(EventManager.SignalName.BeginEndingCutscene);
		}
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tutorialUI = Owner.GetNode<TutorialUI>("Control/TutorialUI");
		nextAnt = 60f/GetStat(Stat.AntBreedings).GetValue();

	}

	public double moveDistance;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		moveDistance = GetStat(Stat.AntSpeed).GetValue()*delta;
		nextAnt -= delta;
		if (nextAnt < 0) {
			SpawnAnt();
			nextAnt = 60f/GetStat(Stat.AntBreedings).GetValue();
		}
	}
}
