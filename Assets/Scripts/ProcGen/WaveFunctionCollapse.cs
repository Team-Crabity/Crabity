using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveFunctionCollapse : MonoBehaviour
{
    public int width = 10;  // Number of cells along the X-axis
    public int height = 10; // Number of cells along the Y-axis
    public int depth = 10;  // Number of cells along the Z-axis
    public float cellWidth = 1.0f;  // Width of each cell
    public float cellHeight = 1.0f; // Height of each cell
    public float cellDepth = 1.0f;  // Depth of each cell
    public Cell[,,] gridComponents; // 3D grid of cells
    public Tile[] tileObjects; // Array of possible tile types
    public GameObject parentObject; // Parent GameObject where all cells will be spawned

    void Start()
    {
        InitializeGrid();
        SetNeighboringCells();
        CollapseWaveFunction();
    }

    void InitializeGrid()
    {
        if (parentObject == null)
        {
            Debug.LogError("Parent GameObject is not assigned!");
            return;
        }

        gridComponents = new Cell[width, height, depth];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    GameObject cellGameObject = new GameObject($"Cell_{x}_{y}_{z}");
                    cellGameObject.transform.parent = parentObject.transform; // Set parent
                    // Set position relative to parent, using specified cell dimensions
                    cellGameObject.transform.localPosition = new Vector3(x * cellWidth, y * cellHeight, z * cellDepth);
                    Cell cellComponent = cellGameObject.AddComponent<Cell>();
                    cellComponent.CreateCell(false, tileObjects); // Initially, all tiles are possible
                    gridComponents[x, y, z] = cellComponent;
                    cellGameObject.isStatic = false;
                }
            }
        }
    }

    void SetNeighboringCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    Cell currentCell = gridComponents[x, y, z];
                    currentCell.upNeighbor = (y < height - 1) ? gridComponents[x, y + 1, z] : null;
                    currentCell.downNeighbor = (y > 0) ? gridComponents[x, y - 1, z] : null;
                    currentCell.leftNeighbor = (x > 0) ? gridComponents[x - 1, y, z] : null;
                    currentCell.rightNeighbor = (x < width - 1) ? gridComponents[x + 1, y, z] : null;
                    currentCell.frontNeighbor = (z < depth - 1) ? gridComponents[x, y, z + 1] : null;
                    currentCell.backNeighbor = (z > 0) ? gridComponents[x, y, z - 1] : null;
                }
            }
        }
    }

    void CollapseWaveFunction()
    {
        // Randomly select a starting cell
        int startX = Random.Range(0, width);
        int startY = Random.Range(0, height);
        int startZ = Random.Range(0, depth);

        // Start collapse from the initial cell
        CollapseCell(gridComponents[startX, startY, startZ]);
    }
    void CollapseCell(Cell cell)
    {
        if (cell.collapsed)
        {
            Debug.Log("Cell already collapsed");
            return;
        }

        Debug.Log("Collapsing Cell");
        Tile chosenTile = cell.tileOptions[Random.Range(0, cell.tileOptions.Length)];
        cell.RecreateCell(new Tile[] { chosenTile });
        cell.collapsed = true;

        InstantiateTile(chosenTile, cell.transform.position);

        UpdateNeighbors(cell);
    }


    void InstantiateTile(Tile tile, Vector3 position)
    {
        Debug.Log($"Instantiating tile at {position}");
        var tileInstance = Instantiate(tile.gameObject, position, Quaternion.identity);
        tileInstance.transform.parent = parentObject.transform;
        tileInstance.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); 
    }


    void UpdateNeighbors(Cell cell)
    {
        List<Cell> neighbors = new List<Cell> { cell.upNeighbor, cell.downNeighbor, cell.leftNeighbor, cell.rightNeighbor, cell.frontNeighbor, cell.backNeighbor };
        foreach (Cell neighbor in neighbors)
        {
            if (neighbor != null && !neighbor.collapsed)
            {
                Tile[] validTiles = neighbor.tileOptions.Where(t => AreTilesCompatible(t, cell.tileOptions[0])).ToArray();
                if (validTiles.Length > 0)
                {
                    neighbor.RecreateCell(validTiles);
                    CollapseCell(neighbor);
                }
                else
                {
                    Debug.LogError($"No valid tiles found for neighbor at {neighbor.transform.position} based on current tile at {cell.transform.position}. Check tile setup.");
                }
            }
        }
    }

    bool AreTilesCompatible(Tile candidate, Tile current)
    {
        return candidate.upNeighbour.Contains(current) ||
               candidate.downNeighbour.Contains(current) ||
               candidate.leftNeighbour.Contains(current) ||
               candidate.rightNeighbour.Contains(current) ||
               candidate.frontNeighbour.Contains(current) ||
               candidate.backNeighbour.Contains(current);
    }
}
