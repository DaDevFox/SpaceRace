// using System;
// using System.Collections;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;


// public class MCQSlideController : MonoBehaviour
// {
//     public static string indicatorPrefabPath { get; } = "UI/Indicator";
//     public Transform[] indicatorContainers;
    

//     public event Action OnComplete;

//     /// <summary>
//     /// if set to a player id, mcq ill be targeted at a specific player and complete once they and only they have ansered the question
//     /// </summary>
//     public int individualPlayer = -1;

//     public Question current;

//     [Header("Scene")]
//     public TextMeshProUGUI categoryText;
//     public TextMeshProUGUI currPlayerText;
//     public Button[] options;
//     public Func<Boolean>[] optionTriggerTests = new Func<Boolean>[] {
//         (() => MainInputController.OneDown),
//         (() => MainInputController.TwoDown),
//         (() => MainInputController.ThreeDown),
//         (() => MainInputController.FourDown)
//     };
//     public GameObject main => transform.parent.gameObject;

//     public static Color defaultColor = Color.white;
//     public static Color correctColor = Color.green;
//     public static Color incorrectColor = Color.red;

//     public float answerHoldTime = 5f;


//     public int[] playerAnswers;

//     /// <summary>
//     /// which player's turn it is (in a turn based game) and whose
//     /// </summary>
//     public int playerTurn = 0;

//     public bool blindAnswers = false;

//     public void Start(){
//         optionTriggerTests = new Func<Boolean>[] {
//             (() => MainInputController.OneDown),
//             (() => MainInputController.TwoDown),
//             (() => MainInputController.ThreeDown),
//             (() => MainInputController.FourDown)
//         };
//     }

//     public void GenerateRandom()
//     {
//         // current = Categories.Random().RandomChild().Random();
//         StartCoroutine("TyperEffectDisplay");
//     }

//     public void UpdatePlayerTurnDisplay()
//     {
//         currPlayerText.text = Game.game.players[playerTurn].name;
//         currPlayerText.color = Game.game.players[playerTurn].color;
//     }

//     public void ResetSlide()
//     {

//         ClearAnswers();

//         if (individualPlayer != -1)
//             playerTurn = individualPlayer;
//         else
//             playerTurn = 0;
//         UpdatePlayerTurnDisplay();

//         for (int i = 0; i < indicatorContainers.Length; i++)
//         {
//             indicatorContainers[i].DestroyChildren();
//             indicatorContainers[i].transform.parent.gameObject.SetActive(false);
//         }

//         StartCoroutine("TypeQuestionText");
//     }

//     public void ClearAnswers(){
//         playerAnswers = new int[Game.game.players.Length];
//         for (int i = 0; i < playerAnswers.Length; i++)
//             playerAnswers[i] = -1;
//     }

//     public IEnumerator TypeQuestionText()
//     {
//         string fullText = current.questionText;

//         for (int i = 0; i < options.Length; i++)
//         {
//             options[i].GetComponent<Image>().color = defaultColor;
//             options[i].gameObject.SetActive(false);
//         }

//         GetComponent<TextMeshProUGUI>().text = fullText;
//         string currText = "";
//         for (int i = 0; i < fullText.Length; i++)
//         {
//             currText += fullText[i];
//             GetComponent<TextMeshProUGUI>().text = currText;
//             yield return new WaitForEndOfFrame();
//         }

//         for (int i = 0; i < current.options.Length; i++)
//         {
//             options[i].gameObject.SetActive(true);
//             indicatorContainers[i].transform.parent.gameObject.SetActive(true);
//             string text = current.options[i];
//             if(optionTriggerTests.Length > i && InputManager.GamepadConnected)
//                 text += $" ({optionTriggerTests[i].GetBindingDisplayString()})";

//             options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
//         }
//     }

//     public void ReportOptionPicked(int option)
//     {
//         if(individualPlayer == -1 && playerAnswers[playerTurn] != -1)
//             return;

//         playerAnswers[playerTurn] = option;

//         if (!blindAnswers)
//         {
//             GameObject indicator = GameObject.Instantiate(Resources.Load<GameObject>(indicatorPrefabPath), indicatorContainers[option]);
//             indicator.GetComponent<Image>().color = Game.players[playerTurn].color;
//         }

//         if (CheckPlayerStillNeedsToAnser() && individualPlayer == -1)
//         {
//             playerTurn++;
//             playerTurn %= Game.players.Length;
//             UpdatePlayerTurnDisplay();
//         }
//         else
//             StartCoroutine(Complete(playerAnswers));

//     }

//     private bool CheckPlayerStillNeedsToAnser()
//     {
//         bool flag = false;
//         for (int i = 0; i < Game.players.Length; i++)
//         {
//             if (playerAnswers[i] == -1)
//                 flag = true;
//         }
//         return flag;
//     }

//     public IEnumerator Complete(params int[] optionsSelected)
//     {
//         foreach (int option in optionsSelected){
//             if (current.correct != option && option != -1)
//                 options[option].GetComponent<Image>().color = incorrectColor;
//         }

//         if(blindAnswers){
//             for (int i = 0; i < optionsSelected.Length; i++){
//                 if(optionsSelected[i] != -1)
//                 {
//                     GameObject indicator = GameObject.Instantiate(Resources.Load<GameObject>(indicatorPrefabPath), indicatorContainers[optionsSelected[i]]);
//                     indicator.GetComponent<Image>().color = Game.players[i].color;
//                 }
//             }
//         }

//         options[current.correct].GetComponent<Image>().color = correctColor;

//         yield return new WaitForSeconds(answerHoldTime);


//         // Debug.Log(OnComplete.GetInvocationList().Length);
//         OnComplete?.Invoke();
//     }

//     public void Update(){
//         if(optionTriggerTests.Length < options.Length){
//             optionTriggerTests = new InputAction[] {
//                 InputManager.MainInput.MCQMapping.OptionBottom,
//                 InputManager.MainInput.MCQMapping.OptionRight,
//                 InputManager.MainInput.MCQMapping.OptionLeft,
//                 InputManager.MainInput.MCQMapping.OptionTop
//             };
//         }

//         for (int i = 0; i < options.Length; i++)
//         {
//             InputAction action = optionTriggerTests[i];
//             if (action.activeControl != null)
//             {
//                 if (action.activeControl is ButtonControl buttonControl)
//                 {
//                     if (buttonControl.wasPressedThisFrame)
//                         ReportOptionPicked(i);
//                 }
//             }
//         }
//     }
// }
