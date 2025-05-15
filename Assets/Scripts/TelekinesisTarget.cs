using System;
using UnityEngine;

public class TelekinesisTarget : MonoBehaviour
{
    public Action levitatingObjectGottenTooFar;
    public InteractableObject currentObject;

    [SerializeField] private float maxDistance = 15f;

    private void Update()
    {
        if (currentObject != null)
        {
            if (Vector3.Distance(transform.parent.position, currentObject.transform.position) > maxDistance)
            {
                levitatingObjectGottenTooFar?.Invoke();
            }
        }
    }
}
