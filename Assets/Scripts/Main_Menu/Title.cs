using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField]
    private float _titleInterval = 0.25f;
    [SerializeField]
    private GameObject _playButton;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TitleRoutine());
    }

    IEnumerator TitleRoutine()
    {
        Text title = this.GetComponent<Text>();
        title.text = "";

        string titleText = "Wasteland Wander"; 

        for (int i = 0; i <= titleText.Length; i++)
        {
            title.text = titleText.Substring(0, i);
            yield return new WaitForSeconds(_titleInterval);
        }

    }
}
