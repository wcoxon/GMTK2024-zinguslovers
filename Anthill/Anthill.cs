using Godot;
using System;
using Godot.Collections;


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

	[Export] public int GoalAntNumber = 1000;

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

		Worker instance = antScene.Instantiate<Worker>();
		instance.Position = antSpawningPos;
		instance.anthill = this;

		//random variations

		RandomNumberGenerator rng = new RandomNumberGenerator();

		instance.Scale = Vector3.One * rng.RandfRange(0.7f, 1f);

		var AntColour = Color.FromHsv(rng.RandfRange(0.02f, 0.07f), rng.RandfRange(0.52f, 0.72f), rng.RandfRange(0.75f, 0.95f));
		MeshInstance3D body = instance.GetNode<MeshInstance3D>("Ants/AntBody");
		body.SetInstanceShaderParameter("albedo",AntColour);
		MeshInstance3D legs = instance.GetNode<MeshInstance3D>("Ants/AntLegs");
		legs.SetInstanceShaderParameter("albedo",AntColour);
		
		//set it on a trail
		instance.chooseTarget();

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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		nextAnt -= delta;
		if (nextAnt < 0) {
			SpawnAnt();
			nextAnt = 60f/GetStat(Stat.AntBreedings).GetValue();
		}
	}
}
