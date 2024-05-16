using UnityEngine;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Build.Layout;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEditor.VersionControl;

public class MazeGeneration : MonoBehaviour
{
    public int dimensions = 10; // Adjust dimensions as needed
    public GameObject cellPrefab;
    private List<MazeCell> gridComponents = new List<MazeCell>(); // Change the list type to MazeCell
    private Vector3 startCellPosition;
    private Vector3 endCellPosition;

    public GameObject startPrefab;
    public GameObject endPrefab;

    public float width;
    public float height;
    public float depth;

    public GameObject XXasset;
    public GameObject XYasset;
    public GameObject XZasset;
    public GameObject YXasset;
    public GameObject YYasset;
    public GameObject YZasset;
    public GameObject ZXasset;
    public GameObject ZYasset;
    public GameObject ZZasset;

    void Awake()
    {
        GenerateMaze();
        SetNeighboringCells();
        GenerateAssets();
    }

    void GenerateMaze()
    {
        for (int z = 0; z < dimensions; z++)
        {
            for (int y = 0; y < dimensions; y++)
            {
                for (int x = 0; x < dimensions; x++)
                {
                    // Adjust position based on height, width, and depth
                    Vector3 cellPosition = new Vector3(x * width, y * height, z * depth);
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
                cell.collapsed = true;
            }
        }
    }

    void GenerateAssets()
    {
        FindShortestPath(); // Calculate the shortest path
        MazeCell currentCell = gridComponents[0];
        // Instantiate startPrefab for the first cell
        Instantiate(startPrefab, currentCell.transform.position, Quaternion.identity);

        // Instantiate assets for each cell in the path
        for (int i = 1; i < gridComponents.Count - 1; i++) // Exclude the first and last cells
        {
            MazeCell nextCell = gridComponents[i];
            if (nextCell.collapsed == false)
            {
                for (int x = i; x < gridComponents.Count - 1; x++)
                {
                    MazeCell nextnextCell = gridComponents[x + 1]; // Get the next next cell
                    if (nextnextCell.collapsed == false || nextnextCell == gridComponents[gridComponents.Count - 1])
                    {
                            // Get movement directions for the current cell, next cell, and next next cell
                            Vector3 movementDirection = nextCell.transform.position - currentCell.transform.position;
                        Vector3 nextMovementDirection = nextnextCell.transform.position - nextCell.transform.position;
                        currentCell = nextCell; // Move to the next cell

                        // Instantiate appropriate asset based on the movement directions
                        if (IsXMovement(movementDirection) && IsXMovement(nextMovementDirection))
                        {
                            Instantiate(XXasset, currentCell.transform.position, Quaternion.identity);
                        }
                        else if (IsXMovement(movementDirection) && IsYMovement(nextMovementDirection))
                        {
                            Instantiate(XYasset, currentCell.transform.position, Quaternion.identity);
                        }
                        else if (IsXMovement(movementDirection) && IsZMovement(nextMovementDirection))
                        {
                            Instantiate(XZasset, currentCell.transform.position, Quaternion.identity);
                        }
                        else if (IsYMovement(movementDirection) && IsXMovement(nextMovementDirection))
                        {
                            Instantiate(YXasset, currentCell.transform.position, Quaternion.identity);
                        }
                        else if (IsYMovement(movementDirection) && IsYMovement(nextMovementDirection))
                        {
                            Instantiate(YYasset, currentCell.transform.position, Quaternion.identity);
                        }
                        else if (IsYMovement(movementDirection) && IsZMovement(nextMovementDirection))
                        {
                            Instantiate(YZasset, currentCell.transform.position, Quaternion.identity);
                        }
                        else if (IsZMovement(movementDirection) && IsXMovement(nextMovementDirection))
                        {
                            Instantiate(ZXasset, currentCell.transform.position, Quaternion.identity);
                        }
                        else if (IsZMovement(movementDirection) && IsYMovement(nextMovementDirection))
                        {
                            Instantiate(ZYasset, currentCell.transform.position, Quaternion.identity);
                        }
                        else if (IsZMovement(movementDirection) && IsZMovement(nextMovementDirection))
                        {
                            Instantiate(ZZasset, currentCell.transform.position, Quaternion.identity);
                        } 
                    }
                }
            }
            else
            {
                continue; // Skip collapsed cells
            }
        }
        // Instantiate endPrefab for the last cell
        Instantiate(endPrefab, gridComponents[gridComponents.Count - 1].transform.position, Quaternion.identity);
    }

    bool IsXMovement(Vector3 direction)
    {
        return Mathf.Abs(direction.x) > Mathf.Abs(direction.y) && Mathf.Abs(direction.x) > Mathf.Abs(direction.z);
    }

    bool IsYMovement(Vector3 direction)
    {
        return Mathf.Abs(direction.y) > Mathf.Abs(direction.x) && Mathf.Abs(direction.y) > Mathf.Abs(direction.z);
    }

    bool IsZMovement(Vector3 direction)
    {
        return Mathf.Abs(direction.z) > Mathf.Abs(direction.x) && Mathf.Abs(direction.z) > Mathf.Abs(direction.y);
    }

}
