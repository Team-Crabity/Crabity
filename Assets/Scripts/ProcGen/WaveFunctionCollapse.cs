using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections;

public class WaveFunctionCollapse : MonoBehaviour
{
    public float cellWidth = 1.0f;
    public float cellHeight = 1.0f;
    public float cellDepth = 1.0f;
    public int dimensions;
    public Tile[] tileObjects;
    public List<Cell> gridComponents;
    public Cell cellObj;

    public Tile backupTile;

    private int iteration;

    private void Awake()
    {
        gridComponents = new List<Cell>();
        InitializeGrid();
    }

    void InitializeGrid()
    {
        for (int z = 0; z < dimensions; z++)
        {
            for (int y = 0; y < dimensions; y++)
            {
                for (int x = 0; x < dimensions; x++)
                {
                    float xPos = x * cellWidth;
                    float yPos = y * cellHeight;
                    float zPos = z * cellDepth;

                    Cell newCell = Instantiate(cellObj, new Vector3(xPos, yPos, zPos), cellObj.transform.rotation);
                    newCell.CreateCell(false, tileObjects);

                    gridComponents.Add(newCell);
                }
            }
        }

        StartCoroutine(CheckEntropy());
        SetNeighboringCells();
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
                    Cell currentCell = gridComponents[index];

                    // set the neighboring cells for the current cell
                    currentCell.frontNeighbor = GetNeighborCell(x, y, z + 1);
                    currentCell.backNeighbor = GetNeighborCell(x, y, z - 1);
                    currentCell.leftNeighbor = GetNeighborCell(x - 1, y, z);
                    currentCell.rightNeighbor = GetNeighborCell(x + 1, y, z);
                    currentCell.upNeighbor = GetNeighborCell(x, y + 1, z);
                    currentCell.downNeighbor = GetNeighborCell(x, y - 1, z);
                }
            }
        }
    }

    Cell GetNeighborCell(int x, int y, int z)
    {
        if (x >= 0 && x < dimensions && y >= 0 && y < dimensions && z >= 0 && z < dimensions)
        {
            return gridComponents[x + y * dimensions + z * dimensions * dimensions];
        }

        return null;
    }

    IEnumerator CheckEntropy()
    {
        List<Cell> tempGrid = new List<Cell>(gridComponents);
        tempGrid.RemoveAll(c => c.collapsed);
        tempGrid.Sort((a, b) => a.tileOptions.Length - b.tileOptions.Length);
        tempGrid.RemoveAll(a => a.tileOptions.Length != tempGrid[0].tileOptions.Length);

        yield return new WaitForSeconds(.025f);

        CollapseCell(tempGrid);
    }
    void CollapseCell(List<Cell> tempGrid)
    {
        int randIndex = UnityEngine.Random.Range(0, tempGrid.Count);
        Cell cellToCollapse = tempGrid[randIndex];

        cellToCollapse.collapsed = true;

        // Select a random tile from the available options
        if (cellToCollapse.tileOptions.Length > 0)
        {
            int randomTileIndex = UnityEngine.Random.Range(0, cellToCollapse.tileOptions.Length);
            Tile selectedTile = cellToCollapse.tileOptions[randomTileIndex];

            cellToCollapse.tileOptions = new Tile[] { selectedTile };

            // Instantiate the selected tile at the cell's position
            GameObject instantiatedTile = Instantiate(selectedTile.gameObject, cellToCollapse.transform.position, selectedTile.transform.rotation);
            instantiatedTile.transform.localScale = Vector3.one; // Set the scale to (1, 1, 1)
        }
        else
        {
            // Handle the case where no valid tile options are available
            Tile selectedTile = backupTile;
            cellToCollapse.tileOptions = new Tile[] { selectedTile };
        }

        UpdateGeneration();
    }

    void UpdateGeneration()
    {
        List<Cell> newGenerationCell = new List<Cell>(gridComponents);

        for (int z = 0; z < dimensions; z++)
        {
            for (int y = 0; y < dimensions; y++)
            {
                for (int x = 0; x < dimensions; x++)
                {
                    var index = x + y * dimensions + z * dimensions * dimensions;

                    if (gridComponents[index].collapsed)
                    {
                        newGenerationCell[index] = gridComponents[index];
                    }
                    else
                    {
                        List<Tile> options = new List<Tile>();
                        foreach (Tile t in tileObjects)
                        {
                            options.Add(t);
                        }

                        // Check the "front" neighbor
                        if (z < dimensions - 1)
                        {
                            Cell front = gridComponents[x + y * dimensions + (z + 1) * dimensions * dimensions];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOption in front.tileOptions)
                            {
                                var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOption);
                                var valid = tileObjects[validOption].backNeighbour;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }

                        // Check the "back" neighbor
                        if (z > 0)
                        {
                            Cell back = gridComponents[x + y * dimensions + (z - 1) * dimensions * dimensions];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOption in back.tileOptions)
                            {
                                var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOption);
                                var valid = tileObjects[validOption].frontNeighbour;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }

                        // Check the "left" neighbor
                        if (x > 0)
                        {
                            Cell left = gridComponents[(x - 1) + y * dimensions + z * dimensions * dimensions];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOption in left.tileOptions)
                            {
                                var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOption);
                                var valid = tileObjects[validOption].rightNeighbour;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }

                        // Check the "right" neighbor
                        if (x < dimensions - 1)
                        {
                            Cell right = gridComponents[(x + 1) + y * dimensions + z * dimensions * dimensions];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOption in right.tileOptions)
                            {
                                var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOption);
                                var valid = tileObjects[validOption].leftNeighbour;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }

                        // Check the "up" neighbor
                        if (y < dimensions - 1)
                        {
                            Cell up = gridComponents[x + (y + 1) * dimensions + z * dimensions * dimensions];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOption in up.tileOptions)
                            {
                                var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOption);
                                var valid = tileObjects[validOption].downNeighbour;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }

                        // Check the "down" neighbor
                        if (y > 0)
                        {
                            Cell down = gridComponents[x + (y - 1) * dimensions + z * dimensions * dimensions];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOption in down.tileOptions)
                            {
                                var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOption);
                                var valid = tileObjects[validOption].upNeighbour;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }

                        Tile[] newTileList = new Tile[options.Count];

                        for (int i = 0; i < options.Count; i++)
                        {
                            newTileList[i] = options[i];
                        }

                        newGenerationCell[index].RecreateCell(newTileList);
                    }
                }
            }
        }

        gridComponents = newGenerationCell;
        iteration++;

        if (iteration < dimensions * dimensions * dimensions)
        {
            StartCoroutine(CheckEntropy());
        }
    }


    void CheckValidity(List<Tile> optionList, List<Tile> validOption)
    {
        for (int x = optionList.Count - 1; x >= 0; x--)
        {
            var element = optionList[x];
            if (!validOption.Contains(element))
            {
                optionList.RemoveAt(x);
            }
        }
    }

}
