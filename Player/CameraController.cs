using Godot;
using System;
using System.Diagnostics;

public partial class CameraController : Node3D
{

	float zoom = 5;
	float pitch = 45;
	public float yaw = 0;
	float sensitivity = 0.5f;

	bool isDragging = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseMotion motionEvent && isDragging){
			yaw += motionEvent.Relative.X*sensitivity;
			pitch += motionEvent.Relative.Y*sensitivity;
			pitch = Mathf.Clamp(pitch,20,70);
		}
		if(@event is InputEventMouseButton buttonEvent){
			
        	if (@event.IsPressed()){
				
        	    if (buttonEvent.ButtonIndex == MouseButton.Right) isDragging=true;
        	    if (buttonEvent.ButtonIndex == MouseButton.WheelUp) zoom -= 1;
        	    if (buttonEvent.ButtonIndex == MouseButton.WheelDown) zoom += 1;
        	    
				zoom = Mathf.Clamp(zoom, 2, 15);
        	}
			if (@event.IsReleased()){
				if (buttonEvent.ButtonIndex == MouseButton.Right) isDragging=false;
			}
		}
    }

	public override void _Process(double delta)
	{

		Position = Quaternion.FromEuler(new Vector3(-Mathf.DegToRad(pitch),-Mathf.DegToRad(yaw),0))*Vector3.Back*zoom;
		LookAt((GetParent() as Node3D).Position);
	}
}
