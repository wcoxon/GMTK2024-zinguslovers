using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class Player : CharacterBody3D
{
	public const float acceleration = 9.0f;
	//public float maxSpeed = 3.0f;
	public AnthillStat maxSpeed;

	//public const float JumpVelocity = 4.5f;
	public AnthillStat JumpVelocity;

	public AnthillStat CollectableValue;
	double cargo = 0;
	public TrailBuilder trailBuilder;
	private Node _trailParticles;

	private AnthillUI _anthillUI;
	private TreeUI _treeUI;
	private PlayerUI _playerUI;
	private TutorialUI _tutorialUI;

	public CameraController _cameraController;

	protected PackedScene leafScene;

	[Export] public Anthill anthill;

	[Export] public PackedScene trailParticle;

	private RandomNumberGenerator _rng = new RandomNumberGenerator();
	
	private Color trailColour;

	public void updateColour(Color colour){
		GetNode<MeshInstance3D>("Ants/AntBody").SetInstanceShaderParameter("albedo",colour);
		GetNode<MeshInstance3D>("Ants/AntLegs").SetInstanceShaderParameter("albedo",colour);
	}

	public double getCargo(){
		return cargo;
	}
	public void DeliverTo(Anthill anthill){
		anthill.Deliver(cargo);
		
		GetNode<Node3D>("Ants/leaf").Hide();
		cargo = 0;
		_playerUI.updateLeafCount(cargo);
		_tutorialUI.completedHint(TutorialUI.Hint.deposit);
	}

	public void EnterAnthill(Area3D area){
		//show UI
		_treeUI.Hide();
		if (!_anthillUI.hidden){
			_anthillUI.Show();
		}
		
	}
	public void ExitAnthill(Area3D area){
		//hide UI
		_anthillUI.Hide();
	}

	public void EnterTree(Area3D area){
		// hide anthill ui
		_anthillUI.Hide();

		//show tree UI
		if (!_treeUI.hidden){
			_treeUI.Show();
		}
		_treeUI.SetTree(area.Owner as Tree);
		(area.Owner as Tree).setOutlined(true);
	}
	public void ExitTree(Area3D area){
		//hide tree UI
		_treeUI.Hide();
		(area.Owner as Tree).setOutlined(false);
	}

	public void updateCollected(){
		var leaf = GetNode<Node3D>("Ants/leaf");

		if(cargo==0) leaf.Hide();
		else leaf.Show();

		leaf.Scale = Vector3.One*(float)Mathf.Lerp(5,20,cargo/100);
		_playerUI.updateLeafCount(cargo);
	}
	public void EnterPickup(Area3D area){
		_tutorialUI.completedHint(TutorialUI.Hint.pickup);
		//add to cargo
		cargo += CollectableValue.GetValue();
		
		updateCollected();

		//delete leaf
		(area.Owner as Node3D).QueueFree();
		//play particles
		LeafPoofAnimation();
	}

	public void StartPathing() {
		_tutorialUI.completedHint(TutorialUI.Hint.trailStart);

		trailBuilder = new TrailBuilder(anthill.Position);
		_trailParticles = new Node();
		Owner.AddChild(_trailParticles);
		trailColour = new Color(
			_rng.RandfRange(0.3f, 1f), 
			_rng.RandfRange(0.3f, 1f), 
			_rng.RandfRange(0.2f, 0.5f)
		);
	}

	public void StopPathing() {
		
		_tutorialUI.completedHint(TutorialUI.Hint.trailEnd);


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


			//leaf progress ratio
			trailBuilder.tree.leafProgress = (float)trailBuilder.tree.leafIndex / (float)trailBuilder.curve.PointCount;
			
		}
		trailBuilder = null;
		_trailParticles = null;
	}

	public void PathAddPoint(){
		//add point to trail
		trailBuilder.AddPoint(Position);
		//add particle emitter
		GpuParticles3D particle = trailParticle.Instantiate<GpuParticles3D>();
		particle.Position = Position + new Vector3(0, 0.05f, 0);
		particle.SetInstanceShaderParameter("albedo", trailColour);
		_trailParticles.AddChild(particle);
	}
	public void PathAddTree(Tree tree) {
		
		_tutorialUI.completedHint(TutorialUI.Hint.trailTree);
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

	public void updateRotation(){
		if(!Velocity.IsZeroApprox()){
			Vector3 up = GetFloorNormal();
			if(up == Vector3.Zero){
				up = Vector3.Up;
			}

			Vector3 forward = GetRealVelocity().Normalized();
			
			if(!up.Cross(forward).IsZeroApprox()){
				GetNode<Node3D>("Ants").LookAt(Position+forward,up,false);
			}
		}
	}

	public void updateAnimation(){
		var animationPlayer = GetNode<AnimationPlayer>("Ants/AnimationPlayer");

		if(Velocity.IsZeroApprox()){
			animationPlayer.Pause();
		}
		else{
			animationPlayer.Play();
			animationPlayer.SpeedScale = Velocity.Length()/2.0f;
		}
	}

    public override void _Ready()
    {
        _anthillUI = Owner.GetNode<AnthillUI>("Control/AnthillUI");
		_treeUI = Owner.GetNode<TreeUI>("Control/TreeUI");
		_playerUI = Owner.GetNode<PlayerUI>("Control/PlayerUI");
		_tutorialUI = Owner.GetNode<TutorialUI>("Control/TutorialUI");

		leafScene = GD.Load<PackedScene>("res://Leaf/PlayerPickupParticles.tscn");
		_cameraController = GetNode<CameraController>("CameraController");
		
		_playerUI.updateLeafCount(cargo);

		maxSpeed = anthill.GetStat(Anthill.Stat.PlayerSpeed);
		JumpVelocity = anthill.GetStat(Anthill.Stat.PlayerJump);
		CollectableValue = anthill.GetStat(Anthill.Stat.CollectableValue);
    }


    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = new Vector3(Velocity.X,0,Velocity.Z);

		Vector2 inputVector = Input.GetVector("left", "right", "up", "down");
		Vector3 inputDirection = new Vector3(inputVector.X, 0, inputVector.Y).Rotated(Vector3.Up,-Mathf.DegToRad(_cameraController.yaw)).Normalized();

		if (inputDirection != Vector3.Zero)
		{
			_tutorialUI.completedHint(TutorialUI.Hint.movement);

			//accelerate in input direction
			velocity += inputDirection * acceleration * (float)delta;

			//limit speed
			velocity = velocity.Normalized() * Mathf.Min(velocity.Length(),(float)maxSpeed.GetValue());
		}
		else
		{
			//decelerate towards 0
			velocity = velocity.Normalized() * Mathf.Max(velocity.Length()-acceleration*(float)delta,0);
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
			velocity.Y = (float)JumpVelocity.GetValue();
		}
		if (Input.IsActionJustReleased("jump") && velocity.Y > 0)
		{
			velocity.Y *= 0.4f;
		}

		Velocity = velocity;

		MoveAndSlide();

		updateAnimation();

		updateRotation();
		

		// if pathing and over 0.5 from the last point, add a new point here
		if (IsPathing() && (trailBuilder.point - Position).Length() > trailBuilder.PointSpacing) {
			PathAddPoint();
		}

		if (Position.Y < -4.9f) {
			Reset();
		}
	}

	public void LeafPoofAnimation(){
		Node3D sceneInstance = (Node3D)leafScene.Instantiate();
		AddChild(sceneInstance);
	}

	public void Reset() {
		Position = anthill.Position;
		if (_trailParticles != null) {
			Owner.RemoveChild(_trailParticles);
			_trailParticles = null;
		}

		cargo = 0;
		updateCollected();

		trailBuilder = null;
	}
}
