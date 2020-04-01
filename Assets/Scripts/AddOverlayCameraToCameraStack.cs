﻿using System;
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

            OverlayCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;

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

            TargetCameraData.cameraStack.Add(OverlayCamera);
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

