using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public int Speed = 10;
    public float RotationSpeed = 0.1f;
    public int DetectionDistance = 25;
    public int BulletVelocity = 1000;
    public GameObject GunGameObject;
    public GameObject Bullet;
    public int GetHP { get { return HP; } }

    private int HP = 100;
    private int Stamina = 100;

    private Rigidbody _rigidbody;
    private GameObject Player;
    private Ray _detectionRay;

    private Vector3 _directionToPlayer;
    private Vector3 _moveDirection;
    private Quaternion _rotateDirection;


    private Ray _gunRay;
    private RaycastHit _hit;
    private RaycastHit _PlayerDetectionHit;

    private int _timer = 0;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        _directionToPlayer = Player.transform.position - GunGameObject.transform.position;
        PlayerDetection();

        if (_timer > 10)
        {
            Gun();
            _timer = 0;
        }      
        _timer++;
        Debug.DrawLine(GunGameObject.transform.position, Player.transform.position, Color.black);

        Rotate();
        Movement();
    }
    private void Movement()
    {
        _rigidbody.AddForce(_moveDirection);
    }
    private void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _rotateDirection, RotationSpeed);
    }
    private void PlayerDetection()
    {
        _detectionRay = new(transform.position, _directionToPlayer);

        if (Physics.Raycast(_detectionRay, out _PlayerDetectionHit, DetectionDistance) 
            && _PlayerDetectionHit.transform.gameObject.CompareTag("Player"))
        {
            _moveDirection = _directionToPlayer.normalized * Speed;
            _rotateDirection = Quaternion.LookRotation(_directionToPlayer, Vector3.up);
        }
        else
        {
            _moveDirection = Vector3.zero;
            _rotateDirection = new(0,0,0,0);
        }
    }
    private void Gun()
    {
        GunGameObject.transform.rotation = Quaternion.LookRotation(_directionToPlayer);

        _gunRay = new(GunGameObject.transform.position, _directionToPlayer);

        if (Physics.Raycast(_gunRay, out _hit, DetectionDistance) && _hit.transform.gameObject.CompareTag("Player"))
        {
            GameObject _tempBullet;
            _tempBullet = Instantiate(Bullet);

            _tempBullet.transform.SetPositionAndRotation(
                GunGameObject.transform.position + GunGameObject.transform.forward, 
                Quaternion.LookRotation(_gunRay.direction));

            _tempBullet.GetComponent<Rigidbody>().AddForce(GunGameObject.transform.forward * BulletVelocity);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            GetDamage(1);
        }

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void DealDamage(Hero hero)
    {
        hero.GetDamage(1);
    }
    public void GetDamage(int damage)
    {
        HP -= damage;
    }
}
/*
if ( Physics.SphereCast(transform.position, DetectionDistance, Player.transform.position - transform.position, out _PlayerDetectionHit) )
{
    _moveDirection = (Player.transform.position - transform.position).normalized * Speed;
    Debug.Log("Detected Player");
}
else if ( (Player.transform.position - transform.position).magnitude < 10)
{
    _moveDirection = Vector3.zero;
}
*/