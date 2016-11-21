using UnityEngine;
using System.Collections;

public class MoveCtrl : MonoBehaviour {
	
	public static bool isEnableCtrl;
	public float moveSpeed = 2f;
	
	public GameObject model;

	// Use this for initialization
	void Start () {
		isEnableCtrl = false;
		
		//Event
		GameManager.Instance.EventGameStart += GameStart;
		GameManager.Instance.EventGameOver += GameOver;
	}
	
	string strHorizontal = "Horizontal";
	string strVertical = "Vertical";
	string strHorizontal90 = "Horizontal90";
	string strVertical90 = "Vertical90";
	string strHorizontal180 = "Horizontal180";
	string strVertical180 = "Vertical180";
	string strHorizontal270 = "Horizontal270";
	string strVertical270 = "Vertical270";
	
	// Update is called once per frame
	void Update () {
		if(isEnableCtrl == true)
		{
			
			string horizontal = "Horizontal";
			string vertical = "Vertical";
			switch (DataManager.Instance.CamRot)
			{
			case eCamRotation.rot0:
				horizontal = strHorizontal;
				vertical = strVertical;
				break;
			case eCamRotation.rot90:
				horizontal = strHorizontal90;
				vertical = strVertical90;
				break;
			case eCamRotation.rot180:
				horizontal = strHorizontal180;
				vertical = strVertical180;
				break;
			case eCamRotation.rot270:
				horizontal = strHorizontal270;
				vertical = strVertical270;
				break;
			default:
				Debug.Log("잘못된 입력");
				break;
			}
			
	
			
			float h = Input.GetAxis(horizontal);
			float v = Input.GetAxis(vertical);
			
			Vector3 dir = new Vector3(h, v, 0f);
			if(dir != Vector3.zero)
			{
				model.transform.rotation = Quaternion.Slerp(model.transform.rotation, Quaternion.LookRotation(dir, Vector3.back), 0.1f);
				transform.Translate(dir * Time.deltaTime * moveSpeed, Space.World);
			}			
		}
	}
	
	void GameStart()
	{
		isEnableCtrl = true;
	}
	
	void GameOver(bool isWin)
	{
		isEnableCtrl = false;
	}
}
