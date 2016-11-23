using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FadeCtrl : MonoBehaviour {
	
	public _2dxFX_Color color;
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void FadeInOut(float inOutTime, float waitTime)
	{
		StartCoroutine(CoFadeInOut(inOutTime, waitTime));
	}
	
	IEnumerator CoFadeInOut(float inOutTime, float waitTime)
	{
		Debug.Log("start fadeInOut");
		yield return StartCoroutine(CoFade(0f, inOutTime * 0.5f));
		yield return new WaitForSeconds(waitTime);
		yield return StartCoroutine(CoFade(1f, inOutTime * 0.5f));
		Debug.Log("end fadeInOut");
		StartCoroutine(CoFadeInOut(inOutTime, waitTime));
	}
	
	public void Fade(float alphaAmt, float time)
	{
		if(time == 0f)
		{
			color._Alpha = alphaAmt;
			return;
		}
		StartCoroutine(CoFade(alphaAmt, time));
	}
	
	IEnumerator CoFade(float alphaAmt, float time)
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
