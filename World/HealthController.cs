using Godot;
using System;

public partial class HealthController : Node2D
{
	[Export]
	public float InitialHealth { get; private set; }
	public float CurrentHealth { get; private set; }


	[Signal]
	public delegate void OnHealthChangeEventHandler(float change);
	
	
	[Signal]
	public delegate void OnHealthZeroEventHandler();

	public void ReduceHealth(int damage)
	{
		CurrentHealth -= damage;
		EmitSignal(SignalName.OnHealthChange, -damage);
		if (CurrentHealth <= 0)
		{
			EmitSignal(SignalName.OnHealthZero);
		}
	} 
	
	
}
