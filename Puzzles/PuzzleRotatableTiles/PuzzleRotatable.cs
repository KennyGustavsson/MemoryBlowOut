using System.Collections.Generic;
using UnityEngine;

public class PuzzleRotatable : MonoBehaviour
{
	//[SerializeField] private GameObject prefab  = default;
	[SerializeField] private float rotationSpeed = 0.1f;
	
	[SerializeField] private float clickDelay = 2f;
	
	[Header("Start and End")]
	[SerializeField] public Vector2Int startLoc = Vector2Int.zero;
	[SerializeField] public Vector2Int startDir = Vector2Int.up;
	[Space]
	[SerializeField] public Vector2Int endLoc = Vector2Int.zero;
	[SerializeField] public Vector2Int endDir = Vector2Int.up;
	[Space] 
	[SerializeField] private Renderer startRend = default;
	[SerializeField] private Renderer endRend = default;
	
	[Header("Buttons")] 
	[SerializeField] private GameObject exitButton  = default;

	[Header("Reactions")] 
	[SerializeField] private Reaction enterReaction = null;
	[SerializeField] private Reaction exitReaction = null;
	[SerializeField] private Reaction winReaction = null;
	[SerializeField] private Reaction loseReaction = null;
	[SerializeField] private Reaction rotateTileReaction = null;

	[Header("Emissive Color")] 
	[SerializeField] private Color poweredEmissiveColor = new Color(168, 70, 0, 0.1f);
	[SerializeField] private Color normalEmissiveColor = Color.black;
	
	[Space]
	
	[SerializeField] private GameObject puzzleInteractableObject  = default;


	private float clickTimer = 0f;

	
	private GameObject[,] tiles = new GameObject[5,5];
	private PuzzleRotatableTile[,] tileScripts = new PuzzleRotatableTile[5,5];

	private bool isActive;

	private int winCounter;

	private Dictionary<GameObject, Vector2Int> objectDictionary;
	private Dictionary<Vector2Int, Vector2Int> walkedPath;
	
	private Camera mainCamera;

	private void Awake()
	{
		mainCamera = Camera.main;
		objectDictionary = new Dictionary<GameObject, Vector2Int>(tiles.GetLength(0) * tiles.GetLength(1));
		
		// Dont generate the tiles, make it in editor

		int length = transform.childCount;

		for (int i = 0; i < length; i++)
		{
			GameObject obj = transform.GetChild(i).gameObject;

			PuzzleRotatableTile rotTile = obj.GetComponent<PuzzleRotatableTile>();
			tiles[rotTile.myPos.x, rotTile.myPos.y] = obj;
			tileScripts[rotTile.myPos.x, rotTile.myPos.y] = rotTile;
			
			rotTile.Init(new Vector2Int(rotTile.myPos.x, rotTile.myPos.y), obj);
			
			objectDictionary.Add(obj, new Vector2Int(rotTile.myPos.x, rotTile.myPos.y));
		}

		Material mat = startRend.material;
		mat.SetColor("Color_894298EC", poweredEmissiveColor);
			
		Material mat2 = endRend.material;
		mat2.SetColor("Color_894298EC", poweredEmissiveColor);
			
		ResetColor();
		
		//Generate the tiles
		// for (int x = 0; x < 5; x++)
		// {
		// 	for (int y = 0; y < 5; y++)
		// 	{
		// 		GameObject obj = Instantiate(prefab);
		// 		obj.transform.parent = transform;
		// 		obj.transform.position 
		// 			= new Vector3(transform.position.x + x,
		// 				transform.position.y,
		// 				transform.position.z + y);
		// 		
		// 		tiles[x, y] = obj;
		// 		tileScripts[x, y] = obj.AddComponent<PuzzleRotatableTile>();
		// 		tileScripts[x, y].Init(new Vector2Int(x, y), obj);
		// 		
		// 		objectDictionary.Add(obj, new Vector2Int(x, y));
		// 	}
		// }
	}

	private void Update()
	{
		if(!isActive || GameManager.Instance.isPaused) return;
		clickTimer -= Time.deltaTime;
		
		if(clickTimer > 0)
			return;

		if (Input.GetMouseButtonDown(0))
		{
			clickTimer = clickDelay;
			OnClick();
		}
	}

	private void OnClick()
	{
		GameObject currentObj = ScreenPointRayObject();

		if(currentObj == null)
			return;
		
		if(currentObj == exitButton)
			ClickOnExitButton();
		
		if (objectDictionary.ContainsKey(currentObj))
		{
			if(rotateTileReaction != null)
				rotateTileReaction.TriggerReaction();
			
			RotateTile(currentObj);
		}
	}

	public void ActivatePuzzle()
	{
		isActive = true;
		winCounter = 0;
		puzzleInteractableObject.layer = 2;
		if(enterReaction) enterReaction.TriggerReaction();
	}

	public void DeactivatePuzzle()
	{
		Debug.Log("Deactivate");
		puzzleInteractableObject.layer = 0;
		isActive = false;
	}

	private void ClickOnExitButton()
	{
		DeactivatePuzzle();
		if(exitReaction) exitReaction.TriggerReaction();
	}
	
	private void Win()
	{
		DeactivatePuzzle();
		winCounter++;
		if (winCounter != 2) return;
		if (winReaction) winReaction.TriggerReaction();
		EventManager.OnPuzzleComplete(gameObject);
	}
	
	private void KillTile()
	{
		DeactivatePuzzle();
		if(loseReaction) loseReaction.TriggerReaction();
	}


	private void RotateTile(GameObject obj)
	{
		// Set rotation
		Vector3 eulerRot = obj.transform.localRotation.eulerAngles;
		eulerRot = new Vector3(eulerRot.x, eulerRot.y + 90, eulerRot.z);

		Quaternion rotation = Quaternion.Euler(eulerRot);
		StartCoroutine(TransformLerp.LerpRot(obj.transform, rotation, rotationSpeed));

		// Set which direction to point in
		Vector2Int currentIndex = objectDictionary[obj];
		var tile = tileScripts[currentIndex.x, currentIndex.y];
		
		tile.firstDir = GetRotatedDirection(tile.firstDir);
		tile.secondDir = GetRotatedDirection(tile.secondDir);
		
		// Check path
		ResetColor();
		StartCheckPath(startLoc, startDir, endLoc);
		StartCheckPath(endLoc, endDir, startLoc);
	}
	
	private GameObject ScreenPointRayObject()
	{
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		return Physics.Raycast(ray, out RaycastHit hit) ? hit.collider.gameObject : null;
	}

	private Vector2Int GetRotatedDirection(Vector2Int direction)
	{
		switch (direction)
		{
			case Vector2Int n when (direction == Vector2Int.up):
				return Vector2Int.right;

			case Vector2Int n when (direction == Vector2Int.down):
				return Vector2Int.left;

			case Vector2Int n when (direction == Vector2Int.right):
				return Vector2Int.down;

			case Vector2Int n when (direction == Vector2Int.left):
				return Vector2Int.up;
		}

		return Vector2Int.zero;
	}

	private Vector2Int GetOppositeDirection(Vector2Int direction)
	{
		switch (direction)
		{
			case Vector2Int n when (direction == Vector2Int.up):
				return Vector2Int.down;

			case Vector2Int n when (direction == Vector2Int.down):
				return Vector2Int.up;

			case Vector2Int n when (direction == Vector2Int.right):
				return Vector2Int.left;

			case Vector2Int n when (direction == Vector2Int.left):
				return Vector2Int.right;
		}

		return Vector2Int.zero;
	}
	
	private void StartCheckPath(Vector2Int pos, Vector2Int dir, Vector2Int endPos)
	{
		walkedPath = new Dictionary<Vector2Int, Vector2Int>();
		CheckPath(pos, dir, endPos);
	}

	private void ResetColor()
	{
		for (int y = 0; y < 5; y++){
			for (int x = 0; x < 5; x++){
				Material mat = tileScripts[x,y].obj.GetComponent<Renderer>().material;
				mat.SetColor("Color_894298EC", normalEmissiveColor);
			}
		}
	}
	
	private void CheckPath(Vector2Int pos, Vector2Int dir, Vector2Int endPos)
	{

		if(walkedPath.ContainsKey(pos)) // makes you not walk on tiles twice
		{
			Debug.Log("I walked on this before");
			return;
		}

		if (pos + dir == endPos)
		{
			Win();
		}
		
		walkedPath.Add(pos, dir);
		
		if(pos.x + dir.x > tileScripts.GetLength(0) - 1 
		   || pos.y + dir.y > tileScripts.GetLength(1) - 1
		   || pos.x + dir.x < 0
		   || pos.y + dir.y < 0)
			return;
		
		var currentTile = tiles[pos.x + dir.x, pos.y + dir.y];


		if (currentTile != null)
		{
			var currentTileScript = currentTile.GetComponent<PuzzleRotatableTile>();
			if (currentTileScript.isKillTile)
			{
				//currentTile.GetComponent<Renderer>().material.color = Color.red;
				
				Material mat = currentTile.GetComponent<Renderer>().material;
				mat.SetColor("Color_894298EC", poweredEmissiveColor);

				KillTile();
				return;
			}


			if (pos == currentTileScript.myPos + currentTileScript.firstDir)
			{
				//currentTile.GetComponent<Renderer>().material.color = Color.green;

				Material mat = currentTile.GetComponent<Renderer>().material;
				mat.SetColor("Color_894298EC", poweredEmissiveColor);

				CheckPath(currentTileScript.myPos, currentTileScript.secondDir, endPos);
				return;
			}
			
			if (pos == currentTileScript.myPos + currentTileScript.secondDir)
			{
				//currentTile.GetComponent<Renderer>().material.color = Color.green;
				
				Material mat = currentTile.GetComponent<Renderer>().material;
				mat.SetColor("Color_894298EC", poweredEmissiveColor);

				CheckPath(currentTileScript.myPos, currentTileScript.firstDir, endPos);
				return;
			}
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		var rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.matrix = rotationMatrix;
		
		Gizmos.color = Color.red;
		Gizmos.DrawRay(new Vector3(startLoc.x, 0, startLoc.y) + Vector3.up * 0.02f, (new Vector3(startDir.x, 0, startDir.y)) * 0.5f);
        
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(new Vector3(endLoc.x, 0, endLoc.y) + Vector3.up * 0.02f, (new Vector3(endDir.x, 0, endDir.y))  * 0.5f);
	}
#endif
}
