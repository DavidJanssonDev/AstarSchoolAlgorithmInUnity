using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeClass;
using UnityEngine.Tilemaps;

public class BaseScript : MonoBehaviour
{
    public Tilemap TilemapObject;
    [Header("Tiles")]
    public Sprite StartingSprite;
    public Sprite EndingSprite;

    [Header("Nodes")]
    public bool NodeSetUpDone = false;
    public List<List<Node>> Nodes = new();


}
