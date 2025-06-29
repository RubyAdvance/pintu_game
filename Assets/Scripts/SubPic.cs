using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SubPic : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    //初始位置
    private Vector2 originPos;
    public bool isDragging = false;
    public bool isPlaced = false;
    /// <summary>
    /// /// 当前拼图的下标，通过下标确定目标位置，然后进行判断
    /// </summary>
    public string index;


    void Awake()
    {
        originPos = transform.localPosition;
    }

    public void Init(string curIndex)
    {
        index = curIndex;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 确保只有一个拼图被拖拽
        if (GameController.Instance.currentSubPic == null && !isDragging)
        {
            isDragging = true;
            GameController.Instance.currentSubPic = this;
            // 轻微放大效果
            transform.localScale = Vector3.one * 1.1f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GameController.Instance.handMove.transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("放开了");

        //判断当前的位置是否在目标位置
    }
}
