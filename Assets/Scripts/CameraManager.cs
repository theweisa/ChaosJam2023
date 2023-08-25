using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : UnitySingleton<CameraManager>
{
    public CameraState currentCameraState = CameraState.StageView;

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

    public void SetCameraState(CameraState newState)
    {
        currentCameraState = newState;
        UpdateCameraFromState();
    }
}
