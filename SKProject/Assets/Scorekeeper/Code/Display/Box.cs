using UnityEngine;
using System;
using System.Collections.Generic;

public class Box : FContainer, FSmartTouchableInterface
{
	public List<BoxSprite> boxSprites = new List<BoxSprite>();
	public List<FSprite> contentSprites = new List<FSprite>();
	public FContainer contentContainer;

	protected Player _player;

	protected Cell _baseCell = new Cell();
	protected Cell _currentCell = new Cell();
	protected Cell _targetCell = new Cell();

	protected float _tweenTimeElapsed;
	protected float _tweenTimeTotal;

	protected float _percent = 0.0f;

	protected bool _isEnabled = true;

	public Action SignalPress;
	public Action SignalRelease;
	public Action SignalReleaseOutside;

	private float _innerAlpha = 1.0f;

	public Box()
	{

	}
	
	virtual public void Init(Player player)
	{
		BoxSprite boxSprite = new BoxSprite();
		boxSprites.Add(boxSprite);
		AddChild(boxSprite);

		AddChild(contentContainer = new FContainer());

		_player = player;
		UpdatePlayer();

		ListenForAfterUpdate(HandleAfterUpdate);
		EnableSmartTouch();
	}

	void HandleAfterUpdate ()
	{
		if(_targetCell.didHaveMajorChange)
		{
			_baseCell = _currentCell.Clone();
			_tweenTimeElapsed = Mathf.Max(0.0f, _tweenTimeElapsed-0.3f);
			_tweenTimeTotal = Mathf.Max(0.3f, _tweenTimeTotal); //if there has been a major change, at least tween a bit
		}

		if(_tweenTimeElapsed < _tweenTimeTotal)
		{
			_tweenTimeElapsed += Time.deltaTime;
		}

		UpdatePosition();
	}

	virtual public void UpdatePosition()
	{
		_percent = Mathf.Clamp01(_tweenTimeElapsed / _tweenTimeTotal);

		if(_percent > 0 && _percent < 1.0f)
		{
			_percent = RXEase.ExpoInOut(_percent);
		}

		_currentCell.SetInterpolated(_baseCell,_targetCell,_percent);

		this.x = _currentCell.x;
		this.y = _currentCell.y;
		boxSprites[0].width = _currentCell.width;	
		boxSprites[0].height = _currentCell.height;
		this.rotation = _currentCell.rotation;
		this.alpha = _currentCell.alpha * _innerAlpha;
	}

	protected void UpdatePlayer ()	
	{
		boxSprites.ForEach(boxSprite => {boxSprite.color = _player.color.color;});
	}

	public void GoToCellTweened(Cell cell, float tweenTime)
	{
		_tweenTimeTotal = tweenTime;
		_tweenTimeElapsed = 0.0f;
		_baseCell = _currentCell.Clone();
		_targetCell = cell;
	}

	public void GoToCellInstantly(Cell cell)
	{
		_tweenTimeTotal = 0.0f;
		_tweenTimeElapsed = 0.001f;
		_baseCell = _currentCell.Clone();
		_targetCell = cell;
		UpdatePosition();
	}

	#region FSmartTouchableInterface implementation

	bool FSmartTouchableInterface.HandleSmartTouchBegan (int touchIndex, FTouch touch)
	{
		if(!_isEnabled) return false;
		if(touchIndex > 0) return false; //we only want the first touch for now

		if(_currentCell.GetLocalRect().Contains(GetLocalTouchPosition(touch)))
		{
			Keeper.instance.CreateEffect(this,Config.PADDING_S);
			if(SignalPress != null) SignalPress();
			return true;
		}
		else 
		{
			return false;
		}
	}

	void FSmartTouchableInterface.HandleSmartTouchMoved (int touchIndex, FTouch touch)
	{
	}

	void FSmartTouchableInterface.HandleSmartTouchEnded (int touchIndex, FTouch touch)
	{
		if(_currentCell.GetGlobalRect().Contains(GetLocalTouchPosition(touch)))
		{
			if(SignalRelease != null) SignalRelease();
		}
		else 
		{
			if(SignalReleaseOutside != null) SignalReleaseOutside();
		}
	}

	void FSmartTouchableInterface.HandleSmartTouchCanceled (int touchIndex, FTouch touch)
	{
		if(SignalReleaseOutside != null) SignalReleaseOutside();
	}

	#endregion

	private void UpdateEnabled ()
	{
		if(_isEnabled)
		{
			_innerAlpha = 1.0f;
		}
		else 
		{
			_innerAlpha = 0.2f;
		}
	}

	public Player player 
	{
		get {return _player;}
		set 
		{
			if(_player != value)
			{
				_player = value;
				UpdatePlayer();
			}
		}
	}

	public Cell currentCell
	{
		get {return _currentCell;}
	}

	public Cell baseCell
	{
		get {return _baseCell;}
	}

	public Cell targetCell
	{
		get {return _targetCell;}
	}

	public bool isEnabled
	{
		get {return _isEnabled;}
		set 
		{
			if(_isEnabled != value)
			{
				_isEnabled = value;
				UpdateEnabled();
			}
		}
	}
}

