using System.Threading.Tasks;
using Godot;

namespace Shuuut.World.Weapons;

public abstract partial class BaseWeapon : Node2D
{
    [Export] private float distanceFromOwner = 0;
    protected WeaponHandler handler;
    protected bool isEquipped = false;

    
    public override void _Ready()
    {
        handler = GetParent() as WeaponHandler;
		
        var sprite = GetChild<Sprite2D>(0);
        if(sprite != null ) 
            sprite.Position = Vector2.Right.Rotated(Mathf.DegToRad(-30)) * (handler.WeaponDistanceFromHandler + distanceFromOwner) ;
        Enable(isEquipped);
    }

    public abstract  Task Sheath();
    public abstract  Task UnSheath();
    
    
    

    protected void Enable(bool v=true)
    {
        isEquipped = v;
        SetProcess(v);
        SetPhysicsProcess(v);
    }
    
    public void OnEquip()
    {
        Enable(isEquipped);
    }

    public void OnUnequip()
    {
        Enable(isEquipped);
    }

}