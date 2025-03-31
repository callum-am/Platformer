using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomType { Start, Normal, Shop, Final }
    public enum Direction { Right, Top, Bottom }

    public RoomType roomType;
    public Direction entryDirection;  // Direction this room connects FROM
    public Direction exitDirection;   // Direction this room connects TO
}
