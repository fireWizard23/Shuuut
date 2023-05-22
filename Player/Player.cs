using Godot;
using System;
using System.Collections.Generic;
using Shuuut.Player.States;
using Shuuut.Scripts;
using Shuuut.Scripts.Hurtbox;
using Shuuut.World;
using Shuuut.World.Weapons;
using Shuuut.World.Zombies;


namespace Shuuut.Player;

public enum State
{
	Normal,
	Attacking,
	InKnockback
}

public partial class Player : StatefulEntity<State, Player>, IAttacker
{
	[Export]
	public float Speed = 100.0f;
	[Export] private HealthController _healthController;
	[Export] internal WeaponHandler _weaponHandler;
	[Export(PropertyHint.Layers2DPhysics)] public uint AttackMask { get; set;}



	internal KnockbackInfo KnockbackInfo;

	protected override void BeforeReady()
	{
		StateManager = new(
			new()
			{
				{ State.Normal,  new NormalState() },
				{ State.Attacking,  new AttackingState() },
				{ State.InKnockback,  new InKnockbackState() },
			},
			this
		);
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		var velocity = Velocity;
		if (StateManager.CurrentStateEnum != State.InKnockback)
		{
			
			var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
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
			Distance = Mathf.Clamp(damageInfo.Damage, Constants.Tile.Size/2, Constants.Tile.Sizex5)
		};
		StateManager.ChangeState(State.InKnockback);
		damageInfo.Dispose();
	}

	public void _on_health_on_health_zero()
	{
		QueueFree();	
	}

}
