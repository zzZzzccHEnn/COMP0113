using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.XR;
using UnityEngine;

public class grasp : MonoBehaviour, IGraspable
{
    Hand grapsed;

    void IGraspable.Grasp(Hand controller)
    {
        grapsed = controller;
    }

    void IGraspable.Release(Hand controller)
    {
        grapsed = null;
    }

    void Update ()
    {
        if (grapsed)
        {
            transform.localPosition = grapsed.transform.position;
            // transform.localRotation = grapsed.transform.rotation;
        }
    }
}