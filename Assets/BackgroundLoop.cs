using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(0, -1) * 1 * Time.deltaTime);
        float height = this.GetComponent<SpriteRenderer>().bounds.size.y;
        if (transform.position.y <= -2 * height)
        {
            transform.position = new Vector3(transform.position.x, height, 0);
        }
    }
}
