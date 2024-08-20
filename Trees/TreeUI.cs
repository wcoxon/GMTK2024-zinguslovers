using Godot;
using System;

public partial class TreeUI : Panel
{
	[Export] public Tree tree;
	[Export] public Player player;


	private Label _infolabel;
	private Slider _weightSlider;
	private string _labelformat;

	private Label capacityLabel;
	private Label regenLabel;
	string capacityFormat;
	string regenFormat;

	public void SetTree(Tree new_tree) {
		tree = new_tree;
		capacityLabel.Text = string.Format(capacityFormat, tree.MaxLeaves);
		regenLabel.Text = string.Format(regenFormat, tree.RegenerationRate);
		_weightSlider.Value = new_tree.Weighting;
	}

	public void WeightChanged(float weight){
		tree.Weighting = weight;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_infolabel = GetNode<Label>("MarginContainer/VBoxContainer/LeafCount");
		_labelformat = _infolabel.Text;

		capacityLabel = GetNode<Label>("MarginContainer/VBoxContainer/MaxLeaves");
		capacityFormat = capacityLabel.Text;
		regenLabel = GetNode<Label>("MarginContainer/VBoxContainer/LeafRegen");
		regenFormat = regenLabel.Text;

		


		_weightSlider = GetNode<Slider>("MarginContainer/VBoxContainer/HSlider");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_infolabel.Text = string.Format(_labelformat, tree.LeafMass);
		
	}
}
