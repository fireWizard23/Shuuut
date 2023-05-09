using System.Linq;
using System.Threading;
using Godot;

namespace Shuuut.World.Zombies.States;

public class IdleState : BaseState<State, ZombieController>
{
    
    public override async void OnEnter()
    {
        base.OnEnter();
        Parent.DesiredVelocity *= 0;
        if (stateManager.PreviousState is null)
        {
            ChangeState(State.Wandering);
            return;
        }
        if (Parent.Target != null)
        {
            ChangeState(State.Chasing);
            return;
        }

        if (stateManager.PreviousStateEnum == State.Wandering)
        {
            await Parent.ToSignal(Parent.GetTree().CreateTimer(Parent.Rng.RandfRange(0.2f, 2)), SceneTreeTimer.SignalName.Timeout);
            ChangeState(State.Wandering);
        }
        
        
    }
}