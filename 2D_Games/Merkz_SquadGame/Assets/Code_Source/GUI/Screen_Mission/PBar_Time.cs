using UnityEngine;
using System.Collections;

public class PBar_Time : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		GetComponent<UIProgressBar>().value = Mission_Controller.Get_TimePercent();
	}
}
