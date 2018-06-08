using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float rocketThrust = 15f;
    [SerializeField] private float levelLoadDelay = 2.5f;

    [SerializeField] private AudioClip _mainEngine;
    [SerializeField] private AudioClip _rocketExplosion;
    [SerializeField] private AudioClip _levelComplete;

    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem rocketExplosionParticles;
    [SerializeField] private ParticleSystem levelCompleteParticles;

    private enum rocketStates
    {
        Alive,
        Dying,
        Transcending

    };

    private rocketStates _currentState = rocketStates.Alive;


    private int _currentLevel;
    private bool _spaceKeyPressed;
    private bool _aKeyPressed;
    private bool _dKeyPresssed;
    private Rigidbody _rocketRB;
    private AudioSource _audioSource;

	// Use this for initialization
	void Start ()
	{
	    _rocketRB = GetComponent<Rigidbody>();
	    _audioSource = GetComponent<AudioSource>();
	    _rocketRB.mass = 1;
        _currentLevel = SceneManager.GetActiveScene().buildIndex;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (_currentState == rocketStates.Alive)
	    {
	        ProcessInput();
	    }
	    
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (_currentState != rocketStates.Alive)
        {
            // ignore collisions
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
            {
                // Do Nothing
            }
            break;

            case "Finish":
            {
                StartSuccessSequence();
            }
            break;

            default:
            {
                StartDeathSequence();
            }
            break;
        }
    }


    private void StartSuccessSequence()
    {
        _currentState = rocketStates.Transcending;
        _audioSource.PlayOneShot(_levelComplete);
        Invoke("CheckLevelIndex", levelLoadDelay); // parameterize time.
        levelCompleteParticles.Play();
    }

    private void StartDeathSequence()
    {
        _currentState = rocketStates.Dying;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_rocketExplosion);
        rocketExplosionParticles.Play();
        Invoke("ReloadCurrentLevel", levelLoadDelay);
    }

    private void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(_currentLevel);
    }

    private void CheckLevelIndex()
    {
        int levelCount = SceneManager.sceneCount;
        if (_currentLevel < levelCount )
        {
            SceneManager.LoadScene(_currentLevel + 1);
        }
        else
        {
            LoadFirstLevel();
        }
       
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }


    private void ProcessInput()
    {
        _spaceKeyPressed = Input.GetKey(KeyCode.Space);
        _aKeyPressed = Input.GetKey(KeyCode.A);
        _dKeyPresssed = Input.GetKey(KeyCode.D);

        RespondToThrustInput();
        RespondToRotateInput();
    }

    private void RespondToThrustInput()
    {
        if (_spaceKeyPressed)
        {
            ApplyThrust();
        }

        else
        {
            _audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        _rocketRB.AddRelativeForce(Vector3.up * rocketThrust);
        if (!_audioSource.isPlaying) 
        {
            _audioSource.PlayOneShot(_mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        _rocketRB.freezeRotation = true; // take manual control of rotation.
        float rotationThisFrame = Time.deltaTime * rcsThrust;

        if (_aKeyPressed)
        {
            transform.Rotate(new Vector3(0, 0, 2 * rotationThisFrame), Space.World);
        }
        else if (_dKeyPresssed)
        {
            transform.Rotate(new Vector3(0, 0, -2 * rotationThisFrame), Space.World);
        }

        _rocketRB.freezeRotation = false; // resume physics' control of rotation.
    }

}
