using UnityEngine;
using System.Collections;

public class Btn_TimePlay : MonoBehaviour {

	void OnClick()
	{
		// Mission_Controller.isPaused = !Mission_Controller.isPaused;
		Mission_Controller.Toggle_IsPaused();
	}

}
