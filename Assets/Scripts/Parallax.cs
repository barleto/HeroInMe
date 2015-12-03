using UnityEngine;
using System.Collections.Generic;

public class Parallax : MonoBehaviour {

	public GameObject FocusOnObject;
	public GameObject leftObject;
	public GameObject rightObject;

	
	///Afeta a velocidade de scroll em relaçao à velocidade do player. (1 igual 100% da velocidade). 
	[Range(0f,1f)]
	//velocidade relativa em relação à main camera
	public float relativeSpeed = 1.0f;

	//Range em x antes de trocar os backgrounds
	public float rangeForChange = 10;
	public float tileSize;

	//switch sytem variables
	public string[] switchVariablesNames;
	private List<bool> switchVariables = new List<bool>();


	private float camPosOldX;
	private float camPosOldY;



	private float currentMovement = 0;
	private float lastPos;
	private Vector3 initialPosRight;
	private Vector3 initialPosLeft;

	
	
	void Start () {
		camPosOldX = Camera.main.transform.position.x;
		camPosOldY = Camera.main.transform.position.y;
		tileSize = tileSize < 0 ? -tileSize : tileSize;
		lastPos = leftObject.transform.position.x;
		initialPosRight = rightObject.transform.position;
		initialPosLeft = leftObject.transform.position;
	}
	
	/// Update is called once per frame
	void Update () {
		Vector3 newVec = new Vector3 ((Camera.main.transform.position.x - camPosOldX) * relativeSpeed, (Camera.main.transform.position.y - camPosOldY) * relativeSpeed, 0);
		//this.transform.position += newVec;
		rightObject.transform.position += newVec;
		leftObject.transform.position += newVec;
		this.currentMovement += leftObject.transform.position.x - lastPos;
		Debug.Log(currentMovement);
		if( (Mathf.Abs(leftObject.transform.position.x - lastPos)) >tileSize){
			restart();
			currentMovement = 0;
		}
		else if((FocusOnObject.transform.position.x - leftObject.transform.position.x) >= rangeForChange){
			leftObject.transform.position = rightObject.transform.position;
			rightObject.transform.position += new Vector3(tileSize,0,0);
			currentMovement = 0;
		}
		else if((FocusOnObject.transform.position.x - leftObject.transform.position.x) <= -rangeForChange){
			rightObject.transform.position = leftObject.transform.position;
			leftObject.transform.position += new Vector3(-tileSize,0,0);
			currentMovement = 0;
		}


		//update values
		camPosOldX = Camera.main.transform.position.x;
		camPosOldY = Camera.main.transform.position.y;
		lastPos = leftObject.transform.position.x;
	}


	void changeObj(){
		GameObject aux = rightObject;
		rightObject = leftObject;
		leftObject = aux;
	}

	public void restart(){
		leftObject.transform.position = initialPosLeft;
		rightObject.transform.position = initialPosRight;
	}

}


/*using UnityEngine;
using System.Collections.Generic;

public class Scroller : MonoBehaviour {

	///Ponto ao lado esquerdo e "fora da tela" onde a posição da image é "resetada". A ecala das imagens devem ser consideradas.
	public float stopPointX;

	///A largura da image. A escala deve ser considera.
	public float imageWidth;

	///Afeta a velocidade de scroll em relaçao à velocidade do player. (1 igual 100% da velocidade). 
	public float relativeSpeed = 1.0f;

	public float factor = 10f;

	///As imagens (sprites) que serão scrolladas. Devem estar organizadas (lado a lado).
	protected List<GameObject> imagesQueue;

	///Velocidade de scroll.
	private float speed;

	private float camPosOld;

	

	void Start () {
		camPosOld = Camera.main.transform.position.x;

		imagesQueue = new List<GameObject> ();

		foreach(Transform child in transform){

			imagesQueue.Add(child.gameObject);
		}
	}

	/// Update is called once per frame
	protected virtual void Update () {

		if(imagesQueue.Count > 0){

			///calcula a velocidade de scroll.
			speed = (Camera.main.transform.position.x - camPosOld)* factor * relativeSpeed;
			Debug.Log(speed);
			ScrollWithSpeed (speed);

			UpdateImagesQueue ();
		}
		camPosOld = Camera.main.transform.position.x;
	}

	///move todas as sprites de acordo com a velocidade.
	protected void ScrollWithSpeed(float speed) {

		foreach(GameObject image in imagesQueue){
			
			image.transform.position = image.transform.position - new Vector3(speed * Time.deltaTime, 0, 0);
		}
	}

	///Atualiza a fila com as imagens scroladas.
	protected void UpdateImagesQueue() {

		///primeiro da fila
		GameObject f = imagesQueue[0];

		///checa se o primeiro da fila chegou ao ponto aonde deve retornar para o final da fila.
		if (f.transform.position.x <= stopPointX){
			///remove o primeiro
			imagesQueue.Remove(f);
			///calcula a nova posicao do elemento removido da fila
			GameObject last = imagesQueue[imagesQueue.Count - 1];
			float np = last.transform.position.x + imageWidth;
			///seta a nova posicao
			f.transform.position = new Vector3(np, f.transform.position.y, f.transform.position.z);
			
			///adiciona o elemento no final da fila.
			imagesQueue.Add(f);
		}

	}

}
*/