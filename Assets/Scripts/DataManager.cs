using UnityEngine;
using System.Collections;

public enum eTrials
{
	NEVER_DONT_USE_None = 0,
	dangerObject,
	wayPointDangerObject,
	secretObstacleOn,
	rotateCamera,
	canLookAt360,
	cantSeeAnything,
	DEACTIVE_confuseControl,
	DEACTIVE_timeOut,
	NEVER_DONT_USE_End = 9
}

public enum eCamRotation
{
	None = 0,
	rot0,
	rot90,
	rot180,
	rot270
}

public class DataManager {
	
	static DataManager instance;
	public static DataManager Instance
	{
		get { if(instance == null) instance = new DataManager();
			return instance; }
	}
	
	public TrialSelector[] trials;
	eCamRotation camRot = eCamRotation.rot0;
	public bool[] isActiveTrial;
	
	int idxGoalPoint;
	public int IdxGoalPoint
	{
		get { return idxGoalPoint; }
		set { idxGoalPoint = value; }
	}
	
	public eCamRotation CamRot
	{
		get { return camRot; }
		set { camRot = value; }
	}
	

	DataManager()
	{
		
	}
	
}
