using Godot;
using System;

public class TrailBuilder {
    public Curve3D curve {get; private set; }
    private Vector3 last_point;
    public Vector3 point;
    public Tree tree;

    public TrailBuilder(Vector3 start) {
        curve = new Curve3D();
        curve.UpVectorEnabled = true;
        last_point = start;
        point = start;
    }

    public void AddPoint(Vector3 next_point) {
        curve.AddPoint(point, -0.25f * (next_point - last_point), 0.25f * (next_point - last_point)); 
        last_point = point;
        point = next_point;
    }

    public void AddFinal(Vector3 next_point) {
        AddPoint(next_point);
        AddPoint(next_point);
    }
}