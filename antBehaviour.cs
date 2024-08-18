using Godot;
using System;
using System.Diagnostics;

public partial class antBehaviour : PathFollow3D
{
	[Export]
	public Anthill anthill;

	public double cargo;
	[Export] public Tree targetTree;

	public override void _Ready()
	{
		Progress = 0;
	}

	public void chooseTarget(){
		var rng = new RandomNumberGenerator();
		int targetIndex = rng.RandiRange(0,anthill.targetTrees.Length-1);

		targetTree = anthill.targetTrees[targetIndex];
		GetParent()?.RemoveChild(this);
		anthill.paths[targetIndex].AddChild(this);

		Progress = 0;
	}

	public override void _Process(double delta)
	{
		double moveDistance = anthill.GetStat(Anthill.Stat.AntSpeed).GetValue()*delta;

		Progress += (float)moveDistance;

		if((Position - GetParent<Path3D>().Curve.GetPointPosition(3)).Length() < moveDistance && cargo==0){
			//take leaf
			cargo = targetTree.TryTakeLeaf(anthill.GetStat(Anthill.Stat.AntCarryCapacity).GetValue());
		}
		else if(ProgressRatio==1){
			// deposit leaf // empty cargo
			anthill.Deliver(cargo);
			cargo = 0;

			chooseTarget();


		}
	}
}
