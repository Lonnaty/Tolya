using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


//public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
//{
//    public float Horizontal { get { return (snapX) ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; } }
//    public float Vertical { get { return (snapY) ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; } }
//    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

//    public float HandleRange
//    {
//        get { return handleRange; }
//        set { handleRange = Mathf.Abs(value); }
//    }

//    public float DeadZone
//    {
//        get { return deadZone; }
//        set { deadZone = Mathf.Abs(value); }
//    }

//    public AxisOptions AxisOptions { get { return AxisOptions; } set { axisOptions = value; } }
//    public bool SnapX { get { return snapX; } set { snapX = value; } }
//    public bool SnapY { get { return snapY; } set { snapY = value; } }

//    [SerializeField] private float handleRange = 1;
//    [SerializeField] private float deadZone = 0;
//    [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
//    [SerializeField] private bool snapX = false;
//    [SerializeField] private bool snapY = false;

//    [SerializeField] protected RectTransform background = null;
//    [SerializeField] private RectTransform handle = null;
//    private RectTransform baseRect = null;

//    private Canvas canvas;
//    private Camera cam;

//    private Vector2 input = Vector2.zero;

//    protected virtual void Start()
//    {
//        HandleRange = handleRange;
//        DeadZone = deadZone;
//        baseRect = GetComponent<RectTransform>();
//        canvas = GetComponentInParent<Canvas>();
//        if (canvas == null)
//            Debug.LogError("The Joystick is not placed inside a canvas");

//        Vector2 center = new Vector2(0.5f, 0.5f);
//        background.pivot = center;
//        handle.anchorMin = center;
//        handle.anchorMax = center;
//        handle.pivot = center;
//        handle.anchoredPosition = Vector2.zero;
//    }

//    public virtual void OnPointerDown(PointerEventData eventData)
//    {
//        OnDrag(eventData);
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        cam = null;
//        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
//            cam = canvas.worldCamera;

//        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
//        Vector2 radius = background.sizeDelta / 2;
//        input = (eventData.position - position) / (radius * canvas.scaleFactor);
//        FormatInput();
//        HandleInput(input.magnitude, input.normalized, radius, cam);
//        handle.anchoredPosition = input * radius * handleRange;
//    }

//    protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
//    {
//        if (magnitude > deadZone)
//        {
//            if (magnitude > 1)
//                input = normalised;
//        }
//        else
//            input = Vector2.zero;
//    }

//    private void FormatInput()
//    {
//        if (axisOptions == AxisOptions.Horizontal)
//            input = new Vector2(input.x, 0f);
//        else if (axisOptions == AxisOptions.Vertical)
//            input = new Vector2(0f, input.y);
//    }

//    private float SnapFloat(float value, AxisOptions snapAxis)
//    {
//        if (value == 0)
//            return value;

//        if (axisOptions == AxisOptions.Both)
//        {
//            float angle = Vector2.Angle(input, Vector2.up);
//            if (snapAxis == AxisOptions.Horizontal)
//            {
//                if (angle < 22.5f || angle > 157.5f)
//                    return 0;
//                else
//                    return (value > 0) ? 1 : -1;
//            }
//            else if (snapAxis == AxisOptions.Vertical)
//            {
//                if (angle > 67.5f && angle < 112.5f)
//                    return 0;
//                else
//                    return (value > 0) ? 1 : -1;
//            }
//            return value;
//        }
//        else
//        {
//            if (value > 0)
//                return 1;
//            if (value < 0)
//                return -1;
//        }
//        return 0;
//    }

//    public virtual void OnPointerUp(PointerEventData eventData)
//    {
//        input = Vector2.zero;
//        handle.anchoredPosition = Vector2.zero;
//    }

//    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
//    {
//        Vector2 localPoint = Vector2.zero;
//        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
//        {
//            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
//            return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
//        }
//        return Vector2.zero;
//    }
//}

//public enum AxisOptions { Both, Horizontal, Vertical }


public class UIVirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [System.Serializable]
    public class Event : UnityEvent<Vector2> { }
    
    [Header("Rect References")]
    public RectTransform containerRect;
    public RectTransform handleRect;

    [Header("Settings")]
    public float joystickRange = 50f;
    public float magnitudeMultiplier = 1f;
    public bool invertXOutputValue;
    public bool invertYOutputValue;

    [Header("Output")]
    public Event joystickOutputEvent;

    void Start()
    {
        SetupHandle();
    }

    private void SetupHandle()
    {
        if(handleRect)
        {
            UpdateHandleRectPosition(Vector2.zero);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Cursor.lockState = CursorLockMode.None;
        OnDrag(eventData);
    }


    public void OnDrag(PointerEventData eventData)
    {
        Debug.LogError("ASAHHSAHSAHSAHFSHSAHSAFHSAHSAHFHSAAFSH");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);
        
        position = ApplySizeDelta(position);
        
        Vector2 clampedPosition = ClampValuesToMagnitude(position);

        Vector2 outputPosition = ApplyInversionFilter(position);

        OutputPointerEventValue(outputPosition * magnitudeMultiplier);

        if(handleRect)
        {
            UpdateHandleRectPosition(clampedPosition * joystickRange);
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OutputPointerEventValue(Vector2.zero);

        if(handleRect)
        {
             UpdateHandleRectPosition(Vector2.zero);
        }
    }

    private void OutputPointerEventValue(Vector2 pointerPosition)
    {
        joystickOutputEvent.Invoke(pointerPosition);
        Debug.Log("Event invoked");
    }

    private void UpdateHandleRectPosition(Vector2 newPosition)
    {
        handleRect.anchoredPosition = newPosition;
    }

    Vector2 ApplySizeDelta(Vector2 position)
    {
        float x = (position.x/containerRect.sizeDelta.x) * 2.5f;
        float y = (position.y/containerRect.sizeDelta.y) * 2.5f;
        return new Vector2(x, y);
    }

    Vector2 ClampValuesToMagnitude(Vector2 position)
    {
        return Vector2.ClampMagnitude(position, 1);
    }

    Vector2 ApplyInversionFilter(Vector2 position)
    {
        if(invertXOutputValue)
        {
            position.x = InvertValue(position.x);
        }

        if(invertYOutputValue)
        {
            position.y = InvertValue(position.y);
        }

        return position;
    }

    float InvertValue(float value)
    {
        return -value;
    }
    
}