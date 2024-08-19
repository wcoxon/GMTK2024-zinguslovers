using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class BigTree : Tree
{
	
	public List<MeshInstance3D> allLeafMeshes;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		regenTimer = GetNode<Timer>("RegenTimer");
		regenTimer.Paused = false;
		LeafMass = 0;
		regenTimer.Timeout += OnLeafTimerTimeout;
		setOutlined(false);

		allLeafMeshes = new List<MeshInstance3D>();

		Node3D TreeModel = GetNode<Node3D>("BiTree");

		TreeModel.GetChildren();

		Debug.WriteLine("Gaming");

		LeafScene = GD.Load<PackedScene>("res://leavesPart.tscn");

		foreach (Node leafMesh in TreeModel.GetChildren())
        {
			Debug.WriteLine($"leafMesh {leafMesh.Name}");
			if (leafMesh is MeshInstance3D){
				Debug.WriteLine($"written leafMesh {leafMesh.Name}");

				if(leafMesh.Name != "Tree"){
					allLeafMeshes.Add(leafMesh as MeshInstance3D);
				}
			}
			// Console.WriteLine($"leafMesh {leafMesh.Name}");
			// if (leafMesh.Name != "Tree"){
			// 	allLeafMeshes.Add(leafMesh);
			// 	Console.WriteLine($"Leafmesh {leafMesh}");
			// }
		}
	}

	public override void setOutlined(bool outline){
		(GetNode<MeshInstance3D>("BiTree/Tree").GetSurfaceOverrideMaterial(0).NextPass as ShaderMaterial).SetShaderParameter("outline_width", outline ? 5 : 0);
	}

	protected override void UpdateLeaves(){
		double factor = LeafMass / MaxLeaves;
		float scale = (float)Mathf.Lerp(0, 1, factor);

		// LeavesMesh.Scale = new Vector3(scale, scale, scale);

		foreach (MeshInstance3D leafMesh in allLeafMeshes){
			leafMesh.Scale = new Vector3(scale, scale, scale);
		}
	}

}
