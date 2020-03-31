using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace UnityPackage.CameraStackHandler.Tests.Runtime
{
    public class MultipleSceneTest
    {
        private Camera BaseCamera { get; set; }
        private Camera OverlayCamera { get; set; }

        private Scene BaseScene { get; set; }
        private Scene OverlayScene { get; set; }

        [SetUp]
        public void SetUp()
        {
            BaseScene = SceneManager.CreateScene("BaseScene");
            OverlayScene = SceneManager.CreateScene("OverlayScene");
            SceneManager.SetActiveScene(BaseScene);
            var goBaseCamera = new GameObject("BaseCamera");
            SceneManager.SetActiveScene(OverlayScene);
            var goOverlayCamera = new GameObject("OverlayCamera");
            BaseCamera = goBaseCamera.AddComponent<Camera>();
            OverlayCamera = goOverlayCamera.AddComponent<Camera>();
        }

        [UnityTest]
        public IEnumerator Sceneを跨いでいても正しくCameraStackに追加される()
        {
            BaseCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Base;
            OverlayCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            OverlayCamera.gameObject.AddComponent<AddOverlayCameraToCameraStack>();
            yield return null;
            Assert.That(BaseCamera.gameObject.scene.name, Is.EqualTo("BaseScene"));
            Assert.That(OverlayCamera.gameObject.scene.name, Is.EqualTo("OverlayScene"));
            Assert.That(
                BaseCamera
                    .GetUniversalAdditionalCameraData()
                    .cameraStack
                    .Contains(OverlayCamera),
                Is.True
            );
            yield return SceneManager.UnloadSceneAsync(OverlayScene);
            yield return SceneManager.UnloadSceneAsync(BaseScene);
        }
    }
}