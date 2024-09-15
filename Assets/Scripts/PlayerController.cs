using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private UIManager uiManager;     // Référence au UIManager
    private GameManager gameManager;

    public float speed = 10f;  // Vitesse d'avancement
    private float laneDistance = 3f;  // Distance entre les voies
    public int actualLane = 1;
    private float distanceTraveled;  // Distance parcourue

    public AudioClip startSound;
    public AudioClip crashSound;
    public AudioClip powerupSound;
    public AudioClip nitro;
    public AudioClip hornSound;

    private AudioSource playerAudio;

    public ParticleSystem fireLeftParticule;
    public ParticleSystem fireRightParticule;

    public Camera cam1;
    public Camera cam2;
    public Camera cam3;
    public Camera activeCam;

    void Start()
    {
        // Trouver l'UIManager dans la scène
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerAudio = GetComponent<AudioSource>();
        playerAudio.PlayOneShot(startSound);
        activeCam = cam1;
    }

    void Update()
    {
        if (gameManager.isGameActive)
        {
            Move();
            Horn();
        }

        CameraChange();
        
        // Mouvement automatique vers l'avant
        transform.Translate(speed * Time.deltaTime * UnityEngine.Vector3.forward);

        // Calculer la distance parcourue
        distanceTraveled = UnityEngine.Vector3.Distance(new(0, 0, 0), transform.position);
        uiManager.UpdateScore(distanceTraveled);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            uiManager.UpdateCollectibles();
            speed++;
            playerAudio.PlayOneShot(powerupSound);
            playerAudio.PlayOneShot(nitro);
            fireLeftParticule.Play();
            fireRightParticule.Play();
            Destroy(other.gameObject);

            if (gameManager.obstacleSpawnRate > gameManager.collectibleSpawnRate / 2)
            {
                gameManager.obstacleSpawnRate -= 0.1f;
            }
        }

        if (other.CompareTag("Obstacle"))
        {
            playerAudio.PlayOneShot(crashSound);
            gameManager.GameOver();
        }
    }

    void Move()
    {
        // Détection des entrées pour les mouvements latéraux
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (actualLane == 1)
            {
                actualLane = 0;
            }

            else if (actualLane == 2)
            {
                actualLane = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (actualLane == 0)
            {
                actualLane = 1;
            }

            else if (actualLane == 1)
            {
                actualLane = 2;
            }
        }

        // On se deplace dans la bonne voie
        if (actualLane == 0)
        {
            UnityEngine.Vector3 targetPosition = new (-laneDistance, 0, transform.position.z);
            float step = speed * Time.deltaTime;
            transform.position = UnityEngine.Vector3.MoveTowards(transform.position, targetPosition, step);
        }

        if (actualLane == 2)
        {
            UnityEngine.Vector3 targetPosition = new (laneDistance, 0, transform.position.z);
            float step = speed * Time.deltaTime;
            transform.position = UnityEngine.Vector3.MoveTowards(transform.position, targetPosition, step);
        }

        if (actualLane == 1)
        {
            UnityEngine.Vector3 targetPosition = new (0, 0, transform.position.z);
            float step = speed * Time.deltaTime;
            transform.position = UnityEngine.Vector3.MoveTowards(transform.position, targetPosition, step);
        }
    }

    void Horn()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerAudio.PlayOneShot(hornSound);
        }
    }

    void CameraChange()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (cam1 == activeCam)
            {
                cam1.enabled = false;
                cam2.enabled = true;
                activeCam = cam2;
            }

            else if (cam2 == activeCam)
            {
                cam2.enabled = false;
                cam3.enabled = true;
                activeCam = cam3;
            }

            else if (cam3 == activeCam)
            {
                cam3.enabled = false;
                cam1.enabled = true;
                activeCam = cam1;
            }
        }
    }
}
