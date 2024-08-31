using Godot;
using System;

public partial class AnthillStat : Node
{
	[Export] public string valueExpression;
	[Export] public string costExpression;
	[Export] public uint level = 1;

	public double value;
	public double cost;


	private Expression _value = new Expression();
	private Expression _cost = new Expression();

	public double GetValue() {
		return value;
	}

	public double GetCost() {
		return cost;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_value.Parse(valueExpression, new string[] {"l"});
		_cost.Parse(costExpression, new string[] {"l"});

		value = _value.Execute(new Godot.Collections.Array {level}).AsDouble();
		cost = _cost.Execute(new Godot.Collections.Array {level}).AsDouble();
	}

	public void Upgrade(){
		level++;
		value = _value.Execute(new Godot.Collections.Array {level}).AsDouble();
		cost = _cost.Execute(new Godot.Collections.Array {level}).AsDouble();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
