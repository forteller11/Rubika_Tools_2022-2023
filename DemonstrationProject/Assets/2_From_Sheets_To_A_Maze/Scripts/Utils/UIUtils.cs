using System;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace Charly.SheetsToMaze.Utils
{
    public static class UIUtils
    {
        private const string UNITY_TEXT_INPUT = "unity-text-input";


        public static void AddErrorUnderline(TextField field)
        {
            var textInput = field.Q(UNITY_TEXT_INPUT);
            textInput.style.borderBottomColor = new Color(0.78f, 0.29f, 0.25f, 1);
            textInput.style.borderBottomWidth = 1f;
        }

        public static void RemoveUnderline(TextField field)
        {
            var textInput = field.Q(UNITY_TEXT_INPUT);
            textInput.style.borderBottomWidth = 0;
        }

        /// <summary>
        /// Converts camelCase "aGoodDog" to dash-based "a-good-dog".
        /// Useful for converting between C# naming conventions and UI Builder's (and HTML/CSS) attribute naming conventions in a typesafe manner.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CamelToDash(string s)
        {
            if (s.Length == 1)
                return s;
            
            StringBuilder builder = new StringBuilder(s.ToLowerInvariant(), s.Length+8);

            int builderOffset = 0;
            for (var i = 1; i < s.Length; i++)
            {
                if (char.IsLower(s, i - 1) && char.IsUpper(s, i))
                {
                    builder.Insert(i + builderOffset, '-');
                    builderOffset++;
                }
            }

            return builder.ToString();
        }

        public static string NameofDashed(Type t)
        {
            return CamelToDash(nameof(t));
        }
    }
}