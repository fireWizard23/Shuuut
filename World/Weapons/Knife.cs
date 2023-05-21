using Godot;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Shuuut.Scripts;

namespace Shuuut.World.Weapons;

public partial class Knife : BaseMeleeWeapon
{

	private bool attacking;



	public override async Task Use()
	{
		if (currentAnimation.CurrentCount != 0 && Input.IsActionJustPressed("attack") && !attacking)
		{
			await Attack();
		}
	}

	public override async Task Sheath()
	{
		
		//Hide();
		await currentAnimation.WaitAsync();
		var tween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetParallel();
		tween.TweenProperty(this, "modulate:a", 0, 0.25f);
		await ToSignal(tween, Tween.SignalName.Finished);
		GD.Print("FINISHED!");
		currentAnimation.Release();
		Enable(false);
	}

	public override async Task UnSheath()
	{
		await currentAnimation.WaitAsync();

		var tween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetParallel();
		tween.TweenProperty(this, "modulate:a", 1, 0.25f);
		await ToSignal(tween, Tween.SignalName.Finished);
		currentAnimation.Release();
		Enable();
	}

	async Task Attack()
	{
		attacking = true;

		var origRot = Rotation;
		await currentAnimation.WaitAsync();
		var windup = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetParallel();
		windup.TweenProperty(this, "rotation", Rotation-Mathf.DegToRad(90), 0.15f);
		
		await ToSignal(windup, Tween.SignalName.Finished);
		
		
		// Attack animation

		handler.OwnerCanMove = false;
		handler.OwnerCanRotate = false;
		hitbox.TurnOn();
		var attackSpeed = 0.15f / 2;
		var attack1 = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetParallel();
		attack1.TweenProperty(this, "rotation", origRot, attackSpeed).SetDelay(0.15f);
		await ToSignal(attack1, Tween.SignalName.Finished);

		

		var attack2 = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetParallel();

		
		attack2.TweenProperty(this, "rotation", origRot + Mathf.DegToRad(90), attackSpeed);
		
		await ToSignal(attack2, Tween.SignalName.Finished);
		
		hitbox.TurnOff();
		handler.OwnerCanMove = true;
		handler.OwnerCanRotate = true;

		// Recovery
		var recovery = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetParallel();
		recovery.TweenProperty(this, "rotation", origRot, 0.5f);
		
		await ToSignal(recovery, Tween.SignalName.Finished);
		
		
		// await ToSignal(GetTree().CreateTimer(0.25f), SceneTreeTimer.SignalName.Timeout);
		
		currentAnimation.Release();
		attacking = false;
		Rotation = 0;

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
