using System;
using UnityEngine;

//this namespace doesn't exist on devices so we can only include it in the compile the UnityEditor is present.
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Charly.ToolsOnASphere
{    
    [SelectionBase]
    public class MoveAlongSurface : MonoBehaviour
    {
        public const string LayerName = "Props";

        //todo allow editing in inspector
        [HideInInspector] public float DistanceFromSurface;
        [HideInInspector] [SerializeField] public SerialHit LatestHit;
        [SerializeField] public Quaternion NormalToLocal;

        public void Update()
        {
            //stub so that script can be disabled in the inspector
        }

        private void Reset()
        {
            Debug.LogWarning($"Changing layer of {gameObject} from {gameObject.layer} to {LayerName}");
            gameObject.layer = LayerMask.NameToLayer(LayerName);
            NormalToLocal = Quaternion.identity;
        }
        
        public Vector3 GetAdjustedDown()
        {
            return  Quaternion.Inverse(NormalToLocal) * -transform.up;
        }

        public Vector3 GetAdjustedNormal()
        {
            return NormalToLocal * LatestHit.Normal;
        }

        public void SetRotationFromSurfaceNormal()
        {
            var normal = GetAdjustedNormal();
            var rotation = Quaternion.FromToRotation(transform.up, normal);
            transform.rotation = rotation * transform.rotation;
        }

#if UNITY_EDITOR
    public void RecordUndo(in string msg)
    {
        Undo.RegisterCompleteObjectUndo(transform, msg);
        Undo.RegisterCompleteObjectUndo(this, msg);
        EditorUtility.SetDirty(transform);
        EditorUtility.SetDirty(this);
    }
#endif
        
    }

    [Serializable]
    public struct SerialHit
    {
        public Vector3 Normal;
        public Vector3 Point;

        public SerialHit(RaycastHit hit)
        {
            Normal = hit.normal;
            Point = hit.point;
        }
    }
}
