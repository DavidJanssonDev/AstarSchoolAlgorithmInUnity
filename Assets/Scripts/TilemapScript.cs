using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using nodeClass;
using System;

public class TilemapScript : MonoBehaviour
{
    private BaseScript ValueScript;
    public Tilemap NodeTilemap;
    private int nodeIdNumber;

    private Sprite StartingNodeSprite;
    private Sprite EndingNodeSprite;


    // Set up every Components that is needed 
    private void Awake()
    {
        ValueScript = GetComponent<BaseScript>();
        NodeTilemap = ValueScript.TilemapObject;
        StartingNodeSprite = ValueScript.StartingSprite;
        EndingNodeSprite = ValueScript.EndingSprite;
    }


    // Set for the project with the Nodes
    public void SetUpProject()
    {
        Create2DNodeList();
        ValueScript.NodeSetUpDone = true;
    }


  

    // Create a Node from the tile map
    private Node CreateNode(Vector3 CenterOfTile, Vector2Int TilePos, Sprite tileNodeSprite)
    {

        // Instantiate an empty GameObject
        GameObject emptyObject = new($"NodeObject {nodeIdNumber}");

        // Set the position of the empty GameObject to the center of the tile
        emptyObject.transform.position = CenterOfTile;

        // Make the empty GameObject a child of the tilemap
        emptyObject.transform.parent = NodeTilemap.transform;

        // Add the Node component to the empty GameObject
        Node nodeComponent = emptyObject.AddComponent<Node>();



        // Set properties of the Node component
        nodeComponent.NodePosition = TilePos;
        nodeComponent.NodeSprite = tileNodeSprite;
        nodeComponent.NodeGameobject = emptyObject;

        // Add the Node to the list of nodes
        return nodeComponent;
    }


    // Create the 2D List For the Nodes
    private void Create2DNodeList()
    {
        // Get the bounds of the tilemap
        BoundsInt bounds = NodeTilemap.cellBounds;

        // Loop through all positions within the bounds
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            List<Node> tempNodes = new();
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                // Get the position of the current tile
                Vector3Int tilePosition = new(x, y, 0);

                // Get the center position of the current tile
                Vector3 tileCenter = NodeTilemap.GetCellCenterWorld(tilePosition);
                Tile tileInTilemap = NodeTilemap.GetTile<Tile>(tilePosition);

                if (tileInTilemap != null)
                {
                    
                    Vector2Int tilePos = new(ValueScript.Nodes.Count, tempNodes.Count);
                    nodeIdNumber++;
                    //Debug.Log($"Tile pos (nr:{nodeIdNumber}): (X) {tilePos.x} / (Y) {tilePos.y}");
                    Node newNodeObject = CreateNode(tileCenter, tilePos , tileInTilemap.sprite);

                    tempNodes.Add(newNodeObject);
                }
            }
            if (tempNodes.Count > 0) ValueScript.Nodes.Add(tempNodes);

        }
    }
}



namespace nodeClass
{
    public class Node : MonoBehaviour
    {
        public Vector2Int NodePosition;
        public GameObject NodeGameobject;
        public Sprite NodeSprite;

        // Postions for the Start tile and the End tile 
        public Node StartNode;
        public Node EndNode;

        public Node parent;

        public List<Node> NeigbourList = new();
       
        public bool Traversable;

        // Alogrithm Permeters
        public float Fcost = 0;
        public float Gcost = 0;
        public float Hcost = 0;


        public void SetFcost()
        {
            SetGcost();
            SetHcost();
            Fcost = Gcost + Hcost;
        }

        // Set the disctance to the Start Node
        public void SetGcost()
        {
            Gcost = Vector2.Distance(NodePosition, StartNode.NodePosition);
        }


        // Set the disctance to the End Node
        public void SetHcost()
        {
            Hcost = Vector2.Distance(NodePosition, EndNode.NodePosition);
        }


        // Set the all of the cost of the node
        public void SetCostOfNode()
        {
            SetFcost();
        }


        // Set the Start and end node of the nodes
        public void SetEndAndStartNodes(Node StartPos, Node EndPos)
        {

            StartNode = StartPos;
            EndNode = EndPos;

        }


        public void DesplayNode()
        {
            Debug.Log($"CURRENT NODE : {NodeGameobject}", NodeGameobject);
        }
    }
}
