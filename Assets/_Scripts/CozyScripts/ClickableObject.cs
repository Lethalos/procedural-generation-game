using System;
using UnityEngine;


public class ClickableObject : MonoBehaviour
{
    public event Action<PoolObjectType> ObjectClicked; // Event declaration
    public PoolObjectType ObjectType;
    public bool IsClicked=false;

    public static Action<GameObject> OnObjectClicked { get; internal set; }

    // Method to raise the event
    public void RaiseObjectClickedEvent(PoolObjectType clickedObjectType)
    {
        ObjectClicked?.Invoke(clickedObjectType);
    }
   
}