using Godot;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody3D
{
	public const float acceleration = 6.0f;
	public const float maxSpeed = 2.0f;
	public const float JumpVelocity = 4.5f;
	double cargo = 0;
	public TrailBuilder trailBuilder;

	private AnthillUI _anthillUI;
	private TreeUI _treeUI;

	[Export] public Anthill anthill;
 
	public void EnterAnthill(Area3D area){
		
		//show UI stuff
		Debug.WriteLine("entered anthill");
		_treeUI.Hide();
		_anthillUI.Show();
		// (deposit player leaves)
		if(cargo>0){
			Debug.WriteLine($"depositing cargo: {cargo}");
			(area.Owner as Anthill).Deliver(cargo);
			cargo = 0;
		}
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
		(area.Owner as Tree).setOutlined(true);
	}
	public void ExitTree(Area3D area){
		//hide UI stuff
		Debug.WriteLine("exited tree");
		_treeUI.Hide();
		(area.Owner as Tree).setOutlined(false);
	}

	public void EnterPickup(Area3D area){
		cargo += 10;
		(area.Owner as Node3D).QueueFree();
	}

	public void StartPathing() {
		trailBuilder = new TrailBuilder(anthill.Position);
	}

	public void StopPathing() {
		if (PathHasTree()) {
			trailBuilder.AddFinal(anthill.Position);
            Path3D path = new Path3D { Curve = trailBuilder.curve };
            Owner.AddChild(path);
			trailBuilder.tree.path = path;
			anthill.targetTrees.Add(trailBuilder.tree);
		}
		trailBuilder = null;
	}

	public void PathAddTree(Tree tree) {
		trailBuilder.tree = tree;
		trailBuilder.AddPoint(Position);
		trailBuilder.AddPoint(Position);
		trailBuilder.AddPoint(tree.Position + Vector3.Up * 2.5f);
		trailBuilder.AddPoint(tree.Position + Vector3.Up * 2.5f);
		trailBuilder.AddPoint(tree.Position + Vector3.Up * 2.5f);
		trailBuilder.AddPoint(Position);
		trailBuilder.AddPoint(Position);

	}

	public bool PathHasTree() {
		return trailBuilder.tree != null;
	}

	public bool IsPathing() {
		return trailBuilder != null;
	}

    public override void _Ready()
    {
        _anthillUI = Owner.GetNode<AnthillUI>("Control/AnthillUI");
		_treeUI = Owner.GetNode<TreeUI>("Control/TreeUI");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add gravity
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity += direction * acceleration * (float)delta;

			velocity = velocity.Normalized() * Mathf.Min(velocity.Length(),maxSpeed);
			
			GetNode<AnimationPlayer>("Ants/AnimationPlayer").Play();
		}
		else
		{
			velocity = velocity.Normalized() * (float)Mathf.Max(velocity.Length()-acceleration*delta,0);
		}
		
		GetNode<AnimationPlayer>("Ants/AnimationPlayer").SpeedScale = velocity.Length()/2.0f;

		if(velocity == Vector3.Zero){
			GetNode<AnimationPlayer>("Ants/AnimationPlayer").Pause();
		}
		else{
			((Node3D)GetChild(0)).LookAt(Position-velocity,Vector3.Up,true);
		}

		Velocity = velocity;
		MoveAndSlide();

		if (IsPathing() && (trailBuilder.point - Position).Length() > 0.5f) {
			trailBuilder.AddPoint(Position);
		}
	}


		
}
