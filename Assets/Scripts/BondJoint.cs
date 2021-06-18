using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondJoint : MonoBehaviour
{
    [SerializeField] private PlayersBond bond = null;
    void Update()
    {
        transform.position = bond.transform.position;
    }
}
