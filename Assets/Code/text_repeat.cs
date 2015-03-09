using UnityEngine;
using System.Collections;

public class text_repeat : MonoBehaviour {

   public GameObject text_ready_in;
   public GameObject text_3;
   public GameObject text_2;
   public GameObject text_1;
   public GameObject sound_ready;

   private level_common level_common_script;
   private shuffle shuffle_script;
   private Actual_Game actual_game_script;

   private float time_passed;

   [HideInInspector]
   public bool isReady;
   private bool check1;
   private bool check2;
   private bool check3;


   void Awake() {
      level_common_script = GetComponent<level_common>();
      shuffle_script = GetComponent<shuffle>();
      actual_game_script = GetComponent<Actual_Game>();
      time_passed = 0;
      isReady = true;
      check1 = true;
      check2 = true;
      check3 = true;
   }

	// Use this for initialization
	void Start () {
      text_ready_in.transform.localScale = new Vector3(1, 1, 1);
      text_ready_in.transform.position = new Vector3(0, level_common_script.screen_world.y / 2 - level_common_script.screen_world.y * 0.25f, 1);
      text_ready_in.renderer.enabled = false;

      text_3.transform.localScale = new Vector3(1, 1, 1);
      text_3.transform.position = text_ready_in.transform.position;
      text_3.renderer.enabled = false;

      text_2.transform.localScale = text_3.transform.localScale;
      text_2.transform.position = text_3.transform.position;
      text_2.renderer.enabled = false;

      text_1.transform.localScale = text_3.transform.localScale;
      text_1.transform.position = text_3.transform.position;
      text_1.renderer.enabled = false;
   }
	
	// Update is called once per frame
	void Update () {
      if (shuffle_script.isShuffling == false && isReady == false) {
         if ((int)time_passed < 1) {
            text_ready_in.renderer.enabled = true;
         }
         else if ((int)time_passed < 2) {
            text_ready_in.renderer.enabled = false;
            text_3.renderer.enabled = true;
            if (check1 == true) {
               check1 = false;
               sound_ready.audio.Play();
            }
         } 
         else if ((int)time_passed < 3) {
            text_3.renderer.enabled = false;
            text_2.renderer.enabled = true;
            if (check2 == true)
            {
               check2 = false;
               sound_ready.audio.Play();
            }
         }
         else if ((int)time_passed < 4)
         {
            text_2.renderer.enabled = false;
            text_1.renderer.enabled = true;
            if (check3 == true)
            {
               check3 = false;
               sound_ready.audio.Play();
            }
         }
         else {
            text_1.renderer.enabled = false;
            isReady = true;
            actual_game_script.color_display_obj.renderer.enabled = true;
         }
         time_passed += Time.deltaTime;
         if (isReady == true) {
            time_passed = 0;
            check1 = true;
            check2 = true;
            check3 = true;
         }
      }
	}
}
