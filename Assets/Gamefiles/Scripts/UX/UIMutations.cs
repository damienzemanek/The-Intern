using EMILtools.Extensions;
using UnityEngine;

public class UIMutations : MonoBehaviour
{
    public void StopPlayerInput(bool useCursor = true)
    {
        FloorManager.Instance.playerController.stopInputReading = true;
        if (useCursor)
            CursorEX.Set(true, CursorLockMode.None);

    }
    public void StartPlayerInput()
    {
        if (!FloorManager.HasInstance) return;
        FloorManager.Instance.playerController.stopInputReading = false;
        CursorEX.Set(false, CursorLockMode.Locked );
    }
}
