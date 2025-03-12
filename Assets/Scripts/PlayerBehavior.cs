using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    // Referencia al gestor del juego
    private GameBehavior _gameManager;

    // Velocidades de movimiento y rotación
    public float MoveSpeed = 10f;
    public float RotateSpeed = 75f;

    // Entradas del jugador
    private float _vInput;
    private float _hInput;

    // Referencia al Rigidbody para la física
    private Rigidbody _rb;

    // Variables para el salto
    public float JumpVelocity = 5f;
    private bool _isJumping;

    // Variables para detectar el suelo
    public float DistanceToGround = 0.1f;
    public LayerMask GroundLayer;
    private CapsuleCollider _col;

    // Variables para el disparo
    public GameObject Bullet;
    public float BulletSpeed = 100f;
    private bool _isShooting;

    void Start()
    {
        // Obtiene el Rigidbody y el Collider del jugador
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();

        // Obtiene la referencia al gestor del juego
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameBehavior>();
    }

    void Update()
    {
        // Captura la entrada del jugador para mover y rotar
        _vInput = Input.GetAxis("Vertical") * MoveSpeed;
        _hInput = Input.GetAxis("Horizontal") * RotateSpeed;

        // Detecta si el jugador presiona espacio para saltar
        _isJumping |= Input.GetKeyDown(KeyCode.Space);

        // Detecta si el jugador presiona click para disparar
        _isShooting |= Input.GetKeyDown(KeyCode.Mouse0);
    }

    // Detecta colisiones con enemigos
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy")
        {
            _gameManager.HP -= 1; // Reduce la vida del jugador
        }
    }

    void FixedUpdate()
    {
        // Si el jugador está en el suelo y presiona saltar
        if (_isJumping && IsGrounded())
        {
            _rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
        }
        _isJumping = false; // Resetea la acción de salto

        // Si el jugador presiona disparar
        if (_isShooting)
        {
            // Crea una nueva bala en frente del jugador
            GameObject newBullet = Instantiate(Bullet, this.transform.position + new Vector3(0, 0, 1), this.transform.rotation);
            Rigidbody BulletRB = newBullet.GetComponent<Rigidbody>();

            // Aplica velocidad a la bala en la dirección del jugador
            BulletRB.velocity = this.transform.forward * BulletSpeed;
        }
        _isShooting = false; // Resetea la acción de disparo

        // Manejo del movimiento y rotación
        Vector3 rotation = Vector3.up * _hInput;
        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        _rb.MovePosition(this.transform.position + this.transform.forward * _vInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * angleRot);
    }

    // Método para verificar si el jugador está en el suelo
    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);

        // Comprueba si el jugador está tocando el suelo
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, DistanceToGround, GroundLayer, QueryTriggerInteraction.Ignore);

        return grounded;
    }
}
