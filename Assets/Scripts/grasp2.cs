using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.XR;
using UnityEngine;

public class grasp2 : MonoBehaviour, IGraspable
{
    Hand grapsed;

    NetworkContext context;

    void IGraspable.Grasp(Hand controller)
    {
        grapsed = controller;
    }

    void IGraspable.Release(Hand controller)
    {
        grapsed = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
    }

    struct Message
    {
        public Vector3 position;
        public Quaternion rotation;

        public Message(Transform transform)
        {
            this.position = transform.localPosition;
            this.rotation = transform.localRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (grapsed)
        {
            transform.localPosition = grapsed.transform.position;
            transform.localRotation = grapsed.transform.rotation;
            context.SendJson(new Message(transform));
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        transform.localPosition = m.FromJson<Message>().position;
        transform.localRotation = m.FromJson<Message>().rotation;
    }
}