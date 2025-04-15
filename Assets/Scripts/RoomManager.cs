using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    private GameObject playerInstance;


    [SerializeField] private GameObject[] rooms; // Assign all rooms in the inspector
    [SerializeField] private int currentRoomIndex = 0;
    public int CurrentRoomIndex => currentRoomIndex; // Public read-only access
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        playerInstance = GameObject.FindGameObjectWithTag("Player");

        // Activate only the first room at start
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].SetActive(i == 0);

            Debug.Log("Room " + i + " active: " + rooms[i].activeSelf);

        }

    }

    public void GoToNextRoom()
    {
        // Deactivate current room
        rooms[currentRoomIndex].SetActive(false);

        Debug.Log("Current Room Index: " + currentRoomIndex);

        // Move to next room
        currentRoomIndex++;

        Debug.Log("Current Room Index: " + currentRoomIndex);


        if (currentRoomIndex < rooms.Length)
        {
            // Activate next room
            rooms[currentRoomIndex].SetActive(true);

            // Find spawn point in new room and move player there
            MovePlayerToRoomSpawn();
        }
        else
        {
            // No more rooms - win the game
            SceneLoadManager.instance.LoadScene("GameWin");
        }
    }

    private void MovePlayerToRoomSpawn()
    {
        // Find the spawn point in the current room

        Debug.Log("Trying to move player to spawn point");

        Transform spawnPoint = rooms[currentRoomIndex].transform.Find("PlayerSpawn");

        if (spawnPoint != null)
        {
            Debug.Log("Spawn point found in room: " + currentRoomIndex);
        }

        if (playerInstance == null)
        {
            playerInstance = GameObject.FindGameObjectWithTag("Player");

            if (playerInstance != null)
            {
                Debug.Log("Player instance found at runtime!");
            }

            else
            {
                Debug.LogError("Player instance not found!");
            }

        }
            if (spawnPoint != null && playerInstance != null)
        {
            playerInstance.transform.position = spawnPoint.position;

            Debug.Log("Player moved to spawn point: " + spawnPoint.position);


        }

    }
}