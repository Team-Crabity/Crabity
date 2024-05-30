using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDAudio : MonoBehaviour //Its called ED bc I cant use 3d
{
    [Header ("Audio")]
    [SerializeField] private List<GameObject> ObjList;
    [SerializeField] private GameObject TargetObject;
    private AudioSource Source;
    [SerializeField] private float VolumeAmplifier;
    [SerializeField] private float maxDist;
    public float finalVol;

    void Start()    
    {
        Source = GetComponent<AudioSource>();
    }
    void Update()
    {
        float Vol1 = AdjustVolume(ObjList[0], TargetObject);
        float Vol2 = AdjustVolume(ObjList[1], TargetObject);

        if (Vol1 > Vol2) 
        {
           finalVol = Vol1;
        }
        else if (Vol1 < Vol2)
        {
            finalVol = Vol2;
        }
        if (finalVol >= 0.3)
        {
            Source.volume = finalVol;
        }
        else
        {
            Source.volume = 0;
            Source.Stop();
        }
    }

    public float CheckDistance(Vector3 pos1, Vector3 pos2)
    {
        float distance = (pos1 - pos2).magnitude;
        return distance;
    }

    public float AdjustVolume(GameObject MovingObj, GameObject StillObj)
    {
        float volume = 0;
        float curDistance = CheckDistance(MovingObj.transform.position, StillObj.transform.position);
        Debug.Log("Current Distance is " + curDistance);
        if (curDistance < maxDist) 
        {
            volume = VolumeAmplifier * Mathf.Clamp(1f / curDistance, 0f, 1f); 
        }
        return volume;
    }
}
