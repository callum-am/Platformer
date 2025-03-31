using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public Room startRoomPrefab;
    public Room finalRoomPrefab;
    public Room shopRoomPrefab;
    public List<Room> roomPrefabs;
    public int numberOfRooms = 10;
    public float roomSize = 16f;
    public int seed = 0; // Add this property to set the seed value

    private Dictionary<Vector2, Room> placedRooms = new Dictionary<Vector2, Room>();

    void Start()
    {
        GenerateDungeon();
    }

    private Room GetRightProgressingRoom(Room.Direction previousExit)
    {
        // Get all rooms that can connect from the previous exit
        List<Room> compatibleRooms = roomPrefabs.FindAll(room => 
            room.entryDirection == previousExit && 
            room.exitDirection == Room.Direction.Right);

        if (compatibleRooms.Count == 0)
        {
            Debug.LogError($"No compatible rooms found for entry: {previousExit} with right exit");
            return null;
        }

        return compatibleRooms[Random.Range(0, compatibleRooms.Count)];
    }

    void GenerateDungeon()
    {
        Random.InitState(seed); // Initialize the random number generator with the seed
        placedRooms.Clear();

        // Place start room (always exits right)
        Vector2 currentPos = Vector2.zero;
        Room currentRoom = PlaceRoom(startRoomPrefab, currentPos);
        if (currentRoom == null) return;

        // Generate middle rooms
        int roomsPlaced = 1;
        while (roomsPlaced < numberOfRooms - 1)
        {
            Vector2 nextPos = GetNextPosition(currentPos, currentRoom.exitDirection);
            
            // Check if next position is already occupied
            if (placedRooms.ContainsKey(nextPos))
            {
                Debug.LogError($"Position {nextPos} is already occupied - trying to find alternative path...");
                break;
            }

            // Special case: Room before shop must exit right
            if (roomsPlaced == (numberOfRooms / 2) - 1)
            {
                Room rightExitRoom = GetRightProgressingRoom(currentRoom.exitDirection);
                if (rightExitRoom == null) break;

                currentPos = GetNextPosition(currentPos, currentRoom.exitDirection);
                currentRoom = PlaceRoom(rightExitRoom, currentPos);
                roomsPlaced++;
                continue;
            }

            // Place shop room (should have right entry and right exit)
            if (roomsPlaced == numberOfRooms / 2)
            {
                currentPos = GetNextPosition(currentPos, currentRoom.exitDirection);
                currentRoom = PlaceRoom(shopRoomPrefab, currentPos);
                roomsPlaced++;
                continue;
            }

            // Special case: Room before final must exit right
            if (roomsPlaced == numberOfRooms - 2)
            {
                Room rightExitRoom = GetRightProgressingRoom(currentRoom.exitDirection);
                if (rightExitRoom == null) break;

                currentPos = GetNextPosition(currentPos, currentRoom.exitDirection);
                currentRoom = PlaceRoom(rightExitRoom, currentPos);
                roomsPlaced++;
                continue;
            }

            // Normal room placement
            currentPos = GetNextPosition(currentPos, currentRoom.exitDirection);
            Room nextRoom = GetCompatibleRoom(currentRoom.exitDirection);
            if (nextRoom == null) break;

            currentRoom = PlaceRoom(nextRoom, currentPos);
            roomsPlaced++;
        }

        // Place final room only if position is free
        Vector2 finalPos = GetNextPosition(currentPos, currentRoom.exitDirection);
        if (!placedRooms.ContainsKey(finalPos))
        {
            PlaceRoom(finalRoomPrefab, finalPos);
        }
    }

    private Room GetCompatibleRoom(Room.Direction requiredEntry)
    {
        List<Room> compatible = roomPrefabs.FindAll(r => r.entryDirection == requiredEntry);
        if (compatible.Count == 0) return null;
        return compatible[Random.Range(0, compatible.Count)];
    }

    private Vector2 GetNextPosition(Vector2 currentPos, Room.Direction exitDirection)
    {
        return exitDirection switch
        {
            Room.Direction.Right => currentPos + Vector2.right,
            Room.Direction.Top => currentPos + Vector2.up,
            Room.Direction.Bottom => currentPos + Vector2.down,
            _ => currentPos
        };
    }

    private Room PlaceRoom(Room prefab, Vector2 position)
    {
        // Check if position is already occupied
        if (placedRooms.ContainsKey(position))
        {
            Debug.LogError($"Cannot place room at {position} - position already occupied!");
            return null;
        }

        Vector3 worldPos = new Vector3(position.x * roomSize, position.y * roomSize, 0);
        Room room = Instantiate(prefab, worldPos, Quaternion.identity);
        placedRooms[position] = room;
        Debug.Log($"Placed {room.roomType} room at {position} with entry: {room.entryDirection}, exit: {room.exitDirection}");
        return room;
    }
}
