using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class Worker : PathFollow3D
{
	[Export]
	public Anthill anthill;

	private Node3D leaf;
	bool hasTarget;
	public double cargo;
	[Export] public Tree targetTree;

	float delay;

	static RandomNumberGenerator rng = new RandomNumberGenerator();

	public override void _Ready()
	{
		leaf = GetNode<Node3D>("leaf");
		Progress = 0;
		hasTarget = anthill.targetTrees.Count > 0;
	}


	public void chooseTarget(){
		var rng = new RandomNumberGenerator();

		float[] weights = anthill.targetTrees.Select(tree => (float)tree.Weighting).ToArray();

		if (anthill.targetTrees.Count == 0 || weights.Sum() <= 0.00001) {
			hasTarget = false;
			GetParent()?.RemoveChild(this);
			anthill.AddChild(this);
			Hide();
			delay = rng.RandfRange(0.1f, 2.5f);
			return;
		}

		Show();

		int index = (int)rng.RandWeighted(weights);
		targetTree = anthill.targetTrees[index];


		GetParent()?.RemoveChild(this);
		targetTree.path.AddChild(this);

		Progress = 0;
		hasTarget = true;
	}

	public override void _Process(double delta)
	{
		if (delay > 0) {
			delay -= (float)delta;
			return;
		}

		if (!hasTarget) {
			chooseTarget();
			return;
		}

		double moveDistance = anthill.GetStat(Anthill.Stat.AntSpeed).GetValue()*delta;

		Progress += (float)moveDistance;

		//Debug.WriteLine(targetTree.leafIndex);
		//if at point [3] in the path, collect a leaf
		if((Position - targetTree.path.Curve.GetPointPosition(targetTree.leafIndex)).Length() < moveDistance && cargo==0){
			//take leaf
			cargo = targetTree.TryTakeLeaf(anthill.GetStat(Anthill.Stat.AntCarryCapacity).GetValue());
			leaf.Show();
			leaf.Scale = Vector3.Zero.Lerp(Vector3.One*10,(float)(cargo/anthill.GetStat(Anthill.Stat.AntCarryCapacity).GetValue()));
		}
		else if(ProgressRatio==1){
			// deposit leaf // empty cargo
			anthill.Deliver(cargo);
			cargo = 0;
			leaf.Hide();

			chooseTarget();
		}
	}
}
