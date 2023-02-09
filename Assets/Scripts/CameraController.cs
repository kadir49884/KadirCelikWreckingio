using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{

    private CinemachineVirtualCamera cmVirtualCam;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private void Start()
    {
        cmVirtualCam = transform.GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cmVirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); ;
    }

    public void ShakeCamera(float intensity, float time)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        StartCoroutine(WaitForShake(time));
    }

    IEnumerator WaitForShake(float getShakeTime)
    {
        yield return new WaitForSeconds(getShakeTime);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }

}
