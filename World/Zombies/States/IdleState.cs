using System.ComponentModel;

namespace Shuuut.World.Zombies.States;

public class IdleState : BaseState<State, ZombieController> 
{
    public override void OnEnter()
    {
        base.OnEnter();
        Parent.Velocity *= 0;
    }

    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        ChangeState(State.Wandering);
    }
}