using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Mission_Controller 
{
	public static bool isPaused=false;
	//Events per Year
	static int maxEvents =5;
	static int minEvents =2;
	static float mission_TotalYearTime=0;
	static float mission_CurrentYearTime=361; //1 above to force trigger new year
	static float rate= 106f; 
	static List<float> event_TriggerTimes=event_TriggerTimes = new List<float>();
	static List<Region> regions= new List<Region>();
 	static UILabel	lbl_YearCount;

	public static void Initialize()
	{
		//Create a Region for each Icon_Node under Pnl_Map
		int regionCount =19;
		for(int x=1;x<regionCount+1;x++)
		{
			GameObject go = GameObject.Find("Pnl_Map/Icon_Node_"+ (x)  );
			if(go==null)
				Debug.LogError("Could not find Region!");
			regions.Add(new Region(go,(x.ToString())));
		}
		lbl_YearCount = GameObject.Find("Lbl_Year").GetComponent<UILabel>();

	}


	//Pass in the Real Time that's passed and calculate the 
	//ingame "Year" time.
	public static void Update(float timeElapsed)
	{
		Check_GameOver();
		Update_Bars();
		lbl_YearCount.text=  "Years Passed:"+( (int)(mission_TotalYearTime)/360);
		//Before applying timeElapsed, check if an event has been triggered
		if(event_TriggerTimes.Count>0)
		{
			for(int x=0;x<event_TriggerTimes.Count;x++)
			{
				if(event_TriggerTimes[x]< mission_CurrentYearTime)
				{
					Trigger_Event();
					//Debug.Log(event_TriggerTimes.Count+"-Triggered At:"+ event_TriggerTimes[x]);

					event_TriggerTimes.RemoveAt(x);
					return; //Do not apply the following calculations
				}
			}
		}

		//1 Year will equal 1 minute oh waiting.
		//60 Seconds = a Year
		//1 Year = 365 days
		//365/60 = How many days pass
		//Use 360 for simplicity, 360/60= 6
		mission_TotalYearTime  += timeElapsed*rate;
		mission_CurrentYearTime+= timeElapsed*rate;
		if(mission_CurrentYearTime> 360)
		{
			//Trigger new Year Logic	
			Trigger_NewYear();
		}
	}

	static void Check_GameOver()
	{
		List<Region> safeRegions = Get_UnCorruptRegions(1);
		if(safeRegions.Count==0)
			Debug.LogError("GAME OVER!");
		else
			Debug.Log("Safe Regions:"+ safeRegions.Count);
	}


	static void Update_Bars()
	{
		for(int x=0;x< regions.Count;x++)
		{
			regions[x].Update_Percent();
		}
	}




	static void Trigger_NewYear()
	{
		mission_CurrentYearTime =   (float)((int)(mission_CurrentYearTime)%360);
		//When a new Year is calculated the number of events need to be created along with their trigger time.
		int events = Random.Range(minEvents, maxEvents+1);


		event_TriggerTimes.Clear();
		//Now to Calculate the event time slots
		while(event_TriggerTimes.Count < events)
		{
			float eventTime= Random.Range(0,360);
			bool isValid=true;
			for(int x=0;x<event_TriggerTimes.Count;x++)
			{
				if(event_TriggerTimes[x]==eventTime)
				{
					isValid=false;
					break;
				}
			}

			if(isValid)
			{
				//Debug.Log("Adding Event at:"+ eventTime);
				event_TriggerTimes.Add(eventTime);
			}

		}
	}

	static void Trigger_Event()
	{
		//Debug.Log("Event has been Triggered!");
		isPaused=true;
		EventManager.Spawn_Events(2); //Always Spawn 2 events (max 3)
	}

	public static float Get_TimePercent()
	{
		return mission_CurrentYearTime/360;
	}

//Returns a List of Regions
	public static List<Region> Get_UniqueRegions(int numRegions)
	{
		List<Region> selectedRegions = new List<Region>();

		//Incase numRegions > Max available regions, just return max
		if(numRegions>= regions.Count)
		{
			for(int x=0;x<regions.Count;x++)
				selectedRegions.Add(regions[x]);
		}
		else
		{
			int index =0;
			while(selectedRegions.Count< numRegions)
			{
				index= Random.Range(0, regions.Count);
				if(!selectedRegions.Contains(regions[index]))	
					selectedRegions.Add(regions[index]);
			}
		}

		return selectedRegions;
	}
//Returns a List of Available uncorrupt regions.
//If there are Less Free regions then requested, duplicates of the free region will be created.
	public static List<Region> Get_UnCorruptRegions(int numRegions)
	{
		List<Region> selectedRegions = new List<Region>();
		List<Region> freeRegions= new List<Region>();

		for(int x=0;x<regions.Count;x++)
		{
			if(regions[x].Get_ContaminationPercent()<1)
			{
				freeRegions.Add(regions[x]);

			}
		}

		//Repeat untill eithor, runout of free regions, or count is reached
		while(freeRegions.Count>0 && selectedRegions.Count<numRegions)
		{
			int index= Random.Range(0,freeRegions.Count);
			selectedRegions.Add(freeRegions[index]);
			freeRegions.RemoveAt(index);
		}

		if(selectedRegions.Count>0)
			while(selectedRegions.Count<numRegions)
			{
				//Here we duplicate a region by re-adding a reference :)
				selectedRegions.Add( selectedRegions[Random.Range(0,selectedRegions.Count)]);
			}
		// Debug.Log("Free Regions:"+ selectedRegions.Count);
		// for(int x=0;x<selectedRegions.Count;x++)
		// {
		// 	Debug.Log("Region:"+ selectedRegions[x].name);	
		// 	Debug.Log("Contamination:"+ selectedRegions[x].Get_ContaminationPercent());
		// }
		return selectedRegions;
	}

	public static void Toggle_IsPaused()
	{
		if(EventManager.CurrentEvents()==0)
			isPaused= !isPaused;
	}
}
