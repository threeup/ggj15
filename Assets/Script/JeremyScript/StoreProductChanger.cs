using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoreProductChanger : MonoBehaviour {

	public enum StoreStates{
		intro,
		noProduct,
		shell,
		noProduct2,
		wings,
		finished
	};

	public string product1Name;
	public int product1Price;
	public Sprite product1Sprite;
	public string product2Name;
	public int product2Price;
	public Sprite product2Sprite;

	public Sprite empty;

	public Image product;
	public Text pName;
	public Text price;

	public StoreStates currentState;


	public void updateProduct()
	{
		if(currentState==StoreStates.intro)
		{
			//Nothing
			product.sprite = empty;
			pName.text = "SOLD\nOUT";
			price.text = "";
		}else if(currentState==StoreStates.noProduct){
			product.sprite = empty;
			pName.text = "SOLD\nOUT";
			price.text = "";
		}else if(currentState==StoreStates.shell){
			product.sprite = product1Sprite;
			pName.text = product1Name;
			price.text = product1Price.ToString();
		}else if(currentState==StoreStates.noProduct2){
			product.sprite = empty;
			pName.text = "SOLD\nOUT";
			price.text = "";
		}else if(currentState==StoreStates.wings){
			product.sprite = product2Sprite;
			pName.text = product2Name;
			price.text = product2Price.ToString();
		}else if(currentState==StoreStates.finished){
			product.sprite = empty;
			pName.text = "SOLD\nOUT";
			price.text = "";
		}
	}
}
