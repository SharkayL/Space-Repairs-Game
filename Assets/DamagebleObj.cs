using UnityEngine;
using System.Collections;

public class  DamagebleObj : MonoBehaviour {

	private IDamage reciver;

	public void RegisterReciver(IDamage rec){
		this.reciver=rec;
	}

	public void DamageBy(int hits){
		reciver.DamageBy(hits);
	}
}

public interface IDamage{

	void DamageBy(int hits);


}