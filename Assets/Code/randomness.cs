using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class randomness {

   public int random_number { get; private set; }
   public int seed { get; private set; }
   public System.Random random_obj { get; private set; }
   public ArrayList random_index_list { get; private set; }
   private ArrayList random_helper_list;

	// Use this for initialization
	public randomness() {
      seed = DateTime.Now.Millisecond;
      random_obj = new System.Random(seed);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public void random_index_list_gen(int difficulty) {
      random_index_list = new ArrayList();
      random_helper_list = new ArrayList();

      for (int i = 0; i < difficulty; i++) {
         random_helper_list.Add(i);
         //Debug.Log("random_helper_list[" + i + "] = " + (int)random_helper_list[i]);
      }

      for (int i = 0; i < difficulty; i++) {
         random_number = random_obj.Next(0,difficulty - i);
         //Debug.Log("random_number = " + random_number);
         random_index_list.Add(random_helper_list[random_number]);
         random_helper_list.RemoveAt(random_number);
      }

      for (int i = 0; i < difficulty; i++)
      {
         //Debug.Log("random_index_list[" + i + "] = " + (int)random_index_list[i]);
      }
   }

   public int random_color_gen(int minValue, int maxValue) {
      random_number = random_obj.Next(minValue, maxValue);
      return random_number;
   }

}
