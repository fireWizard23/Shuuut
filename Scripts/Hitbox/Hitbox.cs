using Godot;

namespace Shuuut.Scripts.Hitbox;

public partial class Hitbox : Area2D
{
    [Signal]
    public delegate void OnHitboxHitEventHandler(Hurtbox.Hurtbox hurtbox);

    [Export] private CollisionShape2D _collisionShape2D;
    
    
    public void _on_area_entered(Area2D area)
    {
        if (area is Hurtbox.Hurtbox hurtbox)
        {
            EmitSignal(SignalName.OnHitboxHit, hurtbox);
        }
    }


    public void TurnOff()
    {
        _collisionShape2D.Disabled = true;
    }

    public void TurnOn()
    {
        _collisionShape2D.Disabled = false;
    }
    
}