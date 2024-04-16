using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCBuilder : MonoBehaviour
{
    [SerializeField] private int Width;
    [SerializeField] private int Height;


    //arrray to store tiles
    private WFCNode[,] _grid;


    //all possible nodes
    public List<WFCNode> Nodes = new List<WFCNode>();

    //list to store the tiles that need to be collapsed
    private List<Vector2Int> _toCollapse = new List<Vector2Int>();

    private Vector2Int[] offsets = new Vector2Int[]{
        new Vector2Int(0,1), // top
        new Vector2Int(0,-1), // bottom
        new Vector2Int(1,0), // right
        new Vector2Int(-1,0) // left
    };

    private void Start()
    {
       _grid = new WFCNode[Width, Height];
       CollapseWorld();
    }

    private void CollapseWorld(){
        _toCollapse.Clear();
        _toCollapse.Add(new Vector2Int(Width/2, Height/2));
        while (_toCollapse.Count > 0){
            int x = _toCollapse[0].x;
            int y = _toCollapse[0].y;

            List<WFCNode> potentialNodes = new List<WFCNode>(Nodes);
            for (int i = 0; i < offsets.Length; i++){
                Vector2Int offset = offsets[i];
                Vector2Int neighbor = new Vector2Int(x + offset.x, y + offset.y);
                if (isInsideGrid(neighbor)){
                    WFCNode neighborNode = _grid[neighbor.x, neighbor.y];
                    if (neighborNode != null){
                        switch (i){
                            case 0:
                                WhittleNodes(potentialNodes, neighborNode.Bottom.compatibleNodes);
                                break;
                            case 1:
                                WhittleNodes(potentialNodes, neighborNode.Top.compatibleNodes);
                                break;
                            case 2:
                                WhittleNodes(potentialNodes, neighborNode.Left.compatibleNodes);
                                break;
                            case 3:
                                WhittleNodes(potentialNodes, neighborNode.Right.compatibleNodes);
                                break;
                        }
                    
                    }else{
                        if (!_toCollapse.Contains(neighbor)){
                            _toCollapse.Add(neighbor);
                        }

                    }
                }

                if (potentialNodes.Count < 1){
                    _grid[x,y] = Nodes[0];
                    Debug.LogWarning("No valid nodes for " + x + ", " + y);
                    return;
                }
                else{
                    _grid[x,y] = potentialNodes[Random.Range(0, potentialNodes.Count)];
                    
                }
                GameObject newNode = Instantiate(_grid[x,y].Prefab, new Vector3(x, 0, y), Quaternion.identity);

                _toCollapse.RemoveAt(0);



                }
            }
        }


    private void WhittleNodes(List <WFCNode> potentialNodes, List <WFCNode> validNodes){
        for (int i = potentialNodes.Count - 1; i > -1; i--){
            if (!validNodes.Contains(potentialNodes[i])){
                potentialNodes.RemoveAt(i);
            }
        }
    }

    private bool isInsideGrid(Vector2Int v2int){
        if (v2int.x > - 1 && v2int.x < Width && v2int.y > -1 && v2int.y < Height){
            return true;
        }
        else{
            return false;
        }
    }




}
