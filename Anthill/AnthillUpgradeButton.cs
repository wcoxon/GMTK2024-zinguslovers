using Godot;
using System;

public partial class AnthillUpgradeButton : Button
{
	[Export]
	public Anthill.Stat upgrade;
	private Anthill anthill;
	private AnthillStat stat;

	String FormatText;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		anthill = (Owner as AnthillUI).anthill;
		stat = anthill.GetStat(upgrade);

		FormatText = Text;
		Text = String.Format(FormatText, stat.level, stat.GetCost());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Disabled = stat.GetCost() > anthill.leafMass;
	}

    public override void _Pressed()
    {
        base._Pressed();
		anthill.Upgrade(upgrade);
		Text = String.Format(FormatText, stat.level, stat.GetCost());
    }
}
