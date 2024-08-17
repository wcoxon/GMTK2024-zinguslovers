using Godot;
using System;

public partial class antBehaviour : PathFollow3D
{
	[Export]
	public Anthill anthill;

	public double cargo;

	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
		double moveDistance = anthill.GetStat(Anthill.Stat.AntSpeed).GetValue()*delta;

		Progress += (float)moveDistance;

		if((Position - GetParent<Path3D>().Curve.GetPointPosition(3)).Length() < moveDistance){
			//take leaf
			cargo = anthill.GetStat(Anthill.Stat.AntCarryCapacity).GetValue();
		}
		else if(ProgressRatio==1){
			// deposit leaf // empty cargo
			anthill.Deliver(cargo);
			cargo = 0;
			Progress=0;
		}
	}
}
