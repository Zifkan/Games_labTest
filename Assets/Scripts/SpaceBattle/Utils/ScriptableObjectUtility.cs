using UnityEditor;
using UnityEngine;

namespace SpaceBattle.Utils
{
    public static class ScriptableObjectUtility
    {
        public static void CreateAsset<T> () where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T> ();


            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath ("Assets/Resources/SpaceBattle/Modules/" +  typeof(T).Name + ".asset");
 
            AssetDatabase.CreateAsset (asset, assetPathAndName);
 
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow ();
            Selection.activeObject = asset;
        }
    }
}