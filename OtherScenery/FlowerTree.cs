using Godot;
using System;

public partial class FlowerTree : Tree
{

	GpuParticles3D pollen;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		regenTimer = GetNode<Timer>("RegenTimer");

		LeavesMesh = GetNode<MeshInstance3D>("FlowerNice/Leaves");

		regenTimer.Paused = true;

		LeafMass = MaxLeaves;

		regenTimer.Timeout += OnLeafTimerTimeout;

		pollen = GetNode<GpuParticles3D>("Leaves");

		LeafScene = GD.Load<PackedScene>("res://Leaf/leavesPart.tscn");

		setOutlined(false);
	}

	public override void setOutlined(bool outline){
		(GetNode<MeshInstance3D>("FlowerNice/Leaves").GetSurfaceOverrideMaterial(0).NextPass as ShaderMaterial).SetShaderParameter("outline_width", outline ? 5 : 0);
	}

	protected override void UpdateLeaves(){
		base.UpdateLeaves();

		if (LeafMass==0){
			pollen.Emitting = false;

			LeavesMesh.Scale = new Vector3(0, 0, 0);
			
		}
	}

	
}
