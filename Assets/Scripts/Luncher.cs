using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Sahab.Photon
{
    public class Luncher : MonoBehaviourPunCallbacks
    {
        [SerializeField] bool isConnecting;

        [SerializeField]
        private GameObject controlPanel;
        [SerializeField]
        private GameObject progressLabel;

        #region Monobehaviour Functions
        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            //Connect();
        }
        #endregion

        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            //if (PhotonNetwork.IsConnected)
            //{
            //PhotonNetwork.JoinRandomRoom();
            //}
            //else
            //{
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
            //}
        }

        #region PUN callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("Discounnected {0}", cause.ToString());
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Room");

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                SceneManager.LoadScene("Room for 1");
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to join room");

            var result = PhotonNetwork.CreateRoom("New Room", new RoomOptions { MaxPlayers = 4 });

            if (result) Debug.Log("Room Created");
            else Debug.Log("Room not creATED"); ;
        }
        #endregion
    }

}
