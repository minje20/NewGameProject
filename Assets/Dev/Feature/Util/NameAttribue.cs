using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class PropertyNameAttribute : PropertyAttribute
{
    public string NewName { get; private set; }
    public PropertyNameAttribute(string name)
    {
        NewName = name;
    }  
}





#if UNITY_EDITOR
[CustomPropertyDrawer (typeof(PropertyNameAttribute))]
public class PropertyNamePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        PropertyNameAttribute nameAttribute = (PropertyNameAttribute)this.attribute;
        label.text = nameAttribute.NewName;
        EditorGUI.PropertyField(position, property, label );
    }
}
#endif