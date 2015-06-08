using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Projectile_Manager {
	static int max_Projectiles=20; // This number will initialize 20 projectiles, and spawn more as needed.
	static GameObject go_PM;



	//List of All Projectile Types
	// static List <Projectile> projectiles_Bullet;

	static Hashtable archive_Available;
	static Hashtable archive_Active;
	//To make things Faster There should be an Available & Active Archive.


	static public void Initialize()
	{
		go_PM = new GameObject();
		go_PM.name= "Projectile_Manager";
		archive_Available = new Hashtable();
		archive_Active = new Hashtable();
		// projectiles = new List<Projectile>();
		string id = "Bullet_1";

		for(int x=0;x<max_Projectiles;x++)
		{
			//Here we skip the activate call and Just ADD the projectile
			Add_Projectile(Create_Projectile(id), id);
		}

	}

	//Create a HashTable that Links Lists to ID's
	//This is done for the Available Archive
	static void Add_Projectile(Projectile projectile, string ID)
	{
		//If it has a "Key" for this projectile type then it has the list.
		if(archive_Available.ContainsKey(ID))
		{
			List<Projectile> temp = (List<Projectile>)archive_Available[ID];
			temp.Add(projectile);
		}
		//Projectile type doesn't exist so create the List at Key
		else
		{
			//Create Projectile List
			archive_Available.Add(ID, new List<Projectile>());
			//Add Projectile
			((List<Projectile>)(archive_Available[ID])).Add(projectile);
		}
	}


	static void Transfer_Archive( Hashtable from, Hashtable to, string ID, Projectile item)
	{
		//If our Target Archive doesn't have a List ready then we create one
		if(!to.ContainsKey(ID))
		{
		 	to.Add(ID, new List<Projectile>());	
		}

		List<Projectile> temp = (List<Projectile>)archive_Available[ID];
		for(int x=0;x<temp.Count;x++)
		{
			if(temp[x]==item)
			{
				Projectile holder = temp[x];
				temp.RemoveAt(x);
				((List<Projectile>)(to[ID])).Add(holder);
			}
		}	



	}

	//This is the only Entry Point needed for classes to Spawn Projectiles
	//
	//Enable a Projectile by providing the information it needs to perform.
	//ID = THe list of projectiles to traverse for an available projectile
	//Origin	= Where the Projectile should Start
	//Direction = What direction the Projectile is heading in
	static public void Activate_Projectile(string ID, Vector2 origin, Vector2 direction)
	{
		//Check if a List exists for Projectile type
		if(archive_Available.ContainsKey(ID))
		{
			List<Projectile> temp = (List<Projectile>)archive_Available[ID];
			for(int x=0;x<temp.Count;x++)
			{
				if(!temp[x].isAlive)
				{
					//Activate Projectile
					temp[x].Setup_Projectile(origin,direction);
					//Remove this item from available
					Transfer_Archive(archive_Available, archive_Active, ID, temp[x]);
					return; //Break out of loop
				}
			}	
		}
		//IF this line is hit, eithor IF wasn't caught, or it was but no Available projectile exists.
		//Eithor way a new one must be created.

		Projectile proj = Create_Projectile(ID);
		Add_Projectile(proj, ID);
		proj.Setup_Projectile(origin,direction);
		Transfer_Archive(archive_Available, archive_Active, ID, proj);
	}

	static Projectile Create_Projectile(string ID)
	{
		Projectile proj = null;
		switch(ID){

			case "Bullet_1":
				proj = new Bullet();
				// Debug.Log("Spawnning Projectile");
			break;

		}

		return proj;
	}


	static public void Update(float timeElapsed)
	{
		//Here we Traverse the List of Projectiles
		foreach (string id in archive_Active.Keys)
		{
			List<Projectile> temp = (List<Projectile>)archive_Active[id];
			//Traverse the List
			for(int x=0;x<temp.Count;x++)
			{
				if(!temp[x].isAlive)
				{
					//If a Projectile isDead move it to the AvailableHashtable
					Transfer_Archive(archive_Active,archive_Available,id,temp[x]);
				}
				else
				{
					//Update the Projectile
					temp[x].Update(timeElapsed);
				}
			}

		}

	}

}
