using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoobleScript : MonoBehaviour {
	
	public bool isMove;
	public float boobleSpeed;
	public float boobleAng;
	public float y,x;
	public static int score=0;
	public static bool gameOver = false;
	
	private static bool toDestroy = false;
	private GameObject boobleObject;
	private int color;

	// Use this for initialization
	void Start () {
		isMove = false;
		boobleSpeed = 350.0f;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void setBooble(GameObject booble){
		boobleObject = booble;
	}

	public void setColor(int c, Material m){
		color = c;
		boobleObject.renderer.material = m;
	}

	public void moveBooble(){
		boobleObject.transform.Translate( new Vector3(x,y,0) * Time.deltaTime * boobleSpeed);

	}

	private static void detectBooblesOf( GameObject boobleDet,int n){
		BoobleScript bs = boobleDet.GetComponent<BoobleScript> ();
		bs.isMove = false;
		List<GameObject> boobleToDestroy = bs.boobleAroundOf ();
		if (boobleToDestroy.Count >= n) {
			foreach (GameObject element in boobleToDestroy) {
				element.tag = "destroyable";
			}
			foreach (GameObject element in boobleToDestroy) {
				if(element != boobleDet){
					detectBooblesOf(element,1);
				}
			}
			toDestroy = true;
		}
	}

	private List<GameObject> boobleAroundOf() {
		Collider[] hitColliders = Physics.OverlapSphere(boobleObject.transform.position, boobleObject.transform.localScale.x);
		List<GameObject> boobleCol = new List<GameObject> ();
		foreach (Collider colliderObject in hitColliders) {
			GameObject colAux = colliderObject.gameObject;
			if(colAux.name == "booble_sphere(Clone)" && colAux.tag == "booble"){
				BoobleScript bs = colAux.GetComponent<BoobleScript> ();
				if(bs.color == this.color && colAux.tag != "destroyable"){
					boobleCol.Add( colAux );
				}
			}
		}
		return boobleCol;
	}

	private static void destroyBoobles(){
		if(toDestroy){
			GameObject[] boobleDestroy = GameObject.FindGameObjectsWithTag("destroyable");
			int i = 1;
			foreach(GameObject element in boobleDestroy){
				score += (10*i++); 
				Destroy(element);
			}
			toDestroy = false;
		}
	}

	private void boobleGameOver() {
		Collider[] hitColliders = Physics.OverlapSphere(boobleObject.transform.position, boobleObject.transform.localScale.x - 10.0f);
		foreach (Collider colliderObject in hitColliders) {
			GameObject colAux = colliderObject.gameObject;
			if(colAux.name == "LimitCollision"){
				gameOver = true;
				break;
			}
		}
	}
	void OnCollisionEnter(Collision collision) {
		GameObject gAux = collision.gameObject;
		if (gAux.name == "booble_sphere(Clone)") {
			isMove = false;
			BoobleScript bs = gAux.GetComponent<BoobleScript> ();
			if(bs.color == this.color){
				detectBooblesOf(boobleObject,3);
				destroyBoobles();
			}
			else{
				boobleGameOver();
			}
		}
		else if (gAux.name == "Techo") {
			isMove = false;
		}

		else if (gAux.name == "Muro1") {
			float ang = 90 - boobleAng; 
			x = Mathf.Cos(ang * Mathf.Deg2Rad);
			y = Mathf.Sin(ang * Mathf.Deg2Rad);
		}
		else if (gAux.name == "Muro2") {
			float ang = 90 - Mathf.Abs(boobleAng); 
			x = Mathf.Cos(ang * Mathf.Deg2Rad)*-1;
			y = Mathf.Sin(ang * Mathf.Deg2Rad);
		}

	}
	
}
