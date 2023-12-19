using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Transform spawnPoint;
    public Game gameScript;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // Join or create a room with a maximum of 2 players
        PhotonNetwork.JoinOrCreateRoom("GameRoom", new Photon.Realtime.RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (playerPrefab != null && spawnPoint != null)
        {
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

            if (player.GetComponent<PhotonView>().IsMine)
            {
                if (gameScript != null)
                {
                    gameScript.AssignPlayer(player);
                }
                else
                {
                    Debug.LogError("Game script not assigned in NetworkManager.");
                }
            }
            Debug.Log("Player spawned.");
        }
        else
        {
            Debug.LogError("Error spawning player! Player prefab or spawn point not set.");
        }
    }
}
