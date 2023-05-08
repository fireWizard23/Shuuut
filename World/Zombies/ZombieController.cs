using System.Collections.Generic;
using Godot;
using Shuuut.World.Zombies.States;

namespace Shuuut.World.Zombies;


using StateManager = StateManager<State, ZombieController>;

public enum State
{
	Idle,
	Chasing,
	Wandering,
	Attacking
}

public partial class ZombieController : CharacterBody2D
{

	[Export] public float MovementSpeed { get; private set; } = 100;
	[Export] public Node2D Player;
	[Export] public Line2D PathLine2D;
	
	public Vector2 SpawnPosition { get; private set; }

	


	private StateManager<State, ZombieController> stateManager;
	
		
	public override void _Ready()
	{
		base._Ready();
		stateManager = new StateManager(
			new Dictionary<State, BaseState<State, ZombieController>>()
			{
				{ State.Idle, new IdleState() },
				{ State.Wandering , new WanderingState()},
				{ State.Attacking , new AttackingState()}
			},
			this
		);

		stateManager.Ready();
		
		SpawnPosition = GlobalPosition;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		stateManager.Process(delta);
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		stateManager.PhysicsProcess(delta);
		MoveAndSlide();
	}

	public void Destroy()
	{
		stateManager.Destroy();
		QueueFree();
	}
	
}
