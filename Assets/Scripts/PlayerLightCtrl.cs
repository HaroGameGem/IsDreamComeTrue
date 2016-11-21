using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerLightCtrl : MonoBehaviour {
	
	public Light light;
	
	public float range = 3f;
	float setRange = 3f;
	public float intensity = 50f;
	float setIntensity = 50f;
	
	bool isSizing = false;
	
	void Start () {
		GameManager.Instance.EventCollectTrial += CollectTrial;
		range = setRange = light.range;
		intensity = setIntensity = light.intensity;
	}
	
	void Update()
	{
		if(isSizing)
		{
			light.range = setRange;
			light.intensity = setIntensity;
		}
	}
	
	void CollectTrial(GameObject trial)
	{
		StartCoroutine(CoCollectTrial());
	}
	
	IEnumerator CoCollectTrial()
	{
		yield return null;
		isSizing = true;
		DOTween.To(()=> setRange, x=> setRange = x, range, 2f).SetEase(Ease.InOutQuad);
		DOTween.To(()=> setIntensity, x=> setIntensity = x, intensity, 2f).SetEase(Ease.OutQuad);
		yield return new WaitForSeconds(2f);
		isSizing = false;
	}
}
