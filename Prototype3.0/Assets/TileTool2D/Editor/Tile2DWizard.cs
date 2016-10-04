﻿using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;
using System.IO;

public class Tile2DWizard : EditorWindow {
	public static EditorWindow win;
	public Sprite[] sprites;
	public Tile2D tile;
	public GameObject gTile;
	public bool toggleView = true;
	public int preCurrentSprite = -1;
	public int currentSprite = -1;
	public int currentArraySize;
	public int spriteSize = 64;
	public bool toggleMultidrop;

	[MenuItem("Window/TileTool2D/Tile Wizard")]
	public static void ShowWindow() {
		win = EditorWindow.GetWindow(typeof(Tile2DWizard));
		win.titleContent = new GUIContent("Tile Wizard");
		win.minSize = new Vector2(461.0f, 713.0f);
	}

	void OnGUI() {

		GUI.color = Color.clear;
		GUILayout.BeginVertical("Box");
		GUI.color = Color.white;

		GUILayout.BeginHorizontal();
		TileSelectGUI();
		GUILayout.EndHorizontal();

		TilePropertiesGUI();
		MultiDropGUI();
		GUISpriteWindows();
		GUILayout.EndVertical();
		GUILayout.BeginVertical("Box");
		GUISelectedSprite();
		GUISelectedSpriteArray();
		GUILayout.EndVertical();
		if (GUI.changed) {
			ReplaceDefaultSprite();
		}
		ChangeCurrentSprite();

	}

	void MultiDropGUI() {
		GUILayout.BeginHorizontal();
		if (gTile && toggleMultidrop) {
			GUILayout.BeginVertical("Box");
			GUILayout.Label("Multidrop sprites must be named correctly. \nBy number like Easy Template texture. (sprite_8_0)\nBy tile sprite name. (tilename_NW_0)\nThe last number (_0) is optional for variations.", GUILayout.Width(305));
			GUILayout.EndVertical();
		}
		MultiDropAreaGUI();
		GUILayout.EndHorizontal();
	}


	void ChangeCurrentSprite() {
		//Fix error event error
		if (preCurrentSprite != currentSprite) {
			currentSprite = preCurrentSprite;
			Sprite[] arr = (Sprite[])tile.GetType().GetField("tile"+tile.SpriteNumberToName(currentSprite)).GetValue(tile);
			if (arr != null) currentArraySize = arr.Length;
		}
	}

	void OnEnable() {
		sprites = Resources.LoadAll<Sprite>("GUITextures/TT2DWizard");
	}

	void TileSelectGUI() {

		EditorGUILayout.LabelField("TileTool2D Tile", GUILayout.Width(100f));
		gTile = (GameObject)EditorGUILayout.ObjectField(gTile, typeof(GameObject), false, GUILayout.Width(175f));
		
		GUILayout.FlexibleSpace();
		if (!tile) {
			if (GUILayout.Button("Create Tile", GUILayout.Width(120), GUILayout.Height(18))) {
				Tile2D oldTile;
				oldTile = PrefabCreator.CreateTile(null, false);
				String path = null;
				if(Directory.Exists("Assets/TileTool2D/Resources/Tiles/Wizard Tiles/"))
					path = SaveFile("Assets/TileTool2D/Resources/Tiles/Wizard Tiles/", oldTile.transform.name + "", "prefab");
				else
					path = SaveFile("Assets/", oldTile.transform.name + "", "prefab");

				if (path != null) {
					var asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
					if (asset) {
						asset = PrefabUtility.ReplacePrefab(oldTile.gameObject, (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)));
					} else {
						asset = PrefabUtility.CreatePrefab(path, oldTile.gameObject);
					}
					string spritePath = AssetDatabase.GetAssetPath(asset);
					gTile = (GameObject)AssetDatabase.LoadAssetAtPath(spritePath, typeof(GameObject));				
				}
				DestroyImmediate(oldTile.gameObject);
			}



		} else {
			if (toggleView && tile) GUI.color = Color.cyan;
			else GUI.color = Color.white;
			if (GUILayout.Button("Toggle View", GUILayout.Width(120), GUILayout.Height(18))) {
				toggleView = !toggleView;
			}
		}
		if (gTile && GUI.changed) {
			tile = (Tile2D)gTile.GetComponent<Tile2D>();
		} else if(!gTile) {
			tile = null;
		}
		GUI.color = Color.white;

	}

	String SaveFile(String folder, String name, String type) {
		var path = EditorUtility.SaveFilePanel("Select Folder ", folder, name, type);
		if (path.Length > 0) {
			if (path.Contains("" + Application.dataPath)) {
				String s = "" + path + "";
				String d = "" + Application.dataPath + "/";
				String p = "Assets/" + s.Remove(0, d.Length);
				return p;
			} else {
				Debug.LogError("Prefab Save Failed: Can't save outside project: " + path);
			}
		}
		return null;
	}

	void TilePropertiesGUI() {
		if (!tile) return;
		GUILayout.Space(4);
		//tile.tileName = EditorGUILayout.TextField("Tile Name", tile.tileName);
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Tile Type", GUILayout.Width(100f));
		tile.tileType = EditorGUILayout.TextField(tile.tileType, GUILayout.Width(175f));
		GUILayout.FlexibleSpace();
		if (!tile) GUI.enabled = false;
		if (toggleMultidrop && tile) GUI.color = Color.cyan;
		else GUI.color = Color.white;
		if (GUILayout.Button("Toggle Multidrop", GUILayout.Width(120), GUILayout.Height(18))) {
			toggleMultidrop = !toggleMultidrop;
		}
		GUI.color = Color.white;
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Tile Size", GUILayout.Width(100f));
		tile.tileSize = EditorGUILayout.FloatField(tile.tileSize, GUILayout.Width(175f));
		GUILayout.EndHorizontal();
		//spriteSize = (int)GUILayout.HorizontalSlider((float)spriteSize, 16.0f, 128.0f);
	}

	void GUISelectedSprite() {
		GUILayout.Space(4);
		if (tile && currentSprite > -1) {
			GUILayout.BeginHorizontal();
			Rect dropRect = GUILayoutUtility.GetRect (spriteSize, spriteSize, GUILayout.ExpandWidth (false));
			Sprite selectedSprite = sprites[currentSprite];
			Rect texCoords = new Rect(  selectedSprite.textureRect.x / selectedSprite.texture.width,
										selectedSprite.textureRect.y / selectedSprite.texture.height,
										selectedSprite.textureRect.width / selectedSprite.texture.width,
										selectedSprite.textureRect.height / selectedSprite.texture.height);
			GUI.DrawTextureWithTexCoords(dropRect, selectedSprite.texture, texCoords);
			Sprite[] arr = (Sprite[])tile.GetType().GetField("tile"+tile.SpriteNumberToName(currentSprite)).GetValue(tile);
			if (arr != null && arr.Length > 0 && arr[0] == null) {
				Undo.RegisterCompleteObjectUndo(gTile, "TileTool2D Wizard: Array Size Change");
				tile.NewArray("tile" + tile.SpriteNumberToName(currentSprite), 0);
				return;
			}
			GUILayout.BeginVertical();	
			if (arr != null && arr.Length > 0) {
				EditorGUILayout.LabelField("Variations", GUILayout.Width(100f));
				currentArraySize = arr.Length;
				currentArraySize = EditorGUILayout.IntField(currentArraySize, GUILayout.Width(100f));
			} else {
				EditorGUILayout.LabelField("No sprites available for this tile segment.");
			}
			GUILayout.EndVertical();
			if (arr != null && currentArraySize != arr.Length && GUI.changed) {
				Undo.RegisterCompleteObjectUndo(gTile, "TileTool2D Wizard: Array Size Change");
				Sprite[] arrCopy = (Sprite[])arr.Clone();
				tile.NewArray("tile" + tile.SpriteNumberToName(currentSprite), currentArraySize);
				arr = (Sprite[])tile.GetType().GetField("tile" + tile.SpriteNumberToName(currentSprite)).GetValue(tile);
				for (int i = 0; i < arr.Length; i++) {
					if (i < arrCopy.Length)
						arr[i] = arrCopy[i];
					else if(arrCopy.Length > 0 && arrCopy[0] != null)
						arr[i] = arrCopy[0];
				}
			}
			GUILayout.EndHorizontal();
		}
	}

	void GUISelectedSpriteArray() {
		if (!tile || currentSprite == -1) return;
		GUILayout.Space(4);
		GUILayout.BeginHorizontal();
		Sprite[] arr = (Sprite[])tile.GetType().GetField("tile"+tile.SpriteNumberToName(currentSprite)).GetValue(tile);
		if (arr != null && arr.Length > 0) {
			for (int i = 0; i < arr.Length; i++) {
				GUISelectedSpriteArrayDragAndDrop(arr, i);
				if (i % 7 == 7 - 1) {
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
				}
			}
		}
		GUILayout.EndHorizontal();
	}

	void GUISelectedSpriteArrayDragAndDrop(Sprite[] arr, int i) {
		Event evt = Event.current;
		Rect dropRect = GUILayoutUtility.GetRect (spriteSize, spriteSize, GUILayout.ExpandWidth (false));
		Sprite selectedSprite = arr[i];
		if (selectedSprite == null) return;
		Rect texCoords = new Rect(  selectedSprite.textureRect.x / selectedSprite.texture.width,
										selectedSprite.textureRect.y / selectedSprite.texture.height,
										selectedSprite.textureRect.width / selectedSprite.texture.width,
										selectedSprite.textureRect.height / selectedSprite.texture.height);
		GUI.DrawTextureWithTexCoords(dropRect, selectedSprite.texture, texCoords);
		switch (evt.type) {
			case EventType.MouseDown:
			case EventType.DragUpdated:
			case EventType.DragPerform:
			if (!dropRect.Contains(evt.mousePosition))
				return;
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
			if (evt.type == EventType.DragPerform) {
				DragAndDrop.AcceptDrag();
				Undo.RegisterCompleteObjectUndo(tile.gameObject, "TileTool 2D Wizard");
				foreach (UnityEngine.Object dragged_object in DragAndDrop.objectReferences) {
					if (dragged_object.GetType() == typeof(Sprite)) {
						arr[i] = (Sprite)dragged_object;
						ReplaceDefaultSprite();
					}
				}
				EditorUtility.SetDirty(tile);
			}
			if (evt.type == EventType.MouseDown) {
				if (dropRect.Contains(evt.mousePosition)) {
					EditorGUIUtility.PingObject(arr[i]);
				}
			}
			break;
		}
	}

	Sprite[] FindSpirtesOfSameType(UnityEngine.Object[] oArr, string nam, string templateNum) {
		ArrayList tileList = new ArrayList();
		for (int i=0; i < oArr.Length; i++) {
			Sprite s = (Sprite)oArr[i];
			string[] splitString = s.name.Split("_"[0]);
			if (splitString[1].ToUpper() == nam.ToUpper()) {
				tileList.Add(s);
			}
			if (splitString[1].ToUpper() == templateNum.ToUpper()) {
				tileList.Add(s);
			}
		}
		Sprite[] a = (Sprite[])tileList.ToArray(typeof(Sprite));
		return a;
	} 

	void MultiDropAreaGUI() {
		if (!tile || !toggleMultidrop) return;
		Event evt = Event.current;
		Rect dropRect = GUILayoutUtility.GetRect (132, 64, GUILayout.ExpandWidth (false));
		Sprite selectedSprite = sprites[47];
		Rect texCoords = new Rect(  selectedSprite.textureRect.x / selectedSprite.texture.width,
										selectedSprite.textureRect.y / selectedSprite.texture.height,
										selectedSprite.textureRect.width / selectedSprite.texture.width,
										selectedSprite.textureRect.height / selectedSprite.texture.height);
		GUI.DrawTextureWithTexCoords(dropRect, selectedSprite.texture, texCoords);
		if (!dropRect.Contains(evt.mousePosition))
			return;
		switch (evt.type) {
			case EventType.DragUpdated:
			case EventType.DragPerform:
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
			if (evt.type == EventType.DragPerform) {
				DragAndDrop.AcceptDrag();
				Undo.RegisterCompleteObjectUndo(tile.gameObject, "TileTool 2D Wizard");
				currentSprite = -1;
				for(int i = 0; i < 47; i++) {
					Sprite[] xArr = FindSpirtesOfSameType(DragAndDrop.objectReferences,tile.SpriteNumberToName(i), "" + i);
					if(xArr.Length > 0) {
						tile.NewArray("tile" + tile.SpriteNumberToName(i), xArr.Length);
						Sprite[] arr = (Sprite[])tile.GetType().GetField("tile"+tile.SpriteNumberToName(i)).GetValue(tile);
						for(int j=0; j < xArr.Length; j++) {
							arr[j] = xArr[j];
						}
					}
				}
				ReplaceDefaultSprite();
				EditorUtility.SetDirty(tile);
			}
			break;
		}
	}

	void GUISpriteWindows() {
		if (!tile) return;
		GUILayout.Space(4);
		EditorGUILayout.BeginVertical();
		GUILayout.Label("Drag & Drop one or more sprites into slots, click to edit, click again to find.");
		GUILayout.BeginHorizontal();
		for (int i = 0; i < 47; i++) {
			DropAreaGUI(i);
			if (i % 7 == 7 - 1) {
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	void ReplaceDefaultSprite() {
		if (tile == null) return;
		for (int i = 0; i < 47; i++) {
			Sprite[] arr = (Sprite[])tile.GetType().GetField("tile"+tile.SpriteNumberToName(i)).GetValue(tile);
			if (arr != null && arr.Length > 0) {
				tile.GetComponent<SpriteRenderer>().sprite = arr[0];
				return;
			}
		}
	}

	public void DropAreaGUI(int i) {
		if (sprites == null) return; //Failsafe
		Event evt = Event.current;
		Rect dropRect = GUILayoutUtility.GetRect (spriteSize, spriteSize, GUILayout.ExpandWidth (false));
		Sprite selectedSprite = sprites[i];
		if (selectedSprite == null) {
			Debug.LogError("Missing TileTool2D editor texture... aborting Tile2D Wizard");
			return; //Failsafe
		}
		if (tile && toggleView) {
			Sprite[] arr = (Sprite[])tile.GetType().GetField("tile"+tile.SpriteNumberToName(i)).GetValue(tile);
			if (arr != null && arr.Length > 0) selectedSprite = arr[0];
		}
		if (selectedSprite != null) {
			Rect texCoords = new Rect(  selectedSprite.textureRect.x / selectedSprite.texture.width,
										selectedSprite.textureRect.y / selectedSprite.texture.height,
										selectedSprite.textureRect.width / selectedSprite.texture.width,
										selectedSprite.textureRect.height / selectedSprite.texture.height);
			GUI.DrawTextureWithTexCoords(dropRect, selectedSprite.texture, texCoords);
		}
		switch (evt.type) {
			case EventType.MouseDown:
			case EventType.DragUpdated:
			case EventType.DragPerform:
			if (!dropRect.Contains(evt.mousePosition))
				return;
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
			if (evt.type == EventType.DragPerform) {
				DragAndDrop.AcceptDrag();
				Undo.RegisterCompleteObjectUndo(tile.gameObject, "TileTool 2D Wizard");
				int l = 0;
				preCurrentSprite = i;
				currentSprite = -1;
				foreach (UnityEngine.Object dragged_object in DragAndDrop.objectReferences) {
					if (dragged_object.GetType() == typeof(Sprite)) {
						l++;
					}
				}
				if (l == 0) return;
				Sprite[] arr = (Sprite[])tile.GetType().GetField("tile"+tile.SpriteNumberToName(i)).GetValue(tile);
				tile.NewArray("tile" + tile.SpriteNumberToName(i), l);
				arr = (Sprite[])tile.GetType().GetField("tile" + tile.SpriteNumberToName(i)).GetValue(tile);

				int n = 0;
				foreach (UnityEngine.Object dragged_object in DragAndDrop.objectReferences) {
					if (dragged_object.GetType() == typeof(Sprite)) {
						//toggleView = true;			
						arr[n] = (Sprite)dragged_object;
						ReplaceDefaultSprite();
						n++;
					}
				}
				EditorUtility.SetDirty(tile);
			}
			if (evt.type == EventType.MouseDown) {
				if (dropRect.Contains(evt.mousePosition)) {
					Sprite[] arrr = (Sprite[])tile.GetType().GetField("tile"+tile.SpriteNumberToName(i)).GetValue(tile);
					if (arrr != null && arrr.Length > 0 && i == currentSprite) EditorGUIUtility.PingObject(arrr[0]);
					preCurrentSprite = i;
					Repaint();
				}
			}
			break;
		}
	}

	void InspectorUpdate() {
		Repaint();
	}
}