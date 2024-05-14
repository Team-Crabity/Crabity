using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public bool collapsed;
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
        else
        {
            Gizmos.color = Color.green;  // Collapsed cells without a chosen tile (if any) are yellow
        }

        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
    }
}
