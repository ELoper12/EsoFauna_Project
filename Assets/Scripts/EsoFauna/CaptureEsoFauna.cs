using Assets.Scripts;
using UnityEngine;

public class CaptureEsoFauna : MonoBehaviour
{
    DictionaryManager dictionaryManager;

    EsoFauna_base esoFauna;
    public void Capture()
    {
        dictionaryManager.coreAnalyzePercent[esoFauna.index]+=1;
    }
}
