using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

public class AudioSettings : EditorWindow 
{
    const string TOOL_NAME = "AudioSettings";
    const string ASSET_PATH = "Assets/Editor/AudioSettings/";
    const string WRITE_PATH = "Assets/Script/Audio/AudioManager.cs";

    Vector2 seScrollViewPos = new Vector2(0, 0);
    Vector2 bgmScrollViewPos = new Vector2(0, 0);

    static AudioSettingsParam paramData = ScriptableObject.CreateInstance<AudioSettingsParam>();

    [MenuItem("Tools/" + TOOL_NAME)]
    static void Open()
    {
        var window = GetWindow(typeof(AudioSettings)) as AudioSettings;
        window.Show();
        window.titleContent = new GUIContent(TOOL_NAME);


        var param = AssetDatabase.LoadAssetAtPath<AudioSettingsParam>(ASSET_PATH + TOOL_NAME + ".asset");
        if (param != null)
        {
            param = paramData;
        }
    }

    void Update()
    { 
    
    }

    void OnEnable()
    {
        var param = AssetDatabase.LoadAssetAtPath<AudioSettingsParam>(ASSET_PATH + TOOL_NAME + ".asset");
        param = paramData;
    }

    void OnGUI()
    {
        GUILayout.Space(10.0f);

        if (GUILayout.Button("反映"))
        {
            paramData.BGMAudioClipList.Clear();
            paramData.SEAudioClipList.Clear();

            var seItems = Resources.LoadAll<AudioClip>("SE/");
            foreach (var item in seItems)
            {
                paramData.SEAudioClipList.Add(item);
            }
            var bgmItems = Resources.LoadAll<AudioClip>("BGM/");
            foreach (var item in bgmItems)
            {
                paramData.BGMAudioClipList.Add(item);
            }
        }

        if (GUILayout.Button("書き出す"))
        {
            var param = AssetDatabase.LoadAssetAtPath<AudioSettingsParam>(ASSET_PATH + TOOL_NAME + ".asset");
            if (param == null)
            {
                AssetDatabase.CreateAsset(paramData, ASSET_PATH + TOOL_NAME + ".asset");
            }

            param = paramData;
            
            Write();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }

        EditorGUILayout.BeginHorizontal();
        {
            seScrollViewPos = EditorGUILayout.BeginScrollView(seScrollViewPos);
            {
                for (int i = 0; i < paramData.SEAudioClipList.Count; i++)
                {
                    paramData.SEAudioClipList[i] = EditorGUILayout.ObjectField(paramData.SEAudioClipList[i], typeof(AudioClip)) as AudioClip;
                }
                GUILayout.Space(5.0f);
            }
            EditorGUILayout.EndScrollView();

            bgmScrollViewPos = EditorGUILayout.BeginScrollView(bgmScrollViewPos);
            {
                for (int i = 0; i < paramData.BGMAudioClipList.Count; i++)
                {
                    paramData.BGMAudioClipList[i] = EditorGUILayout.ObjectField(paramData.BGMAudioClipList[i], typeof(AudioClip)) as AudioClip;
                }
                GUILayout.Space(5.0f);

            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndHorizontal();


    }


    void Write()
    {
        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(WRITE_PATH))
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("namespace Audio");
            sb.AppendLine("{");
            sb.AppendLine("     public enum BGMID");
            sb.AppendLine("     {");
            foreach (var data in paramData.BGMAudioClipList)
            {
                string str = data.name.ToUpperInvariant();
                sb.Append("         " + str + ",");
                sb.Append("  //<" + data.name + "\n");
            }
            sb.AppendLine("     }");

            sb.AppendLine("");

            sb.AppendLine("     public enum SEID");
            sb.AppendLine("     {");
            foreach (var data in paramData.SEAudioClipList)
            {
                string str = data.name.ToUpperInvariant();
                sb.Append("         " + str + ",");
                sb.Append("  //<" + data.name + "\n");
            }

            sb.AppendLine("     }");
            sb.AppendLine("}");

            sb.AppendLine("");

            sb.AppendLine("public class BGMAudioData");
            sb.AppendLine("{");
            sb.AppendLine("     public string label;");
            sb.AppendLine("     public Audio.BGMID id;");
            sb.AppendLine("}");

            sb.AppendLine("");

            sb.AppendLine("public class SEAudioData");
            sb.AppendLine("{");
            sb.AppendLine("     public string label;");
            sb.AppendLine("     public Audio.SEID id;");
            sb.AppendLine("}");

            sb.AppendLine("public class AudioManager");
            sb.AppendLine("{");
            sb.AppendLine("     static public SEAudioData []SEData = new SEAudioData[]");
            sb.AppendLine("     {");
            foreach (var data in paramData.SEAudioClipList)
            {
                string str = data.name.ToUpperInvariant();
                sb.Append("         new SEAudioData(){label =" + "\"" + data.name + "\"" + ",");
                sb.Append("         id = Audio.SEID." + str + "}, \n");
            }
            sb.AppendLine("     };");

            sb.AppendLine("");

            sb.AppendLine("     static public BGMAudioData []BGMData = new BGMAudioData[]");
            sb.AppendLine("     {");
            foreach (var data in paramData.BGMAudioClipList)
            {
                string str = data.name.ToUpperInvariant();
                sb.Append("         new BGMAudioData(){label =" + "\"" + data.name + "\"" + ", ");
                sb.Append("         id = Audio.BGMID." + str + "}, \n");
            }
            sb.AppendLine("     };");
            sb.AppendLine("}");

            sw.Write(sb);
        }
    }
}