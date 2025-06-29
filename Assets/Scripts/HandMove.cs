using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HandMove : MonoBehaviour
{
    public Sprite handOpenSprite;
    public Sprite handClosedSprite;
    /// <summary>
    /// 时候按下
    /// </summary>
    private bool isDown = false;

    public void Init(bool clicked)
    {
        ChangeFingerState(clicked);
    }

    void Update()
    {
        // 更新手的位置跟随鼠标
        transform.position = ClampPositionToScreen(Input.mousePosition);
    }

    public void ChangeFingerState(bool clicked)
    {
        isDown = clicked;
        if (isDown)
        {
            transform.GetComponent<Image>().sprite = handClosedSprite;
        }
        else
        {
            transform.GetComponent<Image>().sprite = handOpenSprite;
        }
    }

    Vector2 ClampPositionToScreen(Vector2 position)
    {
        // 计算屏幕边界
        float minX = 0;
        float maxX = Screen.width;
        float minY = 0;
        float maxY = Screen.height;
        // 应用边界限制
        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);

        return new Vector2(clampedX, clampedY);
    }
}
