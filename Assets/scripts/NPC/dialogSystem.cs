using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogSystem : MonoBehaviour
{
    public static dialogSystem instance { get; private set; }

    [Header("UI")]
    public Text textLabel;

    [Header("Text")]
    public TextAsset textFile;
    public int index;
    public float textSpeed;

    bool textFinished;
    bool cancelTyping;

    public bool textFinishedAll;
    public Animator playerAnimator;

    List<string> textList = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        GetText(textFile);
        textFinishedAll = false;
    }

    private void OnEnable()
    {
        textFinished = true;
        StartCoroutine(SetTextUI());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && index == textList.Count)
        {
            gameObject.SetActive(false); // Close dialog box.
            index = 0;
            textFinishedAll = true;
            playerAnimator.SetTrigger("transport");
            return; // Exit the function.
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (textFinished && !cancelTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!textFinished)
            {
                cancelTyping = !cancelTyping;
            }
        }
    }

    // Get text from file.
    void GetText(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineData = file.text.Split('\n');

        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    // Use coroutine to display text.
    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";

        int letter = 0;
        while (!cancelTyping && letter < textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        cancelTyping = false;

        textFinished = true;
        index++;
    }
}
