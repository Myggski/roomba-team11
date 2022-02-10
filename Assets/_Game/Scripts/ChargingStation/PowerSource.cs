using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour
{
   [SerializeField] private Batteries _battery;
   public Batteries Battery => _battery;
}
