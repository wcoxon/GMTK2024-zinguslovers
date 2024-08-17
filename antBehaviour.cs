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
	public override void _Process(double delta)
	{
		double moveDistance = anthill.GetStat(Anthill.Stat.AntSpeed).GetValue()*delta;

		Progress += (float)moveDistance;

		if((Position - GetParent<Path3D>().Curve.GetPointPosition(3)).Length() < moveDistance && cargo==0){
			//take leaf
			//cargo = anthill.GetStat(Anthill.Stat.AntCarryCapacity).GetValue();
			cargo = targetTree.TryTakeLeaf(anthill.GetStat(Anthill.Stat.AntCarryCapacity).GetValue());
		}
		else if(ProgressRatio==1){
			// deposit leaf // empty cargo
			anthill.Deliver(cargo);
			cargo = 0;
			Progress = 0;
		}
	}
}
