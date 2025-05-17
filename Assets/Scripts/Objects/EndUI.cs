using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EndUI : MonoBehaviour
{
    public static EndUI Instance;
    public GameObject obj;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator Credits()
    {
        yield return obj.transform.DOMoveY(3000f, 20f).WaitForCompletion();
        Application.Quit();
    }
}
