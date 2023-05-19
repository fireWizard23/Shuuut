using Godot;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Shuuut.Scripts;

namespace Shuuut.World.Weapons;

public partial class Knife : BaseMeleeWeapon
{

	
	public SemaphoreSlim currentAnimation = new(1);

	private bool attacking;

	private bool inAnimation = false;

	public void SetAttackMask(uint mask)
	{
		hitbox.CollisionMask = mask;
	}
	


	public void Use()
	{
		if (!inAnimation && Input.IsActionJustPressed("attack") && !attacking)
		{
			Attack();
		}
	}

	public override async Task Sheath()
	{
		
		//Hide();
		inAnimation = true;
		var tween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetParallel();
		tween.TweenProperty(this, "modulate:a", 0, 0.25f);
		await ToSignal(tween, Tween.SignalName.Finished);
		inAnimation = false;
		Enable(false);
	}

	public override async Task UnSheath()
	{
		inAnimation = true;
		var tween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetParallel();
		tween.TweenProperty(this, "modulate:a", 1, 0.25f);
		await ToSignal(tween, Tween.SignalName.Finished);
		inAnimation = false;
		Enable();
	}

	async void Attack()
	{
		inAnimation = true;
		attacking = true;

		var origRot = Rotation;
		await currentAnimation.WaitAsync();
		var windup = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetParallel();
		windup.TweenProperty(this, "rotation", Rotation-Mathf.DegToRad(90), 0.15f);
		
		await ToSignal(windup, Tween.SignalName.Finished);
		
		hitbox.TurnOn();
		var attackSpeed = 0.15f / 2;
		var attack1 = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetParallel();
		attack1.TweenProperty(this, "rotation", origRot, attackSpeed).SetDelay(0.15f);
		await ToSignal(attack1, Tween.SignalName.Finished);

		

		var attack2 = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetParallel();

		
		attack2.TweenProperty(this, "rotation", origRot + Mathf.DegToRad(90), attackSpeed);
		
		await ToSignal(attack2, Tween.SignalName.Finished);
		
		hitbox.TurnOff();

		await ToSignal(GetTree().CreateTimer(0.25f), SceneTreeTimer.SignalName.Timeout);

		inAnimation = false;
		attacking = false;
		Rotation = 0;
		currentAnimation.Release();

	}
	



	public void _on_hitbox_on_hitbox_hit(Hurtbox hurtbox)
	{
		hurtbox.Hurt(new DamageInfo()
		{
			Damage =  10,
			Source =  this
		});
	}
	
}
