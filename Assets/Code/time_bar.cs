using UnityEngine;
using System.Collections;

public class time_bar : MonoBehaviour {

   private float width;
   private float time_passed = 0;
   private Vector3 newScale;
   public const int TIME_INIT = 30;
   private level_common level_common_script;
   public GameObject MainCamera;
   
   [HideInInspector]
   public bool isTimeUp;

   [HideInInspector]
   public shuffle shuffle_script;

   private Actual_Game actual_game_script;
   private text_repeat text_repeat_script;

   void Awake() {
      level_common_script = MainCamera.GetComponent<level_common>();
      shuffle_script = MainCamera.GetComponent<shuffle>();
      text_repeat_script = MainCamera.GetComponent<text_repeat>();
      actual_game_script = MainCamera.GetComponent<Actual_Game>();
      isTimeUp = false;
   }

   void Start() { 
   }
	// Update is called once per frame
	void Update () {
      if (level_common_script.justStarted == false && text_repeat_script.isReady == true && actual_game_script.isOver == false) { // i.e. has started && isn't paused
         if (time_passed == 0) {
            isTimeUp = false;
            //Debug.Log("sarah : timebar is enabled .. so inside Update().. time_passed == 0 ");
            width = level_common_script.screen_world.x;
            for (int i = 0; i < actual_game_script.greys.Length; i++) {
               actual_game_script.greys[i].renderer.enabled = true;
            }
         }

         if (time_passed < 30)
         {
            time_passed += Time.deltaTime;
            newScale = new Vector3(-width * (time_passed - TIME_INIT) / TIME_INIT, transform.localScale.y, 1);
            transform.localScale = newScale;
         }

         else {
            isTimeUp = true;
            text_repeat_script.isReady = false;
            shuffle_script.isShuffling = true;
            Debug.Log("sarah : isTimeUp = " + isTimeUp);
            time_passed = 0;
         }
      }
	}
}
