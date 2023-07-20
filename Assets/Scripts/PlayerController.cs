using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    public float speed = 5f, rotateSpeed = 4f, jumpForce = 5f;
    private PhotonView _photonView;
    public int _attackDamage = 25;
    private int _health = 100;

    public float horizontalSpeed = 1f;
    public float verticalSpeed = 1f;
    private Camera cam;

    public new AudioSource audio;

    public TMP_InputField text;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
        audio = GetComponent<AudioSource>();

        if (!_photonView.IsMine)
            Destroy(GetComponentInChildren<Camera>().gameObject);

        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (!_photonView.IsMine) return;
        
        MovePlayer();
    }
    private void MovePlayer()
    {
        Vector3 totalMovement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            totalMovement -= transform.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            totalMovement += transform.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            totalMovement -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            totalMovement += transform.right;
        }
        if (totalMovement != new Vector3(0f, 0f, 0f) && !audio.isPlaying)
        {
            audio.Play();
        }
        else if(totalMovement == new Vector3(0f, 0f, 0f) && audio.isPlaying)
        {
            audio.Stop();
        }
        _rb.MovePosition(transform.position + totalMovement.normalized * speed * Time.fixedDeltaTime);

        _photonView = GetComponent<PhotonView>();
    }

    private void RotatePlayer()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Input.GetAxis("Mouse X"));
    }

    private void Update()
    {
        if (!_photonView.IsMine) return;

        RotatePlayer();

        Shoot();
    }

    private void Shoot()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Camera cam = transform.GetChild(0)?.GetComponent<Camera>();
            
            Vector3 ray = cam.transform.position;
            Vector3 way = cam.transform.forward;

            if (Physics.Raycast(ray, way, out RaycastHit hitPlayer))
            {
                if (hitPlayer.collider.CompareTag("Player"))
                {
                    hitPlayer.collider.GetComponent<PlayerController>().Damage(_attackDamage);
                    hitPlayer.collider.GetComponent<PlayerController>().Text("Hello");
                    hitPlayer.collider.GetComponent<Rigidbody>().velocity = new Vector3(0f, 2f, 0f);
                }
            }

            if (Physics.Raycast(ray, way, out RaycastHit hitObject))
            {
                if (hitObject.collider.CompareTag("Object"))
                {
                    if(hitObject.point.x == 0) PhotonNetwork.Instantiate("BOOM", hitObject.point, Quaternion.identity);
                    else PhotonNetwork.Instantiate("BOOM", hitObject.point, Quaternion.identity);
                }
            }
        }
    }

    public void Damage(int damage)
    {
        _photonView.RPC("PunDamage", RpcTarget.All, damage);
    }

    public void Text(string text)
    {
        _photonView.RPC("UpdateText", RpcTarget.All, text);
    }

    [PunRPC]
    void PunDamage(int damage)
    {
        if(!_photonView.IsMine) return;

        _health -= damage;
        if (_health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void UpdateText(string text)
    {
        if (!_photonView.IsMine) return;

        this.text.text = text;
    }
}
