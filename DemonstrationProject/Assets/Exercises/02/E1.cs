using UnityEngine;

namespace Exercises.Class02
{
    public class E1 : MonoBehaviour
    {
        [Header("Name Related")]
        [SerializeField] private string _firstName;
        [SerializeField] private string _lastName;
        [HideInInspector] public string NickName;
        
        [Space]
        [SerializeField] [TextArea] private string _bio;
        [Range(150, 200)] [Tooltip("height measured in cm")] public float Height;
    }
}