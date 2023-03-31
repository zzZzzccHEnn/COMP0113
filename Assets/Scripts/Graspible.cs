using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.XR;
using UnityEngine;

public class Graspible : MonoBehaviour, IGraspable
{
    Hand grapsed;
    NetworkContext context;
    private bool currentDestoy = false;

    public void Grasp(Hand controller)
    {
        grapsed = controller;
    }

    public void Release(Hand controller)
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
        public bool isDestroy;

        public Message(Transform transform, bool isDestroy)
        {
            // (float x, float y, float z, float w) rotationTuple = (-1f, 0f, 0f, 0f);
            // this.x_180deg = new Quaternion(rotationTuple.x, rotationTuple.y, rotationTuple.z, rotationTuple.w);
            this.position = transform.localPosition;
            this.rotation = transform.localRotation;
            this.isDestroy = isDestroy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (grapsed)
        {
            transform.localPosition = grapsed.transform.position;
            transform.localRotation = grapsed.transform.rotation * Quaternion.Euler(-160f,15f,0);
            // * Quaternion.Euler(-160f,15f,0);
            context.SendJson(new Message(transform, currentDestoy));
            // if (Input.GetKeyDown(KeyCode.JoystickButton1)) 
            if (currentDestoy || Input.GetKeyDown(KeyCode.JoystickButton3)) 
            {
                currentDestoy = true;
                context.SendJson(new Message(transform, currentDestoy));
            }
        }

        if (currentDestoy)
        {
            Destroy(gameObject);
        }
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var data = m.FromJson<Message>();
        transform.localPosition = m.FromJson<Message>().position;
        transform.localRotation = m.FromJson<Message>().rotation;
        currentDestoy = data.isDestroy;
    }
}