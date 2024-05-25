using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public bool collapsed;
    public bool incompleteCorridor;
    public bool isCorridor; 
    public bool xCorridor; 
    public bool yCorridor;
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
            Gizmos.color = Color.yellow;  // Incomplete corridor cells are yellow
        }
        else if (xCorridor)
        {
            Gizmos.color = Color.magenta;  // xCorridor cells are magenta
        }
        else if (yCorridor)
        {
            Gizmos.color = Color.cyan;  // yCorridor cells are cyan
        }
        else if (isCorridor)
        {
            Gizmos.color = Color.blue;  // Corridor cells are blue
        }
        else if (collapsed)
        {
            Gizmos.color = Color.green;  // Other collapsed cells are green
        }
        else
        {
            Gizmos.color = Color.black;  // Something went highly wrong if this shows
        }

        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
    }
}
