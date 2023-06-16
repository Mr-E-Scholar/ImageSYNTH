using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlterColor : MonoBehaviour
{
    [SerializeField] Button colorButton;
    [SerializeField] GameObject canvas;
    private bool flop = false;
    // Start is called before the first frame update
    void Start()
    {
        colorButton.onClick.AddListener (Alter);
    }

    // Update is called once per frame
    void Update()
    {
        if (flop)
        {
            canvas.SetActive(true);
        }
        else if (!flop)
        {
            canvas.SetActive(false);
        }
    }

    public void Alter()
    {
        flop=!flop;
    }
}
