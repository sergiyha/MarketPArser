using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RozetkaSearchingLinkCreator
{
	private const string searchLink = "http://rozetka.com.ua/search/?section_id=&section=&text=";
	public string htmlQuery;

	public RozetkaSearchingLinkCreator(string searchingInput)
	{
		CreateHtmlQueryAccordingToSearchingInput(searchingInput);
	}

	void CreateHtmlQueryAccordingToSearchingInput(string searchingInput)
	{
		string htmlCorrectInput = searchingInput.Replace(" ", "+");
		htmlQuery = searchLink + htmlCorrectInput;
	}
}
