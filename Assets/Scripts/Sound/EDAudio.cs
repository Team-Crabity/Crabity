using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDAudio : MonoBehaviour //Its called ED bc I cant use 3d
{
    [Header ("Audio")]
    [SerializeField] private List<GameObject> ObjList;
    [SerializeField] private GameObject TargetObject;
    private AudioSource Source;
    private float maxDist = 20f;
    [SerializeField] private float finalVol;
    
    void Start()    
    {
        Source = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (!inRange(ObjList[0], TargetObject) || !inRange(ObjList[1], TargetObject))
        {
            finalVol = 0;
        }
    }
    void FixedUpdate()
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
            finalVol = 0;
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
        if (curDistance < maxDist) 
        {
            //Debug.Log("Current distance is " + curDistance);
            volume = 5 * Mathf.Clamp(1f / curDistance, 0f, 1f); //5 IS A MAGIC NUMBER
        }
        return volume;
    }
    private bool inRange(GameObject MovingObj, GameObject StillObj)
    {
        float curDistance = CheckDistance(MovingObj.transform.position, StillObj.transform.position);
        return curDistance < maxDist;
    }
}
