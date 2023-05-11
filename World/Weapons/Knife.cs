using Godot;
using System;

public partial class Knife : Node2D
{

	private WeaponHandler handler;

	private bool isEquipped = false;
	private bool attacking;

	public override void _Ready()
	{
		handler = GetParent() as WeaponHandler;
		
		var knife = GetChild<Node2D>(0);
		knife.Position = Vector2.Right.Rotated(Mathf.DegToRad(-30)) * handler.WeaponDistanceFromHandler ;
		
		
		Enable(isEquipped);
		
	}

	void Enable(bool v=true)
	{
		SetProcess(v);
		SetPhysicsProcess(v);
	}

	async void Attack()
	{
		this.attacking = true;
		GD.Print("ATTACK");

		var origRot = Rotation;
		
		var windup = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetParallel();
		windup.TweenProperty(this, "rotation", Rotation-Mathf.DegToRad(90), 0.15f);
		await ToSignal(windup, Tween.SignalName.Finished);
		var attackSpeed = 0.15f / 2;
		var attack1 = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetParallel();
		attack1.TweenProperty(this, "rotation", origRot, attackSpeed).SetDelay(0.15f);
		await ToSignal(attack1, Tween.SignalName.Finished);

		

		var attack2 = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut).SetParallel();

		
		attack2.TweenProperty(this, "rotation", origRot + Mathf.DegToRad(90), attackSpeed);
		
		await ToSignal(attack2, Tween.SignalName.Finished);

		await ToSignal(GetTree().CreateTimer(0.25f), SceneTreeTimer.SignalName.Timeout);
		
		this.attacking = false;
		Rotation = 0;


	}
	

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("attack") && !attacking)
		{
			Attack();
		}
	}

	public void OnEquip()
	{
		isEquipped = true;
		Enable(isEquipped);
	}

	public void OnUnequip()
	{
		isEquipped = false;
		Enable(isEquipped);
	}
	
	
}
