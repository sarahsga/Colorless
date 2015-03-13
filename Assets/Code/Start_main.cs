using UnityEngine;
using System.Collections;

public class Start_main : MonoBehaviour
{
   public GameObject chess_back;
   public BoxCollider2D btn_play;
   public BoxCollider2D btn_exit;
   public GameObject title_colorless;

   public Vector3 screen_pixel { get; private set; }
   public Vector2 screen_world { get; private set; }
   public float aspect_ratio { get; private set; }

   private Vector3 touch_world;
   private BoxCollider2D hit;
   public GameObject sound_click;

   void Awake() {

   }

   // Use this for initialization
   void Start()
   {
      //Debug.Log("Inside start");
      if (Application.platform == RuntimePlatform.Android || true)
      {
         screen_pixel = new Vector3(Screen.width, Screen.height, 1);
         screen_world = Camera.main.ScreenToWorldPoint(new Vector2(screen_pixel.x, screen_pixel.y));
         screen_world = new Vector2(screen_world.x * 2, screen_world.y * 2);

         chess_back.transform.localScale = new Vector3(screen_world.x / 4, screen_world.y / 5, 1);
         chess_back.transform.position = new Vector3(-screen_world.x / 2f, screen_world.y / 2f, 1);

         title_colorless.transform.localScale = new Vector3(screen_world.x * 0.8f / 6, screen_world.y * 0.2f / 3, 1);
         title_colorless.transform.position = new Vector3(0, screen_world.y / 2 - screen_world.y * 0.1f, 1);

         btn_play.transform.localScale = new Vector3((screen_world.x / 4f) * 0.5f, (screen_world.y / 2f) * 0.15f, 1);
         btn_play.transform.position = new Vector3(0, screen_world.y / 2 - screen_world.y * 0.4f, 1);

         btn_exit.transform.localScale = new Vector3((screen_world.x / 4f) * 0.5f, (screen_world.y / 2f) * 0.15f, 1);
         btn_exit.transform.position = new Vector3(0, screen_world.y / 2 - screen_world.y * 0.6f, 1);

      }
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
               DontDestroyOnLoad(sound_click);
               if (hit == btn_play)
               {
                  sound_click.audio.Play();
                  Application.LoadLevel("choose_level");
               }

               if (hit == btn_exit)
               {
                  btn_exit.audio.Play();
                  Application.Quit();
               }
            }
         }
      }


   }
}
