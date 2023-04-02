using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [SerializeField] GameObject _PlayerPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (_PlayerPrefab == null) return;

        PhotonNetwork.Instantiate(_PlayerPrefab.name, Vector3.zero, Quaternion.identity);
    }

    #region public functions
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region PUN callbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("Photon: Player entered room: {0} ", newPlayer.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.IsMasterClient)
        {
            LoadArena();
        }
    }
    #endregion


    #region Private Functions
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    #endregion
}
