using UnityEngine;
using System.Collections;

public class GoalManager : MonoBehaviour {
	
	static GoalManager instance;
	public static GoalManager Instance
	{
		get { return instance; }
	}
	
	DataManager dataManager;
	
	public GameObject[] goalPoint;
	
	public TrialSelector[] trials;
	
	int idxGoalPoint = 1;

	// Use this for initialization
	void Start () {
		instance = this;
		dataManager = DataManager.Instance;
		
		Init();
		GameManager.Instance.EventGameStart += GameStart;
		GameManager.Instance.EventCollectTrial += CollectTrial;
	}
	
	void Init()
	{
		DataManager.Instance.trials = trials;
		
		for (int i = 1; i < goalPoint.Length; i++) {
			goalPoint[i].SetActive(false);
		}
		idxGoalPoint = 1;
		SetGoalPoint(1);		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void GameStart()
	{
		StartCoroutine(CoGameStart());
	}
	
	IEnumerator CoGameStart()
	{
		yield return null;
	}
	
	void CollectTrial(GameObject trial)
	{
		if(SetNextGoalPoint())
		{
			GameManager.Instance.OnGameOver(true);
		}
	}
	
	
	bool SetGoalPoint(int goal)
	{
		bool isEndPoint = false;
		if(goal == goalPoint.Length)
		{
			idxGoalPoint = goal;
			dataManager.IdxGoalPoint = idxGoalPoint;
			isEndPoint = true;
			return isEndPoint;
		}
		
		goalPoint[idxGoalPoint].SetActive(false);
		idxGoalPoint = goal;
		dataManager.IdxGoalPoint = idxGoalPoint;
		goalPoint[goal].SetActive(true);
		
		return isEndPoint;
	}
	
	public bool SetNextGoalPoint()
	{
		if(idxGoalPoint == goalPoint.Length)
		{
			Debug.Log("Already EndPoint");
			return false;
		}
		
		//if true is EndPoint
		return SetGoalPoint(idxGoalPoint+1);
	}
	
}
