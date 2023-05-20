using Godot;
using Shuuut.World.Zombies;

namespace Shuuut.World.Weapons;



enum State
{
	InSheath,
	Ready
}

public partial class WeaponHandler : Node2D
{
	private float _weaponDistanceFromHandler = 0.5f;

	public bool OwnerCanMove = true;


	private State currentState = State.InSheath;

	public float WeaponDistanceFromHandler
	{
		get => _weaponDistanceFromHandler * Constants.Tile.Size;
	}

	private BaseWeapon _knife;
	
	
	public override void _Ready()
	{
		base._Ready();
		_knife = GetChild<BaseWeapon>(0);
		_knife.SetAttackMask(
			((IAttacker)GetParent()).AttackMask );
		_knife.Sheath();
		EquipWeapon();
	}

	public void EquipWeapon()
	{
		_knife.OnEquip();
	}

	public async void UnequipWeapon()
	{
		await _knife.Sheath();
		_knife.OnUnequip();
		currentState = State.InSheath;
	}

	public void UseWeapon()
	{
		switch (currentState)
		{
			case State.InSheath:
				EquipWeapon();
				currentState = State.Ready;
				_knife.UnSheath();
				break;
			case State.Ready:
				_knife.Use();
				break;
		}
	}
	
	

}
