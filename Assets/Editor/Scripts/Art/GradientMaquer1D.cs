using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "GradientMaker/1D")]
public class GradientMaquer1D : ScriptableObject
{
	public Gradient gradient;
	public int size;

	public string path;
	public string textureName;

	public bool createTexture = false;

	private void OnValidate()
	{
		if(createTexture)
		{
			createTexture = false;
			CreateTexture();
		}
	}

	public void CreateTexture()
	{
		string tempPath = Application.dataPath + "/" + path;
		if (!Directory.Exists(tempPath)) Directory.CreateDirectory(path);

		Texture2D gtex = new Texture2D(size, 1, TextureFormat.ARGB32, false);

		Color[] pixels = new Color[size];

		for (int i = 0; i < size; i++)
        {
			pixels[i] = gradient.Evaluate(i > 0 ? (float)i / (float)size : 0);
			//Debug.Log(pixels[i]);
		}

		gtex.SetPixels(pixels);
		gtex.Apply();

		byte[] bytes = gtex.EncodeToPNG();

		tempPath = tempPath + "/" + textureName + ".png";
		//Object.Destroy(gtex);
		File.WriteAllBytes(tempPath, bytes);
		AssetDatabase.Refresh();

		tempPath = tempPath + ".meta";

		File.WriteAllText(tempPath,
			Regex.Replace(Regex.Replace(Regex.Replace(File.ReadAllText(tempPath), "wrapU: 0", "wrapU: 1"),
			"wrapV: 0", "wrapV: 1"),
			"wrapW: 0", "wrapW: 1"));
		AssetDatabase.Refresh();
	}
}
