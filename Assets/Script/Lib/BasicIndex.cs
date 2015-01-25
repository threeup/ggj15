using UnityEngine;
using System.Collections;

[System.Serializable]
public class BasicIndex
{
    [SerializeField]
    private int idxVal = -1;
    public int Val { get { return idxVal; } set { idxVal = value; } }
    [SerializeField]
    private int maxVal = 0;
    public int MaxVal { get { return maxVal; } set { maxVal = value; } }

    private bool isLooping = true;
    public BasicIndex(int maxVal, bool isLooping = true)
    {
        this.idxVal = 0;
        this.maxVal = maxVal;
        this.isLooping = isLooping;
    }

    public bool Next()
    {
        if( idxVal == maxVal )
        {
            if( isLooping )
            {
                idxVal = 0;
            }
            return true;
        }
        idxVal++;
        return false;
    }

    public bool Prev()
    {
        if( idxVal == 0 )
        {
            if( isLooping )
            {
                idxVal = maxVal;
            }
            return true;
        }
        idxVal--;
        return false;
    }

    public void SetMax(int maxVal)
    {
        this.maxVal = maxVal;
        this.idxVal = Mathf.Min(idxVal, maxVal);
    }
}