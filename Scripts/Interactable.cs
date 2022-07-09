using System;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public float radius = 0.6f;
    public string interactableText;
    public Image itemIcon;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public virtual void Interact(PlayerManager playerManager)
    {
        Debug.Log("интерактивниал с обектом!!!");
    }
}