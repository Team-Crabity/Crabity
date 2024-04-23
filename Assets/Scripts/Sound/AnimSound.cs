using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSound : MonoBehaviour
{
    public GameObject Player;
    private PlatformSound platformSound;

    void Start() 
    {
        platformSound = Player.GetComponent<PlatformSound>();
    }

    void playSound()
    {
        platformSound.PlayFootStep();
    }
}
