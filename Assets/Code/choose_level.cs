using UnityEngine;
using System.Collections;

public class choose_level : MonoBehaviour
{

   public GameObject chess_back;
   public BoxCollider2D btn_easy;
   public BoxCollider2D btn_medium;
   public BoxCollider2D btn_hard;
   public GameObject header_choose_level;
   public BoxCollider2D btn_back;
   public GameObject top_bar;

   public Vector3 screen_pixel { get; private set; }
   public Vector2 screen_world { get; private set; }
   public float aspect_ratio { get; private set; }

   private Vector3 touch_world;
   private BoxCollider2D hit;

   public GameObject stay_obj;
   public GameObject stay_obj_helper;
   private stay stay_obj_script;
   public GameObject sound_click;

   void Awake() {
      Debug.Log("sarah: choose_level AWAKE");

      stay_obj_script = stay_obj.GetComponent<stay>();
      Debug.Log("sarah : stay_obj is NOT null. stay_obj_script.level = " + stay_obj_script.level );
   }

   // Use this for initialization
   void Start()
   {
      Debug.Log("choose_level START");
      screen_pixel = new Vector3(Screen.width, Screen.height, 1);
      screen_world = Camera.main.ScreenToWorldPoint(new Vector2(screen_pixel.x, screen_pixel.y));
      screen_world = new Vector2(screen_world.x * 2, screen_world.y * 2);

      chess_back.transform.localScale = new Vector3(screen_world.x / 4, screen_world.y / 5, 1);
      chess_back.transform.position = new Vector3(-screen_world.x / 2f, screen_world.y / 2f, 1);

      top_bar.transform.localScale = new Vector3(screen_world.x, screen_world.y * 0.15f, 1);
      top_bar.transform.position = new Vector3(-screen_world.x / 2f, screen_world.y / 2.0f, 1);

      btn_back.transform.localScale = new Vector3((top_bar.transform.localScale.y - top_bar.transform.localScale.y * 0.2f) / 2, (top_bar.transform.localScale.y - top_bar.transform.localScale.y * 0.2f) / 2, 1);
      btn_back.transform.position = new Vector3(-screen_world.x / 2 + top_bar.transform.localScale.y * 0.1f, screen_world.y / 2 - top_bar.transform.localScale.y / 2, 1); 
      
      header_choose_level.transform.localScale = new Vector3(screen_world.x * 0.8f / 6, screen_world.y * 0.1f, 1);
      header_choose_level.transform.position = new Vector3(0, screen_world.y / 2 - screen_world.y * 0.25f, 1);

      btn_easy.transform.localScale = new Vector3((screen_world.x / 4f) * 0.5f, (screen_world.y / 2f) * 0.15f, 1);
      btn_easy.transform.position = new Vector3(0, screen_world.y / 2 - screen_world.y * 0.4f, 1);

      btn_medium.transform.localScale = new Vector3((screen_world.x / 4f) * 0.5f, (screen_world.y / 2f) * 0.15f, 1);
      btn_medium.transform.position = new Vector3(0, screen_world.y / 2 - screen_world.y * 0.6f, 1);

      btn_hard.transform.localScale = new Vector3((screen_world.x / 4f) * 0.5f, (screen_world.y / 2f) * 0.15f, 1);
      btn_hard.transform.position = new Vector3(0, screen_world.y / 2 - screen_world.y * 0.8f, 1);
   }

   // Update is called once per frame
   void Update()
   { 
      if (Input.touchCount > 0)
      {
         Touch touch = Input.GetTouch(0);

         if (touch.phase == TouchPhase.Began)
         {
            touch_world = Camera.main.ScreenToWorldPoint(touch.position);
            touch_world.z = 1;

            hit = (BoxCollider2D)Physics2D.OverlapPoint(touch_world);

            if (hit)
            {
               
               if (hit == btn_back)
               {
                  Application.LoadLevel(Application.loadedLevel - 1);
                  btn_back.audio.Play();
               }
               else if (hit == btn_easy || hit == btn_medium || hit == btn_hard)
               {

                  if (hit == btn_easy)
                  {
                     stay_obj_script.level = 1;
                     Debug.Log("btn easy pressed.. here, script.level = " + stay_obj_script.level);
                  }

                  else if (hit == btn_medium)
                  {
                     stay_obj_script.level = 2;
                     Debug.Log("btn easy pressed.. here, script.level = " + stay_obj_script.level);
                  }

                  else 
                  {
                     stay_obj_script.level = 3;
                     Debug.Log("btn easy pressed.. here, script.level = " + stay_obj_script.level);
                  }
                  sound_click.audio.Play();
                  DontDestroyOnLoad(sound_click);
                  DontDestroyOnLoad(stay_obj);
                  Debug.Log("just before going.. script.level = " + stay_obj_script.level);
                  Application.LoadLevel("the_game");
               }
            }
         }
      }
   }
}
