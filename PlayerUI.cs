using Godot;
using System;

public partial class PlayerUI : Panel
{
	private Label leafCount;
	private string leafFormat;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		leafCount = GetNode<Label>("MarginContainer/LeafCount");
		leafFormat = leafCount.Text;
	}
	public void updateLeafCount(double count){
		leafCount.Text = string.Format(leafFormat,count);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
