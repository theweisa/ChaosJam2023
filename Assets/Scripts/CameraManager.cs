using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : UnitySingleton<CameraManager>
{
    public CameraState currentCameraState = CameraState.StageView;
    public CinemachineBrain mainCamera;

    [SerializeField] public CinemachineVirtualCamera playerCamera, initialCollisionCamera, collisionCamera, stageCamera, winCamera;

    public CinemachineVirtualCamera currentCamera;
    public Transform maxPoint;
    public override void Awake() {
        base.Awake();
        //currentCamera = mainCamera.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
    }
    private void UpdateCameraFromState()
    {
        switch (currentCameraState)
        {
            case CameraState.ThrowView:
                break;
            case CameraState.CollisionView:
                break;
            case CameraState.StageView:
                break;
        }
    }
    public void Shake(float str=1f, float dur=1f, float freq=1f, bool perma=false) {
        CameraShake shake = currentCamera.GetComponent<CameraShake>();
        if (shake == null) return;
        shake.StartShake(str,dur,freq,perma);
    }

    public void SetCameraState(CameraState newState)
    {
        currentCameraState = newState;
        UpdateCameraFromState();
    }
    public void PanToCamera(CinemachineVirtualCamera cam) {
        mainCamera.ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
        cam.gameObject.SetActive(true);
        currentCamera = cam;
    }
}
