using Godot;
using System;
using System.Diagnostics;


public struct Trail{
	Anthill anthill;
	Vector3[] points;
	Tree targetTree;
}

public partial class CharacterBody3d : CharacterBody3D
{
	public const float Speed = 2.0f;
	public const float JumpVelocity = 4.5f;

	private AnthillUI _anthillUI;
	private TreeUI _treeUI;
 
	public void EnterAnthill(Area3D area){
		//show UI stuff
		Debug.WriteLine("entered anthill");
		_treeUI.Hide();
		_anthillUI.Show();
		// (deposit player leaves)
	}
	public void ExitAnthill(Area3D area){
		//hide UI stuff
		Debug.WriteLine("exited anthill");
		_anthillUI.Hide();
	}

	public void EnterTree(Area3D area){
		//show UI stuff
		Debug.WriteLine("entered tree");
		_anthillUI.Hide();
		_treeUI.Show();
		_treeUI.SetTree(area.Owner as Tree);
	}
	public void ExitTree(Area3D area){
		//hide UI stuff
		Debug.WriteLine("exited tree");
		_treeUI.Hide();
	}

    public override void _Ready()
    {
        _anthillUI = Owner.GetNode<AnthillUI>("Control/AnthillUI");
		_treeUI = Owner.GetNode<TreeUI>("Control/TreeUI");
    }


    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
			
			((Node3D)GetChild(0)).LookAt(Position-velocity,Vector3.Up,true);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}


		Velocity = velocity;
		MoveAndSlide();



		//if close to the anthill
		// if press e
		//  create new path3d and reference from player

		//if player has referenced path3d
		// if distance from last point in path3d > 5 or wtv
		//  add point at player position
		// if no tree on path and press e on a tree
		//  attach tree, add point at top of tree to path
		// if tree on path and press e on nest
		//  add nest position to path and add path-tree to anthill
		//  actually if tree of this path is already a targeted tree, replace path of that tree with this path


	}
}
