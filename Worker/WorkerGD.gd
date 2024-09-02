extends PathFollow3D

const Anthill = preload("res://Anthill/Anthill.cs")
const FlowerTree = preload("res://OtherScenery/FlowerTree.cs")
var anthill: Anthill
var targetTree;

var leaf;
var hasTarget;
var cargo;
var delay = 0;
static var rng = RandomNumberGenerator.new();

# Called when the node enters the scene tree for the first time.
func _ready():
	leaf = get_node("leaf");
	progress = 0;
	hasTarget = anthill.targetTrees.size() > 0;

	#random colour and size variation

	var antColour = Color.from_hsv(
		randf_range(0.02, 0.07),
		randf_range(0.52, 0.72),
		randf_range(0.75, 0.95)
	);

	(get_node("Ants/AntBody") as MeshInstance3D).set_instance_shader_parameter("albedo", antColour);
	(get_node("Ants/AntLegs") as MeshInstance3D).set_instance_shader_parameter("albedo", antColour);
	
	scale = Vector3.ONE * randf_range(0.7, 1);

func getWeight(tree):
	return tree.Weighting;

func reduce_test(accum: int, element: int) -> int:
	return accum + element

func chooseTarget():
	
	var weights = anthill.targetTrees.map(getWeight);

	if anthill.targetTrees.size() == 0 || weights.reduce(reduce_test) <= 0.00001:
		hasTarget = false;
		if(get_parent()):
			get_parent().remove_child(self);
		anthill.add_child(self);
		hide();
		
		delay = randf_range(0.1, 2.5);
		return;
	
	show();
	
	var index = rng.rand_weighted(weights);
	targetTree = anthill.targetTrees[index];
	if(get_parent()):
		get_parent().remove_child(self);
	targetTree.path.add_child(self);
	progress = 0;
	hasTarget = true;
	
func wait(delta):
	if delay > 0: 
		delay -= delta
	else:
		chooseTarget()

func move():
	progress += anthill.moveDistance;

func collect():
	cargo = targetTree.TryTakeLeaf(anthill.GetStat(1).GetValue());
	leaf.show();
	leaf.scale = Vector3.ZERO.lerp(Vector3.ONE*10,(float)(cargo/anthill.GetStat(1).GetValue()));

	if targetTree.LeafMass == 0 and targetTree is FlowerTree:
		anthill.removeTrail(targetTree);

func deposit():
	anthill.Deliver(cargo);
	cargo = 0;
	leaf.hide();
	chooseTarget();


func _process(delta):
	if !hasTarget:
		wait(delta)
		return

	move()

	if progress_ratio > targetTree.leafProgress and !leaf.visible:
		collect()
	
	elif progress_ratio == 1:
		deposit()
