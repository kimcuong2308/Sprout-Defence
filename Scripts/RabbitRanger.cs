using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class RabbitRanger : Node2D
{
	[ExportGroup("Attributes")]
	[Export] public int attack_damage = 20;
	[Export] public float attack_interval = 0.5f;
	[Export] public Area2D attack_range;
	private float attack_waittime = 0;
	[Export(PropertyHint.Enum, "North,East,West,South")] public String direction = "East";
	[Export] public bool _test_change_direction = false;


	private bool isAttacking = false;
	private List<Cow> enemy_list = new List<Cow>();
	private List<Node> current_attacking_enemy = new List<Node>();

	// private CustomSignalSingleton _customSignalSingleton;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// _customSignalSingleton = GetNode<CustomSignalSingleton>("root/CustomSignalSingleton");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// isAttacking = attack_range.HasOverlappingAreas();
		// attack_timer.Timeout += _on_attack_interval_timeout;
		// Logger.Instance.Print($"{enemy_list.Count}");
		if (enemy_list.Count > 0)
		{
			
			attack_waittime = timer(delta, attack_waittime);
			if (attack_waittime > attack_interval)
			{
				Logger.Instance.Print($"Going to attack: {enemy_list.Count}");
				_attack();
				attack_waittime = 0;
			}
		}

		if (_test_change_direction)
		{
			_updateDirection(direction);
			_test_change_direction = false;
		}
	}

	private void _on_attack_area_area_entered(Area2D area)
	{
		
		if (area.IsInGroup("Enemy"))
		{
			var enemy = area.GetParent();

			if (enemy is Cow cow) {
				cow.isAlive();
				enemy_list.Add(cow);
			}

			Logger.Instance.Print("Enemy enter attack range");
		}
		Logger.Instance.Print($"enemy list: {enemy_list.Count}");
	}

	private void _on_attack_area_area_exited(Area2D area)
	{
		
		if (area.IsInGroup("Enemy"))
		{
			var enemy = area.GetParent();

			if (enemy is Cow cow) {
				cow.isAlive();
				enemy_list.Remove(cow);
			}

			Logger.Instance.Print("Enemy exit attack range");
		}
		Logger.Instance.Print($"enemy list: {enemy_list.Count}");
	}

	private void _attack()
	{
		Logger.Instance.Print($"Attacking...");


		if (this.enemy_list[0].isAlive())
		{
			Logger.Instance.Print($"Attack enemy");
			this.enemy_list[0].BeAttackBy(attack_damage);
		}
		else{
			Logger.Instance.Print($"Enemy isAlive: {this.enemy_list[0].isAlive()}");
			Logger.Instance.Print($"Remove from list");
			this.enemy_list.RemoveAt(0);
		}

		// _customSignalSingleton.EmitSignal(nameof(CustomSignalSingleton.getDamgeByPlayerEventHandler), this.attack_damage);
	}

	private float timer(double delta, float a)
	{
		return a += 1 * (float)delta;
	}

	private void _updateDirection(String direction)
	{

		switch (direction)
		{
			case "North":
				this.RotationDegrees = -90;
				break;
			case "East":
				this.RotationDegrees = 0;
				break;
			case "South":
				this.RotationDegrees = 90;
				break;
			case "West":
				this.RotationDegrees = 180;
				break;

		}

	}
}
