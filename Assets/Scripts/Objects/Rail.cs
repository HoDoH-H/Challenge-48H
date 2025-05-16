using UnityEngine;

public class Rail : MonoBehaviour
{
    public RailType railType = RailType.Straight;

    public MeshFilter meshFilter;
    public Mesh[] meshes;

    private void OnValidate()
    {
        meshFilter.sharedMesh = meshes[(int)railType >= meshes.Length ? meshes.Length - 1 : (int)railType];
        if (railType == RailType.Left)
        {
            transform.localRotation = Quaternion.Euler(0, -34.5f, 0);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y + 0.0012f, 0f, 0.0012f), transform.position.z);
        }
        else if (railType == RailType.Right)
        {
            transform.localRotation = Quaternion.Euler(0, 34.5f, 0);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y + 0.0012f, 0f, 0.0012f), transform.position.z);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y - 0.0012f, 0f, 0.0012f), transform.position.z);
        }
    }
}

public enum RailType
{
    Straight,
    Left,
    Right
}
