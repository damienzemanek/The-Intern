using System;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float dist = 5f;
    [Required] public GameObject go_xhair;
    [Required] public GameObject go_Int;
    
    
    private void Update()
    {
        go_xhair.gameObject.SetActive(true);
        go_Int.SetActive(false);
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit,
                dist)) return;
        if (!hit.collider.gameObject.CompareTag("INT")) return;
        go_Int.SetActive(true);
        go_xhair.gameObject.SetActive(false);
    }
}
