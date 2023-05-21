using System;
using Godot;

namespace Shuuut.World;

public class BaseState<T, K> where T : struct, Enum where K : Node
{
    protected StateManager<T, K> StateManager;

    protected K Parent;

    protected void ChangeState(T newState)
    {
        StateManager.ChangeState(newState);
    }

    public void Register(StateManager<T, K> stateManager)
    {
        this.StateManager = stateManager;
        Parent = this.StateManager.Parent;
        OnRegister();
    }
    
    public virtual  void OnRegister() {}

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnExit()
    {
        
    }

    public virtual void Ready()
    {
        
    }
    
    
    public virtual void Process(double delta)
    {
    }

    public virtual void PhysicsProcess(double delta)
    {
    }

    public virtual void OnDestroy()
    {
        
    }
    
    

}