using UnityEngine;

public class Scissor : MonoBehaviour
{
    [SerializeField] float speed = 150f;
    [SerializeField] float maxAngle = 75f;
    [SerializeField] Transform right;
    [SerializeField] Transform left;
    private float rot = 0;
    // Update is called once per frame
    void Update()
    {

        rot += speed * Time.deltaTime;

        right.transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0));
        left.transform.rotation = Quaternion.Euler(new Vector3(0, -rot, 0));

        if (speed > 0 && rot >= maxAngle)
        {

            speed *= -1;
            rot += 2 * speed * Time.deltaTime;
        }
        else if (speed < 0 && rot <= -maxAngle)
        {
            speed *= -1;
            rot += 2 * speed * Time.deltaTime;
        }
    }
}
