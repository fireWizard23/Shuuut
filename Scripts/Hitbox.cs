using Godot;
using System;
using Shuuut.Scripts;

public partial class Hitbox : Area2D
{
    [Signal]
    public delegate void OnHitboxHitEventHandler(Hurtbox hurtbox);
    public void _on_area_entered(Area2D area)
    {
        if (area is Hurtbox hurtbox)
        {
            EmitSignal(SignalName.OnHitboxHit, hurtbox);
        }
    }
}
