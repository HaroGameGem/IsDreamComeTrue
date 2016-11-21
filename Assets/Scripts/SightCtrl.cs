using UnityEngine;
using System.Collections;
using DynamicLight2D;
using DG.Tweening;

public class SightCtrl : MonoBehaviour {
	
	public DynamicLight sight;
	
	public float radius = 3f;
	float setRadius = 3f;
	public float angularRange = 50f;
	float setAngularRange = 50f;
	
	public bool isCanLookAt360 = false;
	public bool isCantSeeAnything = false;
	
	
	// Use this for initialization
	void Start () {
		setRadius = radius;
		setAngularRange = angularRange;
		StartCoroutine(IdleRadius());
		StartCoroutine(IdleAngularRange());
	}
	
	// Update is called once per frame
	void Update () {
		sight.LightRadius = setRadius;
		sight.RangeAngle = setAngularRange;
	}
	
	IEnumerator IdleRadius()
	{
		yield return null;
		float minRadius = radius * 0.95f;
		float maxRadius = radius * 1.05f;
		DOTween.To(()=> setRadius, x=> setRadius = x, minRadius, 2f).SetEase(Ease.InOutQuad);
		yield return new WaitForSeconds(2f);
		DOTween.To(()=> setRadius, x=> setRadius = x, maxRadius, 2f).SetEase(Ease.OutQuad);
		yield return new WaitForSeconds(2f);
		StartCoroutine(IdleRadius());
	}
	
	IEnumerator IdleAngularRange()
	{
		yield return null;
		float range = angularRange;
		if(isCanLookAt360)
		{
			range = 360f;
		}
		if(isCantSeeAnything)
		{
			range = 0f;
		}
		
		DOTween.To(()=> setAngularRange, x=> setAngularRange = x, range, 2f).SetEase(Ease.OutQuad);
		yield return new WaitForSeconds(2f);
		StartCoroutine(IdleAngularRange());
	}
}
