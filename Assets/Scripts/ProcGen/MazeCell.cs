using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public bool collapsed;
    public bool incompleteCorridor;
    public bool isCorridor; // New state to represent whether the cell is a corridor
    private int weight;

    // Neighboring cells
    public MazeCell upNeighbor;
    public MazeCell downNeighbor;
    public MazeCell leftNeighbor;
    public MazeCell rightNeighbor;
    public MazeCell frontNeighbor;
    public MazeCell backNeighbor;

    public void SetWeight(int newWeight)
    {
        weight = newWeight;
    }

    public int GetWeight()
    {
        return weight;
    }

    void OnDrawGizmos()
    {
        if (!collapsed)
        {
            Gizmos.color = Color.red;  // Non-collapsed cells are red
        }

        else if (incompleteCorridor)
        {
            Gizmos.color = Color.yellow;
            
        }
        else if (isCorridor)
        {
            Gizmos.color = Color.blue; 
        }
        else if (collapsed)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.black ;  //something went highly wrong if this shows
        }

        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
    }
}
