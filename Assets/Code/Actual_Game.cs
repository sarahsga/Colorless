using UnityEngine;
using System.Collections;

public class Actual_Game : MonoBehaviour
{
   [HideInInspector]
   public GameObject stayed_Obj;

   [HideInInspector]
   public int level = 0;

   public GameObject[] colorsAll;
   public GameObject[] colors;

   public GameObject color_display_obj;
   private int random_int;
   private int color;

   private Vector2 color_display_size;
   private Vector3 color_display_position;

   [HideInInspector]
   public BoxCollider2D[] greys;

   public BoxCollider2D grey_guess;
   public Vector3[] positions;
   private Vector2 colorSize;
   private Vector2 gapSize;
   private level_common level_common_script;

   public randomness randomness_obj;

   public int DIFFICULTY = 6;
   private int ROW_COUNT = 2;
   private int COLUMN_COUNT = 3;


   public Vector3 screen_pixel { get; private set; }
   public Vector2 screen_world { get; private set; }
   public float aspect_ratio { get; private set; }

   private Vector3 touch_world;
   private BoxCollider2D hit;

   private bool start_level = true;
   private bool isCorrect = false;

   public GameObject sound_correct;
   public GameObject sound_wrong;

   private float COLOR_SIZE_PERCENT;

   private bool isSetUp = false;
   public bool isOver;
   private bool chance;
   public int score;

   private int the_correct_one;
   private bool show;
   public int even_odd;

   [HideInInspector]
   public GameObject time_bar;
   public time_bar time_bar_script;
   public shuffle shuffle_script;

   private text_repeat text_repeat_script;

   void Awake()
   {
      Debug.Log("sarah: Actual_Game AWAKE");
      text_repeat_script = GetComponent<text_repeat>();
      shuffle_script = GetComponent<shuffle>();
      level_common_script = GetComponent<level_common>();
      randomness_obj = new randomness();
      time_bar = level_common_script.time_bar;
      time_bar_script = time_bar.GetComponent<time_bar>();   
   }

   // Use this for initialization
   void Start()
   {
      even_odd = 0;
      show = false;
      the_correct_one = -1;
      chance = true;
      isOver = false;
      score = 0;
      Debug.Log("sarah: actual_game START");
      if (GameObject.Find("stay_obj") == null)
      {
         Debug.Log("stay_obj not found in the_game");
      }
      else
      {
         stayed_Obj = GameObject.Find("stay_obj");
         level = stayed_Obj.GetComponent<stay>().level;
         Debug.Log("stay_obj found!! level = " + level);
      }
      Destroy(stayed_Obj); // destroying it as soon as we extract the value of level from it
      screen_pixel = new Vector3(Screen.width, Screen.height, 1);
      Debug.Log(" screen_pixel = (" + screen_pixel.x + ", " + screen_pixel.y + ")");

      screen_world = Camera.main.ScreenToWorldPoint(new Vector2(screen_pixel.x, screen_pixel.y));
      Debug.Log(" screen_world (old) = (" + screen_world.x + ", " + screen_world.y + ")");

      screen_world = new Vector2(screen_world.x * 2, screen_world.y * 2);

      Debug.Log(" screen_world (new) = (" + screen_world.x + ", " + screen_world.y + ")");

      color_display_size = level_common_script.color_display_size;
      color_display_position = level_common_script.color_display_position;
      if (level == 1 || level == 2 || level == 3)
      {
         Check_Level();
         randomness_obj.random_index_list_gen(DIFFICULTY);
      
         colors = new GameObject[DIFFICULTY];
         for (int i = 0; i < colors.Length; i++)
         {
            colors[i] = (GameObject)Instantiate(colorsAll[i]);
         }

         greys = new BoxCollider2D[DIFFICULTY];
         for (int i = 0; i < greys.Length; i++)
         {
            greys[i] = (BoxCollider2D)Instantiate(grey_guess);
         }

         Debug.Log("board size = ( " + level_common_script.board.transform.localScale.x + ", " + level_common_script.board.transform.localScale.y + " )");

         //here's the formula i've used to calculate size of each colorbox and each gap
         // board.y = 6 units
         //i want 30% of the board for each colorbox ( as square ) in level_easy
         // dimensions of each colorbox are 1x1
         // Calculations: ( converting percentage into unity units )
         // 100 units ----> 30
         // 1 unit    ----> 30 / 100
         // board.y units   ----> ( 30 /100 ) * board.y

         colorSize.x = level_common_script.board.transform.localScale.x * COLOR_SIZE_PERCENT;
         colorSize.y = level_common_script.board.transform.localScale.y * COLOR_SIZE_PERCENT;

         for (int i = 0; i < DIFFICULTY; i++)
         {
            colors[i].transform.localScale = new Vector3(colorSize.x, colorSize.y, 1);
            greys[i].transform.localScale = colors[i].transform.localScale;
         }

         //considering level_easy
         // no. of gaps between color boxes ( (only horizontally / only vertically) ) = 3
         // color boxes together took up 30*2 = 60% of the board.. remaining space available for gamps = 40%
         // Calculations: ( converting percenntage into unity units )
         // 100 units ------> 40
         // board.y units   ------> (40 / 100 ) * board.y --- size of 6 gaps together
         // size per gap = ((40 / 100 ) * board.y ) / 3

         
         // here's the sequence:
         // | gap | colorbox | gap | colorbox | gap | colorbox | gap |
         // here's the formula for finding out the positions for each colorbox ( only x coordinate shown
         // board.position.x - gapsize*(horizontal index of colorbox) + colorboxsize*(horizontal index of colorbox - 1) 

         Debug.Log("board position = ( " + level_common_script.board.transform.position.x + ", " + level_common_script.board.transform.position.y + " )");

         positions = new Vector3[DIFFICULTY];
         int counter = 0;
         for (int i = 1; i < ROW_COUNT + 1; i++)
         { // traversing vertical  // i & j initial value 1 bcuz gap index  and index (gap - 1) involved in multiplication in the formulae below
            for (int j = 1; j < COLUMN_COUNT + 1; j++)
            { // traversing horizontal for each vertical
               positions[counter].x = level_common_script.board.transform.position.x + gapSize.x * j + colorSize.x * (j - 1); // x positions only affected by horizontal traverse.. hence * j
               positions[counter].y = level_common_script.board.transform.position.y - gapSize.y * i - colorSize.y * (i - 1); // y positions only affected by vertical traverse.... hence * i 
               Debug.Log("postions[ " + counter + " ] = ( " + positions[counter].x + ", " + positions[counter].y + " )");
               counter++;
            }
         }
         for (int i = 0; i < DIFFICULTY; i++)
         {
            Debug.Log("colors[ " + i + " ] = ( " + colors[i].ToString() + ", " + colors[i].ToString() + " )");
            colors[(int)(randomness_obj.random_index_list[i])].transform.position = new Vector3(positions[i].x, positions[i].y, 1);
            greys[i].transform.position = new Vector3(positions[i].x, positions[i].y, 1);
         }
         for (int i = 0; i < greys.Length; i++)
         {
            greys[i].renderer.enabled = false;
         }
      }
      else
      {
         Debug.Log("Sorry, level = " + level);
      }
   }

   // Update is called once per frame
   void Update()
   {
      if (level == 1 || level == 2 || level == 3)
      {
         if (level_common_script.justStarted == false && isOver == false)
         {
            if (start_level == true)
            {
               Debug.Log("start_level = true");
               for (int i = 0; i < greys.Length; i++)
               {
                  greys[i].renderer.enabled = true;
               }
               start_level = false;
               color = randomness_obj.random_color_gen(0,DIFFICULTY);
               color_display_gen();
            }
            else if (Input.touchCount > 0 && shuffle_script.isShuffling == false && text_repeat_script.isReady == true)
            {
               Touch touch = Input.GetTouch(0);

               if (touch.phase == TouchPhase.Began)
               {
                  Debug.Log("Touch phase began");
                  touch_world = Camera.main.ScreenToWorldPoint(touch.position);
                  touch_world.z = 1;

                  Debug.Log("layer = " + LayerMask.NameToLayer("grey_guess"));

                  hit = (BoxCollider2D)Physics2D.OverlapPoint(touch_world, 1 << LayerMask.NameToLayer("grey_guess"));

                  if (hit)
                  {
                     Debug.Log(" easy hit " + hit.ToString());
                     for (int i = 0; i < greys.Length; i++)
                     {
                        if (hit == greys[i])
                        {
                           Debug.Log(" hit grey[ " + i + "]");
                           greys[i].renderer.enabled = false;
                           Debug.Log("color = " + color + " compared to random_index_list [" + i + "] = " + randomness_obj.random_index_list[i]);
                           if (color == (int)randomness_obj.random_index_list[i])
                           {
                              Debug.Log("touch = correct!!");
                              show = false;
                              even_odd = 0;
                              score++;
                              isCorrect = true;
                              sound_correct.audio.Play();
                           }
                           else
                           {
                              check_isOver();
                              if (isOver != true) {
                                 Debug.Log("blinker: isOver != true..");
                                 for (int k = 0; k < greys.Length; k++) {
                                    if (color == (int)randomness_obj.random_index_list[k]) {
                                       Debug.Log("blinker: the_correct_one = " + k);                                       
                                       the_correct_one = k;
                                       show = true;
                                       break;
                                    }
                                 }
                              }
                              sound_wrong.audio.Play();
                           }
                        }
                     }
                  }
               }
               else if (touch.phase == TouchPhase.Ended)
               {
                  Debug.Log("touch ended");
                  for (int i = 0; i < greys.Length; i++)
                  {
                     Debug.Log("blinker: about to disable...");
                     if (i == the_correct_one && show == true)
                     { }
                     else
                     {
                        greys[i].renderer.enabled = true;
                     }
                  }
                  if (isCorrect == true)
                  {
                     isCorrect = false;
                     color_display_gen();
                  }
               }
            }
            if (show == true) // to show the user the correct one if he went wrong but had a chance
            {
               if (even_odd % 20 == 0) // trial and error.. frequency == 20 looked fine
               {
                  Debug.Log("blinker: even_odd == " + even_odd);
                  if (greys[the_correct_one].renderer.enabled == false)
                  {
                     greys[the_correct_one].renderer.enabled = true;
                  }
                  else {
                     greys[the_correct_one].renderer.enabled = false;
                  }
               }
               even_odd++;
               if (even_odd > 100) { // want to show the correct one not for long
                  even_odd = 0;
                  show = false;
                  greys[the_correct_one].renderer.enabled = true;
               }
            }
         }
         else if (isOver == true && even_odd < 100) {
            if (even_odd == 0)
            {
               for (int i = 0; i < greys.Length; i++)
               {
                  greys[i].renderer.enabled = false;
               }
            }
            else {
               if (even_odd % 20 == 0) {
                  if (greys[0].renderer.enabled == false)
                  {
                     for (int i = 0; i < greys.Length; i++)
                     {
                        greys[i].renderer.enabled = true;
                     }
                  }
                  else {
                     for (int i = 0; i < greys.Length; i++)
                     {
                        greys[i].renderer.enabled = false;
                     }
                  }
               }
            }
            even_odd++;
         }
      }
   }

   private void check_isOver() {
      if (chance == true)
      {
         chance = false;
      }
      else {
         isOver = true;
      }
   }

   private void color_display_gen()
   {
      while (true)
      {
         random_int = randomness_obj.random_color_gen(0,DIFFICULTY);
         if (random_int != color)
         {
            color = random_int;
            break;
         }
         else continue;
      }
      if (color_display_obj != null)
      {
         Destroy(color_display_obj); // destroying the previous color_display_obj
      }
      color_display_obj = (GameObject)Instantiate(colors[color], color_display_position, Quaternion.identity);
      color_display_obj.transform.localScale = color_display_size;
      Debug.Log("color (int) = " + color + " = " + color_display_obj.ToString());
   }

   public void Check_Level() {
      if (level == 1)
      {
         DIFFICULTY = 4;
         ROW_COUNT = 2;
         COLUMN_COUNT = 2;
         COLOR_SIZE_PERCENT = 0.30f;

         gapSize.x = (0.40f * level_common_script.board.transform.localScale.x) / (COLUMN_COUNT + 1);
         gapSize.y = (0.40f * level_common_script.board.transform.localScale.y) / (ROW_COUNT + 1);

         Debug.Log("gap size = ( " + gapSize.x + ", " + gapSize.y + " )");
         Debug.Log("color size = ( " + colorSize.x + ", " + colorSize.y + " )");

      }
      else if (level == 2)
      {
         DIFFICULTY = 6;
         ROW_COUNT = 2;
         COLUMN_COUNT = 3;
         COLOR_SIZE_PERCENT = 0.25f;

         gapSize.x = (0.25f * level_common_script.board.transform.localScale.x) / (COLUMN_COUNT + 1);
         gapSize.y = (0.50f * level_common_script.board.transform.localScale.y) / (ROW_COUNT + 1);

         Debug.Log("gap size = ( " + gapSize.x + ", " + gapSize.y + " )");
         Debug.Log("color size = ( " + colorSize.x + ", " + colorSize.y + " )");

      }

      else if (level == 3)
      {
         DIFFICULTY = 9;
         ROW_COUNT = 3;
         COLUMN_COUNT = 3;
         COLOR_SIZE_PERCENT = 0.25f;

         gapSize.x = (0.25f * level_common_script.board.transform.localScale.x) / (COLUMN_COUNT + 1);
         gapSize.y = (0.25f * level_common_script.board.transform.localScale.y) / (ROW_COUNT + 1);

         Debug.Log("gap size = ( " + gapSize.x + ", " + gapSize.y + " )");
         Debug.Log("color size = ( " + colorSize.x + ", " + colorSize.y + " )");

      }
   }
}
