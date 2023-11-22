using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroCoordinates : MonoBehaviour
{
    [SerializeField]
    private Vector3 defaultCoordinate;

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, defaultCoordinate, 1f * Time.deltaTime);
    }
}
