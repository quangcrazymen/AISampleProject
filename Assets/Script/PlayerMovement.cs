using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.TextCore.Text;

class Static
{
    public Vector3 position;
    public float orientation;
}

public class SteeringOutput
{
    public Vector3 linear;
    public float angular;
}
class Kinemetics
{

    Vector3 position;
    float orientation;
    Vector3 velocity;
    float rotation;

    void Update(SteeringOutput steering, float time)
    {
        // Update the position and orientation.
        position += velocity * time;
        orientation += rotation * time;
        // and the velocity and rotation.
        velocity += steering.linear * time;
        rotation += steering.angular * time;
    }
}
class KinematicSteeringOutput
{
    public Vector3 velocity;
    public float rotation;

}
public class KinematicSeek : PlayerMovement
{

    float newOrientation(float current, Vector3 velocity)
    {
        if (velocity.sqrMagnitude > 0)
        {
            return Mathf.Atan2(-transform.position.x, transform.position.z);
        }
        else
            return current;
    }

    float maxSpeed;

    Static character;
    Static target;
    KinematicSteeringOutput getSteering()
    {

        var result = new KinematicSteeringOutput();
        // Get direction to the target
        result.velocity = target.position - character.position;
        // The velocity is along this direction, at full speed.
        result.velocity = result.velocity.normalized;
        result.velocity *= maxSpeed;
        // Face in the direction we want to move.
        character.orientation = newOrientation(character.orientation, result.velocity);
        result.rotation = 0;
        return result;
    }
}

public class PlayerMovement : MonoBehaviour
{
    public Vector3 orientationToDirectionalVector(float orientation)
    {
        return new Vector3(Mathf.Sin(orientation), Mathf.Cos(orientation), 0.0f);
    }

    public Vector3 getPositionOfEnemy()
    {
        GameObject Enemy = GameObject.Find("Enemy");
        // return new Vector3(Enemy.position.x, Enemy.position.y, Enemy.position.z);
        return Enemy.transform.position;
    }

    [SerializeField]
    [OnChangedCall("onSerializedPropertyChange")]
    private float orientation;
    public void onSerializedPropertyChange()
    {
        // transform.localScale = new Vector3(_size, _size, _size);
        Debug.LogFormat("Vector for orientation is Vector({0},{1},0)", orientationToDirectionalVector(orientation).x,
                            orientationToDirectionalVector(orientation).y);
        Debug.Log(getPositionOfEnemy());
    }

    public float moveSpeed;

    public float jumpForce;

    [SerializeField]
    private bool canJump;

    // orientation to vector
    // Start is called before the first frame update
    Vector3 enemyInitialPosition;
    void Start()
    {
        canJump = true;
        enemyInitialPosition = getPositionOfEnemy();
    }
    // Update is called once per frame
    void OnEnemyPositionChanged(){
        if(enemyInitialPosition != getPositionOfEnemy()){
            enemyInitialPosition = getPositionOfEnemy();
            Debug.Log("Current enemy position is:" + getPositionOfEnemy());
        }
    }

    void Update()
    {
        playerMove();
        // moveOverTime();
        // rotationOverTime();
        OnEnemyPositionChanged();
    }
    void rotationOverTime()
    {
        transform.Rotate(Vector3.up);
    }
    // public void moveOverTime()
    // {
    //     position += velocity * Time.deltaTime;
    //     transform.Translate(position);
    //     orientation += rotation * Time.deltaTime;
    //     transform.Rotate(new Vector3(0, orientation, 0));
    // }
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
                Debug.Log("JUMP");
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