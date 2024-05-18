
/*
Manage the flow of simon
Keep track of turn, and list of buttons to press

Natalia Rodriguez
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonController : MonoBehaviour
{
    [SerializeField] List<SimonButton> buttons;
    [SerializeField] List<int> sequence;
    [SerializeField] float delay;
    [SerializeField] int level;
    [SerializeField] bool playerTurn = false;
    [SerializeField] Text scoreText;

    [SerializeField] int counter = 0;
    [SerializeField] int numButtons;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform buttonParent;


    // Start is called before the first frame update
    void Start()
    {
        // Configure the buttons to be used in the game
        PrepareButtons();
        scoreText.text = "Score: 0";
    }

    // Configure the callback functions for the buttons
    void PrepareButtons()
    {
        for (int i=0; i<numButtons; i++) {
            int index = i;
            // Create the copies of the button as children of the panel
            GameObject newButton = Instantiate(buttonPrefab, buttonParent);
            newButton.GetComponent<Image>().color = Color.HSVToRGB((float)index/numButtons, 1, 1);
            newButton.GetComponent<SimonButton>().Init(index);
            buttons.Add(newButton.GetComponent<SimonButton>());
            buttons[i].gameObject.GetComponent<Button>().onClick.AddListener(() => ButtonPressed(index));
        }
        // Start the game by adding the first button
        AddToSequence();
    }

    // Main function to validate that the button pressed by the user 
    // corresponds with the sequence generated by the CPU
    public void ButtonPressed(int index) {
        if (playerTurn) {
            if (index == sequence[counter++]) {
                // Highlight the button selected by the player
                buttons[index].Highlight();
                scoreText.text = "Score: " + counter;
                if (counter == sequence.Count) {
                    // Finish the player turn to ensure no other actions are
                    // taken into account
                    playerTurn = false;
                    level++;
                    counter = 0;
                    AddToSequence();
                }
            } else {
                Debug.Log("Game Over!");
                RestartGame();
            }
        }
    }

    // Add another number to the sequence and display it
    void AddToSequence()
    {
        // Add a new button to the sequence
        sequence.Add(Random.Range(0, buttons.Count));
        StartCoroutine(PlaySequence());
    }

    // Display every button in the sequence so far
    IEnumerator PlaySequence()
    {
        // Add an initial delay before showing the sequence
        yield return new WaitForSeconds(delay);
        foreach (int index in sequence) {
            buttons[index].Highlight();
            yield return new WaitForSeconds(delay);
        }
        // Switch the turn over to the player
        playerTurn = true;
}
    
        // Restart the game
        public void RestartGame()
        {
            sequence.Clear();
            level = 0;
            counter = 0;
            playerTurn = false;
            scoreText.text = "Score: 0";
            AddToSequence();
        }
}
