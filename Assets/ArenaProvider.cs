using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WeightedEntry<T>{
	public WeightedEntry(T entry,float weight){
		this.value = entry;
		this.weight = weight;
	}

	public T value;
	public float weight;
}

public class WeigthedProvider<T>{

	private List<WeightedEntry<T>> data = new List<WeightedEntry<T>>();

	private float totalWeight=0;

	public void AddEntry(T entry,float weight){
		data.Add(new WeightedEntry<T>(entry,weight));
		totalWeight += weight;
	}

	public T Get(){
		float value = Random.Range (0, totalWeight);
		float acc = 0;
		foreach (WeightedEntry<T> entry in data) {
			acc+=entry.weight;
			if(acc>value){
				return entry.value;
			}
		}
		return default(T);
	}

}

public class TableAccesor<T>{
	private List<List<T>> rows;

	public TableAccesor(int resX,int resY){
		rows = new List<List<T>>(resY);
		for(int i=0;i<resY;i++){
			List<T> row = new List<T>(resX);
			rows.Add(row);
			for(int j=0;j<resX;j++){
				row.Add(default(T));
			}
			//rows.Add
		}
	}

	public void Set(T entry,int x,int y){
		if(rows.ElementAtOrDefault(y)==null){
//			rows[y] = new List<T>();
			return;
		}
		List<T> row = rows[y];
		row[x] = entry;
	}

	public T Get(int x,int y){
		List<T> row = rows.ElementAtOrDefault(y);
		if(row==null){
			return default(T);
		}
		return row.ElementAtOrDefault(x);
	}

}

public class ArenaProvider : MonoBehaviour {

	private List<TileDriver> tiles;
	private GameObject tileObj;
	private Quaternion orientation;
	private float radius=2f;
	private float radial;
	private float offsetX=-50f;
	private float offsetY=-50f;
	private float height=1f;
	public int rowCount = 70;
	public int columnCount = 100;
//	private List<Texture> textures = new List<Texture> ();
	private WeigthedProvider<Texture[]> textures = new WeigthedProvider<Texture[]> ();
	private TableAccesor<TileHandler> arenaTable;
	public GameObject depotObject;
	public GameObject powerObject;
	public GameObject dispenserObject;
	public GameObject endTile;
	private int edgeCount = 20;
	public static IBuildOrder order;

	public static ArenaProvider active;

	void Awake(){
		ArenaProvider.active=this;
	}

	// Use this for initialization
	void Start () {

		tiles = new List<TileDriver>(rowCount*columnCount);
		orientation.eulerAngles = new Vector3 (-180f, 0, 0);
		arenaTable = new TableAccesor<TileHandler>(columnCount,rowCount);
		radial = (Mathf.Sqrt (3)/2)*radius;
		offsetX = -radius * 1.5f * columnCount / 2+radial;
		offsetY = -radial * rowCount+radial/2;

		depotObject = Instantiate( Resources.Load<GameObject>("Depo")) as GameObject;
		depotObject.transform.rotation=orientation;
		depotObject.SetActive(true);
		BulkTileDriver depotDriver = depotObject.GetComponent<BulkTileDriver>();
		depotDriver.Place(columnCount/2,rowCount/2,this);
	
		tileObj = Resources.Load<GameObject>("TileO");
		powerObject = Resources.Load<GameObject>("Power");
		dispenserObject = Resources.Load<GameObject>("BarrelDispenser");
		//endTile = Resources.Load<GameObject>("Modles/edge_tile");

		textures.AddEntry (LoadTex("Tiles/New Tile1"),20);
		textures.AddEntry (LoadTex("Tiles/New Tile2"),20);
		textures.AddEntry (LoadTex("Tiles/New Tile3"),20);
		
		//textures.AddEntry (Resources.Load ("Tiles/New Tile2") as Texture,5);
		//textures.AddEntry (Resources.Load ("Tiles/Tile3") as Texture,20);
		//textures.AddEntry (Resources.Load ("Tiles/Tile4") as Texture,0.5f);
		//textures.AddEntry (Resources.Load ("Tiles/Tile5") as Texture,15);
		//textures.AddEntry (Resources.Load ("Tiles/Tile6") as Texture,15);
		//textures.AddEntry (Resources.Load ("Tiles/Tile7") as Texture,0.5f);
		Random.seed = 42;

		if(order!=null){
			order.Build();
		}

		this.loadTile ();


		GameObject barier = Resources.Load<GameObject>("Border");

		GameObject topBorder = Instantiate(barier) as GameObject;
		topBorder.SetActive(true);
		topBorder.transform.position = (Vector3)GetPosition(columnCount/2,rowCount-1)+new Vector3(0,radial,0);
		topBorder.transform.localScale = new Vector3(radius*columnCount*2f,1,1);

		GameObject botBorder = Instantiate(barier) as GameObject;
		botBorder.SetActive(true);
		botBorder.transform.position = (Vector3)GetPosition(columnCount/2,0);
		botBorder.transform.localScale = new Vector3(radius*columnCount*2f,1,1);

		GameObject rightBorder = Instantiate(barier) as GameObject;
		rightBorder.SetActive(true);
		rightBorder.transform.position = (Vector3)GetPosition(columnCount-1,rowCount/2)+new Vector3(radial,0,0);
		rightBorder.transform.localScale = new Vector3(1,radius*rowCount*2f,1);

		GameObject leftBorder = Instantiate(barier) as GameObject;
		leftBorder.SetActive(true);
		leftBorder.transform.position = (Vector3)GetPosition(0,rowCount/2)-new Vector3(radial,0,0);
		leftBorder.transform.localScale = new Vector3(1,radius*rowCount*2f,1);
		//Generate Tiles here
		//GameObject tileObject = tiles [0];
		for(int y=0;y<rowCount;y++){
			float vertOffset=radial*2*y+offsetY;
			for(int x=0;x<columnCount;x++){
				if(arenaTable.Get(x,y)!=null){
					continue;
				}
				float offset=x*1.5f*radius+offsetX;
				if(x%2==0){
					TileDriver tile = placeTile (tileObj, offset, vertOffset).GetComponent<TileDriver>();
					tile.Init(x,y);
					arenaTable.Set(tile.GetTileHandler(),x,y);
					tiles.Add(tile);
				}
				else{
					TileDriver tile = placeTile (tileObj, offset, radial+vertOffset).GetComponent<TileDriver>();
					tile.Init(x,y);
					arenaTable.Set(tile.GetTileHandler(),x,y);
					tiles.Add (tile);
				}
			}
		}

		for(int y=0-edgeCount;y<0;y++){
			for(int i=0-edgeCount;i<rowCount+edgeCount;i++){
				Vector2 position = GetPosition(y,i);

				GameObject obj = (GameObject)Instantiate(endTile,new Vector3(position.x,position.y,height),orientation);
				obj.SetActive(true);

				position = GetPosition(y+columnCount+edgeCount,i);
				
				obj = (GameObject)Instantiate(endTile,new Vector3(position.x,position.y,height),orientation);
				obj.SetActive(true);
			}

			for(int i=0;i<columnCount;i++){
				Vector2 position = GetPosition(i,y);

				GameObject obj = (GameObject)Instantiate(endTile,new Vector3(position.x,position.y,height),orientation);
				obj.SetActive(true);

				position = GetPosition(i,y+rowCount+edgeCount);
				
				obj = (GameObject)Instantiate(endTile,new Vector3(position.x,position.y,height),orientation);
				obj.SetActive(true);
			}
		}

	}

	void loadTile(){
		tileObj = Resources.Load<GameObject>("TileO");
		//newTile.AddComponent ("TileHandler");
		tileObj.SetActive (false);
	}

	GameObject placeTile(GameObject tile,float x,float y){
		GameObject placedTile = (GameObject)Instantiate (tile, new Vector3 (x, y, height), orientation);
		Quaternion tempQuat = placedTile.transform.rotation;
		Vector3 angles = tempQuat.eulerAngles;
		angles.z=Mathf.RoundToInt(Random.Range(0,6))*60f;
		tempQuat.eulerAngles=angles;
		placedTile.transform.rotation=tempQuat;

		//placedTile.GetComponent<Renderer> ().material.mainTexture = (textures [(int)Mathf.Round (Random.Range (0, 7))]);
		Texture[] tex = textures.Get ();
		Material mat = placedTile.GetComponent<Renderer> ().material;
		mat.mainTexture=tex[0];
		mat.SetTexture("_Illum",tex[1]);

		//placedTile.GetComponent<Renderer> ().material.
		placedTile.SetActive (true);
		return placedTile;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public TableAccesor<TileHandler> GetTilesTable(){
		return arenaTable;
	}

	public Vector2 GetPosition(int x,int y){
		float posX=x*1.5f*radius+offsetX;
		float posY=radial*2*y+offsetY+radial*(x%2);
		return new Vector2(posX,posY);
	}

	public float GetTileRadius() {
		return radius;
	}

	public float GetHeight(){
		return height;
	}

	public TileDriver GetRandomTile(){
		int index = Mathf.RoundToInt(Random.Range(0,tiles.Count));
		return tiles[index];
	}

	public Texture[] LoadTex(string path){
		Texture[] texArray = new Texture[2];
		texArray[0] = Resources.Load(path) as Texture;
		texArray[1] = Resources.Load(path+"A") as Texture;
		return texArray;
	}

	public BulkTileDriver placeBulkTile(GameObject tile,int x,int y){
		GameObject obj = Instantiate(tile) as GameObject;
		obj.SetActive(true);
		BulkTileDriver objDriver = obj.GetComponent<BulkTileDriver>();
		objDriver.Place(x,y,this);
		return objDriver;
	}

	public TileDriver placeTile(GameObject tile,int x,int y){
		GameObject obj = Instantiate(tile) as GameObject;
		obj.SetActive(true);
		TileDriver objDriver = obj.GetComponent<TileDriver>();
		objDriver.Place(x,y,this);
		return objDriver;
	}

	public void Stitch(){

	}
}
