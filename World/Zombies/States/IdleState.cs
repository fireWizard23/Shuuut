using System.Linq;
using Godot;

namespace Shuuut.World.Zombies.States;

public class IdleState : BaseState<State, ZombieController>
{
    
    public override void OnEnter()
    {
        base.OnEnter();
        Parent.DesiredVelocity *= 0;
        if (Parent.Target != null)
        {
            ChangeState(State.Chasing);
        }
    }

    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        ChangeState(State.Wandering);
    }
}