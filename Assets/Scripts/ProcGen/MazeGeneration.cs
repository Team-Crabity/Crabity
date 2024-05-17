using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Tasks;

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
    private float MutationRate = .75f; //75 percent chance
    private int corridorCounter = 0;

    public GameObject XXasset;
    public GameObject XYasset;
    public GameObject XZasset;
    public GameObject YXasset;
    public GameObject YYasset;
    public GameObject YZasset;
    public GameObject ZXasset;
    public GameObject ZYasset;
    public GameObject ZZasset;

    /*
    public GameObject pXpXasset;
    public GameObject pXpYasset;
    public GameObject pXpZasset;
    public GameObject pXmYasset;
    public GameObject pXmZasset;
    public GameObject mXmXasset;
    public GameObject mXpYasset;
    public GameObject mXpZasset;
    public GameObject mXmYasset;
    public GameObject mXmZasset;

    public GameObject pYpYasset;
    public GameObject pYpXasset;
    public GameObject pYpZasset;
    public GameObject pYmXasset;
    public GameObject pYmZasset;
    public GameObject mYmYasset;
    public GameObject mYpXasset;
    public GameObject mYpZasset;
    public GameObject mYmXasset;
    public GameObject mYmZasset;

    public GameObject pZpZasset;
    public GameObject pZpXasset;
    public GameObject pZpYasset;
    public GameObject pZmXasset;
    public GameObject pZmYasset;
    public GameObject mZmZasset;
    public GameObject mZpXasset;
    public GameObject mZpYasset;
    public GameObject mZmXasset;
    public GameObject mZmYasset;
    */

    public GameObject Tasset;

    void Awake()
    {
        GenerateMaze();
        SetNeighboringCells();
        GenerateAssets();
        GenerateCorridors();
    }

    void GenerateMaze()
    {
        for (int z = 0; z < dimensions; z++)
        {
            for (int y = 0; y < dimensions; y++)
            {
                for (int x = 0; x < dimensions; x++)
                {
                    // adjust cells based on height, width, and depth
                    Vector3 cellPosition = new Vector3(x * width, y * height, z * depth);
                    GameObject newCellObject = Instantiate(cellPrefab, cellPosition, Quaternion.identity);
                    newCellObject.name = "Cell (" + x + "," + y + "," + z + ")";
                    MazeCell newCell = newCellObject.AddComponent<MazeCell>(); 
                    gridComponents.Add(newCell); 

                    // Random weight of ceells
                    int weight = Random.Range(1, 101);
                    newCell.SetWeight(weight);

                    // weight of the cell
                    //Debug.Log("Weight of " + newCellObject.name + ": " + weight);

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

    List<MazeCell> FindShortestPath(MazeCell startCell, MazeCell endCell, bool collapseCells = true)
    {
        Dictionary<MazeCell, int> distances = new Dictionary<MazeCell, int>();
        Dictionary<MazeCell, MazeCell> previous = new Dictionary<MazeCell, MazeCell>();

        foreach (var cell in gridComponents)
        {
            distances[cell] = int.MaxValue;
            previous[cell] = null;
        }
        distances[startCell] = 0;
        PriorityQueue<MazeCell> queue = new PriorityQueue<MazeCell>();
        queue.Enqueue(startCell, 0);

        while (queue.Count > 0)
        {
            MazeCell currentCell = queue.Dequeue();
            List<MazeCell> neighbors = GetNeighbors(currentCell);

            foreach (var neighbor in neighbors)
            {
                // Skip collapsed cells
                if (neighbor.collapsed || neighbor.incompleteCorridor || neighbor.isCorridor)
                {
                    continue;
                }

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
        MazeCell current = endCell;
        while (current != null)
        {
            shortestPath.Add(current);
            current = previous[current];
        }

        // Collapse cells on the path or mark them as corridors
        foreach (var cell in shortestPath)
        {
            if (collapseCells)
            {
                cell.collapsed = true;
            }
            else
            {
                cell.isCorridor = true;
                cell.collapsed = false;
            }
        }
        return shortestPath;
    }


    void GenerateAssets()
    {
        FindShortestPath(gridComponents[0], gridComponents[gridComponents.Count - 1], true); // Calculate the shortest path
        Instantiate(startPrefab, gridComponents[0].transform.position, Quaternion.identity);
        Instantiate(endPrefab, gridComponents[gridComponents.Count - 1].transform.position, Quaternion.identity);
        MazeCell currentCell = gridComponents[0];
        // Instantiate assets for each cell in the path that is being collapsed
        for (int i = 1; i < gridComponents.Count - 1; i++) // Exclude the first and last cells
        {
            MazeCell nextCell = gridComponents[i];
            if (nextCell.collapsed == true)
            {
                for (int x = i; x < gridComponents.Count - 1; x++)
                {
                    MazeCell nextnextCell = gridComponents[x + 1]; // Get the next next cell
                    if (nextnextCell.collapsed == true)
                    {
                        Vector3 movementDirection = nextCell.transform.position - currentCell.transform.position;
                        Vector3 nextMovementDirection = nextnextCell.transform.position - nextCell.transform.position;
                        currentCell = nextCell; // Move to the next cell after taking movement.
                        MovementGeneration(currentCell, movementDirection, nextMovementDirection, false);

                    }
                }
            }
            else
            {
                continue; //skip collapsed we do not want to generate assets non collapsed spaces
            }
        }

    }

    void MovementGeneration(MazeCell currentCell, Vector3 movementDirection, Vector3 nextMovementDirection, bool corridor = false)
    {
        if (IsXMovement(movementDirection) && IsXMovement(nextMovementDirection))
        {
            float randomValue = Random.Range(0f, 1f);
            // Debug.Log($"Random Value: {randomValue}, Mutation Rate: {MutationRate}");
            if (corridorCounter <= 4 && randomValue < MutationRate && corridor == false)
            {
                currentCell.isCorridor = true;
                corridorCounter++;
                Debug.Log(corridorCounter);
            }
            else
            {
                Instantiate(XXasset, currentCell.transform.position, Quaternion.identity);
                Debug.Log($"Instantiated XXasset at {currentCell.transform.position}");
            }
        }
        else if (IsXMovement(movementDirection) && IsYMovement(nextMovementDirection))
        {
            Instantiate(XYasset, currentCell.transform.position, Quaternion.identity);
            Debug.Log($"Instantiated XYasset at {currentCell.transform.position}");
        }
        else if (IsXMovement(movementDirection) && IsZMovement(nextMovementDirection))
        {
            Instantiate(XZasset, currentCell.transform.position, Quaternion.identity);
            Debug.Log($"Instantiated XZasset at {currentCell.transform.position}");
        }
        else if (IsYMovement(movementDirection) && IsXMovement(nextMovementDirection))
        {
            Instantiate(YXasset, currentCell.transform.position, Quaternion.identity);
            Debug.Log($"Instantiated YXasset at {currentCell.transform.position}");
        }
        else if (IsYMovement(movementDirection) && IsYMovement(nextMovementDirection))
        {
            Instantiate(YYasset, currentCell.transform.position, Quaternion.identity);
            Debug.Log($"Instantiated YYasset at {currentCell.transform.position}");
        }
        else if (IsYMovement(movementDirection) && IsZMovement(nextMovementDirection))
        {
            Instantiate(YZasset, currentCell.transform.position, Quaternion.identity);
            Debug.Log($"Instantiated YZasset at {currentCell.transform.position}");
        }
        else if (IsZMovement(movementDirection) && IsXMovement(nextMovementDirection))
        {
            Instantiate(ZXasset, currentCell.transform.position, Quaternion.identity);
            Debug.Log($"Instantiated ZXasset at {currentCell.transform.position}");
        }
        else if (IsZMovement(movementDirection) && IsYMovement(nextMovementDirection))
        {
            Instantiate(ZYasset, currentCell.transform.position, Quaternion.identity);
            Debug.Log($"Instantiated ZYasset at {currentCell.transform.position}");
        }
        else if (IsZMovement(movementDirection) && IsZMovement(nextMovementDirection))
        {
            Instantiate(ZZasset, currentCell.transform.position, Quaternion.identity);
            Debug.Log($"Instantiated ZZasset at {currentCell.transform.position}");
        }
    }

    void GenerateCorridors()
    {
        // Keep track of the corridor cells and uncollapsed cells
        List<MazeCell> corridorCells = new List<MazeCell>();
        List<MazeCell> uncollapsedCells = new List<MazeCell>();

        // Separate the corridor cells and uncollapsed cells
        foreach (var cell in gridComponents)
        {
            if (cell.isCorridor)
            {
                corridorCells.Add(cell);
                Debug.Log("added to list");
            }
            else if (!cell.collapsed)
            {
                uncollapsedCells.Add(cell);
                Debug.Log("added collapsed cell to list");
            }
        }
       
        for(int i = 0; i < corridorCells.Count-1; i++)
        {
            // Choose a random corridor cell
            MazeCell corridorCell = corridorCells[i];

            // Choose a random uncollapsed cell
            MazeCell randomUncollapsedCell = uncollapsedCells[Random.Range(0, uncollapsedCells.Count)];

            Debug.Log($"Generating corridor from {corridorCell.gameObject.name} to {randomUncollapsedCell.gameObject.name}");

            // Find the shortest path from corridorCell to randomUncollapsedCell
            List<MazeCell> path = FindShortestPath(corridorCell, randomUncollapsedCell, false);

            // Collapse cells on the path
            foreach (var cell in path)
            {
                cell.collapsed = true;
                if (uncollapsedCells.Contains(cell))
                {
                    uncollapsedCells.Remove(cell);
                }
            }

            // Output the path
            string pathString = "Path: ";
            foreach (var cell in path)
            {
                pathString += cell.gameObject.name + " -> ";
            }
            Debug.Log(pathString.TrimEnd('-', '>', ' ')); // Trim the last arrow and space

            // Generate assets along the collapsed path
            Debug.Log("Generating Assets for Corridors");
            GenerateCorridorAssets(path);
        }
    }
    void GenerateCorridorAssets(List<MazeCell> corridorPath)
    {
        Instantiate(endPrefab, corridorPath[0].transform.position, Quaternion.identity);
        Instantiate(startPrefab, corridorPath[corridorPath.Count-1].transform.position, Quaternion.identity);
        Debug.Log("Generating Corridor Assets...");
        MazeCell currentCell = corridorPath[corridorPath.Count-1];
        for (int i = corridorPath.Count-2; i >= 1; i--)
        {
            MazeCell nextCell = corridorPath[i];
            MazeCell nextnextCell = corridorPath[i - 1];
            Vector3 movementDirection = nextCell.transform.position - currentCell.transform.position;
            Vector3 nextMovementDirection = nextnextCell.transform.position - nextCell.transform.position;
            currentCell = nextCell; //move to next cell before gen
            MovementGeneration(currentCell, movementDirection, nextMovementDirection, true);
            

            // Debug log for each cell
            Debug.Log($"Generated asset for cell {nextCell.gameObject.name}");
        }
        Debug.Log("Corridor Assets Generated.");
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
