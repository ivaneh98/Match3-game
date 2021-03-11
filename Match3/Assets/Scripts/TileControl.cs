using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileControl : MonoBehaviour
{

    public string name;
    public bool isSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnMouseDown()
    {
        isSelected = true;
    }
    public void Unselect()
    {
        isSelected = false;

    }
    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f);
        }

    }

}
