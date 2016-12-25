using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.UI;
using System;
using HtmlAgilityPack;
using System.Threading;

public class Query : MonoBehaviour
{

	//private string url = "http://rozetka.com.ua/12203892/p12203892/";
	private const string searchLink = "http://rozetka.com.ua/search/?section_id=&section=&text=samsung+1++++2";
	private const string itemUrl = "http://hard.rozetka.com.ua/****/p****/";
	private const string itemSearchMarker = "{name: 'eventPosition', value: '_valueToChange_'}";//заменять '_valueToChange_'
																								//private string[] itemsSubStrings = new string[6];

	private string CreateItemLink(string itemId)
	{
		string link = itemUrl;
		return link.Replace("****", itemId);
	}

	InputField inputField;

	void Start()
	{
		Search();
	}

	public void Search()
	{
		StartCoroutine(wait());
	}

	IEnumerator wait()
	{
		yield return new WaitForSeconds(3);
		StartCoroutine(StartSearchRequest("samsung 1 2"));
	}



	IEnumerator StartSearchRequest(string input)
	{

		WWW req = new WWW(new RozetkaSearchingLinkCreator(input).htmlQuery);
		yield return req;
		if (req.error == null)
		{
			var r = new RozetkaSearchingParser(System.Text.Encoding.UTF8.GetString(req.bytes));
			r.parseThreadTrigger.WaitOne();
			Debug.Log(r.resuls.Count);
		}
	}
}