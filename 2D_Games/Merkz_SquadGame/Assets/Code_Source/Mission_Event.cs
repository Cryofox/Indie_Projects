using UnityEngine;
using System.Collections;

public class Mission_Event{
	GameObject go_Event;
	Region targetRegion;
	int contamination=0;
	public Mission_Event(GameObject pnl_Event, Region region)
	{
		go_Event = pnl_Event;
		targetRegion=region;
		Configure_RandomEvent();
	}

	void Configure_RandomEvent()
	{
		//Choose Contamination level

		this.contamination=10;


	}


	//Tells the Region to get infected.
	public void InfectRegion()
	{
		targetRegion.Increment_Contaminate(contamination);
	}

	public void Set_Tween(Vector3 position)
	{
		TweenPosition.Begin(go_Event,0.25f,position);
	}
}
