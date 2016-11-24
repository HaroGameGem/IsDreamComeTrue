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
		}

		int eTrialLength = trials[dataManager.IdxGoalPoint-1].trials.Length;
		for (int j = 0; j < eTrialLength; j++) {
			isActiveTrial[(int)trials[dataManager.IdxGoalPoint-1].trials[j] - 1] = true;			
		}

		TrialsSetting();
	}
	
	void TrialsSetting()
	{
		//초기화
		for (int i = 0; i < isActiveTrial.Length; i++) {
			if(trialsGroup[i] != null)
			{
				trialsGroup[i].SetActive(false);
			}
		}
		SightCtrl sightCtrl = GameManager.Instance.sightCtrl;
		sightCtrl.isCanLookAt360 = false;
		sightCtrl.isCantSeeAnything = false;
			
		//위험한 물체
		int idx = (int)eTrials.dangerObject-1;
		if(isActiveTrial[idx])
		{
			trialsGroup[idx].SetActive(true);
		}
		
		//위험한 물체 웨이포인트
		idx = (int)eTrials.wayPointDangerObject-1;
		if(isActiveTrial[idx])
		{
			trialsGroup[idx].SetActive(true);
		}
		
		//숨겨진 물체
		idx = (int)eTrials.secretObstacleOn-1;
		if(isActiveTrial[idx])
		{
			trialsGroup[idx].SetActive(true);
		}
		
		//카메라 회전
		idx = (int)eTrials.rotateCamera-1;
		if(isActiveTrial[idx])
		{
			GameManager.Instance.OnCamRotation();
		}
		
		//360도 다 보임
		idx = (int)eTrials.canLookAt360-1;
		if(isActiveTrial[idx])
		{
			GameManager.Instance.sightCtrl.isCanLookAt360 = isActiveTrial[idx];
		}
		
		//아무것도 안보임
		idx = (int)eTrials.cantSeeAnything-1;
		if(isActiveTrial[idx])
		{
			GameManager.Instance.sightCtrl.isCantSeeAnything = isActiveTrial[idx];			
		}
		
		//방향키 바뀜
		idx = (int)eTrials.DEACTIVE_confuseControl-1;
		if(isActiveTrial[idx])
		{
			
		}
		
		//시간 제한
		idx = (int)eTrials.DEACTIVE_timeOut-1;
		if(isActiveTrial[idx])
		{
			
		}
	}
	
}
