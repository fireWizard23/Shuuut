using System.Threading.Tasks;
using Godot;

namespace Shuuut.World.Weapons;

public abstract partial class BaseWeapon : Node2D 
{
    protected WeaponHandler handler;
    protected bool isEquipped = false;

    
    public override void _Ready()
    {
        handler = GetParent() as WeaponHandler;
		
        var knife = GetChild<Node2D>(0);
        knife.Position = Vector2.Right.Rotated(Mathf.DegToRad(-30)) * handler.WeaponDistanceFromHandler ;
        Enable(isEquipped);
    }

    public abstract  Task Sheath();
    public abstract  Task UnSheath();
    
    
    

    protected void Enable(bool v=true)
    {
        SetProcess(v);
        SetPhysicsProcess(v);
    }
    
    public void OnEquip()
    {
        isEquipped = true;
        Enable(isEquipped);
    }

    public void OnUnequip()
    {
        isEquipped = false;
        Enable(isEquipped);
    }

}