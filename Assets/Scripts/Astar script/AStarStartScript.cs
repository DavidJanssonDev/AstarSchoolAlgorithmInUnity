using nodeClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarScript : MonoBehaviour
{
    private BaseScript BaseScriptValues;
    private TilemapScript ProjectTilemapScript;

    [Header("NODE STUFF")]

    public List<Node> OpenNodeList = new();
    public List<Node> CloseNodeList = new();
    public List<Node> RecalledList = new();
    public Tile PathTile;


    public Node StartNode = null;
    public Node TargetNode = null;

    public Node CurrentNode = null;

    public Sprite EndSprite;
    public Sprite StartSprite;



    void Start()
    {
        AllCleardForAlgorithm();
        Path();
        DebugResult();
        
    }
    

    // IMPORT ALL THE DEPENDINGS FOR THE SCRIPT TO WORK

    private void AllCleardForAlgorithm()
    {
        BaseScriptValues = GetComponent<BaseScript>();
        ProjectTilemapScript = GetComponent<TilemapScript>();
       
        
        while (!BaseScriptValues.NodeSetUpDone)
        {
            ProjectTilemapScript.SetUpProject();
        }

        // Find Start and End Node
        foreach (List<Node> nodeRow in BaseScriptValues.Nodes)
        {
            foreach (Node node in nodeRow)
            {
                if (node.NodeSprite == StartSprite)
                {
                    StartNode = node;
                }
                else if (node.NodeSprite == EndSprite)
                {
                    TargetNode = node;
                }
            }
        }

        // Set Start and End Node 
        foreach (List<Node> nodeRow in BaseScriptValues.Nodes)
        {
            foreach (Node node in nodeRow)
            {
                node.SetEndAndStartNodes(StartNode, TargetNode);
        
                List<Vector2Int> NeighborListPos = new()
                        {
                            new(node.NodePosition.x - 1, node.NodePosition.y - 1),
                            new(node.NodePosition.x + 1, node.NodePosition.y + 1),
                            new(node.NodePosition.x - 1, node.NodePosition.y    ),
                            new(node.NodePosition.x + 1, node.NodePosition.y    ),
                            new(node.NodePosition.x    , node.NodePosition.y - 1),
                            new(node.NodePosition.x    , node.NodePosition.y + 1),
                            new(node.NodePosition.x - 1, node.NodePosition.y + 1),
                            new(node.NodePosition.x + 1, node.NodePosition.y - 1)
                        };


                foreach (Vector2Int position in NeighborListPos)
                {
                    try
                    {
                        Node tempNode = BaseScriptValues.Nodes[position.x][position.y];
                        node.NeigbourList.Add(tempNode);

                    } catch{}
                }
            }
        }
    }


    void Path() {

        StartNode.SetCostOfNode();
        OpenNodeList.Add(StartNode);
        
        Node lowestFcostNode = null;

        float lowestFcost = StartNode.Fcost;
        
        while (OpenNodeList.Count > 0)
        {   

            foreach(Node node in OpenNodeList)
            {
                if (lowestFcost >= node.Fcost)
                {
                    lowestFcostNode = node;
                    lowestFcost = node.Fcost;
                }
            }
            

  
            Debug.Log($"OpenNodeList COUNT: {OpenNodeList.Count}");

            int OpenNodeListIndex = OpenNodeList.IndexOf(lowestFcostNode);



            OpenNodeList.RemoveAt(OpenNodeListIndex);
            CloseNodeList.Add(lowestFcostNode);



            // IF THE LOWEST FCOST NODE IS THE GOAL NODE;
            if (lowestFcostNode == TargetNode)
            {
                // ADD FUNCTION THAT REVERSE TRACES THE PATH
                break;
            }




            // GO Throw the neighbours in the node
            foreach (Node neighbourNode in lowestFcostNode.NeigbourList)    
            {

                // IF THE NEIGHBOURNODE IS A NODE THAT U CAN NOT GO OVER OR A NODE IN THE CLOSE LIST SKIP IT 
                if (neighbourNode.Traversable == true || CloseNodeList.Contains(neighbourNode))
                {
                    continue;
                }


                float newCostToNeighbour = lowestFcostNode.Gcost + Vector2.Distance(lowestFcostNode.NodePosition, neighbourNode.NodePosition);
                if (neighbourNode.Gcost > newCostToNeighbour || !OpenNodeList.Contains(neighbourNode))
                { 
                    neighbourNode.SetFcost();
                    neighbourNode.parent = lowestFcostNode;

                    if (!OpenNodeList.Contains(neighbourNode))
                    {
                        OpenNodeList.Add(neighbourNode);
                    }
                }
            }
            
        }
        Debug.Log("ALGOTHIM IS DONE");
        
    }


    void DebugResult()
    {
        foreach(Node node in CloseNodeList) 
        {
            Debug.Log($"Node Position: {node.NodePosition} | Node: {node.NodeGameobject} ");
        }

    }
}