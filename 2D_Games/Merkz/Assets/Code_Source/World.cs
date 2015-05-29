using UnityEngine;
using System.Collections;
using MyCollision;
public class World : MonoBehaviour {
	
	public bool ccEnabled = false; 
	// Use this for initialization
	void Start () 
	{
		Collision_Engine.Load_FromScene(); //Load the Colliders for Collision Checks
		Spawn_Player();

		Invoke("SetCustomCursor",0.01f);  
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
		// GameObject.Find("Camera_Focus").transform.parent = ob.transform;

	}


   	void OnDisable()   
    {  
        //Resets the cursor to the default  
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);  
        //Set the _ccEnabled variable to false  
        this.ccEnabled = false;  
    }  

	private void SetCustomCursor()  
    {  
        //Replace the 'cursorTexture' with the cursor    
		Cursor.SetCursor(Resources.Load<Texture2D>("Sprites/AimCursor100x100"),new Vector2(100,100),CursorMode.Auto);
        Debug.Log("Custom cursor has been set.");  
        //Set the ccEnabled variable to true  
        this.ccEnabled = true;  
    }  

}

