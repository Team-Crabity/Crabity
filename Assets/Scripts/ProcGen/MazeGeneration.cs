using UnityEngine;
using System.Collections.Generic;

public class MazeGeneration : MonoBehaviour
{
    public int dimensions = 10; // Adjust dimensions as needed
    public GameObject cellPrefab;
    private List<MazeCell> gridComponents = new List<MazeCell>(); // Change the list type to MazeCell
    private Vector3 startCellPosition;
    private Vector3 endCellPosition;

    void Awake()
    {
        GenerateMaze();
        SetNeighboringCells();
        FindShortestPath();
    }

    void GenerateMaze()
    {
        for (int z = 0; z < dimensions; z++)
        {
            for (int y = 0; y < dimensions; y++)
            {
                for (int x = 0; x < dimensions; x++)
                {
                    Vector3 cellPosition = new Vector3(x, y, z);
                    GameObject newCellObject = Instantiate(cellPrefab, cellPosition, Quaternion.identity);
                    newCellObject.name = "Cell (" + x + "," + y + "," + z + ")";
                    MazeCell newCell = newCellObject.AddComponent<MazeCell>(); // Add MazeCell component to the GameObject
                    gridComponents.Add(newCell); // Add the MazeCell instance to the list

                    // Random weight
                    int weight = Random.Range(1, 101);
                    newCell.SetWeight(weight);

                    // Log the weight of the cell
                    Debug.Log("Weight of " + newCellObject.name + ": " + weight);

                    if (x == 0 && y == 0 && z == 0)
                    {
                        startCellPosition = cellPosition;
                    }
                    else if (x == dimensions - 1 && y == dimensions - 1 && z == dimensions - 1) // Ending cell
                    {
                        endCellPosition = cellPosition;
                    }
                }
            }
        }


    }
    void SetNeighboringCells()
    {
        for (int z = 0; z < dimensions; z++)
        {
            for (int y = 0; y < dimensions; y++)
            {
                for (int x = 0; x < dimensions; x++)
                {
                    int index = x + y * dimensions + z * dimensions * dimensions;
                    MazeCell currentCell = gridComponents[index];

                    // set the neighboring cells for the current cell
                    currentCell.frontNeighbor = GetNeighborCell(x, y, z + 1);
                    currentCell.backNeighbor = GetNeighborCell(x, y, z - 1);
                    currentCell.leftNeighbor = GetNeighborCell(x - 1, y, z);
                    currentCell.rightNeighbor = GetNeighborCell(x + 1, y, z);
                    currentCell.upNeighbor = GetNeighborCell(x, y + 1, z);
                    currentCell.downNeighbor = GetNeighborCell(x, y - 1, z);

                    // Consider neighbors of neighbors
                    if (currentCell.frontNeighbor != null)
                    {
                        currentCell.frontNeighbor.frontNeighbor = GetNeighborCell(x, y, z + 2);
                    }
                    if (currentCell.backNeighbor != null)
                    {
                        currentCell.backNeighbor.backNeighbor = GetNeighborCell(x, y, z - 2);
                    }
                    if (currentCell.leftNeighbor != null)
                    {
                        currentCell.leftNeighbor.leftNeighbor = GetNeighborCell(x - 2, y, z);
                    }
                    if (currentCell.rightNeighbor != null)
                    {
                        currentCell.rightNeighbor.rightNeighbor = GetNeighborCell(x + 2, y, z);
                    }
                    if (currentCell.upNeighbor != null)
                    {
                        currentCell.upNeighbor.upNeighbor = GetNeighborCell(x, y + 2, z);
                    }
                    if (currentCell.downNeighbor != null)
                    {
                        currentCell.downNeighbor.downNeighbor = GetNeighborCell(x, y - 2, z);
                    }
                }
            }
        }
    }
    MazeCell GetNeighborCell(int x, int y, int z)
    {
        if (x >= 0 && x < dimensions && y >= 0 && y < dimensions && z >= 0 && z < dimensions)
        {
            return gridComponents[x + y * dimensions + z * dimensions * dimensions];
        }

        return null;
    }
    List<MazeCell> GetNeighbors(MazeCell cell)
    {
        List<MazeCell> neighbors = new List<MazeCell>();

        // Check if neighboring cells exist and add them to the list
        if (cell.frontNeighbor != null)
        {
            neighbors.Add(cell.frontNeighbor);
        }
        if (cell.backNeighbor != null)
        {
            neighbors.Add(cell.backNeighbor);
        }
        if (cell.leftNeighbor != null)
        {
            neighbors.Add(cell.leftNeighbor);
        }
        if (cell.rightNeighbor != null)
        {
            neighbors.Add(cell.rightNeighbor);
        }
        if (cell.upNeighbor != null)
        {
            neighbors.Add(cell.upNeighbor);
        }
        if (cell.downNeighbor != null)
        {
            neighbors.Add(cell.downNeighbor);
        }

        return neighbors;
    }

    void FindShortestPath()
    {
        Dictionary<MazeCell, int> distances = new Dictionary<MazeCell, int>();
        Dictionary<MazeCell, MazeCell> previous = new Dictionary<MazeCell, MazeCell>();

        // Initialize distances
        foreach (var cell in gridComponents)
        {
            distances[cell] = int.MaxValue;
            previous[cell] = null;
        }
        distances[gridComponents[0]] = 0;
        PriorityQueue<MazeCell> queue = new PriorityQueue<MazeCell>();
        queue.Enqueue(gridComponents[0], 0);

        while (queue.Count > 0)
        {
            MazeCell currentCell = queue.Dequeue();
            List<MazeCell> neighbors = GetNeighbors(currentCell);

            foreach (var neighbor in neighbors)
            {
                int altDistance = distances[currentCell] + neighbor.GetWeight();
                if (altDistance < distances[neighbor])
                {
                    distances[neighbor] = altDistance;
                    previous[neighbor] = currentCell;
                    queue.Enqueue(neighbor, altDistance);
                }
            }
        }
        List<MazeCell> shortestPath = new List<MazeCell>();
        MazeCell current = gridComponents[gridComponents.Count - 1];
        while (current != null)
        {
            shortestPath.Add(current);
            current = previous[current];
        }

        shortestPath.Reverse();
        foreach (var cell in gridComponents)
        {
            if (!shortestPath.Contains(cell))
            {
                Destroy(cell.gameObject);
            }
        }
    }
}
