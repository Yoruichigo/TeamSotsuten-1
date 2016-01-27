using UnityEngine;
using System.Collections;

public class testLine : MonoBehaviour
{

    public GameObject Particle_prefab;
    private GameObject Particle;

    public float DeleteTime;

    void Start()
    {
        //Particle_prefab = (GameObject)Resources.Load("Particles/Trail_Star");
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 screenPosition = new Vector3(Input.mousePosition.x,
                                                 Input.mousePosition.y,
                                                 Camera.main.nearClipPlane + 1.0f);
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(screenPosition);

            Vector3 vecHeartPos = gameObject.transform.position;
            Particle = (GameObject)Instantiate(Particle_prefab, vecHeartPos, Quaternion.identity);
            // 10秒後に消す
            Destroy(Particle, DeleteTime);
        }
    }
}