using System;
using UnityEditor;
using UnityEngine;

namespace Charly.ToolsOnASphere
{
    [CustomEditor(typeof(MoveAlongSurface))]
    public class SphericalParentAuthoring : Editor
    {
        private void OnSceneGUI()
        {
            var mover = (MoveAlongSurface) target;
            if (!mover.enabled)
                return;
            
            RefreshMoverStateBasedOnTransformAndRaycast(mover);
            RotateAndTranslateTangent(mover);
            DoOrthogonalTranslation(mover);
        }

        private static int GetLayerMask()
        {
            int mask = LayerMask.NameToLayer(MoveAlongSurface.LayerName);
            //bitwise NOT (~) to inverse mask, but then make sure there are no negatives
            mask = Math.Abs(~mask);
            return mask;
        }
        private static void RefreshMoverStateBasedOnTransformAndRaycast(MoveAlongSurface mover)
        {
            Transform transform = mover.transform;
            
            bool raycastAtStart = Physics.Raycast(
                new Ray(transform.position, 
                    mover.GetAdjustedDown()), 
                out var hit, 
                float.PositiveInfinity, 
                GetLayerMask());
            if (raycastAtStart)
            {
                mover.DistanceFromSurface = Vector3.Distance(transform.position, hit.point); 
                mover.LatestHit = new SerialHit(hit);
            }
        } 

        private static void RotateAndTranslateTangent(MoveAlongSurface mover)
        {
            var e = Event.current;

            EditorGUI.BeginChangeCheck();
            var normalToLocal = Handles.RotationHandle(mover.NormalToLocal, mover.LatestHit.Point);
            if (EditorGUI.EndChangeCheck())
            {
                mover.NormalToLocal = normalToLocal;
                mover.SetRotationFromSurfaceNormal();
                mover.RecordUndo("Rotate relative to surface normal");
            }
            
            var lookRot = Quaternion.LookRotation(mover.LatestHit.Normal);
            var tangentI = lookRot * Vector3.up;
            var tangentJ = lookRot * Vector3.right;
            
            EditorGUI.BeginChangeCheck();
            Handles.color = Handles.centerColor;
            Handles.Slider2D(
                mover.LatestHit.Point,
                mover.LatestHit.Normal,
                tangentI,
                tangentJ,
                HandleUtility.GetHandleSize(mover.LatestHit.Point) * .2f,
                Handles.CircleHandleCap,
                0.001f);
            if (EditorGUI.EndChangeCheck())
            {
                var sceneView = SceneView.lastActiveSceneView;
                var sceneCamera = sceneView.camera;
                var screenRay = sceneCamera.ScreenPointToRay(new Vector3(
                    e.mousePosition.x,
                    sceneView.camera.pixelHeight - e.mousePosition.y,
                    0));
            
                if (Physics.Raycast(screenRay, out var hit, float.PositiveInfinity, GetLayerMask()))
                {
                    mover.LatestHit = new SerialHit(hit);
                    mover.transform.position = hit.point + (hit.normal * mover.DistanceFromSurface);
                    mover.SetRotationFromSurfaceNormal();
                    mover.RecordUndo("Translate along surface");
                }
            }

            Handles.color = new Color(1, 1, 1, 0.2f);
            Handles.DrawDottedLine(mover.transform.position, mover.LatestHit.Point, 5f);
            Handles.color = Handles.yAxisColor;
            var size = HandleUtility.GetHandleSize(mover.LatestHit.Point) * 0.35f;
            var upRelativeToSurface = (mover.NormalToLocal * mover.transform.up) * size;
            Handles.DrawLine(mover.LatestHit.Point, mover.LatestHit.Point + upRelativeToSurface);
        }
        
        private static void DoOrthogonalTranslation(MoveAlongSurface mover)
        {
            float handleOrthSize = HandleUtility.GetHandleSize(mover.LatestHit.Point) * 1f;
            var transform = mover.transform;
            
            EditorGUI.BeginChangeCheck();
            Handles.color = Handles.centerColor;
            var orthSliderPos = Handles.Slider(
                transform.position,
                mover.LatestHit.Normal,
                handleOrthSize,
                Handles.ArrowHandleCap,
                0.001f);

            if (EditorGUI.EndChangeCheck())
            {
                transform.position = orthSliderPos;
                mover.RecordUndo("Change orthogonal distance from surface.");
                RefreshMoverStateBasedOnTransformAndRaycast(mover);
            }
        }
    }
}