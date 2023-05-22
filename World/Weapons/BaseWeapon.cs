using System.Threading;
using System.Threading.Tasks;
using Godot;

namespace Shuuut.World.Weapons;

public abstract partial class BaseWeapon : Node2D
{
    [Export] protected float DistanceFromOwner = 0;
    protected WeaponHandler Handler;
    protected bool IsEquipped = false;

    protected readonly SemaphoreSlim CurrentAnimation = new(1);

    
    public override void _Ready()
    {
        Handler = GetParent() as WeaponHandler;
		
        var sprite = GetChildOrNull<Sprite2D>(0);
        if(sprite != null ) 
            sprite.Position = Vector2.Right.Rotated(Mathf.DegToRad(-30)) * (Handler.WeaponDistanceFromHandler + DistanceFromOwner) ;
        Enable(IsEquipped);
    }

    public abstract void SetAttackMask(uint mask);



    public abstract  Task Sheath();
    public abstract  Task UnSheath();

    public abstract Task Use();
    
    
    

    protected void Enable(bool v=true)
    {
        IsEquipped = v;
        SetProcess(v);
        SetPhysicsProcess(v);
    }
    
    public async Task OnEquip()
    {
        Enable();
    }

    public async Task OnUnequip()
    {
        Enable(false);
    }

}