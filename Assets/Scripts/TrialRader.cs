using UnityEngine;
using System.Collections;

public class TrialRader : MonoBehaviour {
	
	GameObject trial = null;
	public GameObject trialRader;
	
	void Start()
	{
		GameManager.Instance.EventCollectTrial += CollectTrial;
	}
	
	GameObject GetTrial()
	{
		int idx = DataManager.Instance.IdxGoalPoint;
		if(DataManager.Instance.IdxGoalPoint == GoalManager.Instance.trials.Length)
		{
			Debug.Log("idxGoalPoint : " + DataManager.Instance.IdxGoalPoint);
			idx -= 1;
		}
		return GoalManager.Instance.goalPoint[idx];
	}
	
	void CollectTrial(GameObject trial)
	{
		StartCoroutine(CoCollectTrial(GetTrial()));
	}
	
	IEnumerator CoCollectTrial(GameObject trial)
	{
		yield return null;
		this.trial = trial;
	}
	
	// Update is called once per frame
	void Update () {
		if(trial == null);
		{
			trial = GetTrial();
		}
		Vector3 forward = trialRader.transform.position - trial.transform.position;
		trialRader.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);			
	}
}
