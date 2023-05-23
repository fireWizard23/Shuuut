using System.Threading.Tasks;

namespace Shuuut.Scripts;

public class InputBuffer
{
    public bool IsUsed { get; set; }
    public int TimeMs { get; init; }

    private Task toAwait;
	
    public async void Use()
    {
        if (IsUsed)
        {
            return;
        }
        IsUsed = true;
        toAwait = Task.Delay(TimeMs);
        await toAwait;
        IsUsed = false;
    }
}