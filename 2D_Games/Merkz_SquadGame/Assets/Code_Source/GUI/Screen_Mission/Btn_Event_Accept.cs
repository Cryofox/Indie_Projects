using UnityEngine;
using System.Collections;

public class Btn_Event_Accept : MonoBehaviour {

	// Use this for initialization
	void OnClick()
	{
		//Use the Parent's name as a way of using 1 script for X Event objects :3
		string name = transform.parent.name;
		int index= (int)( name[name.Length-1]  - '0'); //Notice this only works for <10 digits atm. But that should suffice.
		// Debug.Log("Index="+index);
		EventManager.Action_Activate_EventN(index);
	}
}
