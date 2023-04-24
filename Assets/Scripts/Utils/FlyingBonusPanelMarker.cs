using UnityEngine;

public class FlyingBonusPanelMarker : MonoBehaviour
{
    public float MinPosY;
    public float MaxPosY;

    public Vector3 GetRandom()
    {
        return new Vector3(transform.position.x, Random.Range(MinPosY, MaxPosY));
    }

    public FlyingBonusPanelMarker SetToRandomPoint()
    {
        GetComponent<RectTransform>().anchoredPosition = 
            new Vector2(GetComponent<RectTransform>().anchoredPosition.x, Random.Range(MinPosY, MaxPosY));
        return this;
    }
}
