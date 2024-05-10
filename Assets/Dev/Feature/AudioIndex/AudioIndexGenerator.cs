using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MyBox;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
public static class AudioIndexGenerator
{
    private const string STRING_TABLE_PATH = "Assets/Dev/Feature/AudioIndex/StringTable.asset";
    private const string ENUM_TABLE_FILENAME = "EnumTable";
    private const string ENUM_TABLE_FOLDER_PATH = "Assets/Dev/Feature/AudioIndex/";
    private const string SCRIPT_PATH = "Assets/Dev/Feature/AudioIndex/AudioIndex.cs";
    
    public static bool CreateScript()
    {
        var stringTable = LoadStringTable();
        var parsedText = ParseTable(stringTable);

        if (stringTable == false)
            return false;

        try
        {
            using TextWriter writer = new StreamWriter(SCRIPT_PATH, false, Encoding.Unicode);
            writer.Write(parsedText);
            writer.Close();
        }
        catch (Exception e)
        {
            Debug.LogError($"AudioInde.cs 파일 생성 실패.\n{e.Message}");
            return false;
        }

        return true;
    }

    public static bool SaveEnumTable()
    {
        var stringTable = LoadStringTable();
        var enumTable = CreateEnumTable(stringTable);

        if (enumTable == null)
        {
            return false;
        }
        
        if (AssetDatabase.IsValidFolder(ENUM_TABLE_FOLDER_PATH) == false)
        {
            AssetDatabase.CreateFolder("Assets", ENUM_TABLE_FOLDER_PATH);
        }
        
        AssetDatabase.CreateAsset(enumTable, ENUM_TABLE_FOLDER_PATH + $"{ENUM_TABLE_FILENAME}.asset");
        return true;
    }

    private static string ParseTable(AudioIndexStringTable table)
    {
        Debug.Assert(table);

        StringBuilder indexStr = new();
        string endStr = ",\n";

        foreach (var item in table.Table)
        {
            if (string.IsNullOrEmpty(item.Key))
            {
                Debug.LogError("Key is null or empty");
                continue;
            }
            indexStr.Append(item.Key);
            indexStr.Append(endStr);
        }


        string template = $@"
public enum AudioIndex
{{
{indexStr.ToString()}
}}
";

        return template;
    }

    private static AudioIndexEnumTable CreateEnumTable(AudioIndexStringTable stringTable)
    {
        var enumTable = ScriptableObject.CreateInstance<AudioIndexEnumTable>();
        Debug.Assert(enumTable);

        enumTable._table = new List<AudioIndexEnumTableSet>(10);

        List<string> msgs = new List<string>();

        foreach (var item in stringTable.Table)
        {
            stringTable.Table.ForEach(x =>
            {
                if (x == item) return;
                if (x.Key == item.Key)
                {
                    msgs.Add($"key({x.Key})가 중복됩니다.");
                }
            });
        }

        msgs = msgs.Distinct().ToList();
        int nullCount = 0;
        
        List<string> keyNullMsgs = new List<string>();
        foreach (var item in stringTable.Table)
        {
            if (string.IsNullOrEmpty(item.Key))
            {
                keyNullMsgs.Add($"Key is null or empty ({nullCount + 1})");
                continue;
            }

            if (item.Clip == false)
            {
                msgs.Add($"audio clip({item.Key})이 null 입니다.");
                continue;
            }
            
            if (Enum.TryParse(item.Key, out AudioIndex value))
            {
                enumTable._table.Add(new AudioIndexEnumTableSet()
                {
                    _clip = item.Clip,
                    _key =  value
                });
            }
            else
            {
                msgs.Add($"유효하지 않는 enum string({item.Key})");
            }
        }

        if (keyNullMsgs.Count > 0 || msgs.Count > 0)
        {
            msgs.ForEach(Debug.LogError);
            keyNullMsgs.ForEach(Debug.LogError);
            return null;
        }
        
        return enumTable;
    }

    private static AudioIndexStringTable LoadStringTable()
    {
        var table = AssetDatabase.LoadAssetAtPath<AudioIndexStringTable>(STRING_TABLE_PATH);
        
        Debug.Assert(table);

        return table;
    }
}
#endif