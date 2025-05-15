using UnityEngine;

public class Character : MonoBehaviour
{
    public MainHandState mainHand;
    public OffHandState offHand;
    public HeadState head;

    public CharacterGraphic characterGraphic;

    private void OnValidate()
    {
        Equip();
    }

    public void Equip()
    {
        if (mainHand == MainHandState.LongSword && offHand != OffHandState.None)
        {
            offHand = OffHandState.None;
        }

        UpdateGraphic();
    }

    private void UpdateGraphic()
    {
        
    }

    public enum MainHandState
    {
        None,
        Sword,
        LongSword,
    }

    public enum OffHandState
    {
        None,
        Sword,
        BadgeShield,
        RectangleShield,
        RoundShield,
        SpikeShield,
    }

    public enum HeadState
    {
        None,
        Helmet,
    }
}
