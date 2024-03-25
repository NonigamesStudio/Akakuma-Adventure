using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainControl : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField]private int offsetRainY = 7;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offsetRainY, player.transform.position.z);
    }
}
