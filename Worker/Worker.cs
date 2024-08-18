using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class Worker : PathFollow3D
{
	[Export]
	public Anthill anthill;

	bool hasTarget;
	public double cargo;
	[Export] public Tree targetTree;

	public override void _Ready()
	{
		Progress = 0;
		hasTarget = anthill.targetTrees.Count > 0;
	}


	public void chooseTarget(){
		if (anthill.targetTrees.Count == 0) {
			hasTarget = false;
			return;
		}

		var rng = new RandomNumberGenerator();

		float[] weights = anthill.targetTrees.Select(tree => (float)tree.Weighting).ToArray();

		if (weights.Sum() <= 0.00001) {
			hasTarget = false;
			return;
		}

		int index = (int)rng.RandWeighted(weights);
		targetTree = anthill.targetTrees[index];


		GetParent()?.RemoveChild(this);
		anthill.targetTrees[index].path.AddChild(this);

		Progress = 0;
		hasTarget = true;
	}

	public override void _Process(double delta)
	{
		if (!hasTarget) {
			chooseTarget();
			return;
		}

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
