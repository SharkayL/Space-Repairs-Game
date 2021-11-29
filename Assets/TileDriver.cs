using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TileLinks{
	public TileHandler up;
	public TileHandler down;
	public TileHandler left;
	public TileHandler right;
	public TileHandler up_left;
	public TileHandler up_right;
	public TileHandler down_left;
	public TileHandler down_right;

}

public class TileHandler{


	private GameObject current;
	private int gridX;
	private int gridY;

	public TileHandler(int x,int y,GameObject gameObject){
		gridX=x;
		gridY=y;
		current=gameObject;
	}

	public int GridY {
		get {
			return gridY;
		}
	}

	public int GridX {
		get {
			return gridX;
		}
	}
}

public interface ITileDriver{
	bool IsBulk();
	TileHandler GetTileHandler();

}

public class TileDriver : MonoBehaviour, ITileDriver {


	private GameObject decoration;
	private bool canDecorate;
	private TileHandler handler;

	public void Init(int x,int y){
		handler = new TileHandler(x,y,this.gameObject);
	}

	void AddDecor(){

	}

	// Use this for initialization
	void Start () {
	
	}

	void Setup(IList<TileDriver> otherTiles){

	}

	public void Place(int x,int y,ArenaProvider provider){
		this.Init(x,y);
		Vector2 position = provider.GetPosition(x,y);
		Vector3 newPosition = this.transform.position;
		newPosition.Set(position.x,position.y,provider.GetHeight());
		this.transform.position=newPosition;
		Quaternion quat = transform.rotation;
		quat.eulerAngles= new Vector3 (90f, 0, 180);
		this.transform.rotation = quat;

		TableAccesor<TileHandler> table = provider.GetTilesTable();
		table.Set(handler,x,y);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public bool IsBulk(){
		return false;
	}

	public TileHandler GetTileHandler(){
		return handler;
	}
}
