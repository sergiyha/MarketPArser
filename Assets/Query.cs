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
	public InputField inputField;
	public Button searchButton;

	public event Action NotFound;
	public event Action OnNothingToFind;
	public event Action OnSearch;

	private const string itemUrl = "http://hard.rozetka.com.ua/****/p****/";

	private string CreateItemLink(string itemId)
	{
		string link = itemUrl;
		return link.Replace("****", itemId);
	}


	void Start()
	{
		searchButton.onClick.AddListener(Search);
	}


	public void Search()
	{
		if (!string.IsNullOrEmpty(inputField.text))
		{
			ExecuteEvent(OnSearch);
		}
		else
		{
			ExecuteEvent(OnNothingToFind);
		}
	}


	/// <summary>
	/// For StartSearch
	/// </summary>
	/// 
	private void DebugOnStart()
	{
		Debug.Log("start searching");
	}


	private void StartSearchingRequest()
	{
		StartCoroutine(StartSearchRequest(inputField.text));
	}


	IEnumerator StartSearchRequest(string input)
	{
		WWW req = new WWW(new RozetkaSearchingLinkCreator(input).htmlQuery);
		yield return req;
		if (req.error == null)
		{
			string html = System.Text.Encoding.UTF8.GetString(req.bytes);
			var r = new RozetkaSearchingParser(html);
			r.parseThreadTrigger.WaitOne();

			if (r.hasError)
			{
				ExecuteEvent(OnNothingToFind);
			}
			else
			{

				RozetkaItemCreator items = new RozetkaItemCreator(r.resuls);
			
			}
		}
	}


	/// <summary>
	/// On Nothing ToFind
	/// </summary>
	private void DebugNothingToFind()
	{
		Debug.Log("nothing to find");
	}


	/// <summary>
	/// ON NOTFOUND
	/// </summary>
	private void DebugOnError()
	{
		Debug.Log("query incorrect, NOT FOUND");
	}


	/// <summary>
	/// Additional functions
	/// </summary>
	private bool CheckActionIfExist(Action handler)
	{
		if (handler != null)
		{
			return true;
		}
		else
		{
			return false;
		}
	}


	private void ExecuteEvent(Action action)
	{
		CheckActionIfExist(action);
		action();
	}


}