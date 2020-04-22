using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace UnityPackage.CameraStackHandler
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(UniversalAdditionalCameraData))]
    public class AddOverlayCameraToCameraStack : MonoBehaviour
    {
        [SerializeField] private Camera overlayCamera = default;
        [SerializeField] private bool overwriteRenderType = true;
        [SerializeField] private float priority = 0.0f;

        private Camera OverlayCamera => overlayCamera == default ? overlayCamera = GetComponent<Camera>() : overlayCamera;
        private bool OverwriteRenderType => overwriteRenderType;
        private UniversalAdditionalCameraData TargetCameraData { get; set; }

        private void Start()
        {
            if (OverlayCamera == default)
            {
                return;
            }

            if (OverlayCamera.GetUniversalAdditionalCameraData().renderType != CameraRenderType.Overlay && !OverwriteRenderType)
            {
                Debug.LogWarning($"Camera component in {OverlayCamera.gameObject.name} does not seems to be Overlay Camera. Set 'Overlay' to RenderType field.");
                return;
            }

            var originalCameraStack = new List<Camera>();
            if (OverlayCamera.GetUniversalAdditionalCameraData().renderType == CameraRenderType.Base)
            {
                originalCameraStack = OverlayCamera.GetUniversalAdditionalCameraData().cameraStack?.ToList();
                OverlayCamera.GetUniversalAdditionalCameraData().cameraStack?.Clear();
            }

            OverlayCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            OverlayCamera.depth = priority;

            TargetCameraData = FindObjectsOfType<Camera>()
                .Select(x => (x.depth, data: x.GetUniversalAdditionalCameraData()))
                .Where(x => x.data.renderType == CameraRenderType.Base)
                .OrderByDescending(x => x.depth)
                .FirstOrDefault()
                .data;
            if (TargetCameraData == default)
            {
                return;
            }

            var cameraStack = TargetCameraData.cameraStack.ToList();
            cameraStack.Add(OverlayCamera);
            if (originalCameraStack != default && originalCameraStack.Any())
            {
                cameraStack.AddRange(originalCameraStack);
            }

            TargetCameraData.cameraStack.Clear();
            foreach (var c in cameraStack.OrderBy(x => x.depth))
            {
                TargetCameraData.cameraStack.Add(c);
            }
        }

        private void OnDestroy()
        {
            if (TargetCameraData != default && OverlayCamera != default)
            {
                TargetCameraData.cameraStack.Remove(OverlayCamera);
            }
        }

        private void Reset()
        {
            if (overlayCamera == default && GetComponent<Camera>() != default)
            {
                overlayCamera = GetComponent<Camera>();
            }
        }
    }
}

