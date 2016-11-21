using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UIManager : MonoBehaviour {
	
	public GameObject btnStart;
	
	// Use this for initialization
	void Start () {
		GameManager.Instance.EventGameStart += GameStart;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void GameStart()
	{
		btnStart.SetActive(false);
		StartCoroutine(CoGameStart());
	}
	
	IEnumerator CoGameStart()
	{
		yield return null;
	}
}
