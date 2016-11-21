using UnityEngine;
using UnityEditor;
using System.Collections;

public class DynamicLightMenu : Editor {

	static internal DynamicLight2D.DynamicLight light;
	const string menuPath = "GameObject/2D Dynamic Light [2DDL]";

	static SerializedObject tmpAsset;

	private static string _path;
	
	internal void OnEnable(){
		light = target as DynamicLight2D.DynamicLight;

		reloadAsset ();
	}

	static void reloadAsset(){
		if(_path == null)
		{_path = EditorUtils.getMainRelativepath();}
		tmpAsset = new SerializedObject(AssetUtility.LoadAsset<DynamicLightSetting>(_path + "2DLight/Settings", "Settings.asset"));
		//Debug.Log ("reloadAsset");
	}



	[MenuItem ( menuPath + "/Lights/ ☼ Radial NO Material",false,20)]
	static void Create(){
		reloadAsset ();
		//Debug.Log (tmpAsset);
		GameObject gameObject = new GameObject("2DLight");
		DynamicLight2D.DynamicLight dl = gameObject.AddComponent<DynamicLight2D.DynamicLight>();
		dl.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
		dl.Version = AssetUtility.LoadProperty("version", tmpAsset);

	}
	
	[MenuItem ( menuPath + "/Lights/ ☀ Radial Procedural Gradient ",false,31)]
	static void addGradient(){
		reloadAsset ();
		Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/StandardLightMaterialGradient.mat", typeof(Material)) as Material;
		GameObject gameObject = new GameObject("2DLight");
		DynamicLight2D.DynamicLight s = gameObject.AddComponent<DynamicLight2D.DynamicLight>();
		s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
		s.Version = AssetUtility.LoadProperty("version", tmpAsset);
		s.setMainMaterial(m);
		s.Rebuild();
		
	}
	
	[MenuItem ( menuPath + "/Lights/ ☀ Radial with Texture",false,32)]
	static void addTexturized(){
		reloadAsset ();
		Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/StandardLightTexturizedMaterial.mat", typeof(Material)) as Material;
		GameObject gameObject = new GameObject("2DLight");
		DynamicLight2D.DynamicLight s = gameObject.AddComponent<DynamicLight2D.DynamicLight>();
		s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
		s.SolidColor = true;
		s.LightColor = Color.red;
		s.setMainMaterial(m);
		s.Version = AssetUtility.LoadProperty("version", tmpAsset);
		s.Rebuild();
		
	}

	[MenuItem ( menuPath + "/Lights/ ☀ Radial with Ilumination ",false,33)]
	static void addRadialIlum(){
		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Lights/2DDL-Point2DLightWithIlumination.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "2DDL-Point2DLightWithIlumination";
		DynamicLight2D.DynamicLight s = hex.GetComponent<DynamicLight2D.DynamicLight>();
		s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
		s.Version = AssetUtility.LoadProperty("version", tmpAsset);
		//hex.layer = LayerMask.LayerToName(CustomAssetUtility.LoadPropertyAsInt("layer"));
	}

		
	[MenuItem ( menuPath + "/Lights/ ☀ Spot ",false,45)]
	static void addSpot(){
		reloadAsset ();
		Material m = AssetDatabase.LoadAssetAtPath(_path + "Materials/StandardLightMaterialGradient.mat", typeof(Material)) as Material;
		GameObject gameObject = new GameObject("2DLight");
		DynamicLight2D.DynamicLight s = gameObject.AddComponent<DynamicLight2D.DynamicLight>();
		s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
		s.Version = AssetUtility.LoadProperty("version", tmpAsset);
		s.setMainMaterial(m);
		s.Rebuild();
		s.Segments = 4;
		s.RangeAngle = 90;
		
	}

	[MenuItem ( menuPath + "/Lights/ ☀ Spot with Ilumination ",false,46)]
	static void addSpotIlum(){
		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Lights/2DDL-Spot2DWithIlumination.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(5,0,0);
		DynamicLight2D.DynamicLight s = hex.GetComponent<DynamicLight2D.DynamicLight>();
		s.Layer.value = AssetUtility.LoadPropertyAsInt("layerMask", tmpAsset);
		s.Version = AssetUtility.LoadProperty("version", tmpAsset);
		hex.name = "2DDL-Point2DLightWithIlumination";

	}

	
	
	[MenuItem ( menuPath + "/Casters/Empty",false,20)]
	static void addEmpty(){
		reloadAsset ();
		GameObject empty = new GameObject("empty");
		int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
		empty.layer = layerDest;
		empty.AddComponent<SpriteRenderer>();
		GameObject emptyChild = new GameObject("collider");
		emptyChild.AddComponent<PolygonCollider2D>();
		emptyChild.transform.parent = empty.transform;
		emptyChild.layer = empty.layer;
		empty.transform.position = new Vector3(5,0,0);
		DynamicLight2D.DynamicLight.reloadMeshes = true;
	}
	
	[MenuItem ( menuPath + "/Casters/ ☐ Square - PolygonCollider2D",false,31)]
	static void addSquare(){
		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/square.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		//if(light){
			int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);//light.getLayerNumberFromLayerMask(light.Layer.value);
			hex.layer = layerDest;
			hex.transform.FindChild("collider").gameObject.layer = layerDest;
		//}
		
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Square";
		DynamicLight2D.DynamicLight.reloadMeshes = true;
	}
	
	[MenuItem ( menuPath + "/Casters/ ☐ Square - BoxCollider2D",false,32)]
	static void addSquare2(){
		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/square - BoxCollider2D.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

		hex.transform.localEulerAngles = new Vector3 (0, 0, 0.001f);
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Square - BoxCollider2D";
		hex.layer = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
		DynamicLight2D.DynamicLight.reloadMeshes = true;
	}

	[MenuItem ( menuPath + "/Casters/ ☐ Square Iluminated",false,32)]
	static void addSquareIlum(){

		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/Iluminated - Square.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(5,0,0);
		hex.transform.localEulerAngles = new Vector3 (0, 0, 0.001f);
		hex.name = "Square - Iluminated";
		hex.layer = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
		DynamicLight2D.DynamicLight.reloadMeshes = true;
	}
	
	[MenuItem ( menuPath +"/Casters/ ◯ Circle - polygonCollider2D",false,43)]
	static void addCircle(){
		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/circle.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		//if(light){
			int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);//light.getLayerNumberFromLayerMask(light.Layer.value);
			hex.layer = layerDest;
			hex.transform.FindChild("collider").gameObject.layer = layerDest;
		//}
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Circle";
		DynamicLight2D.DynamicLight.reloadMeshes = true;
		
	}
	
	[MenuItem ( menuPath + "/Casters/ ◯ Circle - CircleCollider2D",false,44)]
	static void addCircle2(){

		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/circle - CircleCollider2D.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		//if(light){
			int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			hex.layer = layerDest;
			//hex.transform.FindChild("collider").gameObject.layer = layerDest;
		//}
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Circle - CircleCollider2D";
		DynamicLight2D.DynamicLight.reloadMeshes = true;
		
	}
	
	//[MenuItem ( menuPath + "/Casters/ ◯ Circle - Intellider",false,45)]
	static void addIntelliderCircle(){

		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/circleWithintellider.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		if(light){
			int layerDest =  AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			hex.layer = layerDest;
			hex.transform.FindChild("collider").gameObject.layer = layerDest;
		}
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Circle - Intellider";
		DynamicLight2D.DynamicLight.reloadMeshes = true;
		
	}
	[MenuItem ( menuPath + "/Casters/ ◯ Circle Iluminated",false,45)]
	static void addCircleIlum(){

		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/Iluminated - Circle.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Circle - Iluminated";
		hex.layer = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
		DynamicLight2D.DynamicLight.reloadMeshes = true;
	}
	
	[MenuItem ( menuPath + "/Casters/ ⎯ ⎯ ⎯ Line Edge - EdgeCollider2D",false,56)]
	static void addEdge(){

		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/edge - EdgeCollider2D.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		//if(light){
			int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			hex.layer = layerDest;
			//hex.transform.FindChild("collider").gameObject.layer = layerDest;
	//	}
		hex.transform.position = new Vector3(5,3,0);
		hex.name = "Edge - EdgeCollider2D";
		DynamicLight2D.DynamicLight.reloadMeshes = true;
		
	}
	
	[MenuItem ( menuPath + "/Casters/Hexagon",false,67)]
	static void addHexagon(){

		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/hexagon.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		//if(light){
			int layerDest = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
			hex.layer = layerDest;
			hex.transform.FindChild("collider").gameObject.layer = layerDest;
		//}
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Hexagon";
		DynamicLight2D.DynamicLight.reloadMeshes = true;
		
	}
	[MenuItem ( menuPath + "/Casters/Pacman",false,68)]
	static void addPacman(){

		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/pacman.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Pacman";
		hex.layer = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
		DynamicLight2D.DynamicLight.reloadMeshes = true;

	}
	[MenuItem ( menuPath + "/Casters/Star",false,69)]
	static void addStar(){

		reloadAsset ();
		Object prefab = AssetDatabase.LoadAssetAtPath(_path + "Prefabs/Casters/star.prefab", typeof(GameObject));
		GameObject hex = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
		hex.transform.position = new Vector3(5,0,0);
		hex.name = "Star";
		hex.layer = AssetUtility.LoadPropertyAsInt("layerCaster", tmpAsset);
		DynamicLight2D.DynamicLight.reloadMeshes = true;
	}





	/*
	[MenuItem ( menuPath + "/About/• Open in AssetStore",false,120)]
	static void openAssetStore(){
		Application.OpenURL("http://u3d.as/asp");
	}

	[MenuItem ( menuPath + "/About/• OnLine Documentation",false,131)]
	static void gotoDoc(){
		Application.OpenURL("http://martinysa.com/2d-dynamic-lights-doc/");
	}

	[MenuItem ( menuPath + "/About/• Contact Developer",false,132)]
	static void gotoMail(){
		Application.OpenURL("mailto:info@martinysa.com");
	}
*/




}
