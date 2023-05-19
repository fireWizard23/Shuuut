using Godot;

namespace Shuuut.World.Weapons;

public abstract partial class BaseMeleeWeapon : BaseWeapon
{
    [Export] protected Hitbox hitbox;
}