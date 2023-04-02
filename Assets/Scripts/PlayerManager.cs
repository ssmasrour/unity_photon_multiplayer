using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;
using Photon.Realtime;

using System.Collections;

using Photon.Pun.Demo.PunBasics;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Private Fields
    public float Health = 0;
    bool IsFiring;


    [SerializeField]
    private GameObject beams;


    PhotonView PhotonView;
    #endregion

    #region MonoBehaviour CallBacks

    void Awake()
    {
        if (beams == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
        }
        else
        {
            beams.SetActive(false);
        }

        PhotonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        var cameraController = GetComponent<CameraWork>();

        bool cameraActivation = cameraController != null;
        cameraActivation &= photonView.IsMine;

        if (cameraActivation)
        {
            cameraController.OnStartFollowing();
        } else
        {
            Debug.LogError("Camera is not mine", this);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonView.IsMine)
        {
            return;
        }

        if (!other.name.Contains("Beam"))
        {
            return;
        }

        Health -= 0.1f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!PhotonView.IsMine)
        {
            return;
        }

        if (!other.name.Contains("Beam"))
        {
            return;
        }

        Health -= 0.1f * Time.deltaTime;
    }

    void Update()
    {
        if(PhotonView.IsMine)
        {
            ProcessInputs();

            if (Health <= 0.00f)
                GameManager.Instance.LeaveRoom();

        }

        // trigger Beams active state
        if (beams != null && IsFiring != beams.activeInHierarchy)
        {
            beams.SetActive(IsFiring);
        }

    }

    #endregion


    #region Custom
    void ProcessInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!IsFiring)
            {
                IsFiring = true;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (IsFiring)
            {
                IsFiring = false;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(IsFiring);
            stream.SendNext(Health);
        } else
        {
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
        }
    }

    #endregion
}