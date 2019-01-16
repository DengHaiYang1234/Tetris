using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    private Camera camera;
    private void Awake()
    {
        camera = Camera.main;
    }

    public void ZoomIn()
    {
        camera.DOOrthoSize(12.7f, 0.5f);
    }

    public void ZoomOut()
    {
        camera.DOOrthoSize(14.28f,0.5f);
    }

}
