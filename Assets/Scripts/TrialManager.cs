using UnityEngine;
using System.Collections;

public class TrialManager : MonoBehaviour {
	
	public GameObject trialCollectParticlePrefab;
	
	TrialSelector[] trials;
	bool[] isActiveTrial;
	
	DataManager dataManager;
	
	public GameObject[] trialsGroup;
	
	// Use this for initialization
	void Start () {
		dataManager = DataManager.Instance;
		isActiveTrial = new bool[(int)eTrials.NEVER_DONT_USE_End-1];
		dataManager.isActiveTrial = this.isActiveTrial;
		
		trials = dataManager.trials;
		
		GameManager.Instance.EventGameStart += GameStart;
		GameManager.Instance.EventCollectTrial += CollectTrial;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void GameStart()
	{
		CheckTrials();
	}
	
	void CollectTrial(GameObject trial)
	{
		Vector3 pos = trial.transform.position;
		GameObject particle = Instantiate(trialCollectParticlePrefab, pos, Quaternion.identity) as GameObject;
		Destroy(particle, 2f);
		trial.SetActive(false);
		CheckTrials();
	}
	
	void CheckTrials()
	{
		for (int i = 0; i < isActiveTrial.Length; i++) {
			isActiveTrial[i] = false;
			Debug.Log("isActiveTrial : " + isActiveTrial[i]);
		}
		/*
		for (int i = 0; i < dataManager.IdxGoalPoint; i++) {
			int eTrialCnt = trials[i].trials.Length;
			for (int j = 0; j < eTrialCnt; j++) {
				print(i + " trial : " + ((int)trials[i].trials[j] - 1));
				isActiveTrial[(int)trials[i].trials[j] - 1] = true;
			}
		}
		*/
		int eTrialCnt = trials[dataManager.IdxGoalPoint-1].trials.Length;
		for (int j = 0; j < eTrialCnt; j++) {
			isActiveTrial[(int)trials[dataManager.IdxGoalPoint-1].trials[j] - 1] = true;			
		}
		
		for (int i = 0; i < isActiveTrial.Length; i++) {
			Debug.Log("isActiveTrial : " + isActiveTrial[i]);
		}
		
		TrialsSetting();
	}
	
	void TrialsSetting()
	{
		for (int i = 0; i < isActiveTrial.Length; i++) {
			if(trialsGroup[i] != null)
			{
				trialsGroup[i].SetActive(false);
			}
		}
		
		for (int i = 0; i < isActiveTrial.Length; i++) {
			Debug.Log("isActive : " + isActiveTrial[i]);
		}
		
		int idx = (int)eTrials.dangerObject-1;
		if(isActiveTrial[idx])
		{
			Debug.Log("SetActive0");
			trialsGroup[idx].SetActive(true);
		}
		
		idx = (int)eTrials.flyingDangerObject-1;
		if(isActiveTrial[idx])
		{
			Debug.Log("SetActive1");
			trialsGroup[idx].SetActive(true);
		}
		
		idx = (int)eTrials.rotateCamera-1;
		if(isActiveTrial[idx])
		{
			GameManager.Instance.OnCamRotation();
		}
		
		idx = (int)eTrials.canLookAt360-1;
		if(true)
		{
			GameManager.Instance.sightCtrl.isCanLookAt360 = isActiveTrial[idx];
		}
		
		idx = (int)eTrials.cantSeeAnything-1;
		if(true)
		{
			GameManager.Instance.sightCtrl.isCantSeeAnything = isActiveTrial[idx];			
		}
		
		idx = (int)eTrials.DEACTIVE_confuseControl-1;
		if(isActiveTrial[idx])
		{
			
		}
			
		idx = (int)eTrials.DEACTIVE_timeOut-1;
		if(isActiveTrial[idx])
		{
			
		}
	}
	
}
