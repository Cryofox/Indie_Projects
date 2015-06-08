using UnityEngine;
using System.Collections;

public class Map{

	public Map()
	{
		SpawnNodes();

	}

	void Configuration_1()
	{

		
	}

	void SpawnNodes()
	{
		for(int x=0;x<5;x++)
			new Map_Node(Vector2.zero);
	}


}
