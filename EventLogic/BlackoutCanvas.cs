using Godot;
using System;

public partial class BlackoutCanvas : CanvasLayer
{
	AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("BlackScreen/ScreenModulate");


		if (animationPlayer != null){
			animationPlayer.Play("FadeOut");
		}

		EventManager.instance.BeginEndingCutscene += () => {
			animationPlayer.Play("FadeIn");
		};

		EventManager.instance.SwitchCamera += () => {
			animationPlayer.Play("FadeOut");
		};		
	}

	public void PostFadeIn(){
		EventManager.instance.EmitSignal(EventManager.SignalName.SwitchCamera);
	}
}
