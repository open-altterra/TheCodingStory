using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerRaycastLocker : MonoBehaviour
{
    private void OnEnable()
    {
        try
        {
            LinksStorage.Instance.CharacterController.LockRaycastFromBanner = true;
        }
        catch { }
    }

    private void Start()
    {
        LinksStorage.Instance.CharacterController.LockRaycastFromBanner = true;
    }

    private void OnDisable()
    {
        LinksStorage.Instance.CharacterController.LockRaycastFromBanner = false;
    }
}
