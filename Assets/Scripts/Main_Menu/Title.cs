using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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

    // Update is called once per frame
    void Update()
    {
        //if (!_scalingPlayButton || _playButton.transform.localScale.y <= 1)
        //{
            //_playButton.transform.localScale += new Vector3(0.25f, 0.25f, 0.25f) * Time.deltaTime;
        //}
    }

    IEnumerator TitleRoutine()
    {
        Text text = this.GetComponent<Text>();
        text.text = "W";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Wa";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Was";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Wast";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Waste";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Wastel";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Wastela";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Wastelan";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Wasteland";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Wasteland W";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Wasteland Wa";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Wasteland Wan";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Wasteland Wand";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Wasteland Wande";
        yield return new WaitForSeconds(_titleInterval);
        text.text = "Wasteland Wander";
        yield return new WaitForSeconds(_titleInterval);
    }
}
