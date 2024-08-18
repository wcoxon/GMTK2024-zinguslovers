using Godot;
using System;

public partial class TreeUI : Panel
{
	[Export] public Tree tree;

	private Label _infolabel;
	private string _labelformat;

	public void SetTree(Tree new_tree) {
		tree = new_tree;
		
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_infolabel = GetNode<Label>("MarginContainer/VBoxContainer/Label");
		_labelformat = _infolabel.Text;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_infolabel.Text = String.Format(_labelformat, tree.LeafMass);
	}
}
