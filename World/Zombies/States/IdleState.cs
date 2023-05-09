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
            GD.Print("AWAITING TIMER!");
            await Parent.ToSignal(Parent.GetTree().CreateTimer(1), SceneTreeTimer.SignalName.Timeout);
            ChangeState(State.Wandering);
        }
        
        
    }
}