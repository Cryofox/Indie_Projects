using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Projectile {
	public Animator animController;
	public string proj_ID="Default";

	public Vector2 position;
	public Vector2 direction;
	public bool isAlive=false;
	protected GameObject go_Projectile;
	protected float speed=2;
	//When a list of projectiles are needed to be updated (Beam weapons)
	List<GameObject> go_Projectiles;
	
	public Projectile()
	{
	//Acquire the GameObject needed
		Get_Prefab();
	}
	

	//This will Poll the Projectile_Manager for a Prefab Copy from Projectile_Manager
	
	protected virtual void Get_Prefab()
	{

	}


	public virtual void Setup_Projectile(Vector2 origin, Vector2 direction)
	{

	}

	//This updates the Projectiles Position
	public virtual void Update(float timeElapsed)
	{

	}











}
