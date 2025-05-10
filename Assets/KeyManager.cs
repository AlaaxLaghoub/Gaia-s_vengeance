using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject Door;
    public bool isPickedUp;
    private Vector2 vel;
    public float smoothTime;
    [SerializeField] GameObject bat;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            Vector3 offset = new Vector3(0.3f, 1f, 0f);
            transform.position = Vector2.SmoothDamp(transform.position, player.transform.position + offset, ref vel, smoothTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isPickedUp)
        {
            isPickedUp = true;
            Door.GetComponent<Door>().KeyPickedUp = true;
            bat.SetActive(true);
        }

    }

}
