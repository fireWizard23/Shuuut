using Godot;
using System;
using Shuuut;

public partial class WeaponHandler : Node2D
{
	private float _weaponDistanceFromHandler = 0.5f;

	public float WeaponDistanceFromHandler
	{
		get => _weaponDistanceFromHandler * Constants.Tile.Size;
	}

	private Knife _knife;
	
	
	public override void _Ready()
	{
		base._Ready();
		_knife = GetChild<Knife>(0);
		EquipWeapon();
	}

	public void EquipWeapon()
	{
		_knife.OnEquip();
	}

	public void UnequipWeapon()
	{
		_knife.OnUnequip();
	}
	
	

}
