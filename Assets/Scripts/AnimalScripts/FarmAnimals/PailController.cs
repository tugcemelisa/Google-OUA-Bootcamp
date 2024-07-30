using UnityEngine;

public class PailController : MonoBehaviour
{
    //private Transform player;
    //private Transform liquidTransform;
    //private Rigidbody kovaRigidbody;
    //public float swayAmount = 0.5f;
    //public float spillThreshold = 20f;

    //void Start()
    //{
    //    player = transform.GetComponentInParent<Transform>();
    //    if (kovaRigidbody == null)
    //    {
    //        kovaRigidbody = GetComponent<Rigidbody>();
    //    }
    //}

    //void Update()
    //{
    //    // Player hareketine baðlý olarak kova sallanmasý
    //    Vector3 sway = new Vector3(Mathf.Sin(Time.time) * swayAmount, 0, Mathf.Cos(Time.time) * swayAmount);
    //    kovaRigidbody.MovePosition(player.position + player.forward + sway);

    //    // Sývýnýn dökülmesi kontrolü
    //    //if (Vector3.Angle(Vector3.up, transform.up) > spillThreshold)
    //    //{
    //    //    SpillLiquid();
    //    //}
    //}

    //void SpillLiquid()
    //{
    //    // Sývýnýn dökülmesi
    //    liquidTransform.localScale = new Vector3(liquidTransform.localScale.x, Mathf.Max(0, liquidTransform.localScale.y - Time.deltaTime), liquidTransform.localScale.z);
    //}


}
