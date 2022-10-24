using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Exercises.Class02
{
    public class UnityUIAndIMGUIButtonContrastExample : MonoBehaviour
    {
        public Button Button;

        void Start()
        {
            Button.onClick.AddListener(MyOnClick);
        }

        void MyOnClick()
        {
            Debug.Log("Unity UI Button First Click");
            Button.onClick.RemoveAllListeners();
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(UnityUIAndIMGUIButtonContrastExample))]
    public class UnityUIAndIMGUIButtonContrastExampleEditor : Editor
    {
        private bool alreadyPressedOnce = false;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("IMGUI Button"))
            {
                if (!alreadyPressedOnce)
                    Debug.Log("IMGUI Button First Click");

                alreadyPressedOnce = true;
            }
        }
    }
#endif
    
}