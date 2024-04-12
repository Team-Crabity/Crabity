// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;

// [CustomEditor(typeof(Tile))]
// public class TileEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         Tile tile = (Tile)target;

//         EditorGUI.BeginChangeCheck();

//         tile.upNeighbour = (Tile[])EditorGUILayout.ObjectField("Up Neighbour", tile.upNeighbour, typeof(Tile[]), true);

//         if (EditorGUI.EndChangeCheck())
//         {
//             foreach (Tile neighbour in tile.upNeighbour)
//             {
//                 if (neighbour.downNeighbour != tile)
//                 {
//                     neighbour.downNeighbour = tile;
//                 }
//             }
//         }

//         // Draw the rest of the fields
//         DrawDefaultInspector();
//     }
// }