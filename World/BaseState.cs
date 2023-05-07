using System;

namespace Shuuut.World;

public class BaseState<T> where T : struct, Enum
{
    protected StateManager<T> stateManager;

    public void Register(StateManager<T> stateManager)
    {
        this.stateManager = stateManager;
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
    
    
    public virtual void Process(float delta)
    {
    }

    public virtual void PhysicsProcess(float delta)
    {
    }
    
    

}