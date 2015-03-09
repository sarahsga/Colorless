using UnityEngine;
using System.Collections;

public class stay : MonoBehaviour {

   [HideInInspector]
   public int level;

   void Awake() {
      level = -5; // set to -5 for no reason
   }
   
}
