using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static bool CanPlaceRoom(Vector3 position, float roomSize, LayerMask roomLayer)
    {
        Collider2D overlap = Physics2D.OverlapBox(position, new Vector2(roomSize, roomSize), 0, roomLayer);
        return overlap == null;
    }
}
