using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveFunctionCollapse : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public int depth = 10;
    public float cellWidth = 1.0f;
    public float cellHeight = 1.0f;
    public float cellDepth = 1.0f;
    public Cell[,,] gridComponents;
    public Tile[] tileObjects;
    public GameObject parentObject;

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
                    cellGameObject.transform.parent = parentObject.transform;
                    cellGameObject.transform.localPosition = new Vector3(x * cellWidth, y * cellHeight, z * cellDepth);
                    Cell cellComponent = cellGameObject.AddComponent<Cell>();
                    cellComponent.CreateCell(false, tileObjects);
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
        int startX = Random.Range(0, width);
        int startY = Random.Range(0, height);
        int startZ = Random.Range(0, depth);
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

        // Check compatibility with all neighbors
        bool allNeighborsCompatible = true;
        foreach (Cell neighbor in GetAllNeighbors(cell))
        {
            bool neighborCompatible = false;
            foreach (Tile neighborTile in neighbor.tileOptions)
            {
                if (AreTilesCompatible(cell.tileOptions, neighborTile))
                {
                    neighborCompatible = true;
                    break;
                }
            }
            if (!neighborCompatible)
            {
                allNeighborsCompatible = false;
                break;
            }
        }

        if (allNeighborsCompatible)
        {
            Tile chosenTile = cell.tileOptions[Random.Range(0, cell.tileOptions.Length)];
            cell.RecreateCell(new Tile[] { chosenTile });
            cell.collapsed = true;
            InstantiateTile(chosenTile, cell.transform.position);

            // Update neighbors
            UpdateNeighbors(cell);
        }
        else
        {
            Debug.LogError($"Not all neighbors are compatible for cell at {cell.transform.position}. Cell not collapsed.");
        }
    }


    void InstantiateTile(Tile tile, Vector3 position)
    {
        Debug.Log($"Instantiating tile at {position}");
        var tileInstance = Instantiate(tile.gameObject, position, Quaternion.identity);
        tileInstance.transform.parent = parentObject.transform;
        tileInstance.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
    }

    void UpdateNeighbors(Cell cell)
    {
        List<Cell> neighbors = new List<Cell> { cell.upNeighbor, cell.downNeighbor, cell.leftNeighbor, cell.rightNeighbor, cell.frontNeighbor, cell.backNeighbor };
        foreach (Cell neighbor in neighbors)
        {
            if (neighbor != null && !neighbor.collapsed)
            {
                // Check compatibility of each neighbor tile with all candidate tiles of the current cell
                List<Tile> compatibleTiles = new List<Tile>();
                foreach (Tile neighborTile in neighbor.tileOptions)
                {
                    if (AreTilesCompatible(cell.tileOptions, neighborTile))
                    {
                        compatibleTiles.Add(neighborTile);
                    }
                }

                if (compatibleTiles.Count > 0)
                {
                    neighbor.RecreateCell(compatibleTiles.ToArray());
                    CollapseCell(neighbor);
                }
                else
                {
                    Debug.LogError($"No valid tiles found for neighbor at {neighbor.transform.position} based on current tile at {cell.transform.position}. Check tile setup.");
                }
            }
        }
    }


    bool AreTilesCompatible(Tile[] currentCellTiles, Tile neighborTile)
    {
        foreach (Tile currentTile in currentCellTiles)
        {
            if (currentTile.upNeighbour.Contains(neighborTile) ||
                currentTile.downNeighbour.Contains(neighborTile) ||
                currentTile.leftNeighbour.Contains(neighborTile) ||
                currentTile.rightNeighbour.Contains(neighborTile) ||
                currentTile.frontNeighbour.Contains(neighborTile) ||
                currentTile.backNeighbour.Contains(neighborTile))
            {
                return true;
            }
        }
        return false;
    }





    IEnumerable<Cell> GetAllNeighbors(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();

        if (cell.upNeighbor != null) neighbors.Add(cell.upNeighbor);
        if (cell.downNeighbor != null) neighbors.Add(cell.downNeighbor);
        if (cell.leftNeighbor != null) neighbors.Add(cell.leftNeighbor);
        if (cell.rightNeighbor != null) neighbors.Add(cell.rightNeighbor);
        if (cell.frontNeighbor != null) neighbors.Add(cell.frontNeighbor);
        if (cell.backNeighbor != null) neighbors.Add(cell.backNeighbor);

        return neighbors;
    }

}