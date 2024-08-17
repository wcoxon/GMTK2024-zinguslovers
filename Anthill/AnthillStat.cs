using Godot;
using System;

public partial class AnthillStat : Node
{
	[Export] public string valueExpression;
	[Export] public string costExpression;
	[Export] public uint level = 1;


	private Expression _value = new Expression();
	private Expression _cost = new Expression();

	public double GetValue() {
		return _value.Execute(new Godot.Collections.Array {level}).AsDouble();
	}

	public double GetCost() {
		return _cost.Execute(new Godot.Collections.Array {level}).AsDouble();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_value.Parse(valueExpression, new string[] {"l"});
		_cost.Parse(costExpression, new string[] {"l"});
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
