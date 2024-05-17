using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour
{
    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;
    private float screenWidthInPoints;

    public GameObject[] availableObstacles;
    public GameObject[] availableCoins;

    public List<GameObject> currentObstacles;
    public List<GameObject> currentCoins;

    public float objectsMinDistance = 2.0f;
    public float objectsMaxDistance = 5.0f;

    public float objectsMinY = -1.4f;
    public float objectsMaxY = 1.4f;

    public float obstaclesMinRotation = -45.0f;
    public float obstaclesMaxRotation = 45.0f;

    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            GenerateRoomIfRequired();
            GenerateObjectsIfRequired();
            yield return new WaitForSeconds(0.25f);
        }
    }

    void Start()
    {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
        StartCoroutine(GeneratorCheck());
    }

    void AddRoom(float farthestRoomEndX)
    {
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);
        float roomWidth = room.transform.Find("floor").localScale.x;
        float roomCenter = farthestRoomEndX + roomWidth * 0.5f;
        
        room.transform.position = new Vector3(roomCenter, 0, 0);
        currentRooms.Add(room);
    }

    private void GenerateRoomIfRequired()
    {
        List<GameObject> roomsToRemove = new List<GameObject>();
        bool addRooms = true;
        float playerX = transform.position.x;
        float removeRoomX = playerX - screenWidthInPoints;
        float addRoomX = playerX + screenWidthInPoints;
        float farthestRoomEndX = 0;

        foreach (var room in currentRooms)
        {
            float roomWidth = room.transform.Find("floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;

            if (roomStartX > addRoomX)
            {
                addRooms = false;
            }
            if (roomEndX < removeRoomX)
            {
                roomsToRemove.Add(room);
            }
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }

        foreach (var room in roomsToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }

        if (addRooms)
        {
            AddRoom(farthestRoomEndX);
        }
    }

    void AddObstacle(float lastObjectX)
    {
        int randomIndex = Random.Range(0, availableObstacles.Length);
        GameObject obstacle = (GameObject)Instantiate(availableObstacles[randomIndex]);

        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
        float randomY = Random.Range(objectsMinY, objectsMaxY);

        obstacle.transform.position = new Vector3(objectPositionX, randomY, 0);
        float rotation = Random.Range(obstaclesMinRotation, obstaclesMaxRotation);
        obstacle.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);

        currentObstacles.Add(obstacle);
    }

    void AddCoin(float lastObjectX)
    {
        int randomIndex = Random.Range(0, availableCoins.Length);
        GameObject coin = (GameObject)Instantiate(availableCoins[randomIndex]);

        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
        float randomY = Random.Range(objectsMinY, objectsMaxY);

        coin.transform.position = new Vector3(objectPositionX, randomY, 0);

        currentCoins.Add(coin);
    }

    void GenerateObjectsIfRequired()
    {
        float playerX = transform.position.x;
        float removeObjectsX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;

        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach (var obj in currentObstacles)
        {
            float objX = obj.transform.position.x;
            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            if (objX < removeObjectsX)
            {
                objectsToRemove.Add(obj);
            }
        }
        foreach (var obj in objectsToRemove)
        {
            currentObstacles.Remove(obj);
            Destroy(obj);
        }

        objectsToRemove.Clear();

        foreach (var obj in currentCoins)
        {
            float objX = obj.transform.position.x;
            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            if (objX < removeObjectsX)
            {
                objectsToRemove.Add(obj);
            }
        }
        foreach (var obj in objectsToRemove)
        {
            currentCoins.Remove(obj);
            Destroy(obj);
        }

        if (farthestObjectX < addObjectX)
        {
            if (Random.value > 0.5f)
            {
                AddObstacle(farthestObjectX);
            }
            else
            {
                AddCoin(farthestObjectX);
            }
        }
    }

    void Update()
    {
    }
}
