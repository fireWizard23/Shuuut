using System;
using Godot;

namespace Shuuut.World;

public class BaseState<T, K> where T : struct, Enum where K : Node
{
    protected StateManager<T, K> stateManager;

    public void Register(StateManager<T, K> stateManager)
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