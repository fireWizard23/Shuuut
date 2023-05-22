using Godot;
using System;
using System.Threading.Tasks;
using Shuuut.Scripts;
using Shuuut.Scripts.Hurtbox;
using Shuuut.World.Weapons;

public partial class Fist : BaseMeleeWeapon
{
	private uint _mask;

	public override void _Ready()
	{
		base._Ready();
		Handler.CurrentState = State.Ready;
	}

	public override async Task Sheath()
	{
	}

	public override async Task UnSheath()
	{
	}

	public override void SetAttackMask(uint mask)
	{
		this._mask = mask;
	}

	public override async Task Use()
	{
		var space = GetWorld2D().DirectSpaceState;
		var query = new PhysicsRayQueryParameters2D()
		{
			CollideWithAreas = true,
			CollideWithBodies = false,
			From = GlobalPosition,
			To = GlobalPosition + Vector2.Right.Rotated(GlobalRotation) * 100,
			CollisionMask = _mask
		};
		var res = space.IntersectRay 
			(
			query
			);
		if (res.Count > 0)
		{
			if (res["collider"].As<Hurtbox>() is { } hurtbox)
			{
				hurtbox.Hurt(new(){ Damage = 10, Source = this});
			}
		}

		await Task.Delay(1000);
	}

}
