using UnityEngine;

public class UnlockableDoor : MonoBehaviour
{
    private Animator animator;
    public bool characterNearby = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Method to set the 'character_nearby' parameter to true
    public void SetCharacterNearby(bool value)
    {
        characterNearby = value;
        animator.SetBool("character_nearby", value);
    }
}
