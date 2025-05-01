using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.Color;
// using System.Drawing.Color;

public class Compass : MonoBehaviour
{
    public RawImage compassIMG;
    private Transform _player;

    float compassUnit;

    void Start()
    {
        _player = transform;
        compassUnit = compassIMG.rectTransform.rect.width / 360f;
    }

    void Update()
    {
        compassIMG.uvRect = new Rect(_player.localEulerAngles.y / 360f, 0f, 1f, 1f);
    }
}