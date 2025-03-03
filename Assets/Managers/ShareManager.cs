using UnityEngine;

public class ShareManager : MonoBehaviour
{
    public void ShareLevel()
    {
        DataPersistenceManager.instance.ExportGame();
    }

    public void PlaySharedLevel()
    {
        DataPersistenceManager.instance.ImportGame();
    }

}
