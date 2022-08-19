using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    public static ScreenShake Instance { get; private set; }

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        if (Instance != null)
        {
            Debug.Log("There is more than one ScreenShake " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }

    public void Shake(float intesity = 1f)
    {
        impulseSource.GenerateImpulse(intesity);
    }
}
