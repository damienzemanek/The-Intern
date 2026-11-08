using System;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Required] public GameObject go_xhair;
    [Required] public GameObject go_Int;
    
    
    private void Update()
    {
        go_xhair.gameObject.SetActive(true);
        go_Int.SetActive(false);
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit,
                10)) return;
        if (!hit.collider.gameObject.CompareTag("INT")) return;
        go_Int.SetActive(true);
        go_xhair.gameObject.SetActive(false);
    }
}
