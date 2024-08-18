using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class Worker : PathFollow3D
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

		float[] weights = anthill.targetTrees.Select(tree => (float)tree.Weighting).ToArray();
		int index = (int)rng.RandWeighted(weights);
		targetTree = anthill.targetTrees[index];


		GetParent()?.RemoveChild(this);
		anthill.targetTrees[index].path.AddChild(this);

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
