using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SpriteRenderer Cpsprite;
    public Sprite CheckpointOn;
    public Sprite CheckpointOff;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointController.instance.DeactivateCheckpoint();
            Cpsprite.sprite = CheckpointOn;
            CheckpointController.instance.setSpawnPoint(transform.position);
        }
    }

    public void resetChecpoint() 
    {
        Cpsprite.sprite = CheckpointOff;
    }
}
