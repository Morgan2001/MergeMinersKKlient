using UnityEngine;

public class FlyingBonusPanelMarker : MonoBehaviour
{
    public float MinPosY;
    public float MaxPosY;

    public Vector3 GetRandom()
    {
        return new Vector3(transform.position.x, Random.Range(MinPosY, MaxPosY));
    }
}
