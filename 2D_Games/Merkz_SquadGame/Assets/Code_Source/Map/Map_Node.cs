using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Map_Node{
	GameObject debug_GO;
	List<GameObject> neighbours;
	public Map_Node(Vector2 location)
	{
		// debug_GO =Resources.Load<GameObject>("Prefabs/Tile");
		// debug_GO =GameObject.Instantiate(debug_GO, location, Quaternion.identity) as GameObject;
		// Debug.Log("Region");
		debug_GO = new GameObject();
		debug_GO.transform.position = location;
		neighbours = new List<GameObject>();
	}

}
