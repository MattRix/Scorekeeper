using UnityEngine;
using System;
using System.Collections.Generic;

public class BorderBox : FMeshNode
{
	private float _width;
	private float _height;
	private float _borderThickness;
	private FMeshVertex[] _innerVertices;
	private FMeshVertex[] _outerVertices;
	private float _anchorX = 0.5f;
	private float _anchorY = 0.5f;

	public BorderBox(float width, float height, float borderThickness) : this(width,height,borderThickness, Futile.atlasManager.GetElementWithName("Box"))
	{

	}

	public BorderBox(float width, float height, float borderThickness, FAtlasElement element) : base(FFacetType.Quad, element)
	{
		_width = width;
		_height = height;
		_borderThickness = borderThickness;

		_innerVertices = new FMeshVertex[]{new FMeshVertex(0,0,0,1),new FMeshVertex(0,0,1,1),new FMeshVertex(0,0,1,0),new FMeshVertex(0,0,0,0)};
		_outerVertices = new FMeshVertex[]{new FMeshVertex(0,0,0,1),new FMeshVertex(0,0,1,1),new FMeshVertex(0,0,1,0),new FMeshVertex(0,0,0,0)};

		//_innerVertices = new FMeshVertex[]{new FMeshVertex(),new FMeshVertex(),new FMeshVertex(),new FMeshVertex()};
		//_outerVertices = new FMeshVertex[]{new FMeshVertex(),new FMeshVertex(),new FMeshVertex(),new FMeshVertex()};

		_meshData.AddQuad(_outerVertices[0],_outerVertices[1],_innerVertices[1],_innerVertices[0]);
		_meshData.AddQuad(_outerVertices[1],_outerVertices[2],_innerVertices[2],_innerVertices[1]);
		_meshData.AddQuad(_outerVertices[2],_outerVertices[3],_innerVertices[3],_innerVertices[2]);
		_meshData.AddQuad(_outerVertices[3],_outerVertices[0],_innerVertices[0],_innerVertices[3]);

		UpdateMesh();
	}

	void UpdateMesh ()
	{
		float leftX = _width*-_anchorX;
		float rightX = leftX + _width;
		float bottomY = _height*-_anchorY;
		float topY = bottomY + _height;

		_outerVertices[0].SetPos(leftX,topY);
		_outerVertices[1].SetPos(rightX,topY);
		_outerVertices[2].SetPos(rightX,bottomY);
		_outerVertices[3].SetPos(leftX,bottomY);

		leftX = leftX + _borderThickness;
		rightX = rightX - _borderThickness;
		bottomY = bottomY + _borderThickness;
		topY = topY - _borderThickness;

		_innerVertices[0].SetPos(leftX,topY);
		_innerVertices[1].SetPos(rightX,topY);
		_innerVertices[2].SetPos(rightX,bottomY);
		_innerVertices[3].SetPos(leftX,bottomY);

		_meshData.MarkChanged();
	}
	
	public void SetSize(float width, float height)
	{
		_width = width;
		_height = height;
		UpdateMesh();
	}

	public float borderThickness
	{
		get { return _borderThickness;}
		set { if(_borderThickness != value) {_borderThickness = value; UpdateMesh();}}
	}

	public float width
	{
		get { return _width;}
		set { if(_width != value) {_width = value; UpdateMesh();}}
	}

	public float height
	{
		get { return _height;}
		set { if(_height != value) {_height = value; UpdateMesh();}}
	}

	virtual public float anchorX 
	{
		get { return _anchorX;}
		set { if(_anchorX != value) {_anchorX = value; UpdateMesh();}}
	}
	
	virtual public float anchorY 
	{
		get { return _anchorY;}
		set { if(_anchorY != value) {_anchorY = value; UpdateMesh();}}
	}
	
	//for convenience
	public void SetAnchor(float newX, float newY)
	{
		_anchorX = newX;
		_anchorY = newY;
		UpdateMesh();
	}
	
	public void SetAnchor(Vector2 newAnchor)
	{
		_anchorX = newAnchor.x;
		_anchorY = newAnchor.y;
		UpdateMesh();
	}
	
	public Vector2 GetAnchor()
	{
		return new Vector2(_anchorX,_anchorY);	
	}
	
}

