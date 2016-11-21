using UnityEngine;
using System.Collections;

public class TrialCtrl : MonoBehaviour {
	public int idx = 0;
	public TrialSelector selector;
	
	void Start()
	{
		selector = GoalManager.Instance.trials[idx];
	}
	
	void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.CompareTag("Player"))
		{
			GameManager.Instance.OnCollectTrial(this.gameObject);
		}
	}
}
