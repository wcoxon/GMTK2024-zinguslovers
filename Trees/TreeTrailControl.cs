using Godot;
using System;

public partial class TreeTrailControl : Button
{
	Player player;
	string[] names;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = (Owner as TreeUI).player;

		names = Text.Split('/');
		Text = names[0];
	}

	private Tree tree { get { return (Owner as TreeUI).tree; } }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player.anthill.targetTrees.Contains(tree)) {
			Text = names[1];
			Disabled = false;
		} else if (player.IsPathing() && !player.PathHasTree()) {
			Text = names[0];
			Disabled = false;
		} else {
			Disabled = true;
		}
	}

	public override void _Pressed() {
		if (player.anthill.targetTrees.Contains(tree)) {
			//tree.path = null;
			player.anthill.targetTrees.Remove(tree);
		} else {
			player.PathAddTree(tree);
		}
	}
}
