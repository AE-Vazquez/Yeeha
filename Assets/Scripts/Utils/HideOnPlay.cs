using UnityEngine;
using System.Collections;

public class HideOnPlay : MonoBehaviour
{
	void Awake ()
	{
		if (GetComponent<MeshRenderer> ()) {
			GetComponent<MeshRenderer> ().enabled = false;
		}
	}
}
