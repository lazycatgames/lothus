using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmolRock : MonoBehaviour
{

    public TextMesh text;


    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            text.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                
                Collect();
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        text.gameObject.SetActive(false);
    }

    private void Update()
    {
        text.transform.rotation = Player.Instance.transform.rotation;
    }


    void Collect()
    {
        UIManager.Instance.ItemCollected();
        text.gameObject.SetActive(false);
        Inventory.Instance.data.rock++;
        Destroy(gameObject);
    }
}
