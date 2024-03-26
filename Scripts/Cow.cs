using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

public partial class Cow : Node2D
{

	[ExportGroup("Maps")]
	[Export] public TileMap move_ground_map;

	[Export] public TileMap tile_map;

	[ExportGroup("Attributes")]
	[Export] public bool isBlock = false;
	[Export] public float speed;
	[Export] public float health;
	[Export] public TextureProgressBar healthbar;

	private bool alive = true;
	
	private AStarGrid2D astar_grid;
	private Godot.Collections.Array<Godot.Vector2I> current_id_path;

	private bool isIdle;

	private bool isMoving;

	private bool onMoving;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		setDefaultState();


		healthbar.Value = this.health;

		Logger.Instance.Print($"({move_ground_map}) is movable for ground unit.");
		astar_grid = new AStarGrid2D();
		astar_grid.Region = tile_map.GetUsedRect();
		astar_grid.CellSize = new Godot.Vector2(16, 16);
		astar_grid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
		astar_grid.Update();

		var available_cells = move_ground_map.GetUsedCells(0);

		// Logger.Instance.Print($"{move_map.GetUsedCells(0)}");

		for (int x = 0; x < astar_grid.Region.Size.X; x++)
		{
			for (int y = 0; y < astar_grid.Region.Size.Y; y++)
			{

				Vector2I tile_position = new Vector2I(x, y);
				astar_grid.SetPointSolid(tile_position, true);
			}
		}

		for (int i = 0; i < available_cells.Count; i++)
		{
			Vector2I tile_position = available_cells[i];
			astar_grid.SetPointSolid(tile_position, false);
			// Logger.Instance.Print($"{tile_position} is set to non-solid");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// if (isMoving){
		// 	if (current_position != move_to_this_node)
		// }
	}

	private void setDefaultState()
	{
		isIdle = true;
		isMoving = true;
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("Move"))
		{
			Godot.Collections.Array<Godot.Vector2I> id_path = astar_grid.GetIdPath(
				move_ground_map.LocalToMap(GlobalPosition),
				move_ground_map.LocalToMap(GetGlobalMousePosition())
			);

			Logger.Instance.Print($"Path: {id_path}");
			// Logger.Instance.Log($"Path: {id_path}");

			if (id_path.Count != 0) current_id_path = id_path;

		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (current_id_path == null || current_id_path.Count == 0)
		{
			isIdle = false;
		}
		else
		{
			if (isMoving)
			{
				Godot.Vector2 target_position = move_ground_map.MapToLocal(current_id_path[0]);
				GlobalPosition = GlobalPosition.MoveToward(target_position, speed);

				if (GlobalPosition == target_position)
				{
					current_id_path.RemoveAt(0);
				}
			}


		}
	}

	private void _on_area_2d_area_entered(Area2D area)
	{
		if (area.IsInGroup("Rabbits"))
		{
			Logger.Instance.Print("Collided with Rabbits");
			isMoving = false;
		}
	}

	public void BeAttackBy(float dmg){
		this.health -= dmg;
		healthbar.Value = this.health;
		
		if(this.health <= 0){
			alive = false;
			this.Visible = false;
		}
	}

	public bool isAlive(){
		return alive;
	}
}



