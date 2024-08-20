using Godot;
using System;

public partial class TutorialUI : Panel
{

	public enum Hint
	{
		movement,
		looking,
		zooming,
		pickup,
		deposit,
		trailStart,
		trailTree,
		trailEnd,
		upgrades
	}
	private Label _hintLabel;
	private string[] hints;
	bool finishedTutorial = false;
	//public int tutorialStep = 0;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hintLabel = GetNode<Label>("HintLabel");
		hints = _hintLabel.Text.Split("\n");
		updateHint();
	}

	public void completedHint(Hint hint){
		if(finishedTutorial || hints[(int)hint]=="") return;
		hints[(int)hint] = "";
		updateHint();
	}
	public void updateHint(){
		foreach(string hint in hints){
			if(hint!=""){
				_hintLabel.Text = hint;
				return;
			}
		}
		Hide();
		finishedTutorial = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
