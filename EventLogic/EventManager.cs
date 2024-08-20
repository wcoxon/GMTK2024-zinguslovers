using Godot;
using System;
using System.Diagnostics.Tracing;

public partial class EventManager : Node
{
	public static EventManager instance;

	[Signal]
	public delegate void TestEventHandler();

	public override void _Ready(){
		instance = this;
	}
}
