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
        isPlaced = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPlaced) return;
        // 确保只有一个拼图被拖拽
        if (GameController.Instance.currentSubPic == null && !isDragging)
        {
            isDragging = true;
            GameController.Instance.currentSubPic = this;
            // 轻微放大效果
            transform.localScale = Vector3.one * 1.1f;
            GameController.Instance.handMove.ChangeFingerState(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaced) return;
        transform.position = GameController.Instance.handMove.transform.position;
        transform.SetAsLastSibling();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPlaced) return;
        GameController.Instance.handMove.ChangeFingerState(false);
        // Debug.Log("放开了");
        if (GameController.Instance.CheckDis(index))
        {
            //放置成功
            transform.position = GameController.Instance.main_sub_pic_pos[int.Parse(index)].position;
            transform.localScale = Vector3.one;
            isPlaced = true;
            GameController.Instance.CurFinishedCount++;
        }
        else
        {
            //判断当前的位置是否在目标位置
            transform.localPosition = originPos;
        }

        GameController.Instance.currentSubPic = null;
        isDragging = false;
    }
}
