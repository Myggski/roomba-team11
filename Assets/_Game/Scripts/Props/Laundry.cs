using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laundry : MonoBehaviour
{
   [SerializeField] List<GameObject> laundryVariations = new List<GameObject>();
    GameObject myForm;
    int randomNumber;

    BoxCollider myBoxCollider;

    void Awake() 
    {
        myBoxCollider = GetComponent<BoxCollider>();
    }

    void Start()
    {
        randomNumber = Random.Range(0, 5);
        GameObject myForm = laundryVariations[randomNumber];
        GameObject setForm = Instantiate(myForm, transform.position, Quaternion.identity, transform);

        BoxCollider setFormCollider = setForm.GetComponent<BoxCollider>();

        myBoxCollider.size = setFormCollider.size;
        myBoxCollider.center = setFormCollider.center;

        setFormCollider.enabled = false;

        setForm.transform.parent = transform.GetChild(0).transform;
    }
}
