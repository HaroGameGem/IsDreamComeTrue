using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour {
	
	static GameManager instance;
	static public GameManager Instance { get{return instance;} }
	
	public delegate void Callback();
	public delegate void CallbackOne(bool isWin);
	public delegate void GameObejctCallback(GameObject sender);
	
	public event Callback EventGameStart;
	public event CallbackOne EventGameOver;
	public event GameObejctCallback EventCollectTrial;
	
	public GameObject[] cameras;
	
	public GameObject player;
	public SightCtrl sightCtrl;
	
	public PlayerLightCtrl playerLight;
	public float addTrialLightRange;
	public float addTrialLightIntensity;
	
	public FadeCtrl fadeDanger;
	public FadeCtrl fadeGood;
	public FadeCtrl fadeBlack;
	
	// Use this for initialization
	void Start () {
		if(instance == null)
		{
			instance = this;			
		}
		else
		{
			Debug.Log("GameManager가 둘이상입니다.");
		}
		
		fadeBlack.Fade(1f, 0f);
		
		//Events
		EventGameStart += GameStart;
		EventGameOver += GameOver;
		EventCollectTrial += CollectTrial;
	}
	
	public void OnGameStart()
	{
		EventGameStart();
	}
	
	public void OnClickedBtnGameStart()
	{
		if(EventGameStart != null)
			EventGameStart();
		else
			Debug.Log("EventGameStart Empty");
	}
	
	void GameStart()
	{
		StartCoroutine(CoGameStart());
	}
	
	IEnumerator CoGameStart()
	{
		yield return null;
		DataManager dataManager = DataManager.Instance;
		//init
		dataManager.CamRot = eCamRotation.rot0;
		dataManager.trials = new TrialSelector[GoalManager.Instance.trials.Length - 2];
		
		int trialCnt = GoalManager.Instance.trials.Length - 2;
		addTrialLightRange = addTrialLightRange / trialCnt;
		addTrialLightIntensity = addTrialLightRange / trialCnt;
		
		fadeBlack.Fade(0f, 4f);
		player.transform.position = GoalManager.Instance.goalPoint[0].transform.position;
		yield return new WaitForSeconds(3f);
	}
	
	public void OnGameOver(bool isWin)
	{
		if(EventGameOver != null)
			EventGameOver(isWin);
		else
			Debug.Log("EventGameOver Empty");
	}
	
	void GameOver(bool isWin)
	{
		StartCoroutine(CoGameOver(isWin));
	}
	
	IEnumerator CoGameOver(bool isWin)
	{
		yield return null;
		Debug.Log("Game Over User Win : " + isWin);
		
		if(isWin)		
			fadeGood.Fade(0.5f, 0.5f);
		else
			fadeDanger.Fade(0.5f, 0.5f);
		yield return new WaitForSeconds(0.5f);
		fadeBlack.Fade(1f, 1f);		
		yield return new WaitForSeconds(2f);
		Application.LoadLevel(0);
	}
	
	
	public void OnCamRotation()
	{
		int n = Random.Range(1,4);
		if(DataManager.Instance.CamRot == (eCamRotation)n)
        {
            if (n == 1)
                n++;
            if (n == 4)
                n--;
        }
		
		float camRot = 0f;
		DataManager.Instance.CamRot = (eCamRotation)n;
		switch (DataManager.Instance.CamRot)
		{
		case eCamRotation.rot0:
			DataManager.Instance.CamRot = eCamRotation.rot90;
			camRot = 90f;
			break;
		case eCamRotation.rot90:
			DataManager.Instance.CamRot = eCamRotation.rot180;
			camRot = 180f;
			break;
		case eCamRotation.rot180:
			DataManager.Instance.CamRot = eCamRotation.rot270;
			camRot = 270f;
			break;
		case eCamRotation.rot270:
			DataManager.Instance.CamRot = eCamRotation.rot0;
			camRot = 0f;
			break;
		default:
			break;
		}
		
		for (int i = 0; i < cameras.Length; i++) {
			cameras[i].transform.DORotate(new Vector3(0f,0f,camRot), 0.2f);				
		}
	}
	
	public void OnCollectTrial(GameObject trial)
	{
		if(EventCollectTrial != null)
			EventCollectTrial(trial);
		else
			Debug.Log("EventCollectTrial Empty");
	}	
	
	public void CollectTrial(GameObject trial)
	{
		
		TrialSelector trialInfo = trial.GetComponent<TrialCtrl>().selector;
		sightCtrl.radius += trialInfo.addSightRadius;
		sightCtrl.angularRange += trialInfo.addSightAngularRange;
		
		playerLight.range += addTrialLightRange;
		playerLight.intensity += addTrialLightIntensity;
	}
	
}
