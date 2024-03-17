using Godot;
using System;

public partial class ProjectileStone : CharacterBody2D
{

	private Vector2 velocity = new Vector2(0, 0);
	private float speed = 10;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		var collisionInfo = MoveAndCollide(velocity * speed * (float)delta);
	}

	public void FlyTo(Vector2 target_position) {
		this.LookAt(target_position);
		velocity = target_position - this.GlobalPosition;
		// if (this.Position == target_position) {
		// 	this.QueueFree();
		// 	return true;
		// }
		// return false;
	}

	private void _on_projectile_area_area_entered(Area2D area) {
		this.QueueFree();
	}
}
