using UnityEngine;
using System.Collections;
using MyCollision;
public class World : MonoBehaviour {


	// Use this for initialization
	void Start () 
	{
		Collision_Engine.Load_FromScene(); //Load the Colliders for Collision Checks
		Spawn_Player();


		Projectile_Manager.Initialize();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float time = Time.deltaTime;
		//Creature Movement -> Collision Correction -> Final Position
		Entity_Manager.Update(time); 
		Projectile_Manager.Update(time);
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
		// GameObject.Find("Camera_Focus").transform.parent = ob.transform;

	}


 
}

