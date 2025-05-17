using DG.Tweening;
using System.Drawing;
using UnityEngine;

public class CarInstance : MonoBehaviour
{
    public GameObject brokenCar;
    public GameObject repairedCar;

    public GameObject bcDoor;
    public GameObject rcDoor;
    public GameObject piece;

    public static CarInstance instance;

    private bool state = false;

    private void Awake()
    {
        instance = this;
    }

    public void RepairCar()
    {
        repairedCar.SetActive(true);
        brokenCar.SetActive(false);

        piece.transform.localRotation = Quaternion.Euler(35f, 0, 0);
        piece.SetActive(true);
        piece.transform.DOLocalRotate(new Vector3(0, 0, 0), 2f);

        state = true;
    }

    public void OpenDoor()
    {
        if (state)
        {
            rcDoor.transform.DOLocalRotate(new Vector3(0, -80, 0), 1.25f);
        }
        else
        {
            bcDoor.transform.DOLocalRotate(new Vector3(0, -80, 0), 1.25f);
        }
    }
}
