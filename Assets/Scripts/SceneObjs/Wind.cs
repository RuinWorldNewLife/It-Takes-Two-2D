using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField]
    Vector2 WindDirection;
    [SerializeField]
    private float WindVelocity;
    private Collider2D windCollider;
    public Animator Anim { get; private set; }
    [Header("风暴对角色的力")]
    public float windForceToPlayer;
    private Vector3 stormAddForceToPlayer;
    public LayerMask windTouch2Stop;

    [SerializeField]
    private LayerMask windAddTorceObj;//触碰红角色
    

    //[SerializeField]
    //private LayerMask windAddTorceObjForDark;//触碰黑色角色

    private Player player;


    private void Awake()
    {
        
        Anim = GetComponent<Animator>();
        Anim.SetBool("wind", false);

        //StartCoroutine(GetPlayerRea());
    }

    
    private void Start()
    {
        windCollider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        transform.Translate(new Vector3(WindDirection.x, WindDirection.y, 0) * WindVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (windCollider.IsTouchingLayers(windTouch2Stop))
        {
            OnWindStop();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (windCollider.IsTouchingLayers(windAddTorceObj))
        {
            try
            {
                player = collision.gameObject.GetComponent<Player>();
                stormAddForceToPlayer = (transform.position - collision.transform.position).normalized;
                player.RB.AddForce(stormAddForceToPlayer * windForceToPlayer);
            }
            catch (System.Exception)
            {
                
            }
            
        }
    }
    public void OnWind()
    {
        Anim.SetBool("wind", true);
    }
    public void OnWindStop()
    {
        Anim.SetBool("wind", false);
    }
    public void OnWindDestory()
    {
        //网络销毁物体
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
