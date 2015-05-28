using UnityEngine;
using System.Collections;
using MyCollision;
public class World : MonoBehaviour {
	

	// Use this for initialization
	void Start () 
	{
		Collision_Engine.Load_FromScene(); //Load the Colliders for Collision Checks

		Spawn_Player();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Creature Movement -> Collision Correction -> Final Position
		Entity_Manager.Update(); 


	}


	void Spawn_Player()
	{
		GameObject ob = Resources.Load<GameObject>("Prefabs/Player_Gun");
		ob = GameObject.Instantiate(ob, Vector3.zero, Quaternion.identity) as GameObject;
		ob.AddComponent<Controller>();

		//Add Mob to Entity Man and return reference
		MovingObject mob = Entity_Manager.Add_Entity(ob, Vector2.zero);
		//Link Controller to Mob
		ob.transform.GetComponent<Controller>().Init_MovingObject(mob);
	}

}

