using UnityEngine;
using System.Collections;

public class DangerObjectCtrl : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.CompareTag("Player"))
		{
			//Hit Player
			GameManager.Instance.OnGameOver(false);
		}
	}
}
