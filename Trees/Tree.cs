using Godot;
using System;
using System.Diagnostics;
using System.IO;

public partial class Tree : Node3D
{
	public double LeafMass; //how many leaves are on the tree

	public double Weighting = 1;

	[Export] 
	public Path3D path;
	public int leafIndex = 3;

	[Export]
	public double MaxLeaves; //maximum leaves on a tree

	[Export]
	public double RegenerationRate; //per second

	protected Timer regenTimer;

	protected MeshInstance3D LeavesMesh;

	protected PackedScene LeafScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		regenTimer = GetNode<Timer>("RegenTimer");

		LeavesMesh = GetNode<MeshInstance3D>("Tree2/Leaves");

		regenTimer.Paused = false;

		LeafMass = 0;

		regenTimer.Timeout += OnLeafTimerTimeout;

		LeafScene = GD.Load<PackedScene>("res://leavesPart.tscn");

		setOutlined(false);

		
	}

	public virtual void setOutlined(bool outline){
		(GetNode<MeshInstance3D>("Tree2/Tree").GetSurfaceOverrideMaterial(0).NextPass as ShaderMaterial).SetShaderParameter("outline_width", outline ? 5 : 0);
	}

	public void OnLeafTimerTimeout(){
		LeafMass += RegenerationRate * regenTimer.WaitTime;
		LeafMass = Mathf.Min(LeafMass,MaxLeaves);
		UpdateLeaves();
	}

	public double TryTakeLeaf(double takeMass){
		double leavesTaken = Mathf.Min(takeMass, LeafMass); //i.e if trying to take 4 leaves from 3 leaves on the tree, take 3

		LeafMass -= leavesTaken; //then remove that many leaves from the tree, I don't think we need the Max but I'm just being safe so we don't get negative leaves somehow

		UpdateLeaves();
		
		var sceneInstance = LeafScene.Instantiate();
		AddChild(sceneInstance);

		return leavesTaken;
	}

	protected virtual void UpdateLeaves(){
		double factor = LeafMass / MaxLeaves;

		float scale = (float)Mathf.Lerp(0, 1, factor);

		LeavesMesh.Scale = new Vector3(scale, scale, scale);
	}

}
