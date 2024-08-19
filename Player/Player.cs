using Godot;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody3D
{
	public const float acceleration = 9.0f;
	public const float maxSpeed = 3.0f;
	public const float JumpVelocity = 4.5f;
	double cargo = 0;
	public TrailBuilder trailBuilder;
	private Node _trailParticles;

	private AnthillUI _anthillUI;
	private TreeUI _treeUI;
	public CameraController _cameraController;

	protected PackedScene leafScene;

	[Export] public Anthill anthill;

	[Export] public PackedScene trailParticle;

	public double getCargo(){
		return cargo;
	}
	public void DeliverTo(Anthill anthill){
		Debug.WriteLine($"depositing cargo: {cargo}");
		anthill.Deliver(cargo);
		cargo = 0;
	}

	public void EnterAnthill(Area3D area){
		//show UI
		_treeUI.Hide();
		_anthillUI.Show();
	}
	public void ExitAnthill(Area3D area){
		//hide UI
		_anthillUI.Hide();
	}

	public void EnterTree(Area3D area){
		// hide anthill ui
		_anthillUI.Hide();

		//show tree UI
		_treeUI.Show();
		_treeUI.SetTree(area.Owner as Tree);
		(area.Owner as Tree).setOutlined(true);
	}
	public void ExitTree(Area3D area){
		//hide tree UI
		_treeUI.Hide();
		(area.Owner as Tree).setOutlined(false);
	}

	public void EnterPickup(Area3D area){
		cargo += 10;
		(area.Owner as Node3D).QueueFree();
		LeafPoofAnimation();
	}

	public void StartPathing() {
		trailBuilder = new TrailBuilder(anthill.Position);
		_trailParticles = new Node();
		Owner.AddChild(_trailParticles);
	}

	public void StopPathing() {
		Owner.RemoveChild(_trailParticles);
		if (PathHasTree()) {
			trailBuilder.AddFinal(anthill.Position);
			Debug.WriteLine("setting tree path to new one");
			
			if (trailBuilder.tree.path==null){
				Path3D path = new Path3D{Curve = trailBuilder.curve};
				trailBuilder.tree.path = path;
				Owner.AddChild(path);
			}
			else trailBuilder.tree.path.Curve = trailBuilder.curve;

			trailBuilder.tree.pathTrail = _trailParticles;
			trailBuilder.tree.path.AddChild(_trailParticles);

			anthill.targetTrees.Add(trailBuilder.tree);
		}
		trailBuilder = null;
		_trailParticles = null;
	}

	public void PathAddTree(Tree tree) {
		trailBuilder.tree = tree;
		trailBuilder.AddPoint(Position);
		trailBuilder.AddPoint(Position);

		
		trailBuilder.AddPoint(tree.Position + Vector3.Up * tree.TreeHeight);
		trailBuilder.AddPoint(tree.Position + Vector3.Up * tree.TreeHeight);
		trailBuilder.AddPoint(tree.Position + Vector3.Up * tree.TreeHeight);
		trailBuilder.tree.leafIndex = trailBuilder.curve.PointCount;

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
		leafScene = GD.Load<PackedScene>("res://PlayerPickupParticles.tscn");
		_cameraController = GetNode<CameraController>("CameraController");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = new Vector3(Velocity.X,0,Velocity.Z);

		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y).Rotated(Vector3.Up,-Mathf.DegToRad(_cameraController.yaw)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity += direction * acceleration * (float)delta;

			//limit walking speed
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


		//add vertical component after limited speed
		velocity.Y = Velocity.Y;
		//add gravity
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		// Handle Jump
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		if (Input.IsActionJustReleased("jump"))
		{
			if (velocity.Y > 0){
				velocity.Y *= 0.4f;
			}
			
		}

		if(!velocity.IsEqualApprox(Vector3.Zero)){
			
			Vector3 up = GetFloorNormal();
			if(up==Vector3.Zero){
				up = Vector3.Up;
			}
			if(Mathf.Abs(velocity.Normalized().Y)!=1)GetNode<Node3D>("Ants").LookAt(Position+velocity,up,false);
		}
		
		Velocity = velocity;
		MoveAndSlide();

		if (IsPathing() && (trailBuilder.point - Position).Length() > 0.5f) {
			trailBuilder.AddPoint(Position);
			Node3D particle = trailParticle.Instantiate<Node3D>();
			particle.Position = Position + new Vector3(0, 0.05f, 0);
			_trailParticles.AddChild(particle);
		}
	}

	public void LeafPoofAnimation(){
		Node3D sceneInstance = (Node3D)leafScene.Instantiate();
		AddChild(sceneInstance);
	}
}
