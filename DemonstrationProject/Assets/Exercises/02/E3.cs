using System;
using UnityEditor;
using UnityEngine;

namespace Exercises.Class02
{
    public class E3 : EditorWindow
    {
        private string _firstName;
        private string _lastName;
        private string _fullName;
        
        [MenuItem("Window/Name Validator")]
        private static void Create()
        {
            var window = EditorWindow.GetWindow<E3>();
            window.titleContent = new GUIContent("Name Validator");
            window.Show();
        }

        private void OnGUI()
        {
            _firstName = EditorGUILayout.TextField("First Name", _firstName);
            _lastName = EditorGUILayout.TextField("Last Name", _lastName);

            char firstNameFirstLetter = char.ToLower(_firstName[0]);
            char lastNameFirstLetter = char.ToLower(_lastName[0]);
            
            if (firstNameFirstLetter != ' ' && firstNameFirstLetter == lastNameFirstLetter)
            {
                var skin = new GUIStyle();
                skin.normal.textColor = new Color(1f, 0.26f, 0.16f);
                EditorGUILayout.LabelField($"Cannot have first and last name with same letter \"{firstNameFirstLetter}\"", skin);
            }
            else
            {
                EditorGUILayout.LabelField("Full name", _firstName + ' ' + _lastName);
            }
        }
    }
}