using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hero : MonoBehaviour
{
    public int Speed = 5;
    public int BulletVelocity = 150;
    public GameObject GunGameObject;
    public GameObject Bullet;

    private int HP = 100;
    private int Stamina = 100;
    public int GetCurrentStamina { get { return Stamina; } }
    public int GetCurrentHp { get { return HP; } }

    private Rigidbody _rigidbody;
    private float _axisX, _axisY;
    private Vector3 _movementDirection;
    private Ray CameraRay, GunRay;

    private bool IsLeftMouseDown = false;
    private bool _isPlayerDead = false;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            IsLeftMouseDown = true;
        }else
        {
            IsLeftMouseDown = false;
        }
    }
    private void FixedUpdate()
    {
        Movement();
        Gun();
    }
    private void Movement()
    {
        _axisX = Input.GetAxis("Horizontal") * Speed;
        _axisY = Input.GetAxis("Vertical") * Speed;
        _movementDirection = transform.forward * _axisY + transform.right * _axisX;
        _rigidbody.AddForce(_movementDirection);
    }
    private void Gun()
    {
        CameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        GunRay = new(GunGameObject.transform.position, GunGameObject.transform.forward);
        GunGameObject.transform.rotation = Quaternion.LookRotation(CameraRay.direction);

        if (IsLeftMouseDown)
        {
            GameObject _tempBullet;
            _tempBullet = Instantiate(Bullet);

            _tempBullet.transform.SetPositionAndRotation( GunGameObject.transform.position + GunGameObject.transform.forward, Quaternion.LookRotation(GunRay.direction) );

            _tempBullet.GetComponent<Rigidbody>().AddForce(GunGameObject.transform.forward * BulletVelocity);
        }
        Debug.DrawRay(GunRay.origin, GunRay.direction, Color.red);
    }
    public void GetDamage(int damage)
    {
        HP -= damage;
    }
    private void IsDead()
    {
        if (HP <= 0)
        {
            _isPlayerDead = true;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            GetDamage(1);
        }
        IsDead();
    }
}