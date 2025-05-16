using UnityEngine;

[CreateAssetMenu(menuName = "CreateObject/NewObject")]
public class ItemOne : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private string dream;
    [SerializeField] private string nightmare;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private AudioClip dreamSfx;
    [SerializeField] private AudioClip nightmareSfx;


    [SerializeField] private AudioId interactionSfx;

    public string Name => name;
    public string Dream => dream;
    public string Nightmare => nightmare;

    public AudioClip Sfx => sfx;
    public AudioClip DreamSfx => dreamSfx;
    public AudioClip NightmareSfx => nightmareSfx;
    
    
}
