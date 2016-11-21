using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FadeCtrl : MonoBehaviour {
	
	public _2dxFX_Color color;
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void OnFadeInOut(float inOutTime, float waitTime)
	{
		StartCoroutine(FadeInOut(inOutTime, waitTime));
	}
	
	IEnumerator FadeInOut(float inOutTime, float waitTime)
	{
		Debug.Log("start fadeInOut");
		yield return StartCoroutine(Fade(0f, inOutTime * 0.5f));
		yield return new WaitForSeconds(waitTime);
		yield return StartCoroutine(Fade(1f, inOutTime * 0.5f));
		Debug.Log("end fadeInOut");
		StartCoroutine(FadeInOut(inOutTime, waitTime));
	}
	
	public void OnFade(float alphaAmt, float time)
	{
		if(time == 0f)
		{
			color._Alpha = alphaAmt;
			return;
		}
		StartCoroutine(Fade(alphaAmt, time));
	}
	
	IEnumerator Fade(float alphaAmt, float time)
	{
		bool isFadeOut = color._Alpha < alphaAmt ? true : false;
		if(isFadeOut == true)
		{
			Debug.Log("Start fadeOut");
			while(color._Alpha < 0.95f)
			{
				yield return null;
				DOTween.To(()=> color._Alpha, x=> color._Alpha = x, 1f, time).SetEase(Ease.Linear);			
			}
			color._Alpha = 1f;
			Debug.Log("End fadeOut");
		}
		else
		{
			Debug.Log("Start fadeIn");
			while(color._Alpha > 0.05f)
			{
				yield return null;
				DOTween.To(()=> color._Alpha, x=> color._Alpha = x, 0f, time).SetEase(Ease.Linear);			
			}
			color._Alpha = 0f;
			Debug.Log("End fadeIn");			
		}		
	}
}
