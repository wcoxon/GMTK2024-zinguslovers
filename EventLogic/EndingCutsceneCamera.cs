using Godot;
using System;

public partial class EndingCutsceneCamera : Camera3D
{
	AnimationPlayer animationPlayer;

	Camera3D cutsceneCamera;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("EndingAnimation");

		cutsceneCamera = (Camera3D)this;

		EventManager.instance.SwitchCamera += () => {
			cutsceneCamera.MakeCurrent();
			animationPlayer.Play("ZoomOut");
		};	
	}


	public void PostEndingCutscene(){
		EventManager.instance.EmitSignal(EventManager.SignalName.PostEndingCutscene);
	}

}
