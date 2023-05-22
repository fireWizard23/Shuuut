using System.Threading.Tasks;

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
            await Task.Delay(Parent.Rng.RandiRange(1000, 2000));
            if (StateManager.CurrentStateEnum != State.Idle) return;
            ChangeState(State.Wandering);
        }
        
        
    }
}