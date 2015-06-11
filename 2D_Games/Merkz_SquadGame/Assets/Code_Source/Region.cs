using UnityEngine;
using System.Collections;

public class Region {
	float contamination=0; 
	float max_Contamination=10;
	GameObject gObject;
	UIProgressBar uiBar;
	public string name;
	public Region(GameObject go ,string name="0")
	{
		gObject = go;
		this.uiBar= gObject.transform.GetChild(0).gameObject.GetComponent<UIProgressBar>();
		this.uiBar.value=0;
		this.name=name;
	}

	public void Update_Percent()
	{
		uiBar.value = contamination/max_Contamination;
	}


	public void Increment_Contaminate(int val)
	{
		contamination += (float) val;
		if(contamination>=max_Contamination)
		{
			contamination=max_Contamination;
			uiBar.alpha=0;
		}
	}

	public float Get_ContaminationPercent()
	{
		return contamination/max_Contamination;
	}

}
