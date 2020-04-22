using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace UnityPackage.CameraStackHandler.Tests.Runtime
{
    public class TakeOverCameraStackTest
    {
        private Camera BaseCamera1 { get; set; }
        private Camera BaseCamera2 { get; set; }
        private Camera OverlayCamera1 { get; set; }
        private Camera OverlayCamera2 { get; set; }

        private Scene BaseScene1 { get; set; }
        private Scene BaseScene2 { get; set; }
        private Scene OverlayScene1 { get; set; }
        private Scene OverlayScene2 { get; set; }

        [SetUp]
        public void SetUp()
        {
            {
                BaseScene1 = SceneManager.CreateScene("BaseScene1");
                SceneManager.SetActiveScene(BaseScene1);
                var goBaseCamera = new GameObject("BaseCamera1");
                BaseCamera1 = goBaseCamera.AddComponent<Camera>();
            }
            {
                BaseScene2 = SceneManager.CreateScene("BaseScene2");
                SceneManager.SetActiveScene(BaseScene2);
                var goBaseCamera = new GameObject("BaseCamera2");
                BaseCamera2 = goBaseCamera.AddComponent<Camera>();
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
        }

        [UnityTest]
        public IEnumerator Overlay変換時にStackを引き継ぐ()
        {
            BaseCamera1.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Base;
            BaseCamera2.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Base;
            OverlayCamera1.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            OverlayCamera2.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            BaseCamera1.depth = 10.0f;
            BaseCamera2.depth = 20.0f;
            yield return null;

            var componentForOverlayCamera1 = OverlayCamera1.gameObject.AddComponent<AddOverlayCameraToCameraStack>();
            componentForOverlayCamera1.GetType().GetField("priority", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(componentForOverlayCamera1, -10.0f);
            yield return null;

            Assert.That(BaseCamera2.GetUniversalAdditionalCameraData().cameraStack[0], Is.EqualTo(OverlayCamera1));

            var componentForBaseCamera = BaseCamera2.gameObject.AddComponent<AddOverlayCameraToCameraStack>();
            var componentForOverlayCamera2 = OverlayCamera2.gameObject.AddComponent<AddOverlayCameraToCameraStack>();
            componentForBaseCamera.GetType().GetField("priority", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(componentForBaseCamera,  10.0f);
            componentForOverlayCamera2.GetType().GetField("priority", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(componentForOverlayCamera2,   0.0f);
            yield return null;
            var cameraStack = BaseCamera1.GetUniversalAdditionalCameraData().cameraStack;
            Assert.That(cameraStack[0], Is.EqualTo(OverlayCamera1));
            Assert.That(cameraStack[1], Is.EqualTo(OverlayCamera2));
            Assert.That(cameraStack[2], Is.EqualTo(BaseCamera2));
            yield return SceneManager.UnloadSceneAsync(OverlayScene2);
            yield return SceneManager.UnloadSceneAsync(OverlayScene1);
            yield return SceneManager.UnloadSceneAsync(BaseScene2);
            yield return SceneManager.UnloadSceneAsync(BaseScene1);
        }
    }
}