using UnityEngine;
using System.Collections;

public class Main_Loop : MonoBehaviour {

	// Use this for initialization
	Map map;
	void Start () {
		map = new Map();
		Mission_Controller.Initialize();
		EventManager.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		float time = Time.deltaTime;
		if(!Mission_Controller.isPaused)
			Mission_Controller.Update(time);


	}
}
