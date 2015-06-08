using UnityEngine;
using System.Collections;
using MyCollision;
public class Bullet:Projectile {

	bool deathFlag=false;
	public Bullet()
	{
		//Perform Needed Initializer Tasks then Add to Manager
		proj_ID = "Bullet_1"; //Make sure this matches ProjectileMan.CreateProjectile
		speed= 50;
		// Projectile_Manager.Add_Projectile(this);
	}
	protected override void Get_Prefab()
	{	
		go_Projectile=Resources.Load<GameObject>("Prefabs/FX_Projectile");
		go_Projectile=GameObject.Instantiate(go_Projectile, Vector2.zero, Quaternion.identity) as GameObject;
		go_Projectile.SetActive(false);		
		//Set Parent
		go_Projectile.transform.parent= GameObject.Find("Projectile_Manager").transform;

		animController = go_Projectile.GetComponent<Animator>();
		//Set the Correct AnimationController
		animController.runtimeAnimatorController= Resources.Load<RuntimeAnimatorController>("AnimController/FX_Shot1");

	}
	public override void Setup_Projectile(Vector2 origin, Vector2 direction)
	{
		isAlive=true;
		go_Projectile.SetActive(true);
		this.position = origin;
		this.direction= direction;

		go_Projectile.transform.position=origin;
		//Now to Rotate
		// go_Projectile.transform.rotation = Quaternion.LookRotation(direction,new Vector3(0,0,1));
		// go_Projectile.transform.LookAt(direction, Vector3.forward);	
		// go_Projectile.transform.rotation= Quaternion.FromToRotation(go_Projectile.transform.forward, direction);

		//Using Direction, Rotation Can be Discovered.

		//Only need to Rotate around "Z"
		float rotation= Mathf.Atan( direction.y/ direction.x) * 180/Mathf.PI;
		go_Projectile.transform.rotation = Quaternion.Euler(0,0, rotation);

		//Inverse Rotate if NEgative Direction
		if(direction.x<0)
			go_Projectile.transform.localScale= new Vector3(-1,1,1);


		// UnityEngine.Debug.Log("Projectile is now Setup!");
		// UnityEngine.Debug.Log("O="+origin+ "D="+ direction);

		animController.SetTrigger("Fire");
		this.deathFlag=false;
	}
	//This updates the Projectiles Position
	public override void Update(float timeElapsed)
	{
		if(deathFlag)
		{
			//Check if Impact Animation is done.
			if(animController.GetCurrentAnimatorStateInfo(0).IsName("Dump"))
			{
				KillProjectile();
			}
		}
		else
		{

			Vector2 newPosition = position + (direction* speed * timeElapsed);
			//Perform Collision Check at the currentposition
			Check_Collision(position, newPosition);

						// Debug.Log("Updating Proj");
			if(!deathFlag)
			{
				position=newPosition;
				go_Projectile.transform.position= position;
			}
		}
	}

	void Check_Collision(Vector2 oldPosition, Vector2 newPosition)
	{

		// Box Contains is problematic due to rotations
		if(Collision_Engine.Collision_Check_BoxContains(newPosition)!=null)
		{
			deathFlag=true;
			animController.SetTrigger("Impact");
		}
		else
		{
			Edge edge = Collision_Engine.Collision_Check_FirstAny(oldPosition, newPosition );
			if(edge.side!= Edge_Side.None)
			{
				deathFlag=true;
				animController.SetTrigger("Impact");
			}
		}
	}

	//The Projectile doesn't get freed, just returned to the pool
	void KillProjectile()
	{
		//Set Alive Flag to False
		isAlive=false;
		go_Projectile.SetActive(false);
		go_Projectile.transform.position= Vector2.zero;
	}
}
