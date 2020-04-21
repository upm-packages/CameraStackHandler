using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace UnityPackage.CameraStackHandler.Tests.Runtime
{
    public class CameraPriorityTest
    {
        private Camera BaseCamera { get; set; }
        private Camera OverlayCamera1 { get; set; }
        private Camera OverlayCamera2 { get; set; }
        private Camera OverlayCamera3 { get; set; }

        private Scene BaseScene { get; set; }
        private Scene OverlayScene1 { get; set; }
        private Scene OverlayScene2 { get; set; }
        private Scene OverlayScene3 { get; set; }

        [SetUp]
        public void SetUp()
        {
            {
                BaseScene = SceneManager.CreateScene("BaseScene");
                SceneManager.SetActiveScene(BaseScene);
                var goBaseCamera = new GameObject("BaseCamera");
                BaseCamera = goBaseCamera.AddComponent<Camera>();
            }
            {
                OverlayScene1 = SceneManager.CreateScene("OverlayScene1");
                SceneManager.SetActiveScene(OverlayScene1);
                var goOverlayCamera = new GameObject("OverlayCamera1");
                OverlayCamera1 = goOverlayCamera.AddComponent<Camera>();
            }
            {
                OverlayScene2 = SceneManager.CreateScene("OverlayScene2");
                SceneManager.SetActiveScene(OverlayScene2);
                var goOverlayCamera = new GameObject("OverlayCamera2");
                OverlayCamera2 = goOverlayCamera.AddComponent<Camera>();
            }
            {
                OverlayScene3 = SceneManager.CreateScene("OverlayScene3");
                SceneManager.SetActiveScene(OverlayScene3);
                var goOverlayCamera = new GameObject("OverlayCamera3");
                OverlayCamera3 = goOverlayCamera.AddComponent<Camera>();
            }
        }

        [UnityTest]
        public IEnumerator 優先順の設定が反映される()
        {
            BaseCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Base;
            OverlayCamera1.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            OverlayCamera2.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            OverlayCamera3.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            var component1 = OverlayCamera1.gameObject.AddComponent<AddOverlayCameraToCameraStack>();
            var component2 = OverlayCamera2.gameObject.AddComponent<AddOverlayCameraToCameraStack>();
            var component3 = OverlayCamera3.gameObject.AddComponent<AddOverlayCameraToCameraStack>();
            component1.GetType().GetField("priority", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(component1,  10.0f);
            component2.GetType().GetField("priority", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(component2, -10.0f);
            component3.GetType().GetField("priority", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(component3,   0.0f);
            yield return null;
            var cameraStack = BaseCamera.GetUniversalAdditionalCameraData().cameraStack;
            Assert.That(cameraStack[0], Is.EqualTo(OverlayCamera2));
            Assert.That(cameraStack[1], Is.EqualTo(OverlayCamera3));
            Assert.That(cameraStack[2], Is.EqualTo(OverlayCamera1));
            yield return SceneManager.UnloadSceneAsync(OverlayScene3);
            yield return SceneManager.UnloadSceneAsync(OverlayScene2);
            yield return SceneManager.UnloadSceneAsync(OverlayScene1);
            yield return SceneManager.UnloadSceneAsync(BaseScene);
        }
    }
}