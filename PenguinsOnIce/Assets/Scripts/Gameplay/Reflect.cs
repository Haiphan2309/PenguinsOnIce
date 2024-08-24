using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> ownRenderedList = new List<SpriteRenderer>();
    [SerializeField] private List<SpriteRenderer> reflectRenderedList = new List<SpriteRenderer>();
    [SerializeField] private Transform ownGraphicTransform, reflectGraphicTransform;

    private void Update()
    {
        for (int i=0; i< ownRenderedList.Count; i++)
        {
            reflectRenderedList[i].sprite = ownRenderedList[i].sprite;
            reflectRenderedList[i].transform.localRotation = Quaternion.Euler(0, 0, ownRenderedList[i].transform.localRotation.z * 180 / 3.14f);
            reflectRenderedList[i].transform.localScale = new Vector3(ownRenderedList[i].transform.localScale.x, ownRenderedList[i].transform.localScale.y);
            reflectRenderedList[i].transform.localPosition = new Vector3(ownRenderedList[i].transform.localPosition.x, ownRenderedList[i].transform.localPosition.y);
        }
       
        reflectGraphicTransform.localRotation = Quaternion.Euler(0, 0, -ownGraphicTransform.localRotation.z*180/3.14f);
        reflectGraphicTransform.localScale = new Vector3(ownGraphicTransform.localScale.x, -ownGraphicTransform.localScale.y/1.5f);
        reflectGraphicTransform.localPosition = new Vector3(ownGraphicTransform.localPosition.x, -ownGraphicTransform.localPosition.y / 1.5f);
    }
}
