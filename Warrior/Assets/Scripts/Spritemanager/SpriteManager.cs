
//-----------------------------------------------------------------
//  SpriteManager v0.63 (6-1-2009)
//  Copyright 2009 Brady Wright and Above and Beyond Software
//  All rights reserved
//-----------------------------------------------------------------
// A class to allow the drawing of multiple "quads" as part of a
// single aggregated mesh so as to achieve multiple, independently
// moving objects using a single draw call.
//-----------------------------------------------------------------


using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------
// Describes a UV animation
//-----------------------------------------------------------------
//  NOTE: Currently, you should assign at least two frames to an
//  animation, or else you can expect problems!
//-----------------------------------------------------------------
public class UVAnimation
{
	protected Vector2[] frames;      // Array of UV coordinates (for quads) defining the frames of an animation

	// Animation state vars:
	protected int curFrame = 0;      // The current frame
	protected int stepDir = 1;            // The direction we're currently playing the animation (1=forwards (default), -1=backwards)
	protected int numLoops = 0;      // Number of times we've looped since last animation

	public string name;        // The name of the 
	public int loopCycles = 0;            // How many times to loop the animation (-1 loop infinitely)
	public bool loopReverse = false;                // Reverse the play direction when the end of the animation is reached? (if true, a loop iteration isn't counted until we return to the beginning)
	public float framerate;       // The rate in frames per second at which to play the animation


	// Resets all the animation state vars to ready the object
	// for playing anew:
	public void Reset()
	{
		curFrame = 0;
		stepDir = 1;
		numLoops = 0;
	}

	// Sets the stepDir to -1 and sets the current frame to the end
	// so that the animation plays in reverse
	public void PlayInReverse()
	{
		stepDir = -1;
		curFrame = frames.Length - 1;
	}

	// Stores the UV of the next frame in 'uv', returns false if
	// we've reached the end of the animation (this will never
	// happen if it is set to loop infinitely)
	public bool GetNextFrame(ref Vector2 uv)
	{
		// See if we can advance to the next frame:
		if ((curFrame + stepDir) >= frames.Length || (curFrame + stepDir) < 0)
		{
			// See if we need to loop (if we're reversing, we don't loop until we get back to the beginning):
			if (stepDir > 0 && loopReverse)
			{
				stepDir = -1;   // Reverse playback direction
				curFrame += stepDir;

				uv = frames[curFrame];
			}
			else
			{
				// See if we can loop:
				if (numLoops + 1 > loopCycles && loopCycles != -1)
					return false;
				else
				{   // Loop the animation:
					++numLoops;

					if (loopReverse)
					{
						stepDir *= -1;
						curFrame += stepDir;
					}
					else
						curFrame = 0;

					uv = frames[curFrame];
				}
			}
		}
		else
		{
			curFrame += stepDir;
			uv = frames[curFrame];
		}

		return true;
	}

	// Constructs an array of UV coordinates based upon the info
	// supplied.
	//
	// start    -   The UV of the lower-left corner of the first
	//        cell
	// cellSize -    width and height, in UV space, of each cell
	// cols  -    Number of columns in the grid
	// rows  -    Number of rows in the grid
	// totalCells-  Total number of cells in the grid (left-to-right,
	//        top-to-bottom ordering is assumed, just like reading
	//        English).
	// fps    - Framerate (frames per second)
	public Vector2[] BuildUVAnim(Vector2 start, Vector2 cellSize, int cols, int rows, int totalCells, float fps)
	{
		int cellCount = 0;

		frames = new Vector2[totalCells];
		framerate = fps;

		frames[0] = start;

		for (int row = 0 ; row < rows ; ++row)
		{
			for (int col = 0 ; col < cols && cellCount < totalCells ; ++col)
			{
				frames[cellCount].x = start.x + cellSize.x * ((float)col);
				frames[cellCount].y = start.y + cellSize.y * ((float)row);

				++cellCount;
			}
		}

		return frames;
	}

	// Assigns the specified array of UV coordinates to the
	// animation, replacing its current contents
	public void SetAnim(Vector2[] anim)
	{
		frames = anim;
	}

	// Appends the specified array of UV coordinates to the
	// existing animation
	public void AppendAnim(Vector2[] anim)
	{
		Vector2[] tempFrames = frames;

		frames = new Vector2[frames.Length + anim.Length];
		tempFrames.CopyTo(frames, 0);
		anim.CopyTo(frames, tempFrames.Length);
	}
}




//-----------------------------------------------------------------
// Holds a single mesh object which is composed of an arbitrary
// number of quads that all use the same material, allowing
// multiple, independently moving objects to be drawn on-screen
// while using only a single draw call.
//-----------------------------------------------------------------
public class SpriteManager : MonoBehaviour
{
	// In which plane should we create the sprites?
	public enum SPRITE_PLANE
	{
		XY,
		XZ,
		YZ
	};

	// Which way to wind polygons?
	public enum WINDING_ORDER
	{
		CCW,        // Counter-clockwise
		CW      // Clockwise
	};

	public Material material;            // The material to use for the sprites
	public int allocBlockSize;        // How many sprites to allocate space for at a time. ex: if set to 10, 10 new sprite blocks will be allocated at a time. Once all of these are used, 10 more will be allocated, and so on...
	public SPRITE_PLANE plane;        // The plane in which to create the sprites
	public WINDING_ORDER winding = WINDING_ORDER.CCW; // Which way to wind polygons
	public bool autoUpdateBounds = false;   // Automatically recalculate the bounds of the mesh when vertices change?

	protected ArrayList availableBlocks = new ArrayList(); // Array of references to sprites which are currently not in use
	protected bool vertsChanged = false;    // Have changes been made to the vertices of the mesh since the last frame?
	protected bool uvsChanged = false;    // Have changes been made to the UVs of the mesh since the last frame?
	protected bool colorsChanged = false;   // Have the colors changed?
	protected bool vertCountChanged = false;// Has the number of vertices changed?
	protected bool updateBounds = false;    // Update the mesh bounds?
	protected Sprite[] sprites;    // Array of all sprites (the offset of the vertices corresponding to each sprite should be found simply by taking the sprite's index * 4 (4 verts per sprite).
	protected ArrayList activeBlocks = new ArrayList(); // Array of references to all the currently active (non-empty) sprites
	protected ArrayList activeBillboards = new ArrayList(); // Array of references to all the *active* sprites which are to be rendered as billboards
	protected float boundUpdateInterval;    // Interval, in seconds, to update the mesh bounds

	protected MeshFilter meshFilter;
	protected MeshRenderer meshRenderer;
	protected Mesh mesh;                    // Reference to our mesh (contained in the MeshFilter)

	protected Vector3[] vertices;         // The vertices of our mesh
	protected int[] triIndices;    // Indices into the vertex array
	protected Vector2[] UVs;                // UV coordinates
	protected Color[] colors;            // Color values
	//protected Vector3[] normals;      // Normals

	//--------------------------------------------------------------
	// Utility functions:
	//--------------------------------------------------------------

	// Converts pixel-space values to UV-space scalar values
	// according to the currently assigned material.
	// NOTE: This is for converting widths and heights-not
	// coordinates (which have reversed Y-coordinates).
	// For coordinates, use PixelCoordToUVCoord()!
	public Vector2 PixelSpaceToUVSpace(Vector2 xy)
	{
		Texture t = material.GetTexture("_MainTex");

		return new Vector2(xy.x / ((float)t.width), xy.y / ((float)t.height));
	}

	// Converts pixel-space values to UV-space scalar values
	// according to the currently assigned material.
	// NOTE: This is for converting widths and heights-not
	// coordinates (which have reversed Y-coordinates).
	// For coordinates, use PixelCoordToUVCoord()!
	public Vector2 PixelSpaceToUVSpace(int x, int y)
	{
		return PixelSpaceToUVSpace(new Vector2((float)x, (float)y));
	}

	// Converts pixel coordinates to UV coordinates according to
	// the currently assigned material.
	// NOTE: This is for converting coordinates and will reverse
	// the Y component accordingly.  For converting widths and
	// heights, use PixelSpaceToUVSpace()!
	public Vector2 PixelCoordToUVCoord(Vector2 xy)
	{
		Vector2 p = PixelSpaceToUVSpace(xy);
		p.y = 1.0f - p.y;
		return p;
	}

	// Converts pixel coordinates to UV coordinates according to
	// the currently assigned material.
	// NOTE: This is for converting coordinates and will reverse
	// the Y component accordingly.  For converting widths and
	// heights, use PixelSpaceToUVSpace()!
	public Vector2 PixelCoordToUVCoord(int x, int y)
	{
		return PixelCoordToUVCoord(new Vector2((float)x, (float)y));
	}

    public void SetMaterial(Material mat)
    {
        material = mat;
        meshRenderer.GetComponent<Renderer>().material = material;
    }

	//--------------------------------------------------------------
	// End utility functions
	//--------------------------------------------------------------

	protected virtual void Awake()
	{
		gameObject.AddComponent<MeshFilter>();
		gameObject.AddComponent<MeshRenderer>();

		meshFilter = (MeshFilter)GetComponent(typeof(MeshFilter));
		meshRenderer = (MeshRenderer)GetComponent(typeof(MeshRenderer));
        meshRenderer.receiveShadows = false;
        meshRenderer.castShadows = false;

		meshRenderer.GetComponent<Renderer>().material = material;
		mesh = meshFilter.mesh;

		// Create our first batch of sprites:
		EnlargeArrays(allocBlockSize);

		// Move the object to the origin so the objects drawn will not
		// be offset from the objects they are intended to represent.
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
	}

	// Allocates initial arrays
	protected void InitArrays()
	{
		sprites = new Sprite[1];
		GameObject dummyGO = new GameObject("UiTemp");

				dummyGO.transform.parent = AutoElementManager.Instance.transform;

		sprites[0] = (Sprite)dummyGO.AddComponent(typeof(Sprite));
		sprites[0].dummyGO = dummyGO;
		vertices = new Vector3[4];
		UVs = new Vector2[4];
		colors = new Color[4];
		triIndices = new int[6];
	}

	// Enlarges the sprite array by the specified count and also resizes
	// the UV and vertex arrays by the necessary corresponding amount.
	// Returns the index of the first newly allocated element
	// (ex: if the sprite array was already 10 elements long and is 
	// enlarged by 10 elements resulting in a total length of 20, 
	// EnlargeArrays() will return 10, indicating that element 10 is the 
	// first of the newly allocated elements.)
	protected int EnlargeArrays(int count)
	{
		int firstNewElement;
		GameObject dummyGO;

		if (sprites == null)
		{
			InitArrays();
			firstNewElement = 0;
			count = count - 1;  // Allocate one less since InitArrays already allocated one sprite for us
		}
		else
			firstNewElement = sprites.Length;

		// Resize sprite array:
		Sprite[] tempSprites = sprites;
		sprites = new Sprite[sprites.Length + count];
		tempSprites.CopyTo(sprites, 0);

		// Vertices:
		Vector3[] tempVerts = vertices;
		vertices = new Vector3[vertices.Length + count * 4];
		tempVerts.CopyTo(vertices, 0);

		// UVs:
		Vector2[] tempUVs = UVs;
		UVs = new Vector2[UVs.Length + count * 4];
		tempUVs.CopyTo(UVs, 0);

		// Colors:
		Color[] tempColors = colors;
		colors = new Color[colors.Length + count * 4];
		tempColors.CopyTo(colors, 0);

		// Triangle indices:
		int[] tempTris = triIndices;
		triIndices = new int[triIndices.Length + count * 6];
		tempTris.CopyTo(triIndices, 0);

		// Inform existing sprites of the new vertex and UV buffers:
		for (int i = 0 ; i < firstNewElement ; ++i)
		{
			sprites[i].SetBuffers(vertices, UVs);
		}

		// Setup the newly-added sprites and Add them to the list of available 
		// sprite blocks. Also initialize the triangle indices while we're at it:
		for (int i = firstNewElement ; i < sprites.Length ; ++i)
		{
			// Create and setup sprite:

			dummyGO = new GameObject("UiTemp");

			dummyGO.transform.parent = AutoElementManager.Instance.transform;

			sprites[i] = (Sprite)dummyGO.AddComponent(typeof(Sprite));
			sprites[i].dummyGO = dummyGO;
			sprites[i].index = i;
			sprites[i].manager = this;

			sprites[i].SetBuffers(vertices, UVs);

			// Setup indices of the sprite's vertices in the vertex buffer:
			sprites[i].mv1 = i * 4 + 0;
			sprites[i].mv2 = i * 4 + 1;
			sprites[i].mv3 = i * 4 + 2;
			sprites[i].mv4 = i * 4 + 3;

			// Setup the indices of the sprite's UV entries in the UV buffer:
			sprites[i].uv1 = i * 4 + 0;
			sprites[i].uv2 = i * 4 + 1;
			sprites[i].uv3 = i * 4 + 2;
			sprites[i].uv4 = i * 4 + 3;

			// Setup the indices to the color values:
			sprites[i].cv1 = i * 4 + 0;
			sprites[i].cv2 = i * 4 + 1;
			sprites[i].cv3 = i * 4 + 2;
			sprites[i].cv4 = i * 4 + 3;

			// Setup the default color:
			sprites[i].SetColor(Color.white);

			// Add as an available sprite:
			availableBlocks.Add(sprites[i]);

			// Init triangle indices:
			if (winding == WINDING_ORDER.CCW)
			{   // Counter-clockwise winding
				triIndices[i * 6 + 0] = i * 4 + 0;  //    0_ 2            0 ___ 3
				triIndices[i * 6 + 1] = i * 4 + 1;  //  | /      Verts:  |   /|
				triIndices[i * 6 + 2] = i * 4 + 3;  // 1|/                1|/__|2

				triIndices[i * 6 + 3] = i * 4 + 3;  //      3
				triIndices[i * 6 + 4] = i * 4 + 1;  //   /|
				triIndices[i * 6 + 5] = i * 4 + 2;  // 4/_|5
			}
			else
			{   // Clockwise winding
				triIndices[i * 6 + 0] = i * 4 + 0;  //    0_ 1            0 ___ 3
				triIndices[i * 6 + 1] = i * 4 + 3;  //  | /      Verts:  |   /|
				triIndices[i * 6 + 2] = i * 4 + 1;  // 2|/                1|/__|2

				triIndices[i * 6 + 3] = i * 4 + 3;  //      3
				triIndices[i * 6 + 4] = i * 4 + 2;  //   /|
				triIndices[i * 6 + 5] = i * 4 + 1;  // 5/_|4
			}
		}

		vertsChanged = true;
		uvsChanged = true;
		colorsChanged = true;
		vertCountChanged = true;

		return firstNewElement;
	}

	// Adds a sprite to the manager at the location and rotation of the client 
	// GameObject and with its transform.  Returns a reference to the new sprite
	// Width and height are in world space units
	// leftPixelX and bottomPixelY- the bottom-left position of the desired portion of the texture, in pixels
	// pixelWidth and pixelHeight - the dimensions of the desired portion of the texture, in pixels
	public Sprite AddSprite(GameObject client, float width, float height, int leftPixelX, int bottomPixelY, int pixelWidth, int pixelHeight, bool billboarded)
	{
		return AddSprite(client, width, height, PixelCoordToUVCoord(leftPixelX, bottomPixelY), PixelSpaceToUVSpace(pixelWidth, pixelHeight), billboarded);
	}

	// Adds a sprite to the manager at the location and rotation of the client 
	// GameObject and with its transform.  Returns a reference to the new sprite
	// Width and height are in world space units
	// lowerLeftUV - the UV coordinate for the upper-left corner
	// UVDimensions - the distance from lowerLeftUV to place the other UV coords
	public Sprite AddSprite(GameObject client, float width, float height, Vector2 lowerLeftUV, Vector2 UVDimensions, bool billboarded)
	{
		int spriteIndex;

		// Get an available sprite:
		if (availableBlocks.Count < 1)
			EnlargeArrays(allocBlockSize);  // If we're out of available sprites, allocate some more:

		// Use a sprite from the list of available blocks:
		spriteIndex = ((Sprite)availableBlocks[0]).index;
		availableBlocks.RemoveAt(0);    // Now that we're using this one, remove it from the available list

		// Assign the new sprite:
		Sprite newSprite = sprites[spriteIndex];
		newSprite.client = client;
		newSprite.lowerLeftUV = lowerLeftUV;
		newSprite.uvDimensions = UVDimensions;

		switch (plane)
		{
		case SPRITE_PLANE.XY:
			newSprite.SetSizeXY(width, height);
			break;
		case SPRITE_PLANE.XZ:
			newSprite.SetSizeXZ(width, height);
			break;
		case SPRITE_PLANE.YZ:
			newSprite.SetSizeYZ(width, height);
			break;
		default:
			newSprite.SetSizeXY(width, height);
			break;
		}

		// Save this to an active list now that it is in-use:
		if (billboarded)
		{
			newSprite.billboarded = true;
			activeBillboards.Add(newSprite);
		}
		else
			activeBlocks.Add(newSprite);

		// Transform the sprite:
		newSprite.Transform();

		// Setup the UVs:
		UVs[newSprite.uv1] = lowerLeftUV + Vector2.up * UVDimensions.y;  // Upper-left
		UVs[newSprite.uv2] = lowerLeftUV;                         // Lower-left
		UVs[newSprite.uv3] = lowerLeftUV + Vector2.right * UVDimensions.x;// Lower-right
		UVs[newSprite.uv4] = lowerLeftUV + UVDimensions;                     // Upper-right

		// Set our flags:
		vertsChanged = true;
		uvsChanged = true;

		return newSprite;
	}

	public void SetBillboarded(Sprite sprite)
	{
		// Make sure the sprite isn't in the active list
		// or else it'll get handled twice:
		activeBlocks.Remove(sprite);
		activeBillboards.Add(sprite);
	}

	public void RemoveSprite(Sprite sprite)
	{
		sprite.SetSizeXY(0, 0);
		sprite.v1 = Vector3.zero;
		sprite.v2 = Vector3.zero;
		sprite.v3 = Vector3.zero;
		sprite.v4 = Vector3.zero;

		vertices[sprite.mv1] = sprite.v1;
		vertices[sprite.mv2] = sprite.v2;
		vertices[sprite.mv3] = sprite.v3;
		vertices[sprite.mv4] = sprite.v4;

		// Remove the sprite from the billboarded list
		// since that list should only contain active
		// sprites:
		if (sprite.billboarded)
			activeBillboards.Remove(sprite);
		else
			activeBlocks.Remove(sprite);

		// Clean the sprite's settings:
		sprite.client = null;
		sprite.billboarded = false;
		sprite.hidden = false;
		sprite.SetColor(Color.white);
		sprite.offset = Vector3.zero;

		availableBlocks.Add(sprite);

		vertsChanged = true;
	}

	public void HideSprite(Sprite sprite)
	{
		// Remove the sprite from the billboarded list
		// since that list should only contain sprites
		// we intend to transform:
		if (sprite.billboarded)
			activeBillboards.Remove(sprite);
		else
			activeBlocks.Remove(sprite);

		sprite.m_hidden___DoNotAccessExternally = true;

		vertices[sprite.mv1] = Vector3.zero;
		vertices[sprite.mv2] = Vector3.zero;
		vertices[sprite.mv3] = Vector3.zero;
		vertices[sprite.mv4] = Vector3.zero;

		vertsChanged = true;
	}

	public void ShowSprite(Sprite sprite)
	{
		// Only show the sprite if it has a client:
		if (sprite.client == null)
			return;

		if (!sprite.m_hidden___DoNotAccessExternally)
			return;

		sprite.m_hidden___DoNotAccessExternally = false;

		// Update the vertices:
		sprite.Transform();

		if (sprite.billboarded)
			activeBillboards.Add(sprite);
		else
			activeBlocks.Add(sprite);

		vertsChanged = true;
	}

	public Sprite GetSprite(int i)
	{
		if (i < sprites.Length)
			return sprites[i];
		else
			return null;
	}

	// Updates the vertices of a sprite based on the transform
	// of its client GameObject
	public void Transform(Sprite sprite)
	{
		sprite.Transform();

		vertsChanged = true;
	}

    public void TransformToGround(Sprite sprite)
    {
        sprite.TransformToGround();
        vertsChanged = true;
    }

	// Updates the vertices of a sprite such that it is oriented
	// more or less toward the camera
	public void TransformBillboarded(Sprite sprite)
	{
		Vector3 pos = sprite.clientTransform.position;
		Transform t = Camera.main.transform;

		vertices[sprite.mv1] = pos + t.TransformDirection(sprite.v1);
		vertices[sprite.mv2] = pos + t.TransformDirection(sprite.v2);
		vertices[sprite.mv3] = pos + t.TransformDirection(sprite.v3);
		vertices[sprite.mv4] = pos + t.TransformDirection(sprite.v4);

		vertsChanged = true;
	}

	// Informs the SpriteManager that some vertices have changed position
	// and the mesh needs to be reconstructed accordingly
	public void UpdatePositions()
	{
		vertsChanged = true;
	}

	// Updates the UVs of the specified sprite and copies the new values
	// into the mesh object.
	public void UpdateUV(Sprite sprite)
	{
		UVs[sprite.uv1] = sprite.lowerLeftUV + Vector2.up * sprite.uvDimensions.y;  // Upper-left
		UVs[sprite.uv2] = sprite.lowerLeftUV;                              // Lower-left
		UVs[sprite.uv3] = sprite.lowerLeftUV + Vector2.right * sprite.uvDimensions.x;// Lower-right
		UVs[sprite.uv4] = sprite.lowerLeftUV + sprite.uvDimensions;     // Upper-right

		uvsChanged = true;
	}

	// Updates the color values of the specified sprite and copies the
	// new values into the mesh object.
	public void UpdateColors(Sprite sprite)
	{
		colors[sprite.cv1] = sprite.color;
		colors[sprite.cv2] = sprite.color;
		colors[sprite.cv3] = sprite.color;
		colors[sprite.cv4] = sprite.color;

		colorsChanged = true;
	}

	// Instructs the manager to recalculate the bounds of the mesh
	public void UpdateBounds()
	{
		updateBounds = true;
	}

	// Schedules a recalculation of the mesh bounds to occur at a
	// regular interval (given in seconds):
	public void ScheduleBoundsUpdate(float seconds)
	{
		boundUpdateInterval = seconds;
		InvokeRepeating("UpdateBounds", seconds, seconds);
	}

	// Cancels any previously scheduled bounds recalculations:
	public void CancelBoundsUpdate()
	{
		CancelInvoke("UpdateBounds");
	}

	// Use this for initialization
	void Start()
	{

	}

	virtual protected void Update()
	{
		int count = activeBillboards.Count;
		for (int i = 0 ; i < count ; i++)
			TransformBillboarded(activeBillboards[i] as Sprite);
	}
	// LateUpdate is called once per frame
	virtual protected void LateUpdate()
	{
		// Were changes made to the mesh since last time?
		if (vertCountChanged)
		{
			vertCountChanged = false;
			colorsChanged = false;
			vertsChanged = false;
			uvsChanged = false;
			updateBounds = false;

			mesh.Clear();
			mesh.vertices = vertices;
			mesh.uv = UVs;
			mesh.colors = colors;
			//mesh.normals = normals;
			mesh.triangles = triIndices;
		}
		else
		{
			if (vertsChanged)
			{
				vertsChanged = false;

				if (autoUpdateBounds)
					updateBounds = true;

				mesh.vertices = vertices;
			}

			if (updateBounds)
			{
				mesh.RecalculateBounds();
				updateBounds = false;
			}

			if (colorsChanged)
			{
				colorsChanged = false;

				mesh.colors = colors;
			}

			if (uvsChanged)
			{
				uvsChanged = false;
				mesh.uv = UVs;
			}
		}
	}
}