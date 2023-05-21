using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Godot;
using Godot.Collections;
using Shuuut.Scripts;
using Shuuut.World.Weapons;
using Shuuut.World.Zombies.States;

namespace Shuuut.World.Zombies;


using StateManager = StateManager<State, ZombieController>;

public interface IAttacker
{
	public uint AttackMask { get; set; }
}

internal enum State
{
	Idle,
	Chasing,
	Wandering,
	Attacking,
	InKnockback
}


public partial class ZombieController : CharacterBody2D, IAttacker
{

	[Export] public float MovementSpeed { get; private set; } = 100;
	[Export] public Line2D PathLine2D;
	[Export] public Area2D Detector { get; private set; }
	[Export] public Label stateLabel;
	[Export] public HealthController HealthController;
	[Export] public WeaponHandler WeaponHandler;
	
	[Export(PropertyHint.Layers2DPhysics)]
	public uint AttackMask { get; set; }

	
	[Export(PropertyHint.Layers2DPhysics)] private uint entitySteerAwayLayer;
	
	
	public Vector2 SpawnPosition { get; private set; }
	public Node2D Target { get; set; }


	public Vector2 DesiredVelocity;

	private StateManager<State, ZombieController> stateManager;

	private Array<Rid> exclude;
	public RandomNumberGenerator Rng = new();
	
	public KnockbackInfo KnockbackInfo { get; set; }


	
		
	public override void _Ready()
	{
		base._Ready();
		
		exclude = new Array<Rid>(){ GetRid()};
		SpawnPosition = GlobalPosition;
		Rng.Randomize();
		
		stateManager = new StateManager(
			new System.Collections.Generic.Dictionary<State, BaseState<State, ZombieController>>()
			{
				{ State.Idle, new IdleState() },
				{ State.Wandering , new WanderingState()},
				{ State.Attacking , new AttackingState()},
				{ State.Chasing , new ChasingState()},
				{ State.InKnockback, new KnockbackState()},
				
			},
			this
		);

		stateManager.Ready();
	}


	public override void _Draw()
	{
		base._Draw();
		// DrawLine(Vector2.Zero, Velocity, Colors.Aqua);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		stateManager.Process(delta);
		QueueRedraw();
	}


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		stateManager.PhysicsProcess(delta);
		var ac = stateManager.CurrentStateEnum is State.Attacking or State.InKnockback
			? Vector2.Zero
			: ContextSteer(DesiredVelocity.Normalized(), entitySteerAwayLayer);

		if (stateManager.CurrentStateEnum is not State.InKnockback)
		{
			var desiredDirection = DesiredVelocity.Normalized();
			Velocity = (desiredDirection + ac).Normalized() * MovementSpeed;
		}
		
		MoveAndSlide();
		stateLabel.Text = stateManager.CurrentStateEnum.ToString();
		stateLabel.Rotation = -Rotation;
		stateLabel.Position = Vector2.Zero;
		
		//Rotation
		if (stateManager.CurrentStateEnum is not State.InKnockback)
		{
			var targetAngle = Velocity.Normalized().Angle();
			if (Velocity.LengthSquared() > 0)
			{
				Rotation = (float)Mathf.LerpAngle(Rotation, targetAngle, 8 * delta);
			} 
		}
	}

	Vector2 ContextSteer(Vector2 desiredDirection, uint collisionLayer,int rayCount=8,int rayLength=100)
	{
		var directions = new Vector2[rayCount].Select((v, i) => Vector2.Right.Rotated(2 * i * Mathf.Pi / rayCount)).ToArray();
		var dangers = new float[rayCount];
		var interestWeights = new float[rayCount];
		var space = GetWorld2D().DirectSpaceState;

		for (int i = 0; i < rayCount; i++)
		{
			var direction = directions[i];
			interestWeights[i] = Mathf.Max(direction.Dot(desiredDirection),0);
		}
		
		
		for (var i = 0; i < rayCount; i++)
		{
			var direction = directions[i];
			var query = new PhysicsRayQueryParameters2D()
			{
				Exclude = exclude,
				From = GlobalPosition,
				To = GlobalPosition + direction * rayLength,
				CollisionMask = collisionLayer,

			};
			var hit = space.IntersectRay(query);
			if (hit.Count <= 0) continue;
			
			var dire = GlobalPosition.DirectionTo(hit["position"].AsVector2());
			dangers[i] = 1 - (dire.Length() / rayLength);

			if (hit["collider"].As<Node2D>() is ZombieController z)
			{
				dangers[i] = 0.8f + (GlobalPosition.DistanceTo(hit["position"].AsVector2()) / rayLength) * 0.5f;
			}

		}

		Vector2 go = Vector2.Zero;
		for (int i = 0; i < rayCount; i++)
		{
			var direction = directions[i];
			go +=( interestWeights[i] - dangers[i]) * direction;
		}
		

		return go;

	}

	public void Destroy()
	{
		stateManager.Destroy();
		QueueFree();
	}


	public void _on_health_on_health_zero()
	{
		Destroy();
	}
	

	public void _on_hurtbox_on_hurt(DamageInfo damageInfo)
	{
		HealthController.ReduceHealth(damageInfo.Damage);
		this.KnockbackInfo = new KnockbackInfo()
		{
			Direction = damageInfo.Source.GlobalPosition.DirectionTo(GlobalPosition),
			Distance = Mathf.Clamp(damageInfo.Damage, Constants.Tile.Size/2, Constants.Tile.Sizex5)

		};
		stateManager.ChangeState(State.InKnockback);
		damageInfo.Dispose();
	}

}
