using UnityEngine;
using System.Collections.Generic;

public class CellTesting : MonoBehaviour
{
    public GameObject[] assets; // Array to hold all assets
    public GameObject cellPrefab; // Prefab for the cell
    public float spacing = 2f; // Spacing between cells
    public int numberOfCells; // Number of cells to create

    private List<Vector3> cellPositions = new List<Vector3>(); // Store positions of cells

    void Awake()
    {
        GenerateCells();
    }

    void GenerateCells()
    {
        int assetCount = assets.Length;
        int cellCount = Mathf.Min(numberOfCells, assetCount);

        for (int i = 0; i < cellCount; i++)
        {
            // Calculate the position for each cell
            Vector3 cellPosition = new Vector3(i * spacing, 0, 0);
            cellPositions.Add(cellPosition);

            // Instantiate the cell
            GameObject newCellObject = Instantiate(cellPrefab, cellPosition, Quaternion.identity);
            newCellObject.name = "Cell (" + i + ")";

            // Instantiate the asset at the cell position
            Instantiate(assets[i], cellPosition, Quaternion.identity, newCellObject.transform);
        }
    }

    void OnDrawGizmos()
    {
        // Draw gizmos for cell centers
        Gizmos.color = Color.red;
        foreach (Vector3 position in cellPositions)
        {
            Gizmos.DrawSphere(position, 1f); // Draw a small sphere at the cell position
        }
    }
}
