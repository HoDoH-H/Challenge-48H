using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateObject/NewObject")]
public class ObjectBase : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private AudioId interactionSfx;
    [SerializeField] private ObjectType objectType = ObjectType.JustShow;
    [SerializeField] private string dreamText;
    [SerializeField] private string nightmareText;

    public string Name => name;
    public AudioClip Sfx => sfx;
    public AudioId InteractionSfx => interactionSfx;
    public ObjectType ObjectType => objectType;
    public string DreamText => dreamText;
    public string NightmareText => nightmareText;

    public void Interact()
    {
        // Play interaction SFX
        AudioManager.Instance.PlaySFX(interactionSfx, randomPitch: true);
        GameManager.instance.gotCurrentObject = true;
    }

    public void Use()
    {

    }
}

public enum ObjectType { JustShow, NeedShowAndRotation, NeedChoices, NeedSpecialChoices}
