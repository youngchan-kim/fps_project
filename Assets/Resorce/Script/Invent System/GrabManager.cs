using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class GrabManager : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Slot currentSlot;

    public Sprite uiMask;

    private Canvas canvas;

    private GraphicRaycaster gRayCaster;

    public GameObject dragHandler;

    private Vector2 lastMousePosition;


    // Start is called before the first frame update
    void Start()
    {
        uiMask = GetComponent<Image>().sprite;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        currentSlot = this.transform.parent.GetComponent<Slot>();

        if (currentSlot.empty)
            return;
        lastMousePosition = transform.position;
        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0) / transform.lossyScale.x;

        
        if(!canvas)
        {
            canvas = dragHandler.GetComponent<Canvas>();
            gRayCaster = canvas.GetComponent<GraphicRaycaster>();
        }
        transform.SetParent(canvas.transform.parent, true);
        transform.SetAsLastSibling();
        
        
    }
    
    public void OnDrag (PointerEventData eventData)
    {
        if(currentSlot.empty)
            return;
        Vector2 currentMousePosition = eventData.position;
        transform.position = currentMousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentSlot.empty)
            return;

        var results = new List<RaycastResult>();
        gRayCaster.Raycast(eventData, results);
        foreach (var hit in results)
        {
            var slot = hit.gameObject.GetComponent<Slot>();
            if(slot)
            {
                if(slot.empty)
                {
                    var currentSlotID = currentSlot.slotID;
                    var newSlotID = slot.slotID;

                    var anotherItem = slot.transform.GetChild(0);
                    anotherItem.SetParent(currentSlot.transform);
                    anotherItem.transform.localPosition = Vector3.zero;

                    currentSlot.empty = true;


                    currentSlot = slot;
                    transform.SetParent(currentSlot.transform);
                    transform.localPosition = Vector3.zero;

                    currentSlot.empty = false;
                    break;
                }
            }
        }
        transform.SetParent(currentSlot.transform);
        transform.localPosition = Vector3.zero;
       
    }
}
