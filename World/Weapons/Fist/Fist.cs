using Godot;
using System;
using System.Threading.Tasks;
using Shuuut.Scripts;
using Shuuut.World.Weapons;

public partial class Fist : BaseMeleeWeapon
{
	private uint mask;
	public override async Task Sheath()
	{
	}

	public override async Task UnSheath()
	{
	}

	public override void SetAttackMask(uint mask)
	{
		this.mask = mask;
	}

	public override void Use()
	{
		var space = GetWorld2D().DirectSpaceState;
		var query = new PhysicsRayQueryParameters2D()
		{
			CollideWithAreas = true,
			CollideWithBodies = false,
			From = GlobalPosition,
			To = GlobalPosition + Vector2.Right.Rotated(GlobalRotation) * 100,
			CollisionMask = mask
		};
		var res = space.IntersectRay 
			(
			query
			);
		if (res.Count > 0)
		{
			if (res["collider"].As<Hurtbox>() is { } hurtbox)
			{
				hurtbox.Hurt(new DamageInfo(){ Damage = 10, Source = this});
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
