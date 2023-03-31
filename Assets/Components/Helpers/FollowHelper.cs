using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;

public class FollowHelper
{
    private Vector3 localGrabPoint;
    private Quaternion localGrabRotation;
    private Transform transform;
    private Transform follow;

    public bool IsGrasped => follow != null;

    public FollowHelper(Transform transform)
    {
        this.transform = transform;
    }

    public void Grasp(Hand controller)
    {
        var handTransform = controller.transform;
        localGrabPoint = handTransform.InverseTransformPoint(transform.position);
        localGrabRotation = Quaternion.Inverse(handTransform.rotation) * transform.rotation;
        follow = handTransform;
    }

    public void Release(Hand controller)
    {
        follow = null;
    }

    public bool Update()
    {
        if (follow)
        {
            transform.rotation = follow.rotation * localGrabRotation;
            transform.position = follow.TransformPoint(localGrabPoint);
            return true;
        }
        return false;
    }
}
