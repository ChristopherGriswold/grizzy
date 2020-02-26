using UnityEngine;

public class DontMoveInvisible : MonoBehaviour
{
    public bool isVisible;

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
}
