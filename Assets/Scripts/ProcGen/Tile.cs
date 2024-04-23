
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public Tile[] upNeighbour;
    public Tile[] downNeighbour;
    public Tile[] frontNeighbour;
    public Tile[] backNeighbour;
    public Tile[] leftNeighbour;
    public Tile[] rightNeighbour;


    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }


}
