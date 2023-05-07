using System;
using System.Collections.Generic;
using Godot;

namespace Shuuut.World;

public class StateManager<T> where T : struct, Enum
{
    private Dictionary<T, BaseState<T>> allStates;
    public readonly Node Parent;

    public BaseState<T> CurrentState { get; private set; }
    public BaseState<T> PreviousState { get; private set; }

    public T CurrentStateEnum{ get; private set; }
    public T? PreviousStateEnum { get; private set; }
    
    public StateManager(Dictionary<T, BaseState<T>> states, Node parent)
    {
        Parent = parent;

        allStates = states;
        int index = 0;
        foreach (var h in allStates)
        {
                
            h.Value?.Register(this);
            if (index == 0)
            {
                ChangeState(h.Key);
                index++;
            }
        }
        
    }

    public void ChangeState(T newState)
    {
        PreviousStateEnum = CurrentStateEnum;
        CurrentStateEnum = newState;
        PreviousState = CurrentState;
        CurrentState = allStates[newState];
        CurrentState?.OnEnter();
        PreviousState?.OnExit();
    }

    public void Ready()
    {
        foreach (var v in allStates)
        {
            v.Value?.Ready();
        }
    }

    public void Process(float delta)
    {
        CurrentState?.Process(delta);
    }

    public void PhysicsProcess(float delta)
    {
        CurrentState?.PhysicsProcess(delta);
    }
    

}
