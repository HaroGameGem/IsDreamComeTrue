using UnityEngine;
using UnityEditor;
using System.Collections;


public class CustomDragData{
	public int originalIndex;
	public IList originalList;
}


[CustomEditor (typeof (DynamicLight2D.DynamicLight))] 
[CanEditMultipleObjects]

public class DynamicLightEditor : Editor {

	static internal DynamicLight2D.DynamicLight light;

	SerializedProperty version, lmaterial, lUseSolidColor, lColor, strokeRender, radius, segments, range;
	SerializedProperty layerm, useEvents, usePersistFOV, intelliderConvex, staticScene, recalcNorms, debugLines, sortOrder;
	SerializedProperty DDLEvent_OnEnterFOV, DDLEvent_OnExitFOV, DDLEvent_InsideFOV;

	/*
	string _lastVersion;
	Material  _lastLmaterial;
	bool _lastlUseSolidColor, _lastUseEvents, _lastIntelliderConvex, _lastStaticScene, _lastDebugLines, _lastRecalcNorms;
	Color _lastColor;
	float _lastRadius, _lastRange;
	int _lastSegments, _lastSortOrder, _lastLayerm;
	DynamicLight2D.DynamicLightEvent _event_OnEnterFOV;
	*/


	private GUIStyle titleStyle, subTitleStyle, bgStyle, btnStyle;

	private Vector2 lightRectOrigin, lightRectSize;

	private string _path;

	internal void OnEnable(){

		//serialtarget = new SerializedObject(target);
		light = target as DynamicLight2D.DynamicLight;

		version = serializedObject.FindProperty("Version");
		lmaterial = serializedObject.FindProperty("LightMaterial");
		lUseSolidColor = serializedObject.FindProperty("SolidColor");
		lColor = serializedObject.FindProperty("LightColor");
		radius = serializedObject.FindProperty ("LightRadius");
		segments = serializedObject.FindProperty ("Segments");
		range = serializedObject.FindProperty ("RangeAngle");
		layerm = serializedObject.FindProperty ("Layer");
		useEvents = serializedObject.FindProperty("notifyGameObjectsReached");
		usePersistFOV = serializedObject.FindProperty("objectsWithinSight");
		intelliderConvex = serializedObject.FindProperty("intelliderConvex");
		staticScene = serializedObject.FindProperty("staticScene");
		recalcNorms = serializedObject.FindProperty("recalculateNormals");
		strokeRender = serializedObject.FindProperty("strokeRender");
		debugLines = serializedObject.FindProperty("debugLines");
		DDLEvent_OnEnterFOV = serializedObject.FindProperty("DDLEvent_OnEnterFOV");
		DDLEvent_OnExitFOV = serializedObject.FindProperty("DDLEvent_OnExitFOV");
		DDLEvent_InsideFOV = serializedObject.FindProperty("DDLEvent_InsideFOV");


		// Bounds
		getLightBounds();

		Undo.undoRedoPerformed += refreshLightObject;

		_path = EditorUtils.getMainRelativepath();

	}

	internal void getLightBounds(){
		//Rect lightRect = new Rect(light.renderer.bounds.min.x,light.renderer.bounds.max.y, light.renderer.bounds.max.x, light.renderer.bounds.min.y);    
		//lightRect = new Rect(Camera.main.WorldToScreenPoint(lightRect.x),Camera.main.WorldToScreenPoint(lightRect.y),Camera.main.WorldToScreenPoint(lightRect.width),Camera.main.WorldToScreenPoint(lightRect.height));
		lightRectOrigin = new Vector2(light.GetComponent<Renderer>().bounds.min.x,light.GetComponent<Renderer>().bounds.min.y);
		lightRectSize = new Vector2(light.GetComponent<Renderer>().bounds.max.x, light.GetComponent<Renderer>().bounds.max.y);
	}

	internal void OnDisable(){
		Undo.undoRedoPerformed -= refreshLightObject;
	}



	public override void OnInspectorGUI () {
		if (light == null){return;}

		initStyles();

		EditorGUI.BeginChangeCheck();

			float fRange = range.floatValue;
			if(fRange > 360.001f)
				fRange = 360f;
			if(fRange < .999f)
				fRange = 1f;
			
			
			//Debug.Log(fRange - Mathf.FloorToInt(fRange));
			if(Mathf.Abs(fRange - Mathf.FloorToInt(fRange)) > 0.5f){
				fRange = Mathf.Round(fRange);
				range.floatValue = fRange;
			}
			
			if(range.floatValue != fRange){
				range.floatValue = fRange;
			}
			
			float fradius = radius.floatValue;
			if(fradius < 0)
				fradius *= -1;
			
			if(radius.floatValue != fradius){
				radius.floatValue = fradius;
			}
			
			string v = version.stringValue;
			
			EditorGUILayout.Separator();
			GUILayout.Label("------  2D Light Object  ------", titleStyle);
			//GUILayout.Label("----------------");
			
			
			EditorGUILayout.Separator();
			//GUILayout.Label("Main Prefs", subTitleStyle);
			
			
			
			if(fRange < 360f){
				if(segments.intValue < 5) segments.intValue = 5;
				EditorGUILayout.IntSlider(segments, 5, 360, new GUIContent("Segments","Quantity of line segments is used for build mesh render of 2DLight. 5 at least in spot Lights"));
			}else{
				EditorGUILayout.IntSlider(segments, 3, 360, new GUIContent("Segments","Quantity of line segments is used for build mesh render of 2DLight. 3 at least"));
			}
			
			EditorGUILayout.PropertyField(radius,new GUIContent("Radius", "Radius Size of current light "));
			EditorGUILayout.PropertyField(range, new GUIContent("Angular Range", "from 0 to 360 degrees. Define the light amplitude in degrees"));
			
			EditorGUILayout.Separator();
			
			EditorGUILayout.PropertyField(lmaterial, new GUIContent("Light Material", "Material Object used for render into light mesh"));
			EditorGUILayout.PropertyField(lUseSolidColor, new GUIContent("Is Solid Color", "if TRUE, render properly texturized or vertex color lights, if is FALSE, allow work with procedural gradient shaders."));
			if(lUseSolidColor.boolValue == true)
				EditorGUILayout.PropertyField(lColor, new GUIContent("Light Color", "Only can be used with Is Solid Color check to TRUE"));
			
			EditorGUILayout.PropertyField(strokeRender, new GUIContent("Offset Render","Is the offset between vertices and the mesh of light. (Mostly Applicable on Stencil shaders)"));
			
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			//GUILayout.Label("--- Layer ---", subTitleStyle);
			EditorGUILayout.PropertyField(layerm, new GUIContent("--- Layer ---", "Current layer where light is working. Must be the same in the colliders"));
			
			
			
			
			//EditorGUILayout.HelpBox("Events", MessageType.None);
			//GUILayout.Label("--- Events ---", subTitleStyle);
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(useEvents, new GUIContent("--- Events ---", "Enable events when light touch casters"));
			if(useEvents.boolValue == true){
				
				EditorGUILayout.PropertyField(usePersistFOV, new GUIContent("Persistent", "Enable 'OnInside' list. Do a persistent checking when caster is within sight of current 2DLight"));
				
				EditorGUILayout.PropertyField(DDLEvent_OnEnterFOV, new GUIContent("OnEnter", "List of callback functions called when caster stay in sight of current light"));
				EditorGUILayout.PropertyField(DDLEvent_OnExitFOV, new GUIContent("OnExit", "List of callback functions when caster just exit from sight of current light"));
				
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				
				if(usePersistFOV.boolValue == true)
				{
					EditorGUILayout.PropertyField(DDLEvent_InsideFOV, new GUIContent("OnInside", "List of callback functions when caster just exit from sight of current light"));
					EditorGUILayout.Separator();	
				}
			}
			//EditorGUILayout.Separator();
			
			GUILayout.Label("--- Optimizations ---", subTitleStyle);
			//EditorGUILayout.HelpBox("Optimizations", MessageType.None);
			//EditorGUILayout.PropertyField(lightMode);
			EditorGUILayout.PropertyField(staticScene, new GUIContent("Static Scene", "Ignore any caster created after start scene, this means that you need preload all caster before run the app"));
			EditorGUILayout.PropertyField(intelliderConvex, new GUIContent("Ignore middle vertices","Forces 2Dlight to ignore middle vertices per collider when is building the final mesh"));
			//EditorGUILayout.Separator();
			
			GUILayout.Label("--- Rendering and Debug Options ---", subTitleStyle);
			//EditorGUILayout.HelpBox("Rendering and Debug Options", MessageType.None);
			//EditorGUILayout.PropertyField(sortOrder);
			EditorGUILayout.PropertyField(recalcNorms);
			EditorGUILayout.PropertyField(debugLines);
			
			
			
			EditorGUILayout.Separator();
			if (GUILayout.Button("Refresh")){
				//light.Rebuild();
				refreshLightObject();
			}
			
			
			
			
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			GUILayout.Label("Info", subTitleStyle);
			EditorGUILayout.HelpBox("2DDL PRO version: " + v, MessageType.None);
			EditorGUILayout.Separator();
			
			
			
			
			
			if (GUILayout.Button("Settings")){
				SettingsWindow.Init();
			}
			
			if (GUILayout.Button("About")){
				DynamicLightAboutWindow.Init();
			}
			
			EditorGUILayout.Separator();
			if (GUILayout.Button("Support")){
				Application.OpenURL("mailto:info@martinysa.com");
			}


			


		if(EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(target);
			
			applyChanges();
			
			serializedObject.ApplyModifiedProperties();
		}
			
			


	}



	private static Vector3 pointSnap =  Vector3.one * 0.1f;
	
	void OnSceneGUI () {
		if(light){

			Transform lTransform = light.transform;
			Vector3 oldPoint = lTransform.TransformPoint(new Vector3(light.LightRadius,0,0));
			float size = HandleUtility.GetHandleSize(oldPoint);


			Undo.RecordObject(light, "Move Light Radius Point");


			Vector3 newPoint = Handles.FreeMoveHandle(
				oldPoint, Quaternion.identity,size * 0.05f, pointSnap, Handles.DotCap);
			
			if (oldPoint != newPoint) {
				Undo.RecordObject(light, "resize radiusLight");
				light.LightRadius = (lTransform.InverseTransformPoint(newPoint).magnitude);
				light.Rebuild();
			}
			
			Vector3 oldPoint2 = lTransform.TransformPoint(new Vector3(-light.LightRadius,0,0));
			//oldPoint2.x *=-1;
			Vector3 newPoint2 = Handles.FreeMoveHandle(
				oldPoint2, Quaternion.identity,size * 0.05f, pointSnap, Handles.DotCap);
			
			if (oldPoint2 != newPoint2) {
				Undo.RecordObject(light, "resize radiusLight");
				light.LightRadius = (lTransform.InverseTransformPoint(newPoint2).magnitude);
				light.Rebuild();
			}
			
			
			
			Handles.color = Color.yellow;
			Handles.DrawWireDisc(lTransform.position,new Vector3(0,0,-1), light.LightRadius);
			if(light.RangeAngle < 360){
				Handles.DrawLine(lTransform.position, lTransform.TransformPoint(light.getMaxFromAllVerts()));
				Handles.DrawLine(lTransform.position, lTransform.TransformPoint(light.getMinFromAllVerts()));
			}

			dragAndDropEvents();
		}
		
	}


	void applyChanges(){
		Undo.RecordObject(light, "Apply changes");

		foreach (DynamicLight2D.DynamicLight s in targets) {
			s.LightMaterial = (Material)lmaterial.objectReferenceValue;
			s.SolidColor = lUseSolidColor.boolValue; 
			s.LightColor = lColor.colorValue; 
			s.strokeRender = strokeRender.floatValue;
			s.LightRadius =  radius.floatValue; 
			s.Segments = segments.intValue; 
			s.RangeAngle =  range.floatValue; 
			s.Layer = layerm.intValue; 
			s.notifyGameObjectsReached = useEvents.boolValue; 
			s.objectsWithinSight = usePersistFOV.boolValue; 
			s.intelliderConvex =  intelliderConvex.boolValue; 
			s.staticScene = staticScene.boolValue; 
			s.recalculateNormals = recalcNorms.boolValue; 
			s.debugLines = debugLines.boolValue;
		}


	}

	void refreshLightObject(){
		foreach (DynamicLight2D.DynamicLight s in targets) {
			s.Rebuild();
		}
	}

	internal void initStyles(){
		if(_path == null)
				{_path = EditorUtils.getMainRelativepath();}

		titleStyle = new GUIStyle(GUI.skin.label);
		titleStyle.fontSize = 15;
		titleStyle.fontStyle = FontStyle.Bold;
		titleStyle.alignment = TextAnchor.MiddleCenter;
		titleStyle.margin = new RectOffset(4, 4, 10, 0);

		subTitleStyle = new GUIStyle(GUI.skin.label);
		subTitleStyle.fontSize = 13;
		subTitleStyle.fontStyle = FontStyle.Bold;
		subTitleStyle.alignment = TextAnchor.MiddleLeft;
		subTitleStyle.margin = new RectOffset(4, 4, 10, 0);

		bgStyle = new GUIStyle(GUI.skin.box);
		bgStyle.margin = new RectOffset(4, 4, 0, 4);
		bgStyle.padding = new RectOffset(1, 1, 1, 2);

		btnStyle = new GUIStyle(GUI.skin.button);
		Sprite bg = (Sprite)AssetDatabase.LoadAssetAtPath(_path + "Textures/box_sprite.png", typeof(Sprite));
		Sprite bgClicked = (Sprite)AssetDatabase.LoadAssetAtPath(_path + "Textures/box_sprite.png", typeof(Sprite));
		btnStyle.margin = new RectOffset(0, 0, 0, 0);
		btnStyle.padding = new RectOffset(0, 0, 4, 4);
		btnStyle.normal.background = bg.texture;
		btnStyle.active.background = bgClicked.texture;

	}

	internal Material lastMat;
	internal Material newMat;
	internal bool previewingNewMat = false;

	void dragAndDropEvents(){
		Event evt = Event.current;

		foreach (Object dragged_object in DragAndDrop.objectReferences) {
			if(dragged_object.GetType() != typeof(Material))
				return;
		}

		Vector2 Origin = Camera.current.WorldToScreenPoint(lightRectOrigin);
		Vector2 Size = Camera.current.WorldToScreenPoint(lightRectSize);


		Rect lightRect = new Rect(Origin.x,Origin.y,Size.x,Size.y);    


		// To screen point
		Vector2 mouseWorld = Event.current.mousePosition;
		mouseWorld.y = Screen.height - (mouseWorld.y + 25);



		//Debug.Log(" rect " +lightRect);
		//Debug.Log(" mouse " +evt.mousePosition);

		if(!lastMat)
			lastMat = light.LightMaterial;


		bool isOver = lightRect.Contains(mouseWorld);
		//Debug.Log(isOver);


		switch (evt.type) {
			case EventType.MouseDown:
				DragAndDrop.PrepareStartDrag();// reset data
				break;
			case EventType.MouseUp:
				// Clean up, in case MouseDrag never occurred:
				DragAndDrop.PrepareStartDrag();
				//newMat = null;
				//if(!isOver){
				//	light.LightMaterial = lastMat;
				//}
				
				break;

			case EventType.DragUpdated:
				if(!isOver){
					DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
					evt.Use();
				}else{
					foreach (Object dragged_object in DragAndDrop.objectReferences) {
						newMat = (Material) dragged_object;
						light.LightMaterial = newMat;
					}
				}
				
				break;
				
				

			case EventType.DragPerform:
			
			DragAndDrop.AcceptDrag ();
			if(isOver){
				foreach (Object dragged_object in DragAndDrop.objectReferences) {
					newMat = (Material) dragged_object;
					light.LightMaterial = newMat;
				}
				evt.Use();
			}
			break;


		}
	}



}
