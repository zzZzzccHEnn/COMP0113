using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubiq.Extensions;
using Ubiq.Messaging;
using Ubiq.Spawning;
using Ubiq.XR;
using UnityEngine;

public class MarkerPurple: MonoBehaviour, IGraspable, IComponent
{
    /// <summary>
    /// // This property fulfils INetworkSpawnable. Spawnable objects need to 
    /// have their Ids set by the Object Spawner before they are registered, so
    /// all spawned objects can communicate with eachother.
    /// </summary>
    public NetworkId NetworkId { get; set; } 

    private FollowHelper follow;
    private NetworkContext context;
    private ContraptionManager manager;

    public void Grasp(Hand controller)
    {
        follow.Grasp(controller);
    }

    public void Release(Hand controller)
    {
        follow.Release(controller);
        // Attach();
    }

    private void Awake()
    {
        follow = new FollowHelper(transform);
    }

    void Start()
    {
    //    string name = "Whiteboard";
    //    GameObject go = GameObject.Find(name);
    //    Destroy(go);

        context = NetworkScene.Register(this);
        manager = context.Scene.GetClosestComponent<ContraptionManager>();
    }

    
    void Update()
    {
        // // if (Input.GetKeyDown(KeyCode.JoystickButton4)){
        // if (Input.GetKeyDown(KeyCode.JoystickButton2)) 
        // {
        //     Destroy(gameObject);
        // }
        
        if (follow.Update())
        {
            SendUpdate();
        }
    }

    private struct Message
    {
        public Vector3 position;
        public Quaternion rotation;
        // public NetworkId attachedId;
    }

    public void SendUpdate()
    {
        context.SendJson(new Message()
        {
            position = manager.GetLocalPosition(transform),
            rotation = manager.GetLocalRotation(transform),
            // attachedId = manager.GetNetworkId(transform.parent)
        });
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var message = m.FromJson<Message>();
        transform.position = manager.GetWorldPosition(message.position);
        transform.rotation = manager.GetWorldRotation(message.rotation);
        // Attach(manager.GetComponentRigidBody(message.attachedId));
    }
}
