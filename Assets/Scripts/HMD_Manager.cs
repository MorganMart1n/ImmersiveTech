using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class HMD_Manager : MonoBehaviour
{
    [SerializeField] GameObject xrPlayer;
    [SerializeField] GameObject fpsPlayer;
    // Start is called before the first frame update
    void Start()
    {
       



        Debug.Log("Using Device: " + XRSettings.loadedDeviceName);
        if (XRSettings.isDeviceActive)
        {
            xrPlayer.SetActive(true);
            fpsPlayer.SetActive(false);
        }
        else
        {
            xrPlayer.SetActive(false);
            fpsPlayer.SetActive(true);
        }
    }
}
