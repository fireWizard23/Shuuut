using System;
using Godot;

namespace Shuuut.World;

public class BaseState<T, K> where T : struct, Enum where K : Node
{
    protected StateManager<T, K> stateManager;

    protected K Parent;

    protected void ChangeState(T newState)
    {
        stateManager.ChangeState(newState);
    }

    public void Register(StateManager<T, K> stateManager)
    {
        this.stateManager = stateManager;
        Parent = this.stateManager.Parent;
    }

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