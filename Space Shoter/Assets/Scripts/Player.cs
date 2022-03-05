using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //data type(int(1,2,3,..),float(1.2,2.4,5.9),bool(true/false),string(A,B,C,d)
    
      
    [SerializeField]
    private float  _speed = 3.5f;

    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _firerate = 0.2f;
    private float _canfire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldBoostActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score;

    private UIManager _uiManager;



    // Start is called before the first frame update
    void Start()
    {
        //take the current position = new postion
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manger is null");
        }
        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canfire)
        {
            LaserSpawen();
        }
            
    }

    void LaserSpawen()
    {

            _canfire = Time.time + _firerate;

        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);

        }

    }
    void CalculateMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 diraction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(diraction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 3), 0);

        if (transform.position.x >= 7.3f)
        {
            transform.position = new Vector3(-7.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -7.3f)
        {
            transform.position = new Vector3(7.3f, transform.position.y, 0);
        }

    }
    public void Damage()
    {
        if(_isShieldBoostActive == true)
        {
            _isShieldBoostActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        _lives--;

        _uiManager.UpdateLives(_lives);

        if(_lives < 1)
        {
            _spawnManager.PlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerRoutine());
    }
    IEnumerator SpeedBoostPowerRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsBoostActive()
    {
        _isShieldBoostActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
