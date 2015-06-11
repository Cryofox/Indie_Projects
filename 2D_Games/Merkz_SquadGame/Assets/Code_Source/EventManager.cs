using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EventManager
{
	//GameObjects that get Re-Linked
	static List <Mission_Event> li_Event = new List<Mission_Event>();

	float gui_Distance=131;
 	static UILabel	lbl_EventCount;
 	static int eventCount=0;
	public static void Initialize()
	{

		lbl_EventCount = GameObject.Find("Lbl_Events").GetComponent<UILabel>();
	}
	// static void Update_Events()
	// {
	// 	//Check the count
	// 	//Position each Go where it needs to be.


	// }
	public static void Spawn_Events(int eventNum)
	{
		eventCount+=eventNum;
		lbl_EventCount.text= "Events:"+ eventCount;
		//This is called by Mission Controller, it then accesses Mission controller for it's
		//regions.

		//Get Num Regions equal to Num Events needed
		List<Region> reg = Mission_Controller.Get_UnCorruptRegions(eventNum);

		li_Event.Clear(); //Ensuring event list is cleared
		for(int x=0;x<reg.Count;x++)
		{
			li_Event.Add( new Mission_Event(GameObject.Find( "Pnl_Event"+(x+1) ), reg[x]));
		}

		//Set&Forget Tween Targets
		for(int x=0;x<li_Event.Count && x<reg.Count;x++)
		{
			li_Event[x].Set_Tween(new Vector3(18+131*x,-5,0));
		}
		//Now that events are existant we can pretend they don't exist anymore :D
	}

	//This is Called by a Button
	//It Chooses which of the Spawned Event's the user wishes to "Accept"
	public static void Action_Activate_EventN(int index)
	{
		//Index is base 1, modify to base 0
		index--;

		//Infect all regions that mission was not accepted on
		for(int x=0;x<li_Event.Count;x++)
		{
			// if(x!=index)
			// {
				li_Event[x].InfectRegion();
			// }
		}

		//Set&Forget Tween Targets
		for(int x=0;x<li_Event.Count;x++)
		{
			li_Event[x].Set_Tween(new Vector3(18+131*x,-150,0));
		}
		//De-Activate the GO's
		li_Event.Clear();
		Mission_Controller.Toggle_IsPaused();
	}	
	public static int CurrentEvents()
	{
		return li_Event.Count;
	}
}
