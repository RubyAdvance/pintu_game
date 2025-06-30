using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubPic : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // 初始位置
    private Vector2 originPos;
    public bool isDragging = false;
    public bool isPlaced = false;
    public string index;
    
    // 点击模式相关
    private bool isInClickMode = false;
    private bool isFirstClick = true;
    private float lastClickTime;
    private const float clickModeTimeout = 3f; // 点击模式超时时间，超过时间则触发目标位置检测（预留暂未启用）

    void Awake()
    {
        originPos = transform.localPosition;
    }

    public void Init(string curIndex)
    {
        index = curIndex;
        isPlaced = false;
        isInClickMode = false;
        isFirstClick = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPlaced) return;
        
        // 处理点击模式
        if (isInClickMode)
        {
            // 点击模式下再次点击 - 尝试放置
            HandlePlacement();
            return;
        }
        
        // 第一次点击 - 开始点击模式
        StartClickMode();
    }

    private void StartClickMode()
    {
        isDragging = true;
        isInClickMode = true;
        GameController.Instance.currentSubPic = this;
        transform.localScale = Vector3.one * 1.1f;
        GameController.Instance.handMove.ChangeFingerState(true);
        transform.SetAsLastSibling();
        
        // 开始跟随鼠标
        StartCoroutine(FollowMouseRoutine());
    }

    private IEnumerator FollowMouseRoutine()
    {
        lastClickTime = Time.time;
        
        while (isInClickMode && !isPlaced)
        {
            // 跟随鼠标
            transform.position = GameController.Instance.handMove.transform.position;
            
            // 这个地方预留
            // if (Time.time - lastClickTime > clickModeTimeout)
            // {
            //     CancelClickMode();
            // }
            
            yield return null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaced) return;
        
        // 如果处于点击模式，转换为拖拽模式
        if (isInClickMode)
        {
            isInClickMode = false;
            isDragging = true;
            GameController.Instance.currentSubPic = this;
            transform.SetAsLastSibling();
            GameController.Instance.handMove.ChangeFingerState(true);
        }
        
        // 处理拖拽
        if (isDragging)
        {
            transform.position = GameController.Instance.handMove.transform.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPlaced || !isDragging) return;
        
        // 拖拽模式下松开 - 尝试放置
        if (!isInClickMode)
        {
            HandlePlacement();
        }
    }
    
    private void HandlePlacement()
    {
        GameController.Instance.handMove.ChangeFingerState(false);
        
        if (GameController.Instance.CheckDis(index))
        {
            // 放置成功
            PlaceSuccessfully();
        }
        else
        {
            // 回到原位
            ReturnToOrigin();
        }
        
        ResetState();
    }
    
    private void PlaceSuccessfully()
    {
        transform.position = GameController.Instance.main_sub_pic_pos[int.Parse(index)].position;
        transform.localScale = Vector3.one;
        isPlaced = true;
        GameController.Instance.CurFinishedCount++;
    }
    
    private void ReturnToOrigin()
    {
        transform.localPosition = originPos;
        transform.localScale = Vector3.one;
    }
    
    private void ResetState()
    {
        isDragging = false;
        isInClickMode = false;
        GameController.Instance.currentSubPic = null;
    }
    /// <summary>
    /// 点击后，长时间无操作自动检测退出点击模式
    /// </summary>
    public void CancelClickMode()
    {
        if (isInClickMode)
        {
            ReturnToOrigin();
            ResetState();
            GameController.Instance.handMove.ChangeFingerState(false);
        }
    }
}