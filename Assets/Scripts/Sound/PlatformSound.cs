using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSound : MonoBehaviour
{
    public List<AudioClip> pipeSound;
    public List<AudioClip> brickSound;
    public List<AudioClip> woodSound; //floors and walls
    private AudioSource source;

    private FSMaterial TagOfMat;

    enum FSMaterial
    {
        Pipe,Wood,Brick,Empty
    }
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Debug.Log(TagOfMat);
    }

    public void PlayFootStep()
    {
        AudioClip clip = null;
        switch (TagOfMat)
        {
            case FSMaterial.Pipe:
                clip = pipeSound[Random.Range(0,pipeSound.Count)];
                break;
            case FSMaterial.Wood:
                clip = woodSound[Random.Range(0,woodSound.Count)];
                break;
            case FSMaterial.Brick:
                clip = brickSound[Random.Range(0,brickSound.Count)];
                break;
            default:
                break;
        }
        if (TagOfMat != FSMaterial.Empty)
        {
            source.clip = clip;
            source.volume = Random.Range(0.2f, 0.3f);
            source.pitch = Random.Range(0.95f, 1.05f);
            source.Play();
            // Debug.Log(source.clip);
        }
    }

    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject.tag == "Pipe") 
        {
            TagOfMat = FSMaterial.Pipe;
        }
        else if (col.gameObject.tag == "Wood") 
        {
            TagOfMat = FSMaterial.Wood;
        }
        else if (col.gameObject.tag == "Brick") 
        {
            TagOfMat = FSMaterial.Brick;
        }
        else
        {
            TagOfMat = FSMaterial.Empty;
        }
    }

    void OnCollisionExit () 
    {
        TagOfMat = FSMaterial.Empty;
    }
}
