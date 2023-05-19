using Godot;

namespace Shuuut.World.Weapons;

public abstract partial class BaseMeleeWeapon : BaseWeapon
{
    [Export] protected Hitbox hitbox;

    public  override void SetAttackMask(uint mask)
    {
        if(hitbox != null)
            hitbox.CollisionMask = mask;
    }
    
}