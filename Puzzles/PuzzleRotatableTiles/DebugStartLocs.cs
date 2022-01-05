using UnityEngine;

public class DebugStartLocs : MonoBehaviour
{
    [SerializeField] private PuzzleRotatable puzzle = default;
    [Space]
    [SerializeField] private GameObject obj1 = default;
    [SerializeField] private GameObject obj2 = default;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(obj1 == null || obj2 == null || puzzle == null)
            return;
        
        var dir1 = puzzle.startDir;
        var loc1 = puzzle.startLoc;
        
        var dir2 = puzzle.endDir;
        var loc2 = puzzle.endLoc;
        
        Debug.DrawRay(transform.position + new Vector3(loc1.x, 0, loc1.y) + Vector3.up * 0.02f, (new Vector3(dir1.x, 0, dir1.y)) * 0.05f, Color.red, 0.01f);
        Debug.DrawRay(transform.position + new Vector3(loc2.x, 0, loc2.y) + Vector3.up * 0.02f, (new Vector3(dir2.x, 0, dir2.y)) * 0.05f, Color.blue, 0.01f);
    }
#endif
}
