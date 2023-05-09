using Godot;
using Microsoft.VisualBasic;

namespace Shuuut.World.Zombies.States;

public class AttackingState : BaseState<State, ZombieController>
{
    private bool CanAttack = true;
    
    public override async void OnEnter()
    {
        base.OnEnter();
        if (!CanAttack)
        {
            ChangeState(State.Idle);
            return;
        }
        GD.Print("ATTACK!");
        Parent.DesiredVelocity *= 0;
        CanAttack = false;
        var timer = Parent.GetTree().CreateTimer(1);
        ChangeState(State.Idle);
        await Parent.ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
        CanAttack = true;
    }
}