using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public GameObject newBooble;
	public GameObject arrow;
	public bool readyToShoot, firstShoot;
	public Material[] materialColor;

	private float arrowSpeed;
	private float arrowAngle;
	private int angLimit;
	private BoobleScript boobleShoot;
	private Vector3 positionShoot;
	// Use this for initialization
	void Start () {
		readyToShoot = true;
		firstShoot = true;
		angLimit = 60;
		arrowAngle = arrow.transform.eulerAngles.z;
		arrowSpeed = 50.0f;
		positionShoot = new Vector3 (400,1,636);
		createStage ();
		createBooble (positionShoot);
	}
	
	// Update is called once per frame
	void Update () {
		directionShoot ();
		if (!readyToShoot && !boobleShoot.isMove && !BoobleScript.gameOver){
			createBooble (positionShoot);
			readyToShoot = true;
		}
		else if(readyToShoot && !boobleShoot.isMove ) {
			disparo();
		}
		else if (boobleShoot.isMove) {
			boobleShoot.moveBooble ();
		}
	}

	public void restartBooble(){
		GameObject[] boobleDestroy = GameObject.FindGameObjectsWithTag("booble");
		foreach(GameObject element in boobleDestroy){
			Destroy(element);
		}
		createStage ();
		readyToShoot = false;
		firstShoot = true;
	}

	private void directionShoot(){
		if (Input.GetKey (KeyCode.LeftArrow) && arrowAngle <= angLimit) {
			arrow.transform.RotateAround(positionShoot , Vector3.forward, Time.deltaTime * arrowSpeed);
		}
		if (Input.GetKey (KeyCode.RightArrow) & arrowAngle >= -angLimit) {
			arrow.transform.RotateAround(positionShoot , Vector3.back, Time.deltaTime * arrowSpeed);
		}
		arrowAngle = arrow.transform.eulerAngles.z;
		if (arrowAngle > 180)
			arrowAngle = arrowAngle - 360;
	}

	private void disparo(){
		if (Input.GetKey (KeyCode.Space)) {
			boobleShoot.y = Mathf.Cos(arrowAngle * Mathf.Deg2Rad);
			boobleShoot.x = Mathf.Sin(arrowAngle * Mathf.Deg2Rad) *-1;
			boobleShoot.boobleAng = arrowAngle;
			boobleShoot.isMove = true;
			readyToShoot = false;
			if(firstShoot)
				firstShoot = false;
		}
	}

	private void createBooble(Vector3 posicion){
		int c = Random.Range(0, 4);
		GameObject gAux = (GameObject)Instantiate (newBooble, posicion, Quaternion.identity);
		boobleShoot = gAux.GetComponent<BoobleScript> ();
		boobleShoot.setBooble( gAux );
		boobleShoot.setColor (c, materialColor[c] );
	}

	public bool winBooble(){
		GameObject[] boobleStage = GameObject.FindGameObjectsWithTag("booble");
		if (boobleStage.Length < 6)
			return true;
		return false;
	}

	private void createStage(){
		float radio = newBooble.transform.localScale.x;
		float[] minX = new float[] {100.0f, 100.0f + radio/2};
		float minY = 634.0f, minZ = 636.0f;
		for(int j= 0; j<4;j++){
			for(int i = 0; i < 15; i++ ){
				createBooble(new Vector3(minX[j%2] + radio * i, minY - radio * j , minZ));
			}
		}
	}
}
