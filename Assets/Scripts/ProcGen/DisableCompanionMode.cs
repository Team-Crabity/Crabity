using UnityEngine;

public class DisableCompanionMode : MonoBehaviour
{
    public PlayerManager playerManager;

     void Update()
    {
            if(playerManager.CompanionMode == true)
            {
                playerManager.CompanionMode = false;
                Debug.Log("Companion Mode not allowed in time trial.");
            }
    }
}
