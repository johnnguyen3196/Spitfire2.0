using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBubble : MonoBehaviour
{
    public GameObject target;
    //Textbubble will stay up for 3 seconds
    private float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        gameObject.transform.position = screenPos;
        lifetime = Time.time + 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > lifetime || target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        screenPos.x += 50;
        screenPos.y += 50;
        gameObject.transform.position = screenPos;
    }

    public void UpdateText(string text)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
}
