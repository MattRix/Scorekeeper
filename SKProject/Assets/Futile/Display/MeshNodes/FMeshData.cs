
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class FMeshData
{
	public List<FMeshFacet> facets;
	public FFacetType facetType;
	public int version = 0;

	public delegate void UpdateDelegate();
	
	public event UpdateDelegate SignalUpdate;

	public FMeshData()
	{

	}

	public FMeshData(List<FMeshFacet> inFacets)
	{
		facets = inFacets;

		if(facets.Count != 0)
		{
			facetType = facets[0].facetType;
		}

	}

	public FMeshData(params FMeshFacet[] inFacets)
	{
		facets = new List<FMeshFacet>(inFacets);
		
		if(facets.Count != 0)
		{
			facetType = facets[0].facetType;
		}
	}

	public FMeshFacet AddFacet(FMeshFacet facet)
	{
		if(facets == null)
		{
			facets = new List<FMeshFacet>();
			facetType = facet.facetType;
		}

		if(facetType != facet.facetType) //check if the facet type is different from what we already have
		{
			Debug.LogError("You can't mix facet types in FMeshData!");
		}

		facets.Add(facet);

		return facet;
	}

	public FMeshTriangle AddTriangle()
	{
		FMeshTriangle triangle = new FMeshTriangle();
		AddFacet(triangle);
		return triangle;
	}

	public FMeshTriangle AddTriangle(FMeshTriangle triangle)
	{
		AddFacet(triangle);
		return triangle;
	}

	public FMeshQuad AddQuad()
	{
		FMeshQuad quad = new FMeshQuad();
		AddFacet(quad);
		return quad;
	}
	
	public FMeshQuad AddQuad(FMeshQuad quad)
	{
		AddFacet(quad);
		return quad;
	}

	public void MarkChanged()
	{
		version++; //things that use meshdata will check the version
		if(SignalUpdate != null) SignalUpdate();
	}
}

public class FMeshFacet
{
	public FFacetType facetType;

	public FMeshVertex[] vertices;

	public FMeshFacet SetVertex(int index, float x, float y, float u, float v)
	{
		FMeshVertex vertex = vertices[index];

		vertex.x = x;
		vertex.y = y;
		vertex.u = u;
		vertex.v = v;
		
		return this; //for chaining
	}

	public FMeshFacet SetVertex(int index, float x, float y, float u, float v, Color color)
	{
		FMeshVertex vertex = vertices[index];

		vertex.x = x;
		vertex.y = y;
		vertex.u = u;
		vertex.v = v;
		vertex.color = color;
		
		return this; //for chaining
	}

	public FMeshFacet SetVertexPos(int index, float x, float y)
	{
		vertices[index].x = x;
		vertices[index].y = y;
		return this; //for chaining
	}

	public FMeshFacet SetVertexPos(int index, Vector2 pos)
	{
		vertices[index].x = pos.x;
		vertices[index].y = pos.y;
		return this; //for chaining
	}

	public FMeshFacet SetVertexUV(int index, float u, float v)
	{
		vertices[index].u = u;
		vertices[index].v = v;
		return this; //for chaining
	}

	public FMeshFacet SetVertexUV(int index, Vector2 uv)
	{
		vertices[index].u = uv.x;
		vertices[index].v = uv.y;
		return this; //for chaining
	}

	public FMeshFacet SetVertexColor(int index, Color color)
	{
		vertices[index].color = color;

		return this; //for chaining
	}

	public FMeshFacet SetColorForAllVertices(Color color)
	{
		int count = vertices.Length;

		for(int v = 0; v<count; v++)
		{
			vertices[v].color = color;	
		}

		return this; //for chaining
	}

	public FMeshFacet OffsetPos(float offsetX, float offsetY)
	{
		int count = vertices.Length;
		
		for(int v = 0; v<count; v++)
		{
			vertices[v].x += offsetX;
			vertices[v].y += offsetY;	
		}
		
		return this; //for chaining
	}
}

public class FMeshQuad : FMeshFacet
{
	public FMeshQuad()
	{
		facetType = FFacetType.Quad;
		vertices = new FMeshVertex[] {new FMeshVertex(),new FMeshVertex(),new FMeshVertex(),new FMeshVertex()};
	}

	public FMeshQuad SetPosRect(float leftX, float bottomY, float width, float height)
	{
		vertices[0].SetPos(leftX,bottomY+height);
		vertices[1].SetPos(leftX+width,bottomY+height);
		vertices[2].SetPos(leftX+width,bottomY);
		vertices[3].SetPos(leftX,bottomY);
		
		return this; //for chaining
	}

	public FMeshQuad SetPosRect(Rect rect)
	{
		float leftX = rect.xMin;
		float rightX = rect.xMax;
		float bottomY = rect.yMin;
		float topY = rect.yMax;
		
		vertices[0].SetPos(leftX,topY);
		vertices[1].SetPos(rightX,topY);
		vertices[2].SetPos(rightX,bottomY);
		vertices[3].SetPos(leftX,bottomY);
		
		return this; //for chaining
	}

	public FMeshQuad SetUVRect(float leftX, float bottomY, float width, float height)
	{
		vertices[0].SetUV(leftX,bottomY+height);
		vertices[1].SetUV(leftX+width,bottomY+height);
		vertices[2].SetUV(leftX+width,bottomY);
		vertices[3].SetUV(leftX,bottomY);
		
		return this; //for chaining
	}

	public FMeshQuad SetUVRect(Rect rect)
	{
		float leftX = rect.xMin;
		float rightX = rect.xMax;
		float bottomY = rect.yMin;
		float topY = rect.yMax;
		
		vertices[0].SetUV(leftX,topY);
		vertices[1].SetUV(rightX,topY);
		vertices[2].SetUV(rightX,bottomY);
		vertices[3].SetUV(leftX,bottomY);
		
		return this; //for chaining
	}

	public FMeshQuad SetUVRectFull() //creates a uv rect that takes up the whole element texture
	{
		vertices[0].SetUV(0.0f,1.0f);
		vertices[1].SetUV(1.0f,1.0f);
		vertices[2].SetUV(1.0f,0.0f);
		vertices[3].SetUV(0.0f,0.0f);
		
		return this; //for chaining
	}

	public FMeshQuad SetUVRectFromElement(FAtlasElement element) //creates a uv rect that takes up the whole element texture
	{
		float leftX = element.uvRect.xMin;
		float rightX = element.uvRect.xMax;
		float bottomY = element.uvRect.yMin;
		float topY = element.uvRect.yMax;

		vertices[0].SetUV(leftX,topY);
		vertices[1].SetUV(rightX,topY);
		vertices[2].SetUV(rightX,bottomY);
		vertices[3].SetUV(leftX,bottomY);
		
		return this; //for chaining
	}
}

public class FMeshTriangle : FMeshFacet
{
	public FMeshTriangle()
	{
		facetType = FFacetType.Triangle;
		vertices = new FMeshVertex[] {new FMeshVertex(),new FMeshVertex(),new FMeshVertex()};
	}
}


public class FMeshVertex
{
	public float x;
	public float y;

	public float u;
	public float v;

	public Color color = Futile.white;

	public FMeshVertex()
	{

	}

	public FMeshVertex(float x, float y, float u, float v)
	{
		this.x = x;
		this.y = y;
		this.u = u;
		this.v = v;
	}

	public FMeshVertex(Vector2 pos, Vector2 uv)
	{
		x = pos.x;
		y = pos.y;
		u = uv.x;
		v = uv.y;
	}

	public void Set(float x, float y, float u, float v)
	{
		this.x = x;
		this.y = y;
		this.u = u;
		this.v = u;
	}

	public void SetPos(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	public void SetUV(float u, float v)
	{
		this.u = u;
		this.v = u;
	}

	//making the getters and setters below a little bit clumsy so people prefer the direct values instead

	public void SetPos(Vector2 pos)
	{
		x = pos.x;
		y = pos.y;
	}

	public void SetUV(Vector2 uv)
	{
		u = uv.x;
		v = uv.y;
	}

	public Vector2 GetPos()
	{
		return new Vector2(x,y);
	}

	public Vector2 GetUV()
	{
		return new Vector2(u,v);
	}
}

















