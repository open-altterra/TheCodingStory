using UnityEngine;
using UnityEngine.EventSystems;

public class WindowController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private RectTransform window;
    [SerializeField]
    private RectTransform leftResizeZone, topResizeZone, rightResizeZone, bottomResizeZone;
    [SerializeField]
    private RectTransform dragZone;

    private Vector2 originalPointerPosition;
    private Vector3 originalWindowPosition;
    private Vector2 originalWindowSize;

    private bool isLeftResizing, isTopResizing, isRightResizing, isBottomResizing;
    private bool isDrag;
    [SerializeField]
    private Vector2Int minSize;
    [SerializeField]
    private Vector2Int maxSize;

    private Vector2 originalMousePosition;
    private bool isResizing;
    private bool mousePositionCached = false;
    private bool isFullscreenMode = false;
    private Vector2 cachedSize;
    private Vector2 cachedPosition;
    private Vector2 cachedPivot;
    private void Awake()
    {
        window = GetComponent<RectTransform>();

    }

    private void Start()
    {
        CloseWindow();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        window.SetSiblingIndex(window.parent.childCount - 1);

        originalPointerPosition = eventData.position;
        originalWindowPosition = window.position;
        originalWindowSize = window.sizeDelta;

        isLeftResizing = leftResizeZone.isMouseOverUI();
        isTopResizing = topResizeZone.isMouseOverUI();
        isRightResizing = rightResizeZone.isMouseOverUI();
        isBottomResizing = bottomResizeZone.isMouseOverUI();

        isResizing = isLeftResizing || isTopResizing || isRightResizing || isBottomResizing;

        if (!isResizing)
            isDrag = dragZone.isMouseOverUI();

        originalMousePosition = eventData.position;
        mousePositionCached = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mousePositionCached = isLeftResizing = isTopResizing = isRightResizing = isBottomResizing = isDrag = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!mousePositionCached) return;
        if (isFullscreenMode) return;

        if (isDrag)
        {
            Vector3 offset = eventData.position - originalPointerPosition;
            window.position = originalWindowPosition + offset;
        }

        if (isResizing)
        {
            if (isLeftResizing)
                window.SetPivot(new Vector2(1f, window.pivot.y));
            if (isTopResizing)
                window.SetPivot(new Vector2(window.pivot.x, 0f));
            if (isRightResizing)
                window.SetPivot(new Vector2(0f, window.pivot.y));
            if (isBottomResizing)
                window.SetPivot(new Vector2(window.pivot.x, 1f));

            Vector2 mouseDelta = eventData.position - originalMousePosition;
            if (isLeftResizing) mouseDelta.x = -mouseDelta.x;
            if (isBottomResizing) mouseDelta.y = -mouseDelta.y;

            Vector2 sizeDelta = originalWindowSize + mouseDelta / GetComponentInParent<Canvas>().scaleFactor;

            sizeDelta.x = Mathf.Clamp(sizeDelta.x, minSize.x, maxSize.x);
            sizeDelta.y = Mathf.Clamp(sizeDelta.y, minSize.y, maxSize.y);

            if (isLeftResizing || isRightResizing)
                window.sizeDelta = new Vector2(sizeDelta.x, window.sizeDelta.y);

            if (isTopResizing || isBottomResizing)
                window.sizeDelta = new Vector2(window.sizeDelta.x, sizeDelta.y);
        }
    }

    public void CloseWindow()
    {
        window.gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        window.gameObject.SetActive(true);
        window.SetSiblingIndex(window.parent.childCount - 1);
    }

    public void ChangeFullScreenMode()
    {
        window.SetSiblingIndex(window.parent.childCount - 1);

        if (isFullscreenMode)
        {
            isFullscreenMode = false;
            window.localPosition = cachedPosition;
            window.sizeDelta = cachedSize;
            window.pivot = cachedPivot;
        }
        else
        {
            isFullscreenMode = true;
            cachedPosition = window.localPosition;
            cachedSize = window.sizeDelta;
            cachedPivot = window.pivot;


            window.pivot = Vector2.zero;

            Vector2 fullSize = GetComponentInParent<Canvas>().GetComponent<RectTransform>().sizeDelta;

            window.sizeDelta = fullSize;
            window.localPosition = -fullSize / 2f;

        }
    }
}

public static class ExtensionMethods
{
    public static Rect GetGlobalPosition(this RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    }

    public static bool isMouseOverUI(this RectTransform rectTransform)
    {
        Rect position = rectTransform.GetGlobalPosition();
        return position.Contains(Input.mousePosition);
    }

    public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
    {
        if (rectTransform == null) return;

        Vector2 size = rectTransform.rect.size;
        Vector2 deltaPivot = rectTransform.pivot - pivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPosition;
    }
}
