using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public Slider volumeSlider;
    //public AudioSource musicSource;

    public Button muteButton;
    public Sprite normalButtonImage;
    public Sprite mutedButtonImage;

    public bool isMuted = false;

    private void Start()
    {
        //volumeSlider.value = musicSource.volume;
    }

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void DecreaseVolume()
    {
        volumeSlider.value -= 0.001f;
        SetVolume(volumeSlider.value); // Update the volume immediately
    }

    public void IncreaseVolume()
    {
        volumeSlider.value += 0.001f;
        SetVolume(volumeSlider.value); // Update the volume immediately
    }

    /*public void ToggleMute()
    {
        if (musicSource != null)
        {
            isMuted = !isMuted;
            musicSource.mute = isMuted;
            ChangeButtonImage(isMuted ? mutedButtonImage : normalButtonImage);
            SetFocusToNull();
        }
        else
        {
            Debug.LogError("No audiosource.");
        }
    }*/

    private void ChangeButtonImage(Sprite newImage)
    {
        Image buttonImage = muteButton.GetComponent<Image>();

        if (buttonImage != null)
        {
            buttonImage.sprite = newImage;
        }
        else
        {
            Debug.LogError("Button component or Image component not found.");
        }
    }

    private void SetFocusToNull()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }
}