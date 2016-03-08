using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class init : MonoBehaviour {
	public Text score, win,lose,restart;
	private GameObject arrow;
	private bool isPlaying;
	Shoot shootScript;

	// Use this for initialization
	void Start () {
		shootScript = GetComponent<Shoot>();
		score = GameObject.Find("canvas/Score").GetComponent<Text>();
		win = GameObject.Find("canvas/WinTitle").GetComponent<Text>();
		lose = GameObject.Find("canvas/LoseTitle").GetComponent<Text>();
		restart = GameObject.Find("canvas/Restart").GetComponent<Text>();
		arrow = GameObject.Find("Arrow");
		arrow.SetActive (false);
		score.enabled = false;
		restart.enabled = false;
		win.enabled = false;
		lose.enabled = false;
		isPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		playBooble ();
		score.text = BoobleScript.score.ToString();
		if (shootScript.winBooble () && isPlaying && shootScript.firstShoot) {
			win.enabled = true;
		}
		if (BoobleScript.gameOver) {
			lose.enabled = true;
		}
	}

	private void playBooble(){
		if (Input.GetKeyUp (KeyCode.Return) && !isPlaying) {
			Text title = GameObject.Find("canvas/Title").GetComponent<Text>();
			Text inst = GameObject.Find("canvas/Instrucciones").GetComponent<Text>();

			shootScript.enabled = true;
			arrow.SetActive(true);
			score.enabled = true;
			restart.enabled = true;
			title.enabled = false;
			inst.enabled = false;
			isPlaying = true;
		}
		else if (Input.GetKeyUp (KeyCode.Return) && isPlaying) {
			win.enabled = false;
			lose.enabled = false;
			shootScript.restartBooble();
			BoobleScript.score = 0;
			BoobleScript.gameOver = false;
		}
		if (shootScript.firstShoot) {
			BoobleScript.score = 0;
		}
	}
}
