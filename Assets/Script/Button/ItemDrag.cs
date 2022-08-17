using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 Position;
    private GameObject TargetParent;

    private GameObject SelectObj;
    private Action<GameObject> EndDragExec;
    private Action<GameObject> DragExec;
    private Action<GameObject> BeginExec;

    public bool IsCanDrag = true;
    private bool IsStart = false;

    void Update()
    {
        if (IsStart && Position != null && TargetParent != null)
        {
            GameObject obj = GetCorrGameObject(Position, TargetParent);
            if (!obj && !SelectObj)
                return;
            SelectObj = obj;
            DragExec?.Invoke(SelectObj);
        }
    }

    /// <summary>
    /// ����Ŀ�길����
    /// </summary>
    /// <param name="parent"></param>
    public void SetTargetParent(GameObject parent)
    {
        TargetParent = parent;
    }

    /// <summary>
    /// ������ק��ʼ�ص�
    /// </summary>
    /// <param name="exec"></param>
    public void SetBeginDragExec(Action<GameObject> exec)
    {
        BeginExec = exec;
    }

    /// <summary>
    /// ������ק�����ص�
    /// </summary>
    /// <param name="exec"></param>
    public void SetEndDragExec(Action<GameObject> exec)
    {
        EndDragExec = exec;
    }

    /// <summary>
    /// ������קʱ�ص�
    /// </summary>
    /// <param name="exec"></param>
    public void SetDragExec(Action<GameObject> exec)
    {
        DragExec = exec;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsCanDrag)
        {
            IsStart = true;
            BeginExec?.Invoke(gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsCanDrag)
            return;
        SetDraggedPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsStart = false;
        if (!IsCanDrag)
            return;
        EndDragExec?.Invoke(SelectObj);
    }

    private void SetDraggedPosition(PointerEventData eventData)
    {
        Position = eventData.position;
    }

    /// <summary>
    /// ���
    /// </summary>
    /// <param name="position"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    private GameObject GetCorrGameObject(Vector2 position, GameObject parent)
    {
        EventSystem eventSystem = EventSystem.current;
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = position;
        GameObject obj;
        //���߼��ui
        List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, uiRaycastResultCache);
        for (int i = 0; i < uiRaycastResultCache.Count; i++)
        {
            obj = uiRaycastResultCache[i].gameObject;
            if (obj.transform.parent.gameObject == parent)
                return obj;
        }
        return null;
    }
}