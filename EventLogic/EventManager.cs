using Godot;
using System;
using System.Diagnostics.Tracing;

public partial class EventManager : Node
{
	public static EventManager instance;

	[Signal]
	public delegate void TestEventHandler();

	[Signal]
	public delegate void BeginEndingCutsceneEventHandler();

	[Signal]
	public delegate void SwitchCameraEventHandler();

	[Signal]
	public delegate void PostEndingCutsceneEventHandler();

	public override void _Ready(){
		instance = this;
	}
}
