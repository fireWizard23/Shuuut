using System.IO.IsolatedStorage;
using Shuuut.Scripts;
using Shuuut.World;

namespace Shuuut.Player.States;

public class AttackingState : BaseState<State, Player>
{
    public override async void OnEnter()
    {
        base.OnEnter();
        await Parent._weaponHandler.UseWeapon();
        ChangeState(State.Normal);
    }
}