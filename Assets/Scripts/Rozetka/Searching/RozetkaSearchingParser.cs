using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RozetkaSearchingParser
{
	public ManualResetEvent parseThreadTrigger;
	event Action OnParseThreadStart;
	public Thread parserThread;
	HtmlDocument fullDocument;
	public List<string> resuls;
	public bool hasError;

	public RozetkaSearchingParser(string searchedHtml)
	{
		parseThreadTrigger = new ManualResetEvent(false);
		fullDocument = CreateSearchingHtmlDoc(searchedHtml);
		OnParseThreadStart += ParseThreadStart;
		hasError = false;


		parserThread = new Thread(new ThreadStart(OnParseThreadStart));
		parserThread.Start();
	}


	private void ParseThreadStart()
	{
		resuls = CreateListOfSearchedItems(fullDocument);
		parseThreadTrigger.Set();
		parserThread.Abort();
	}



	private HtmlDocument CreateSearchingHtmlDoc(string html)
	{
		HtmlDocument doc = new HtmlDocument();
		doc.LoadHtml(html);
		return doc;
	}


	private List<string> CreateListOfSearchedItems(HtmlDocument doc)
	{
		var listOfValues = new List<string>();
		for (int i = 2; i < 12; i++)
		{

			var node = doc.DocumentNode.SelectSingleNode(" //*[@id=\"block_with_search\"]/div/div[" + i + "]/div/div/div/input");
			if (node == null)
			{
				Debug.Log("query incorrect");
				hasError = true;
				break;
				//continue;
			}
			var value = node.Attributes["name"].Value;
			listOfValues.Add(value);
			Debug.Log(value);
		}
		return listOfValues;
	}
}
