using System.Threading;
using System.Threading.Tasks;
using Godot;

namespace Shuuut.World.Weapons;

public abstract partial class BaseWeapon : Node2D
{
    [Export] protected float distanceFromOwner = 0;
    protected WeaponHandler handler;
    protected bool isEquipped = false;
    
    public SemaphoreSlim currentAnimation = new(1);

    
    public override void _Ready()
    {
        handler = GetParent() as WeaponHandler;
		
        var sprite = GetChildOrNull<Sprite2D>(0);
        if(sprite != null ) 
            sprite.Position = Vector2.Right.Rotated(Mathf.DegToRad(-30)) * (handler.WeaponDistanceFromHandler + distanceFromOwner) ;
        Enable(isEquipped);
    }

    public abstract void SetAttackMask(uint mask);



    public abstract  Task Sheath();
    public abstract  Task UnSheath();

    public abstract Task Use();
    
    
    

    protected void Enable(bool v=true)
    {
        isEquipped = v;
        SetProcess(v);
        SetPhysicsProcess(v);
    }
    
    public async Task OnEquip()
    {
        Enable(isEquipped);
    }

    public async Task OnUnequip()
    {
        Enable(isEquipped);
    }

}