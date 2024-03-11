using Godot;
using System;
using System.Numerics;

public partial class Character : Node2D
{


	private TileMap tile_map;
	private AStarGrid2D astar_grid;
	private Godot.Collections.Array<Godot.Vector2I> current_id_path;

	private bool isIdle;
	private bool isMoving;
	private bool onMoving; // check if character is done walking to 01 next cell in grid



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Logger.Instance.Print("Starting game!");
		Logger.Instance.Log("Starting game.");
		Logger.Instance.Log("First message.");
		Logger.Instance.Log("Second message.");

		tile_map = GetNode<TileMap>("../Map");
		GD.Print(tile_map);
		astar_grid = new AStarGrid2D();
		astar_grid.Region = tile_map.GetUsedRect();
		astar_grid.CellSize = new Godot.Vector2(16, 16);
		astar_grid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
		astar_grid.Update();

		Logger.Instance.Print($"{tile_map.GetUsedRect()}");
		for (int x = 0; x < tile_map.GetUsedRect().Size.X; x++)
		{
			for (int y = 0; x < tile_map.GetUsedRect().Size.Y; x++)
			{
				
				Vector2I tile_position = new Vector2I(
					x + tile_map.GetUsedRect().Size.X,
					y + tile_map.GetUsedRect().Size.Y
					);

				Godot.TileData tile_data = tile_map.GetCellTileData(0, tile_position);

				Logger.Instance.Print($"({x};{y}) is {tile_data.GetCustomData("Walkable")}");

				if (tile_data == null || (bool)tile_data.GetCustomData("Walkable") == false)
				{
					astar_grid.SetPointSolid(tile_position);
				}

			}
		}
	}

	private void setDefaultState()
	{
		isIdle = true;
		isMoving = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("Move"))
		{
			Godot.Collections.Array<Godot.Vector2I> id_path = astar_grid.GetIdPath(
				tile_map.LocalToMap(GlobalPosition),
				tile_map.LocalToMap(GetGlobalMousePosition())
			);

			Logger.Instance.Print($"Path: {id_path}");
			Logger.Instance.Log($"Path: {id_path}");

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
			Godot.Vector2 target_position = tile_map.MapToLocal(current_id_path[0]);
			GlobalPosition = GlobalPosition.MoveToward(target_position, 2);

			if (GlobalPosition == target_position)
			{
				current_id_path.RemoveAt(0);
			}

		}


	}
}
