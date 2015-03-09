using UnityEngine;
using System.Collections;

public class scoring : MonoBehaviour {

   public GUIText gui_text;
   private int score;
   public GameObject score_box;
   private level_common level_common_script;
   private Actual_Game actual_game_script;

   private Vector2 screen_world;
   private Vector2 score_pos;
   
   void Awake() {
      actual_game_script = GetComponent<Actual_Game>();
      level_common_script = GetComponent<level_common>();
   }

	// Use this for initialization
	void Start () {
      score = actual_game_script.score;
      screen_world = level_common_script.screen_world;

      score_box.transform.localScale = new Vector3( level_common_script.btn_back.transform.localScale.x * 4, level_common_script.btn_back.transform.localScale.y * 2, 1);
      score_box.transform.position = new Vector3(level_common_script.screen_world.x / 2 - level_common_script.top_bar.transform.localScale.y * 0.1f - score_box.transform.localScale.x, level_common_script.top_bar.transform.position.y - level_common_script.top_bar.transform.localScale.y * 0.1f, 1);

      //score_pos is a vvector representing the position of gui_text displaying the score
      // guitext font size can only be in the range [0,1]
      //in order to fit the score gui_text inside the score_box, we do the following calculations
      // consider score_pos.x
      // having anchor of gui_text as middle centre,
      // its position will be (size_of_scoreBox / 2 ) + (right margin of scorebox)
      // all calculations need to be reduced to the range [0,1]
      // first, calculating size_of_scorebox / 2 :
      // the score_box is scaled in the range [0, screen_world.x]
      // using Unity rule of maths, we conclude that the size of score_box corresponding ..
      // .. to the range [0,1] will be  its (currnet size) / screen_world.x
      // next, finding right margin of scorebox in range [0,1]
      // it will be top_bar.y*0.1f in range [0, screen_world.x]
      // converting it into range [0,1], we get top_bar.y*0.1f / screen_world.x
      // hence, the formula below:
      score_pos.x = 1 - (score_box.transform.localScale.x/2  + level_common_script.top_bar.transform.localScale.y * 0.1f) / screen_world.x;
      score_pos.y = 1 - (score_box.transform.localScale.y / 2 + level_common_script.top_bar.transform.localScale.y * 0.1f) / screen_world.y;

      gui_text.transform.localScale = score_box.transform.localScale;
      gui_text.transform.position = new Vector3(score_pos.x , score_pos.y, 1); // for gui text, maximum is (1,1,1)
      OnGui();
      gui_text.text = "" + score + "";
	}
	
	// Update is called once per frame
	void Update () {
      if (score != actual_game_script.score)
      {
         score = actual_game_script.score;
         gui_text.text = "" + score + "";
      } 
      //score++;
      //gui_text.text = "" + score + "";
	}

   void OnGui()
   {
      gui_text.fontSize = Mathf.Min(Screen.width, Screen.height) / 10;

      //gui_text.fontSize = (int)(NativeHeight / Screen.height * default_font_size);
      //float rx = Screen.width / NativeWidth;
      //float ry = Screen.height / NativeHeight;
      //// Scale width the same as height - cut off edges to keep ratio the same
      //GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(ry, ry, 1));
      //// Get width taking into account edges being cut off or extended
      //float adjustedWidth = NativeWidth * (rx / ry);

      //// Do your gui stuff as usual now - you can pretend the GUI is always Native_height and adjustedWidth
      //gui_text.fontSize = 
   }
}
