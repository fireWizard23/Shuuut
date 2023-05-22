using Godot;

public partial class HealthController : Node2D
{
	[Export] public float InitialHealth { get; private set; } = 100;
	public float CurrentHealth { get; private set; }
	


	[Signal]
	public delegate void OnHealthChangeEventHandler(float change);
	
	
	[Signal]
	public delegate void OnHealthZeroEventHandler();

	public override void _Ready()
	{
		base._Ready();
		CurrentHealth = InitialHealth;
	}


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
