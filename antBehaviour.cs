using Godot;
using System;

public partial class antBehaviour : CharacterBody3D
{
	int currentPathNode = 0;
	[Export]
	public Vector3[] path = {new Vector3(0,0,0), new Vector3(2,0,0), new Vector3(2,2,0)};

	public const float Speed = 1.0f;
	public bool returning = false;

	public override void _Ready()
	{
		//Position = path[currentPathNode];
	}
	public override void _PhysicsProcess(double delta)
	{
		if(currentPathNode==0) returning = false;
		else if(currentPathNode==path.Length-1) returning = true;
		
		Vector3 start = path[currentPathNode];
		Vector3 target = returning ? path[currentPathNode-1] : path[currentPathNode+1];

		Vector3 moveDirection = (target - Position).Normalized();

		if(moveDirection==Vector3.Up || moveDirection==Vector3.Down) LookAt(Position-moveDirection,Vector3.Left,true);
		else LookAt(Position-moveDirection,Vector3.Up,true);


		if(delta*Speed >  (Position-target).Length()){
			Position = target;
			currentPathNode += returning ? -1 : 1;
		}
		else{
			Position += moveDirection*Speed*(float)delta;
		}
	}
}
