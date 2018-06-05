using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    //TODO fix lighting bug.
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float rocketThrust = 15f;

    private enum rocketStates
    {
        Alive,
        Dying,
        Transcending

    };

    private rocketStates _currentState = rocketStates.Alive;


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
}
	
	// Update is called once per frame
	void Update ()
	{
        //TODO stop sound on death
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
                
            }
            break;

            case "Finish":
            {
                _currentState = rocketStates.Transcending;
                Invoke("CheckLevelIndex", 1f); // parameterize time.
            }
            break;

            default:
            {
                print("Hit something deadly");
                _currentState = rocketStates.Dying;
                Invoke("ReloadCurrentLevel", 1f);
            }
            break;
        }
    }

    private void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void CheckLevelIndex()
    {
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCount)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (_spaceKeyPressed)
        {
            _rocketRB.AddRelativeForce(Vector3.up * rocketThrust);
            if (!_audioSource.isPlaying) // so it do
            {
                _audioSource.Play();
            }
        }

        else
        {
            _audioSource.Stop();
        }
    }

    private void Rotate()
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
