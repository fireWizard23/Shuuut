using System;
using System.Linq;
using Godot;
using Godot.Collections;
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
	[Export(PropertyHint.Layers2DPhysics)] private uint entitySteerAwayLayer;
	
	public Vector2 SpawnPosition { get; private set; }


	public Vector2 DesiredVelocity;

	private StateManager<State, ZombieController> stateManager;

	private Array<Rid> exclude;
	
		
	public override void _Ready()
	{
		base._Ready();
		stateManager = new StateManager(
			new System.Collections.Generic.Dictionary<State, BaseState<State, ZombieController>>()
			{
				{ State.Idle, new IdleState() },
				{ State.Wandering , new WanderingState()},
				{ State.Attacking , new AttackingState()}
			},
			this
		);

		stateManager.Ready();

		exclude = new Array<Rid>(){ GetRid()};
		SpawnPosition = GlobalPosition;
	}

	public override void _Draw()
	{
		base._Draw();
		DrawLine(Vector2.Zero, Velocity, Colors.Aqua);
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
		var ac = ContextSteer(DesiredVelocity.Normalized(), entitySteerAwayLayer);
		var desiredDirection = DesiredVelocity.Normalized();
		Velocity = (desiredDirection + ac).Normalized() * DesiredVelocity.Length();
		MoveAndSlide();
		
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
	
}
