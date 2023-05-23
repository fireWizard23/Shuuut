using Godot;
using System.Threading.Tasks;
using Shuuut.Scripts.Hurtbox;

namespace Shuuut.World.Weapons;

public partial class Knife : BaseMeleeWeapon
{

	private bool _isAttacking;



	public override async Task Use()
	{
		if (CurrentAnimation.CurrentCount != 0  && !_isAttacking)
		{
			await Attack();
		}
	}

	public override async Task Sheath()
	{
		
		await CurrentAnimation.WaitAsync();
		var tween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetParallel();
		tween.TweenProperty(this, "modulate:a", 0, 0.25f);
		await ToSignal(tween, Tween.SignalName.Finished);
		CurrentAnimation.Release();
		Enable(false);
	}

	public override async Task UnSheath()
	{
		await CurrentAnimation.WaitAsync();

		var tween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetParallel();
		tween.TweenProperty(this, "modulate:a", 1, 0.25f);
		await ToSignal(tween, Tween.SignalName.Finished);
		CurrentAnimation.Release();
		Enable();
	}

	async Task Attack()
	{
		_isAttacking = true;

		var origRot = Rotation;
		await CurrentAnimation.WaitAsync();
		var windup = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetParallel();
		windup.TweenProperty(this, "rotation", Rotation-Mathf.DegToRad(90), 0.15f);
		
		await ToSignal(windup, Tween.SignalName.Finished);
		
		
		// Attack animation

		Handler.OwnerCanMove = false;
		Handler.OwnerCanRotate = false;
		Hitbox.TurnOn();
		var attackSpeed = 0.15f / 2;
		var attack1 = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetParallel();
		attack1.TweenProperty(this, "rotation", origRot, attackSpeed).SetDelay(0.15f);
		await ToSignal(attack1, Tween.SignalName.Finished);

		

		var attack2 = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetParallel();

		
		attack2.TweenProperty(this, "rotation", origRot + Mathf.DegToRad(90), attackSpeed);
		
		await ToSignal(attack2, Tween.SignalName.Finished);
		
		Hitbox.TurnOff();
		Handler.OwnerCanMove = true;
		Handler.OwnerCanRotate = true;

		// Recovery
		var recovery = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetParallel();
		recovery.TweenProperty(this, "rotation", origRot, 0.5f);
		
		await ToSignal(recovery, Tween.SignalName.Finished);
		
		
		CurrentAnimation.Release();
		_isAttacking = false;
		Rotation = 0;
	}


	private void _on_hitbox_on_hitbox_hit(Hurtbox hurtbox)
	{
		hurtbox.Hurt(new()
		{
			Damage =  10,
			Source =  this
		});
	}
	
}
