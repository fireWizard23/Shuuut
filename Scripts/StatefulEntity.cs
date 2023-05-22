using System;
using Godot;
using Shuuut.World;
using Shuuut.World.Zombies;

namespace Shuuut.Scripts;

public partial class StatefulEntity<T, K> : CharacterBody2D where T: struct, Enum where K : Node
{
    protected StateManager<T, K> StateManager;

    protected virtual void BeforeReady()
    {
        
    }

    public override void _Ready()
    {
        base._Ready();
        BeforeReady();
        StateManager.Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        StateManager.Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        StateManager.PhysicsProcess(delta);
    }

    public void ChangeState(T newState) => StateManager.ChangeState(newState);




}