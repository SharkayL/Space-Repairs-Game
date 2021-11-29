using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulkTileDriver : MonoBehaviour, ITileDriver {

	public string name;
	private TileHandler center;
	private List<TileHandler> tiles;

	// Use this for initialization
	void Start () {

	}

	public void Place(int x,int y,ArenaProvider provider){
		tiles = new List<TileHandler>(6);
		Vector2 position = provider.GetPosition(x,y);
		Vector3 newPosition = this.transform.position;
		newPosition.Set(position.x,position.y,provider.GetHeight());
//		Debug.Log(newPosition);
		this.transform.position=newPosition;
		Quaternion quat = transform.rotation;
		quat.eulerAngles= new Vector3 (-180f, 0, 180);
		this.transform.rotation = quat;

		if(x%2!=0){
		tiles.Add(new TileHandler(x-1,y+1,this.gameObject));
		tiles.Add(new TileHandler(x,y+1,this.gameObject));
		tiles.Add(new TileHandler(x+1,y+1,this.gameObject));
		tiles.Add(new TileHandler(x-1,y,this.gameObject));
		center = new TileHandler(x,y,this.gameObject);
		tiles.Add (center);
		tiles.Add (new TileHandler(x+1,y,this.gameObject));
		tiles.Add (new TileHandler(x,y-1,this.gameObject));
		}
		else{
			tiles.Add(new TileHandler(x-1,y,this.gameObject));
			tiles.Add(new TileHandler(x,y+1,this.gameObject));
			tiles.Add(new TileHandler(x+1,y,this.gameObject));
			tiles.Add(new TileHandler(x-1,y-1,this.gameObject));
			center = new TileHandler(x,y,this.gameObject);
			tiles.Add (center);
			tiles.Add (new TileHandler(x+1,y-1,this.gameObject));
			tiles.Add (new TileHandler(x,y-1,this.gameObject));
		}
		TableAccesor<TileHandler> table = provider.GetTilesTable();
		foreach(TileHandler tile in tiles){
			table.Set(tile,tile.GridX,tile.GridY);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public bool IsBulk(){
		return true;
	}

	public TileHandler GetTileHandler(){
		return center;
	}
	
}
