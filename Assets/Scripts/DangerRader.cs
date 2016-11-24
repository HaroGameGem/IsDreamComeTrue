using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DangerRader : MonoBehaviour {
	
	FadeCtrl fadeDanger;
	List<DangerObjectCtrl> dangerObjectList;
	
	// Use this for initialization
	void Start () {
		fadeDanger = GameManager.Instance.fadeDanger;
		dangerObjectList = new List<DangerObjectCtrl>();
		StartCoroutine(CheckDistance());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.CompareTag("DangerObject"))
		{
			if(coll.gameObject.activeSelf)
			{
				dangerObjectList.Add(coll.GetComponent<DangerObjectCtrl>());	
			}
			
		}
	}
	
	void OnTriggerExit2D(Collider2D coll)
	{
		if(coll.CompareTag("DangerObject"))
		{
			dangerObjectList.Remove(coll.GetComponent<DangerObjectCtrl>());
		}
	}
	
	IEnumerator CheckDistance()
	{
		yield return null;
		
		float distance = 1000f;
		for (int i = 0; i < dangerObjectList.Count; i++) {
			if(!dangerObjectList[i].gameObject.active)
			{
				dangerObjectList.Remove(dangerObjectList[i]);
				i = 0;
				if(dangerObjectList.Count == 0)
					fadeDanger.Fade(0f,0f);
			}
			
			if(dangerObjectList.Count > 0)
			{
				Vector3 dangerObjPos = dangerObjectList[i].gameObject.transform.position;
				float newDistance = Vector3.Distance(dangerObjPos, transform.position);
				distance = distance > newDistance ? newDistance : distance;
			}
			else
			{
				distance = 10f;
			}
		}
		if(dangerObjectList.Count > 0)
			fadeDanger.Fade(0.5f - distance * 0.1f, 0f);
		StartCoroutine(CheckDistance());
	}
	
}
