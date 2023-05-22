using System;
using System.Collections.Generic;
using Godot;

namespace Shuuut.World;

public class StateManager<T, K> where T : struct, Enum where K : Node
{
    
    
    
    public readonly K Parent;

    public BaseState<T, K> CurrentState { get; private set; }
    public BaseState<T, K> PreviousState { get; private set; }

    public T CurrentStateEnum{ get; private set; }
    public T? PreviousStateEnum { get; private set; }
    
    private readonly Dictionary<T, BaseState<T, K>> _allStates;

    
    
    public StateManager(Dictionary<T, BaseState<T, K> > states, K parent)
    {
        Parent = parent;

        _allStates = states;
        var index = 0;
        T firstState = default;
        foreach (var h in _allStates)
        {
                
            h.Value?.Register(this);
            if (index == 0)
            {
                firstState = h.Key;
            }
            index++;
        }
        ChangeState(firstState);
    }

    public void ChangeState(T newState)
    {
        PreviousStateEnum = CurrentStateEnum;
        CurrentStateEnum = newState;
        PreviousState = CurrentState;
        CurrentState = _allStates[newState];
        CurrentState?.OnEnter();
        PreviousState?.OnExit();
    }

    public void Ready()
    {
        foreach (var v in _allStates)
        {
            v.Value?.Ready();
        }
    }

    public void Process(double delta)
    {
        CurrentState?.Process(delta);
    }

    public void PhysicsProcess(double delta)
    {
        CurrentState?.PhysicsProcess(delta);
    }

    public void Destroy()
    {
        foreach (var state in _allStates.Values)
        {
            state.OnDestroy();
        }
        
    }
    

}
