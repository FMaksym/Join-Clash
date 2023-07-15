using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float _playerHorizontalSpeed;
    [SerializeField] private float _velocity;
    [SerializeField] private float _swipeSensetive;
    [SerializeField] private float _roadSpeed;
    [SerializeField] private Transform _road;
    public bool MoveTouch;
    public bool IsGame;
    public bool Attack;
    private Vector3 _direction;

    public List<Rigidbody> _rigidbodyList = new List<Rigidbody>();
    public List<Rigidbody> RigidbodyList { get; private set; }
    public BossManager BossManager { get; private set; }
    public EventManager EventManager { get; private set; }
    [Inject] private EventManager eventManager;

    private void Awake()
    {
        EventManager = eventManager;
        _rigidbodyList.Add(transform.GetChild(0).GetComponent<Rigidbody>());
        RigidbodyList = _rigidbodyList;
    }

    private void Start()
    {
        BossManager = GameObject.FindObjectOfType<BossManager>();
        IsGame = true;
    }

    private void Update()
    {
        if (IsGame)
        {
            //MoveInput();
            if (MoveInput())
            {
                _direction = new Vector3(Mathf.Lerp(_direction.x, Input.GetAxis("Mouse X"), _playerHorizontalSpeed * Time.deltaTime), 0f);
                _direction = Vector3.ClampMagnitude(_direction, 1f);

                _road.position = new Vector3(0f, 0f, Mathf.SmoothStep(_road.position.z, -100f, _roadSpeed * Time.deltaTime));
                SetFloatParametres(RigidbodyList, "Run", 1);
            }
            else
            {
                SetFloatParametres(RigidbodyList, "Run", 0);
            }

            foreach (var _playerRigidbody in RigidbodyList)
            {
                if (_playerRigidbody.velocity.magnitude > 0.5f)
                {
                    _playerRigidbody.rotation = Quaternion.Slerp(_playerRigidbody.rotation, Quaternion.LookRotation(_playerRigidbody.velocity, Vector3.up), Time.deltaTime * (_velocity / 2));
                }
                else
                {
                    _playerRigidbody.rotation = Quaternion.Slerp(_playerRigidbody.rotation, Quaternion.identity, Time.deltaTime * _velocity);
                }
            }
        }
        else
        {
            if (!BossManager.BossIsAlive)
            {
                EventManager.PlayerWin(RigidbodyList);
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsGame)
        {
            if (MoveInput())
            {
                Vector3 Displacement = new Vector3(_direction.x, 0f, 0f) * Time.fixedDeltaTime;
                foreach (var _playerRigidbody in RigidbodyList)
                {
                    _playerRigidbody.velocity = new Vector3(_direction.x * Time.fixedDeltaTime * _swipeSensetive, 0f, 0f) + Displacement;
                }
            }
            else
            {
                foreach (var _playerRigidbody in RigidbodyList)
                {
                    _playerRigidbody.velocity = Vector3.zero;
                }
            }
        }
    }

    private bool MoveInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveTouch = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            MoveTouch = false;
        }
        return MoveTouch;
    }

    public void SetFloatParametres(List<Rigidbody> list, string name, float value)
    {
        foreach (var element in list)
        {
            if (element != null && element.gameObject.activeSelf)
                element.GetComponent<Animator>().SetFloat(name, value);
        }
    }

    public void SetBoolParametres(List<Rigidbody> list, string name, bool value)
    {
        foreach (var element in list)
        {
            if (element != null && element.gameObject.activeSelf)
                element.GetComponent<Animator>().SetBool(name, value);
        }
    }

    public void SetRandomFloatParametres(List<Rigidbody> list, string name, float valueMin, float valueMax)
    {
        foreach (var element in list)
        {
            if (element != null && element.gameObject.activeSelf)
                element.GetComponent<Animator>().SetFloat(name, Random.Range(valueMin, valueMax));
        }
    }
}
