using UnityEngine;
using System.Collections;

public class Gun {


//A core class that contains parameters Inherited classes can replace.
//Such as Num Projectiles, Projectile Angles, Projectile Types,
//Reload Speeds, Rate of Fire (Projectile Spawn Rate).
	public Gun(){}

	public virtual void Fire(Vector2 origin, Vector2 direction)
	{
		UnityEngine.Debug.Log("Firing Projectile");
		//Request Projectile Type:
		Projectile_Manager.Activate_Projectile("Bullet_1", origin, direction);
	}
}
