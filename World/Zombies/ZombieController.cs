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
	private StateManager<State, ZombieController> stateManager;

	public override void _Ready()
	{
		base._Ready();
		stateManager = new StateManager(
			new Dictionary<State, BaseState<State, ZombieController>>()
			{
				{ State.Idle, new IdleState() }
			},
			this
		);
	}
}
