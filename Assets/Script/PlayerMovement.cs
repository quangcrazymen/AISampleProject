using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.TextCore.Text;

class SteeringOutput
{
    Vector3 linear;
    float angular;
}
public class PlayerMovement : MonoBehaviour
{
    class Static
    {
        Vector3 position;
        float orientation;

    }
    class KinematicSteeringOutput
    {
        Vector3 velocity;
        float rotation;

    }
    // class KinematicSeek
    // {
    // KinematicSeek
    float newOrientation(float current, Vector3 velocity)
    {
        if (velocity.sqrMagnitude > 0)
        {
            return Mathf.Atan2(-transform.position.x, transform.position.z);
        }
        else return current;
    }
    // }
    float maxSpeed;

    // character : static
    // target : static
    KinematicSteeringOuput getSteering()
    {

        result = new KinematicSteeringOutput()
        // Get direction to the target
        result.velocity = target.position - Character.position;
# The velocity is along this direction, at full speed.
        result.velocity.normalize()
            result.velocity *= maxSpeed;
# Face in the direction we want to move.
        character.orientation = newOrientation(
            character.orientation,
                result.velocity)
        result.rotation = 0
        return result
    }

    public float moveSpeed;

    public float jumpForce;

    // Kinemetics
    [SerializeField]
    Vector3 position;
    [SerializeField]
    float orientation;
    [SerializeField]
    Vector3 velocity;
    [SerializeField]
    float rotation;
    private bool canJump;

    // orientation to vector
    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
    }
    // Update is called once per frame
    void Update()
    {
        playerMove();
        moveOverTime();
        // rotationOverTime();
    }
    void rotationOverTime()
    {
        transform.Rotate(Vector3.up);
    }
    void moveOverTime()
    {
        position += velocity * Time.deltaTime;
        transform.Translate(position);
        orientation += rotation * Time.deltaTime;
        transform.Rotate(new Vector3(0, orientation, 0));
    }
    public void playerMove()
    {

        Vector3 Move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Move = Vector3.ClampMagnitude(Move, 1);

        transform.Translate(Move * Time.deltaTime * moveSpeed);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump == true)
            {
                Rigidbody playerRB = transform.GetComponent<Rigidbody>();
                playerRB.AddForce(Vector2.up * jumpForce);
            }

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = false;
        }
    }
}