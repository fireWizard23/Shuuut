using System.Linq;
using System.Threading;
using Godot;

namespace Shuuut.World.Zombies.States;

internal class IdleState : BaseState<State, ZombieController>
{
    
    public override async void OnEnter()
    {
        base.OnEnter();
        Parent.DesiredVelocity *= 0;
        if (StateManager.PreviousState is null)
        {
            ChangeState(State.Wandering);
            return;
        }
        if (Parent.Target != null)
        {
            ChangeState(State.Chasing);
            return;
        }

        if (StateManager.PreviousStateEnum is State.Wandering or State.Chasing)
        {
            await Parent.ToSignal(Parent.GetTree().CreateTimer(Parent.Rng.RandfRange(1,2)), SceneTreeTimer.SignalName.Timeout);
            if (StateManager.CurrentStateEnum != State.Idle) return;
            ChangeState(State.Wandering);
        }
        
        
    }
}