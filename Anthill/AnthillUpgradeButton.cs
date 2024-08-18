using Godot;
using System;

public partial class AnthillUpgradeButton : Button
{
	[Export]
	public Anthill.Stat upgrade;
	private Anthill anthill;
	private AnthillStat stat;

	String FormatText;

	private void DoFormat() {
		if (upgrade == Anthill.Stat.NewAnt) {
			Text = String.Format(FormatText, stat.level, stat.GetCost(), anthill.numAnts, anthill.numAnts + 1);
		} else {
			stat.level++;
			  double new_val = stat.GetValue();
			stat.level--;
			Text = String.Format(FormatText, stat.level, stat.GetCost(), stat.GetValue(), new_val);
		}

	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		anthill = (Owner as AnthillUI).anthill;
		stat = anthill.GetStat(upgrade);

		FormatText = Text;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Disabled = stat.GetCost() > anthill.leafMass;
		DoFormat();
	}

    public override void _Pressed()
    {
        base._Pressed();
		anthill.Upgrade(upgrade);
		DoFormat();
    }
}
