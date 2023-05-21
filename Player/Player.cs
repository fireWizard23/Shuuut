using Godot;
using System;
using System.Collections.Generic;
using Shuuut.Scripts;
using Shuuut.World;
using Shuuut.World.Weapons;
using Shuuut.World.Zombies;


namespace Shuuut.Player;

internal enum State
{
	Normal,
	Attacking,
	InKnockback
}

public partial class Player : CharacterBody2D, IAttacker
{
	[Export]
	public float Speed = 100.0f;

	[Export] internal WeaponHandler _weaponHandler;
	[Export] private HealthController _healthController;

	[Export(PropertyHint.Layers2DPhysics)] public uint AttackMask { get; set;}

	private StateManager<State, Player> _stateManager;

	internal KnockbackInfo KnockbackInfo;

	public override void _Ready()
	{
		base._Ready();
		_stateManager = new StateManager<State, Player>(
			new Dictionary<State, BaseState<State, Player>>()
			{
				{ State.Normal,  new NormalState() },
				{ State.Attacking,  new AttackingState() },
				{ State.InKnockback,  new InKnockbackState() },
			},
			this
		);
		
		
		_stateManager.Ready();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		_stateManager.Process(delta);
		
	}
	
	

	public override void _PhysicsProcess(double delta)
	{
		_stateManager.PhysicsProcess(delta);
		Vector2 velocity = Velocity;
		if (_stateManager.CurrentStateEnum != State.InKnockback)
		{
			
			Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
			if (direction != Vector2.Zero)
			{
				velocity = direction.Normalized() * Speed;

			}
			else
			{
				velocity = velocity.MoveToward(Vector2.Zero, Speed);
			}

			Velocity = velocity;
			if (!_weaponHandler.OwnerCanMove)
			{
				Velocity *= 0;
			}
		}
		MoveAndSlide();
	}

	public void _on_hurtbox_on_hurt(DamageInfo damageInfo)
	{
		_healthController.ReduceHealth(damageInfo.Damage);
		KnockbackInfo = new()
		{
			Direction = damageInfo.Source.GlobalPosition.DirectionTo(GlobalPosition),
			Distance = Mathf.Clamp(damageInfo.Damage, 32, 64 * 5)
		};
		_stateManager.ChangeState(State.InKnockback);
		damageInfo.Dispose();
	}

	public void _on_health_on_health_zero()
	{
		QueueFree();	
	}

}
