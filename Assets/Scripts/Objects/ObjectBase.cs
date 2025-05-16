using UnityEngine;

[CreateAssetMenu(menuName = "CreateObject/NewObject")]
public class ObjectBase : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private AudioId interactionSfx;
    [SerializeField] private ObjectType objectType = ObjectType.None;

    public string Name => name;
    public AudioClip Sfx => sfx;
    public AudioId InteractionSfx => interactionSfx;
    public ObjectType ObjectType => objectType;

    public void Interact()
    {
        // Play interaction SFX
        AudioManager.Instance.PlaySFX(interactionSfx, randomPitch: true);
    }
}

public enum ObjectType { None, NeedShow, NeedChoices, NeedSpecialChoices}
