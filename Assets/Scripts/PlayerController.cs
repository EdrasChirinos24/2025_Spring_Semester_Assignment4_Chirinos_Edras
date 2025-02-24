using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;
    public float jumpForce = 10;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //obtains the rigidbody component
        playerRb = GetComponent<Rigidbody>();

        playerAnim = GetComponent<Animator>();

        //sets the rate of gravity
        Physics.gravity *= gravityModifier;

        playerAudio = GetComponent <AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //causes player to jump when space is pressed and sets isOnGround to false.
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            dirtParticle.Stop();

            //trigger jump animation
            playerAnim.SetTrigger("Jump_trig");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        //sets isOnGround to true when player hits the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        }

        //sets a game over when player collides with an obstacle
        else if (collision.gameObject.CompareTag("Obstacle"))
        {           
            Debug.Log("Game Over!");
            gameOver = true;

            playerAudio.PlayOneShot(crashSound, 1.0f);
            explosionParticle.Play();
            dirtParticle.Stop();

            //triggers death animation
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
        }
    }
}
