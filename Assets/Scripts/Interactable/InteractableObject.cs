using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("References")]
    public Transform graphic;

    [Header("Settings")]
    public bool isGlowing = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        SetGlowing(isGlowing ? 4 : 1);
    }

    public void SetGlowing(float intensity = 0)
    {
        graphic.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * intensity);
    }
}
