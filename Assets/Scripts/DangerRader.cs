using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DangerRader : MonoBehaviour {
	
	FadeCtrl fadeDanger;
	List<DangerObjectCtrl> dangerObjectList;
	
	// Use this for initialization
	void Start () {
		/*
		fadeDanger = GameManager.Instance.fadeDanger;
		dangerObjectList = new List<DangerObjectCtrl>();
		StartCoroutine(CheckDistance());
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	/*
	IEnumerator CheckDistance()
	{
		yield return null;
		float distance = 1000f;
		for (int i = 0; i < dangerObjectList.Count; i++) {
			Vector3 dangerObjPos = dangerObjectList[i].gameObject.transform.position;
			float newDistance = Vector3.Distance(dangerObjPos, transform.position);
			distance = distance > newDistance ? newDistance : distance;
			Debug.Log("distance : " + distance);
		}
	}
	*/
	
}
