using Godot;

namespace Shuuut.World.Weapons;

public abstract partial class BaseMeleeWeapon : BaseWeapon
{
    [Export] protected Scripts.Hitbox Hitbox;

    public  override void SetAttackMask(uint mask)
    {
        if(Hitbox != null)
            Hitbox.CollisionMask = mask;
    }
    
}