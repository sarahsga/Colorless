using UnityEngine;
using System.Collections;

public class level_common : MonoBehaviour
{

   public BoxCollider2D background;
   public GameObject top_bar;
   public BoxCollider2D btn_back;
   public GameObject board;
   public GameObject time_bar;

   public GameObject text_tap_to_go;
   public GameObject text_colourless;
   

   public Vector2 color_display_size { get; private set; }
   public Vector3 color_display_position { get; private set; }

   public Vector3 screen_pixel { get; private set; }
   public Vector2 screen_world { get; private set; }
   public float aspect_ratio { get; private set; }

   private Vector3 touch_world;
   private BoxCollider2D hit;

   public bool justStarted { get; private set; }
   void Awake()
   {
      //Debug.Log("sarah: level_common AWAKE");
      screen_pixel = new Vector3(Screen.width, Screen.height, 1);

      screen_world = Camera.main.ScreenToWorldPoint(new Vector2(screen_pixel.x, screen_pixel.y));
      screen_world = new Vector2(screen_world.x * 2, screen_world.y * 2);

      background.transform.localScale = new Vector3(screen_world.x, screen_world.y, 1);
      background.transform.position = new Vector3(-screen_world.x / 2f, screen_world.y / 2f, 1);

      top_bar.transform.localScale = new Vector3(screen_world.x, screen_world.y * 0.15f, 1);
      top_bar.transform.position = new Vector3(-screen_world.x / 2f, screen_world.y / 2.0f, 1);

      btn_back.transform.localScale = new Vector3((top_bar.transform.localScale.y - top_bar.transform.localScale.y * 0.2f) / 2, (top_bar.transform.localScale.y - top_bar.transform.localScale.y * 0.2f) / 2, 1);
      btn_back.transform.position = new Vector3(-screen_world.x / 2 + top_bar.transform.localScale.y * 0.1f, screen_world.y / 2 - top_bar.transform.localScale.y / 2, 0);

      board.transform.localScale = new Vector3(screen_world.x, screen_world.y * 0.60f, 1);
      board.transform.position = new Vector3(-(screen_world.x / 2), (screen_world.y / 2.0f) - screen_world.y * 0.4f, 1);

      time_bar.transform.localScale = new Vector3(screen_world.x, screen_world.y * 0.05f, 1);
      time_bar.transform.position = new Vector3(-(screen_world.x / 2.0f), screen_world.y / 2 - screen_world.y * 0.15f, 1);

      setText();

      color_display_size = new Vector2(screen_world.x, screen_world.y * 0.20f);
      color_display_position = new Vector3(-(screen_world.x / 2f), screen_world.y / 2f - screen_world.y * 0.20f, 1);

      justStarted = true;
   }

   public void setText() {
      text_tap_to_go.transform.localScale = new Vector3(1,1, 1);
      text_tap_to_go.transform.position = new Vector3(0, screen_world.y / 2.0f - screen_world.y*0.20f, 1);

      text_colourless.transform.localScale = new Vector3(1,1, 1);
      text_colourless.transform.position = new Vector3(0, screen_world.y / 2.0f - screen_world.y * 0.30f, 1);

      
   }

   void Start()
   {

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
            if (justStarted == true)
            {
               hit = (BoxCollider2D)Physics2D.OverlapPoint(touch_world, (1 << LayerMask.NameToLayer("back_button")) | (1 << LayerMask.NameToLayer("background")));
               Destroy(text_tap_to_go);
               Destroy(text_colourless);
            }
            else
            {
               hit = (BoxCollider2D)Physics2D.OverlapPoint(touch_world, 1 << LayerMask.NameToLayer("back_button"));
            }

            if (hit)
            {
               //Debug.Log("touch its a hit level_common.. hit = " + hit.ToString());
               if (hit == btn_back) // the order of ifs and else ifs matters here... background needs to be iffed after btn_pause and btn_resume
               {
                  //Debug.Log(" touch hit == back button");
                  btn_back.audio.Play();
                  Application.LoadLevel("choose_level");
               }
               if (hit == background)
               {
                  justStarted = false;
               }

            }

         }
      }
   }
}