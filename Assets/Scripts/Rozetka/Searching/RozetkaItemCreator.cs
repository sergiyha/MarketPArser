using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RozetkaItemCreator : MonoBehaviour {


	private const string itemUrl = "http://hard.rozetka.com.ua/****/p****/";
	private List<string> itemsIds;

	public RozetkaItemCreator(List<string> result)
	{
		GetSids(result);
	}


	private void GetSids(List<string> result)
	{
		foreach (var item in result)
		{
			string resultString = Regex.Match(item, @"\d+").Value;
			itemsIds.Add(resultString);
		}
	}


	//private void ()
	////*[@id="image_item699334"]/a/img
	////*[@id="image_item9899579"]/a/img

	////*[@id="block_with_search"]/div/div[3]/div/div/div/div/div[2]/a
	//*[@id="block_with_search"]/div/div[4]/div/div/div/div[2]/div[2]/a
	//mini
	//*[@id="offers"]/div/ul/li[1]/a/img

	//*[@id="offers"]/div/ul/li[1]/a/img



}
