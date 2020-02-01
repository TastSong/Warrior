using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour {

    private bool isPress = false;
    private Transform button;
    public static float h = 0;
    public static float v = 0;

    void Awake()
    {
        button = transform.Find("Button");
    }

    void OnPress(bool isPress)
    {
        this.isPress = isPress;

        if (isPress == false)
        {
            button.localPosition = Vector3.zero;
            h = 0; v = 0;
        }
    }

    void Update()
    {

        if (isPress)
        {
            Vector2 touchPos = UICamera.lastTouchPosition;
            touchPos -= new Vector2(182, 182);
            float distance = Vector2.Distance(Vector2.zero, touchPos);
            if(distance > 400)
            {
                return;
            }
            if (distance > 140)
            {
                touchPos = touchPos.normalized * 140;
                button.localPosition = touchPos;
            }
            else
            {
                button.localPosition = touchPos;
            }

            h = touchPos.x / 140;
            v = touchPos.y / 140;
        }

    }
}
