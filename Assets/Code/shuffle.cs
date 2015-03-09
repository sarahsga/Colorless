using UnityEngine;
using System.Collections;
using System;

public class shuffle : MonoBehaviour {

   [HideInInspector]
   public bool isShuffling;

   private Actual_Game actual_game_script;
   private time_bar time_bar_script;
   private Vector3[] current;
   private Vector3[] target;
   private int[] target_int;
   private randomness randomness_obj2;
   private int count;
   private ArrayList random_index_list_shuffling;
   private int equality_count;
   private int even_odd;

   void Awake() {
      even_odd = 0;
      actual_game_script = GetComponent<Actual_Game>();
      time_bar_script = actual_game_script.time_bar.GetComponent<time_bar>();
      randomness_obj2 = actual_game_script.randomness_obj;
      isShuffling = false;
      count = 0;
      equality_count = 0;

   }

   void Start() {

   }

	// Update is called once per frame
	void Update () {
      if (time_bar_script.isTimeUp == true && actual_game_script.isOver == false) {

         if (count == 0) {
            count++;
            Debug.Log("sarah: shuffle: isshuffling = false, count == 0");

            actual_game_script.color_display_obj.renderer.enabled = false;

            for( int i = 0 ; i < actual_game_script.greys.Length ; i++ ) {
               actual_game_script.greys[i].renderer.enabled = false;
            }

            int n = 0;
            foreach (int i in randomness_obj2.random_index_list) {
               Debug.Log("shuffle: random_index_list[i] = " + i);
            }
            random_index_list_shuffling = new ArrayList();
            foreach ( int i in randomness_obj2.random_index_list) {
               Debug.Log("shuffle: inside foreach");
               random_index_list_shuffling.Add(i);
               Debug.Log("shuffle: random_index_list_shuffling[" + n +"] = " + (int)random_index_list_shuffling[n]);
               n++;
            }


            while (true) { // making sure that indeed the shuffling happens ( that the new list generated is different from the old one)
               randomness_obj2.random_index_list_gen(actual_game_script.DIFFICULTY);
               Debug.Log("shuffle: new list generated");
               equality_count = 0;
               foreach (int i in random_index_list_shuffling) {
                  if (i == (int)randomness_obj2.random_index_list[i]) {
                     equality_count++;
                  }
                  Debug.Log("shuffle: random_index_list[" + i + "] (new) = " + (int)randomness_obj2.random_index_list[i]);
               }
               Debug.Log("shuffle: equality_count = " + equality_count);
               if (equality_count == actual_game_script.DIFFICULTY)
               {
                  Debug.Log("trying again");
                  continue;
               }
               else {
                  Debug.Log("shuffle: new list generated successfully");
                  break; 
               }
            }

            target = new Vector3[actual_game_script.DIFFICULTY];
            current = new Vector3[actual_game_script.DIFFICULTY];
            target_int = new int[actual_game_script.DIFFICULTY];

            Debug.Log("shuffle: difficulty = " + actual_game_script.DIFFICULTY);
            Debug.Log("shuffle: target.length = " + target.Length);
            Debug.Log("shuffle: current.length = " + current.Length);
            Debug.Log("shuffle: target_int.length = " + target_int.Length);
            Debug.Log("shuffle: colors.length = " + actual_game_script.colors.Length);
            Debug.Log("shuffle: position.length = " + actual_game_script.positions.Length);
            for (int i = 0; i < random_index_list_shuffling.Count; i++) {
               Debug.Log("shuffle: inside for");
               try
               {
                  Debug.Log("shuffle: line 1");
                  current[i] = actual_game_script.colors[(int)random_index_list_shuffling[i]].transform.position;
                  Debug.Log("shuffle: line 2");                  
                  current[i].z = 1;
                  Debug.Log("shuffle: line 3");
                  target_int[i] = randomness_obj2.random_index_list.IndexOf((int)random_index_list_shuffling[i]);
                  Debug.Log("shuffle: line 4.. target_int [i] = " + target_int[i]);
                  target[i].z = 1;
                  Debug.Log("shuffle: line 5");
                  target[i] = actual_game_script.positions[target_int[i]];
                  Debug.Log("shuffle: line 6");
                  Debug.Log("c&t: current[" + i + "] = (" + current[i].x + ", " + current[i].y);
                  Debug.Log("c&t: target[" + i + "] = " + target[i].x + ", " + target[i].y);
               }
               catch (Exception ex) { Debug.Log("shuffle: " + ex.Message); }
            }
         }
         if (even_odd % 2 == 0)
         {
            for (int i = 0; i < actual_game_script.DIFFICULTY; i++)
            {
               current[i] = Vector3.MoveTowards(current[i], target[i], Time.deltaTime*2);
               actual_game_script.colors[(int)random_index_list_shuffling[i]].transform.position = current[i];
            }
         }
         even_odd++;
         for (int i = 0; i < actual_game_script.DIFFICULTY ; i++)
         { // see if shuffling has completed
            if (current[i] == target[i])
            {
               isShuffling = false;
            }
            else
            {
               isShuffling = true;
               break;
            }
         }
         Debug.Log("letsee: isShuffling = " + isShuffling);
         if (isShuffling == false) {
            count = 0;
            time_bar_script.isTimeUp = false;    
         }
      }
	}
}
