using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool collapsed;
    public Tile[] tileOptions;
    public Cell upNeighbor;
    public Cell downNeighbor;
    public Cell leftNeighbor;
    public Cell rightNeighbor;
    public Cell frontNeighbor;
    public Cell backNeighbor;

    public void CreateCell(bool collapsedState, Tile[] tiles)
    {
        collapsed = collapsedState;
        tileOptions = tiles;
    }

    public void RecreateCell(Tile[] tiles)
    {
        tileOptions = tiles;
    }

    void OnDrawGizmos()
    {
        if (collapsed && tileOptions.Length > 0)
        {
            Gizmos.color = Color.green;  // Collapsed cells with a chosen tile are green
        }
        else if (!collapsed)
        {
            Gizmos.color = Color.red;  // Non-collapsed cells are red
        }
        else
        {
            Gizmos.color = Color.yellow;  // Collapsed cells without a chosen tile (if any) are yellow
        }

        // Draw a cube at the position of the GameObject
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));  // Adjust the size if necessary
    }
}
