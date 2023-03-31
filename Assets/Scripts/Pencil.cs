using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Ubiq.Messaging;
using Ubiq.XR;

public class Pencil : MonoBehaviour
{
    [SerializeField] private Transform _tip;
    private int _pensize = 10;

    private Renderer _renderer;
    private Color[] _colors;
    private float _tipheight;
    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos, _lasttouchPos;
    private bool _touchLastFrame;
    private Quaternion _lasttouchRot;


    void Start()
    {
        _renderer = _tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, _pensize * _pensize).ToArray();
        _tipheight = _tip.localScale.y;
    }

    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (Physics.Raycast(_tip.position, transform.up, out _touch, _tipheight))
        {
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _whiteboard.texturesize.x - (_pensize / 2));
                var y = (int)(_touchPos.y * _whiteboard.texturesize.y - (_pensize / 2));

                if (x < 0 || x > _whiteboard.texturesize.x || y < 0 || y > _whiteboard.texturesize.y) return;

                if (_touchLastFrame)
                {
                    _whiteboard.texture.SetPixels(x, y, _pensize, _pensize, _colors);

                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lasttouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lasttouchPos.y, y, f);
                        _whiteboard.texture.SetPixels(lerpX, lerpY, _pensize, _pensize, _colors);
                    }

                    transform.rotation = _lasttouchRot;

                    _whiteboard.texture.Apply();
                }

                _lasttouchPos = new Vector2(x, y);
                _lasttouchRot = transform.rotation;
                _touchLastFrame = true;
                return;

            }
        }

        _whiteboard = null;
        _touchLastFrame = false;

    }

}





