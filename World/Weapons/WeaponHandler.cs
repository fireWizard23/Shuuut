using Godot;

namespace Shuuut.World.Weapons;



enum State
{
	InSheath,
	Ready
}

public partial class WeaponHandler : Node2D
{
	private float _weaponDistanceFromHandler = 0.5f;


	private State currentState = State.InSheath;

	public float WeaponDistanceFromHandler
	{
		get => _weaponDistanceFromHandler * Constants.Tile.Size;
	}

	private Knife _knife;
	
	
	public override void _Ready()
	{
		base._Ready();
		_knife = GetChild<Knife>(0);
		_knife.Sheath();
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
