using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WayPointDangerObjectCtrl : MonoBehaviour {
	
	DOTweenPath tweenPath = null;
	
	void Awake()
	{
		tweenPath = GetComponent<DOTweenPath>();
	}

    void Start()
    {
        tweenPath.DOTogglePause();
    }

    // Update is called once per frame
    void Update () {
	
	}
	
	void OnEnable()
	{
		tweenPath.DOPlay();
	}
	
	void OnDisable()
	{
		tweenPath.DOPause();
	}
}
