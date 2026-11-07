using UnityEngine;

public class UIMutations : MonoBehaviour
{
    public void StopPlayerInput()
    {
        FloorManager.Instance.playerController.stopInputReading = true;
    }
    public void StartPlayerInput()
    {
        FloorManager.Instance.playerController.lookLocked = false;
    }
}
