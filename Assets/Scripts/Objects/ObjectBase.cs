using UnityEngine;

[CreateAssetMenu(menuName = "CreateObject/NewObject")]
public class ObjectBase : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private AudioId interactionSfx;

    public string Name => name;
    public AudioClip Sfx => sfx;
    public AudioId InteractionSfx => interactionSfx;

    public void Interact()
    {
        // Play interaction SFX
        AudioManager.Instance.PlaySFX(interactionSfx, randomPitch: true);
    }
}
