using Godot;
using Shuuut.Scripts.Hitbox;


namespace Shuuut.World.Weapons;

public abstract partial class BaseMeleeWeapon : BaseWeapon
{
    [Export] protected Hitbox Hitbox;

    public  override void SetAttackMask(uint mask)
    {
        if(Hitbox != null)
            Hitbox.CollisionMask = mask;
    }
    
}