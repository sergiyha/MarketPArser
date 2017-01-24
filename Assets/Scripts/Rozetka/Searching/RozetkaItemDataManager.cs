using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

public class RozetkaItemDataManager : MonoBehaviour
{
	Thread createDataThread;
	ManualResetEvent createItemDataTrigger;
	static private RozetkaItemDataManager _instance;
	static public RozetkaItemDataManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<RozetkaItemDataManager>();
				if (_instance == null)
				{
					GameObject obj = new GameObject();
					obj.hideFlags = HideFlags.HideAndDontSave;
					_instance = obj.AddComponent<RozetkaItemDataManager>();
				}
			}
			return _instance;
		}
	}

	void Awake()
	{
		createItemDataTrigger = new ManualResetEvent(false);
	}

	private const string itemUrl = "http://hard.rozetka.com.ua/****/p****/";
	private List<string> itemsId;
	private List<RozetkaItemDataHandler> itemsData;

	public void CreateItems(List<string> result, string html)
	{
		doc = CreateItemDocument(html);
		GetSids(result);
		CreateItemInformation();

	}


	private void GetSids(List<string> result)
	{
		itemsId = new List<string>();
		foreach (var item in result)
		{
			string resultString = Regex.Match(item, @"\d+").Value;
			Debug.Log(resultString);
			itemsId.Add(resultString);
		}
	}

	private void CreateItemInformation()
	{
		itemsData = new List<RozetkaItemDataHandler>();
		foreach (var item in itemsId)
		{

			StartItemDataInformationRequest
			(
				item,
			(itemHandler) =>
			{
				itemHandler.id = item;
				itemsData.Add(itemHandler);
			}
			);
		}
	}



	
	HtmlDocument doc;
	void StartItemDataInformationRequest(string item, Action<RozetkaItemDataHandler> AddItem)
	{
		createDataThread = new Thread(() => { AddItem(CreateItemData(item)); });
		createItemDataTrigger = new ManualResetEvent(false);
		createDataThread.Start();
		createItemDataTrigger.WaitOne();

	}


	private RozetkaItemDataHandler CreateItemData(string itemId)
	{
		var itemData = new RozetkaItemDataHandler
		{	
			id = itemId,
			//image = GetImageLink(doc),
			price = GetPrice(itemId),
			//title = GetTitle(doc)
		};
		createItemDataTrigger.Set();
		return itemData;
	}

	private HtmlDocument CreateItemDocument(string html)
	{
		HtmlDocument doc = new HtmlDocument();
		doc.LoadHtml(html);

		return doc;
	}

	private string GetPrice(string itemId)
	{
		var node = doc.DocumentNode.SelectSingleNode("//*[@id=\"js - product_"+itemId+" - price\"]");
		var value = node.InnerText;
		Debug.Log(value);
		return value;
	}

	private string GetImageLink(HtmlDocument doc)
	{
		var node = doc.DocumentNode.SelectSingleNode("//*[@id=\"base_image\"]");
		var value = node.Attributes["src"].Value;
		Debug.Log(value);
		return value;
	}


	private string GetTitle(HtmlDocument doc)
	{
		var node = doc.DocumentNode.SelectSingleNode("//*[@id=\"head_banner_container\"]//div//div[2]//div//header//div[2]//h1");
		var value = node.InnerText;
		Debug.Log(value);
		return value;
	}





}
