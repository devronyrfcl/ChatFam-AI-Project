using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

namespace Reallusion.Import
{
    [InitializeOnLoad]
    public static class WindowManager
    {
        public static Scene currentScene;
        public static PreviewScene previewScene;
        public static bool showTools = true;
        static WindowManager()
        {
            // Even if update is not the most elegant. Using hierarchyWindowChanged for CPU sake will not work in all cases, because when hierarchyWindowChanged is called, Time's values might be all higher than current values. Why? Because current values are set at the first frame. If you keep reloading the same scene, this case happens.
            EditorApplication.update += WindowManager.MonitorScene;             
        }        

        private static void MonitorScene()
        {
            Scene activeScene = EditorSceneManager.GetActiveScene();
            if (currentScene != activeScene)
            {
                currentScene = activeScene;
                previewScene = PreviewScene.GetPreviewScene();
            }

            bool isPlayerShown = AnimPlayerGUI.IsPlayerShown();
            bool validPreviewScene = previewScene.IsValid;

            if (validPreviewScene) 
            {                
                if (showTools && !isPlayerShown)
                {                    
                    AnimPlayerGUI.CreatePlayer(previewScene, ImporterWindow.Current?.Character?.Fbx);
                }
                else if (!showTools && isPlayerShown)
                {                    
                    AnimPlayerGUI.DestroyPlayer();
                }
            }
            else 
            {
                if (isPlayerShown)
                {                    
                    AnimPlayerGUI.DestroyPlayer();
                }
            }
        }

        public static void DoSceneViewOrbit()
        {
            if (!isSceneViewOrbit)
            {
                trackTarget = null;
                EditorApplication.update += SceneViewOrbitUpdate;
                isSceneViewOrbit = true;
            }
            else
            {
                trackTarget = null;
                EditorApplication.update -= SceneViewOrbitUpdate;
                isSceneViewOrbit = false;
            }
        }

        public static void StopSceneViewOrbit()
        {
            if (isSceneViewOrbit)
            {
                EditorApplication.update -= SceneViewOrbitUpdate;
                trackTarget = null;
                isSceneViewOrbit = true;
            }
        }

        public static void DoSceneViewOrbitTracking()
        {            
            if (!isSceneViewOrbit)
            {
                trackTarget = Selection.activeTransform;
                if (trackTarget)
                {
                    pivotDisplacement = SceneView.lastActiveSceneView.pivot - trackTarget.position;
                    EditorApplication.update += SceneViewOrbitUpdate;
                    isSceneViewOrbit = true;
                }
            }
            else
            {
                EditorApplication.update -= SceneViewOrbitUpdate;
                isSceneViewOrbit = false;
            }

        }

        private static bool isSceneViewOrbit;
        private static Vector3 pivotDisplacement;
        private static Transform trackTarget;

        static void SceneViewOrbitUpdate()
        {
            if (isSceneViewOrbit)
            {
                SceneView scene = SceneView.lastActiveSceneView;
                Vector3 pivot = scene.pivot;
                Vector3 pos = scene.camera.transform.position;
                Vector3 boom = pos - pivot;
                if (trackTarget)
                {
                    pivot = trackTarget.position + pivotDisplacement;
                }
                float fov = scene.camera.fieldOfView;
                float dist = scene.cameraDistance;
                float size = Mathf.Sin(Mathf.Deg2Rad * fov / 2f) * dist;

                boom = Quaternion.AngleAxis(0.1f, Vector3.up) * boom;

                SceneView.lastActiveSceneView.LookAtDirect(pivot, Quaternion.LookRotation(-boom, Vector3.up), size);
                SceneView.RepaintAll();
            }
        }

        public static void DoMatchSceneCamera()
        {
            if (!isMatchSceneViewCamera)
            {
                EditorApplication.update += MatchSceneCameraUpdate;
                isMatchSceneViewCamera = true;
            }
            else
            {
                EditorApplication.update -= MatchSceneCameraUpdate;
                isMatchSceneViewCamera = false;
            }
        }

        public static void StopMatchSceneCamera()
        {
            if (isSceneViewOrbit)
            {
                EditorApplication.update -= MatchSceneCameraUpdate;
                isMatchSceneViewCamera = true;
            }
        }        

        private static bool isMatchSceneViewCamera;
        
        static void MatchSceneCameraUpdate()
        {
            if (isMatchSceneViewCamera)
            {
                Transform cam = previewScene.GetCamera();
                SceneView scene = SceneView.lastActiveSceneView;                
                Vector3 pos = scene.camera.transform.position;
                Quaternion rot = scene.camera.transform.rotation;
                float fov = scene.camera.fieldOfView;                
                cam.position = pos;
                cam.rotation = rot;
                Camera camera = cam.GetComponent<Camera>();
                if (camera)
                {
                    camera.fieldOfView = fov;
                }
            }
        }

        public static void TakeScreenShot()
        {

            string dateStamp = DateTime.Now.ToString("yyMMdd-hhmmss");
            string fileName = "Screenshot-" + dateStamp + ".png";
            Debug.Log("Saving screenshot to: " + fileName);
            ScreenCapture.CaptureScreenshot(fileName);
        }
    }
}
