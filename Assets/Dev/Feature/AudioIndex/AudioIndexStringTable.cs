using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MyBox;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class AudioIndexStringTableSet
{
    [SerializeField] private bool _useStringKey = true;
    [field: SerializeField, ConditionalField(nameof(_useStringKey))] private string _key;
    [SerializeField] private AudioClip _clip;

    public string Key
    {
        get
        {
            if (_useStringKey == false)
            {
                return _clip ? _clip.name : string.Empty;
            }

            return _key;
        }
    }
    public AudioClip Clip => _clip;
}

[CreateAssetMenu(menuName = "IndieLINY/AudioIndex", fileName = "StringTable")]
public class AudioIndexStringTable : ScriptableObject
{
    [SerializeField] private List<AudioIndexStringTableSet> _table;

    public IReadOnlyList<AudioIndexStringTableSet> Table => _table;

    [ButtonMethod]
    private void CreateScript()
    {
#if UNITY_EDITOR
        bool isCreated = AudioIndexGenerator.CreateScript();

        if (isCreated == false)
        {
            EditorUtility.DisplayDialog("코드 생성 실패", "AudioIndex.cs 파일 생성에 실패하였습니다.", "확인");
        }
        else
        {
            EditorUtility.DisplayDialog("코드 생성 성공", "AudioIndex.cs 파일이 생성되었습니다.\n컴파일 후 'CreateEnumTable' 버튼을 눌러 enum table을 생성하십시오.", "확인");
        }
#endif
    }
    
    [ButtonMethod]
    private void CreateEnumTable()
    {
#if UNITY_EDITOR
        bool isCreated = AudioIndexGenerator.SaveEnumTable();

        if (isCreated == false)
        {
            EditorUtility.DisplayDialog("생성 실패", "Enum Table 생성 실패.\n자세한 내용은 로그를 확인하십시오.", "확인");
        }
        else
        {
            EditorUtility.DisplayDialog("생성 완료", "Enum Table 생성 완료.", "확인");
        }
#endif
    }
}
