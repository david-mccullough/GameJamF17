using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoverAndSelector : MonoBehaviour {

	public float speed = 10.5f;
	public float nodeStoppingSpace = 1.0f;
	public List<GameObject> nodes;

	private Vector3 target;
	public List<GameObject> selected = new List<GameObject>();
	public List<GameObject> moving = new List<GameObject>();
	public bool selectionInProgress = false;

	void Start () {
	}

	void Update()
	{
		if( Input.GetMouseButtonDown(0) )
		{
			Collider2D clicked_collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if(clicked_collider == null) //Didn't select a game object or node
			{
				selectedBlankSpace();
			}
			else{ //Selected a game object
				selectedUnit(clicked_collider);
			}
		}

		moveUnits();

	}

	void selectedBlankSpace(){
		selectionInProgress = false;

		foreach(var unit in selected){
			moving.Add(unit);
		}

		foreach(var unit in selected){
			unit.GetComponent<SpriteRenderer>().color = Color.white;
		}

		target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target.z = transform.position.z;

		selected.Clear();

	}

	void removeSelected(GameObject unit){
		unit.GetComponent<SpriteRenderer>().color = Color.white;
	}

	void selectedUnit(Collider2D clicked_collider){

		selectionInProgress = true;
		GameObject clicked = clicked_collider.gameObject;

		if (selected.Contains(clicked)){
			removeSelected(clicked);
		}

		else {
			selected.Add(clicked);
			clicked.GetComponent<SpriteRenderer>().color = Color.red;
		}
	}

	void moveUnits(){

		if(!selectionInProgress){
			for(int i = 0; i < moving.Count; i++) {
				moving[i].transform.position = Vector3.MoveTowards(moving[i].transform.position, target, speed * Time.deltaTime);
				if(Vector3.Distance(moving[i].transform.position,target) < nodeStoppingSpace){
					moving.Remove(moving[i]);
				}
			}
		}

	}


}
