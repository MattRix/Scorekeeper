using UnityEngine;
using System;
using System.Collections.Generic;


public class MathBox : RepeatableBox
{
	public Slot slot = null;
	public MathType mathType;
	public MathSprite mathSprite;
	
	public MathBox(Slot slot, MathType mathType)
	{
		this.slot = slot;
		this.mathType = mathType;

		this.hasHyperRepeatZones = true;
		
		base.Init(slot.player);
		
		Setup();
	}

	public MathBox(MathType mathType) //for use outside the slot
	{
		this.mathType = mathType;
		
		this.hasHyperRepeatZones = false;
		
		base.Init(Player.NullPlayer);
		
		Setup();
	}

	void Setup()
	{
		if(mathType == MathType.Plus)
		{
			normalSoundName = "UI/Add";
			fastSoundName = "UI/AddFast";
			fastestSoundName = "UI/AddFastest";
			contentContainer.AddChild(mathSprite = new MathSprite("Icons/Plus"));
		}
		else 
		{
			normalSoundName = "UI/Subtract";
			fastSoundName = "UI/SubtractFast";
			fastestSoundName = "UI/SubtractFastest";
			contentContainer.AddChild(mathSprite = new MathSprite("Icons/Minus"));
		}
		
		mathSprite.color = Color.black;
	}
	
	public float GetNeededWidth()
	{
		return slot.height;
	}
}

public enum MathType
{
	Plus,
	Minus
}

