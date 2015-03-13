using UnityEngine;
using System.Collections;

public class game_over : MonoBehaviour {

   private Actual_Game actual_game_script;
   public GameObject game_over_box;
   private GameObject[] game_over_box_list;
   private Vector2 screen_world;
   private level_common level_common_script;
   public BoxCollider2D btn_play_again;
   private Vector2 touch_world;
   private BoxCollider2D hit;
   public GUIText[] gui_text;
   private float gui_text_pos_y;
   public GameObject sound_obj;
   public GameObject stayed_obj;
   [HideInInspector]
   public bool isHighScore;
   private int even_odd;
   private bool over_just_now;
   public GameObject[] stars;
   private randomness randomness_obj;
   private ArrayList stars_list;
   private int level;
   private GoogleMobileAdsScript ads_script;

   void Awake() {
      ads_script = GetComponent<GoogleMobileAdsScript>();
      even_odd = 0;
      randomness_obj = new randomness();
      isHighScore = false;
      actual_game_script = GetComponent<Actual_Game>();
      level_common_script = GetComponent<level_common>();
      over_just_now = true;
   }

   void Start() {
      ads_script.RequestBanner();
      screen_world = level_common_script.screen_world;
      game_over_box.transform.localScale = new Vector3(screen_world.x, (screen_world.y * 0.8f - level_common_script.top_bar.transform.localScale.y) / 5, 1);
      game_over_box.transform.position = new Vector3(0, level_common_script.top_bar.transform.position.y - level_common_script.top_bar.transform.localScale.y - level_common_script.time_bar.transform.localScale.y, 1);

      game_over_box_list = new GameObject[7];

      game_over_box_list[0] = game_over_box;

      game_over_box_list[0].renderer.enabled = false;

      for (int i = 1; i < game_over_box_list.Length; i++) {
         game_over_box_list[i] = (GameObject)Instantiate(game_over_box, new Vector3(0, game_over_box_list[i - 1].transform.position.y - game_over_box.transform.localScale.y, 1), Quaternion.identity);
         game_over_box_list[i].renderer.enabled = false;
      }


      for (int i = 0; i < gui_text.Length - 1; i++) {
         gui_text_pos_y = 1 - (level_common_script.top_bar.transform.localScale.y + level_common_script.time_bar.transform.localScale.y + game_over_box.transform.localScale.y * i) / screen_world.y;
         gui_text[i].transform.position = new Vector3(0.5f, gui_text_pos_y, 1);
         gui_text[i].fontSize = Screen.height / 15;
         gui_text[i].enabled = false;
      }

      gui_text[4].transform.position = new Vector3(0.5f, ( gui_text[2].transform.position.y + gui_text[3].transform.position.y ) /2,1); // y == average of [3]and[4]
      gui_text[4].fontSize = Screen.height / 13;
      gui_text[4].enabled = false;

      gui_text[0].text = "Your Score is";
      gui_text[2].text = "Score to beat";
      gui_text[4].text = "High Score";

      btn_play_again.transform.localScale = new Vector3((screen_world.x / 4f) * 0.5f, (screen_world.y / 2f) * 0.15f, 1);      
      btn_play_again.transform.position = game_over_box_list[4].transform.position;
      btn_play_again.renderer.enabled = false;

      for (int i = 0; i < stars.Length; i++) {
         stars[i].transform.localScale = new Vector3(screen_world.x * 0.1f, screen_world.x * 0.1f, 1);
      }
   }

   // Update is called once per frame
   void Update()
   {
      if (actual_game_script.isOver == true && actual_game_script.even_odd >= 100 ) {
         if (over_just_now == true) {
            //Debug.Log("inside over_just_now");
            over_just_now = false;
            gui_text[1].text = "" + actual_game_script.score + "";
            ads_script.bannerView.Show();
            for (int i = 0; i < game_over_box_list.Length; i++) {
               game_over_box_list[i].renderer.enabled = true;
            }            
            gui_text[0].enabled = true;
            gui_text[1].enabled = true;
            btn_play_again.renderer.enabled = true;

            level = actual_game_script.level;
            stayed_obj.GetComponent<stay>().level = level;

            if (level == 1) {
               check_high("high_score_easy");
            }
            if (level == 2) {
               check_high("high_score_medium");
            }
            if (level == 3) {
               check_high("high_score_hard");
            }
         }

         if (Input.touchCount > 0)
         {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
               touch_world = Camera.main.ScreenToWorldPoint(touch.position);

               hit = (BoxCollider2D)Physics2D.OverlapPoint(touch_world, 1 << LayerMask.NameToLayer("game_over"));

               if (hit == btn_play_again)
               {
                  ads_script.bannerView.Hide();
                  ads_script.bannerView.Destroy();
                  DontDestroyOnLoad(stayed_obj);
                  sound_obj.audio.Play();
                  DontDestroyOnLoad(sound_obj);
                  DontDestroyOnLoad(actual_game_script.stayed_Obj);
                  Application.LoadLevel("the_game");
               }
            }
         }
         if (isHighScore == true) {
            if (even_odd % 20 == 0)
            {
               if (gui_text[4].enabled == true)
               {
                  gui_text[4].enabled = false;
               }
               else
               {
                  gui_text[4].enabled = true;
               }
            }

            if (even_odd % 5 == 0) {
               if (even_odd % 30 == 0)
               {
                  stars_list.Add((GameObject)Instantiate(stars[randomness_obj.random_color_gen(0,stars.Length)], new Vector3((float)randomness_obj.random_color_gen((int)-screen_world.x/2,(int)screen_world.x/2), screen_world.y / 2, 1), Quaternion.identity));
               }
               for (int j = 0; j < stars_list.Count; j++) {
                  GameObject a_star = (GameObject)stars_list[j];
                  a_star.transform.position = new Vector3( a_star.transform.position.x, a_star.transform.position.y - screen_world.y * 0.01f, 1);
                  //Debug.Log("a_star y: " + a_star.transform.position.y + " .. screen.y = " + screen_world.y);
                  if (a_star.transform.position.y < - screen_world.y/2) {
                     //Debug.Log("destroy a_star");
                     Destroy(a_star);
                     stars_list.RemoveAt(j);
                  }
               }
            }
            even_odd++;
         }
      }
	}

   private void check_high(string key) {
      if (actual_game_script.score > PlayerPrefs.GetInt(key, 0))
      {
         PlayerPrefs.SetInt(key, actual_game_script.score);
         isHighScore = true;
         stars_list = new ArrayList();
         gui_text[2].enabled = false;
         gui_text[3].enabled = false;

         gui_text[4].enabled = true;
      }
      else
      {
         gui_text[3].text = "" + PlayerPrefs.GetInt(key, 0) + "";

         gui_text[2].enabled = true;
         gui_text[3].enabled = true;

         gui_text[4].enabled = false;
      }
   }
}
