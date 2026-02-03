using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private Transform decalProjector;
    [SerializeField] private LayerMask groundLayer;
    private float maxDistance = 200f;

    private Vector3 vector = new Vector3(0,5,0);

    void LateUpdate()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, groundLayer))
        {
            if (!decalProjector.gameObject.activeSelf)
                decalProjector.gameObject.SetActive(true);

            decalProjector.position = hit.point + vector;

            //decalProjector.rotation = Quaternion.Euler(90, 0, 0);

        }
        else
        {
            if (decalProjector.gameObject.activeSelf)
                decalProjector.gameObject.SetActive(false);
        }
    }

}
