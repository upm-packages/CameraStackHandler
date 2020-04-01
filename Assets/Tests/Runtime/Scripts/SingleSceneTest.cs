using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace UnityPackage.CameraStackHandler.Tests.Runtime
{
    public class SingleSceneTest
    {
        private Camera BaseCamera { get; set; }
        private Camera OverlayCamera { get; set; }

        private Scene SingleScene { get; set; }

        [SetUp]
        public void SetUp()
        {
            SingleScene = SceneManager.CreateScene("SingleScene");
            SceneManager.SetActiveScene(SingleScene);
            var goBaseCamera = new GameObject("BaseCamera");
            var goOverlayCamera = new GameObject("OverlayCamera");
            BaseCamera = goBaseCamera.AddComponent<Camera>();
            OverlayCamera = goOverlayCamera.AddComponent<Camera>();
        }

        [UnityTest]
        public IEnumerator OverlayCameraが正しくCameraStackに追加される()
        {
            BaseCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Base;
            OverlayCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            OverlayCamera.gameObject.AddComponent<AddOverlayCameraToCameraStack>();
            yield return null;
            Assert.That(
                BaseCamera
                    .GetUniversalAdditionalCameraData()
                    .cameraStack
                    .Contains(OverlayCamera),
                Is.True
            );
            yield return SceneManager.UnloadSceneAsync(SingleScene);
        }

        [UnityTest]
        public IEnumerator BaseCameraはCameraStackに追加されない()
        {
            BaseCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Base;
            OverlayCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Base;
            var addOverlayCameraToCameraStack = OverlayCamera.gameObject.AddComponent<AddOverlayCameraToCameraStack>();
            addOverlayCameraToCameraStack.GetType().GetField("overwriteRenderType", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(addOverlayCameraToCameraStack, false);
            yield return null;
            Assert.That(
                BaseCamera
                    .GetUniversalAdditionalCameraData()
                    .cameraStack
                    .Count,
                Is.Zero
            );
            yield return SceneManager.UnloadSceneAsync(SingleScene);
        }

        [UnityTest]
        public IEnumerator OverwriteRenderTypeを真にするとCameraStackに追加される()
        {
            BaseCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Base;
            OverlayCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Base;
            var addOverlayCameraToCameraStack = OverlayCamera.gameObject.AddComponent<AddOverlayCameraToCameraStack>();
            addOverlayCameraToCameraStack.GetType().GetField("overwriteRenderType", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(addOverlayCameraToCameraStack, true);
            yield return null;
            Assert.That(
                BaseCamera
                    .GetUniversalAdditionalCameraData()
                    .cameraStack
                    .Contains(OverlayCamera),
                Is.True
            );
            yield return SceneManager.UnloadSceneAsync(SingleScene);
        }
    }
}