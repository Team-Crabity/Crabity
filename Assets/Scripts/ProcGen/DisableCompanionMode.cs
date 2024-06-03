using UnityEngine;

public class DisableCompanionMode : MonoBehaviour
{
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = PlayerManager.instance;
        if (playerManager != null)
        {
            playerManager.CompanionMode = false;
        }
        else
        {
            Debug.LogError("PlayerManager instance not found.");
        }
    }
}
