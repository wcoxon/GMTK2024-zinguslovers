using Godot;
using System;

public partial class Pause : Control
{
	private Button resumeBtn;
	private Button quitBtn;

	public override void _Ready()
	{
	    ProcessMode = Node.ProcessModeEnum.Always;


		resumeBtn = GetNode<Button>("VBoxContainer/Resume");
		quitBtn = GetNode<Button>("VBoxContainer/Quit");

		resumeBtn.Pressed += onResumeBtnPressed;
		quitBtn.Pressed += onQuitBtnPressed;
	}


	public override void _Input(InputEvent @event) {
		if(@event.IsActionPressed("ui_cancel")) {
			if (GetTree().Paused) {
				GetTree().Paused = false;
				Hide();
			} else {
				GetTree().Paused = true;
				Show();
			}
		}
	}
	

	private void onResumeBtnPressed() {
		GetTree().Paused = false;
		Hide();
	}
	private void onQuitBtnPressed() {
        GetTree().Quit();
	}
}
