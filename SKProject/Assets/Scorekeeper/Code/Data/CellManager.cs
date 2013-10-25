using UnityEngine;
using System;
using System.Collections.Generic;

public class CellManager
{
	public static List<Cell> allCells = new List<Cell>();

	public static Cell megaNewPlayer = new Cell();
	public static Cell megaTimer = new Cell();
	public static Cell megaSort = new Cell();
	public static Cell megaReset = new Cell();
	public static Cell megaSettings = new Cell();

	public static Cell middleCell = new Cell();
	
	public static void Recalculate()
	{
		Config.Setup();
		SetupMega();
	}

	static void SetupMega ()
	{
		float padding = Config.PADDING_M;
		float fullHeight = Mathf.Round(Config.HEIGHT - padding*2);
		float fullWidth = Mathf.Round(Config.MEGA_WIDTH - padding*2);
		float halfHeight = Mathf.Round((fullHeight - padding)/2.0f);
		float thirdHeight = Mathf.Round((fullHeight - padding*2)/3.0f);
		
		megaNewPlayer.SetSize(fullWidth,halfHeight);
		megaNewPlayer.SetPosition(-Config.HALF_WIDTH + megaNewPlayer.width/2 + padding,Config.HALF_HEIGHT-megaNewPlayer.height/2-padding);
		allCells.Add(megaNewPlayer);
		
		megaTimer.SetSize(fullWidth,halfHeight);
		megaTimer.SetPosition(-Config.HALF_WIDTH + megaTimer.width/2 + padding,-Config.HALF_HEIGHT+megaTimer.height/2+padding);
		allCells.Add(megaTimer);
		
		megaSort.SetSize(fullWidth,thirdHeight);
		megaSort.SetPosition(Config.HALF_WIDTH - megaSort.width/2-padding,Config.HALF_HEIGHT-megaSort.height/2-padding);
		allCells.Add(megaSort);
		
		megaReset.SetSize(fullWidth,thirdHeight);
		megaReset.SetPosition(Config.HALF_WIDTH - megaReset.width/2-padding,0);
		allCells.Add(megaReset);
		
		megaSettings.SetSize(fullWidth,thirdHeight);
		megaSettings.SetPosition(Config.HALF_WIDTH - megaSettings.width/2-padding,-Config.HALF_HEIGHT+megaSettings.height/2+padding);
		allCells.Add(megaSettings);

		middleCell.SetSize(Futile.screen.halfWidth, Futile.screen.halfHeight);
		allCells.Add(middleCell);
	}

	public static void Refresh ()
	{
		for(int c = 0; c<allCells.Count; c++)
		{
			allCells[c].didHaveMajorChange = false;
		}
	}
}

