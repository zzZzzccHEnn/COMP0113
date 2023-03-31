using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ubiq.Extensions;
using Ubiq.Messaging;
using Ubiq.Spawning;
using Ubiq.XR;

public class Whiteboard : MonoBehaviour,IComponent
{
    public Texture2D texture;
    public Vector2 texturesize = new Vector2(2048, 2048);
    public NetworkId NetworkId { get; set; } 
    private NetworkContext context;
    private bool currentDestroy = false;

    private Vector3 NewPosition = new Vector3(-0.43F, 0F, 5.5F);

    void Start()
    {

        var r = GetComponent<Renderer>();
        texture = new Texture2D((int)texturesize.x, (int)texturesize.y);
        r.material.mainTexture = texture;

        context = NetworkScene.Register(this);
    }

    struct Message
    {
        public bool isDestroy;

        public Message(bool isDestroy)
        {
            this.isDestroy = isDestroy;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1)) 
        {
            currentDestroy = true;
        }

        if (currentDestroy)
        {
            context.SendJson(new Message(currentDestroy));
            Destroy(gameObject);
        }

    }

    public void SendUpdate()
    {

    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var data = m.FromJson<Message>();
        currentDestroy = data.isDestroy;
    }


}