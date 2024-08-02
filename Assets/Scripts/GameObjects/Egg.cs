using System.Collections;
using UnityEngine;

public class Egg : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(50.0f);
        Destroy(gameObject);
    }
}
