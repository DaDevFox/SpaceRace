using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class WorldObject{
    public int player;
    public MeshRenderer[] renderers;

    public bool faded;
}

public class Game : MonoBehaviour
{
    public enum Stage{
        PLAYER_SELECT = -1,
        MCQ = 0,
        SHOP = 1,
        TRAJECTORY = 2,
        LAUNCH = 3,
        VICTORY = 4
    }

    
    public enum UIMode{
        LIGHT,
        DARK
    }

    public static UIMode uiMode = UIMode.LIGHT;
    public static bool postCollisionThrust = false;

    private static Stage _stage = Stage.MCQ;
    public static Stage stage {
        get => _stage;
        set {
            if(_stage != value){
                _stage = value;
                stageChange?.Invoke(value);

                game.UpdateStage();
            }
        }
    }

    public static Game game;


    [Header("Scene")]
    public GameObject canvas;
    public ComparerCamera mainCamera;
    public MCQController MCQ;

    
    public List<WorldObject> worldObjects = new List<WorldObject>();
    

    public Player currPlayer => players[TurnSystem.currPlayer];
    public static void NotifyPlayerChange(int player) => playerChange?.Invoke(player);
    public void SetCurrPlayer(int player){
        TurnSystem.currPlayer = player % 4;
        playerChange?.Invoke(player);
    }

    private int _focusingPlayer = -1;
    public int focusingPlayer {
        get => _focusingPlayer;
        set{
            if(_focusingPlayer != value){
                _focusingPlayer = value;
                UpdateFocus(value);
            }
        }
    }

    public static List<int> bonusesUnclaimed = new List<int>(){
        200, 400, 600, 800
    };

    [Header("Game")]
    public Player[] players = new Player[4];

    public static int gameVictor = -1;
    public static int winHeight = 1000;
   
    public int round = 1;

    public int numPlayers => players.Length;
    
    [SerializeField]
    private int f_currentPlayer;
    [SerializeField]
    private int f_focusingPlayer;
    [SerializeField]
    private Stage f_stage;

    public static event Action<int> focusChange;
    public static event Action<int> playerChange;
    public static event Action<Stage> stageChange;

    [Header("Questions")]
    public int questionsPerRound = 1;
    private int questionsThisRound = 0;

    public List<Question> allQuestions = new List<Question>();
    private List<Question> usedQuestions = new List<Question>();

    public static bool prelaunch = true;

    public void Start(){
        stage = Stage.PLAYER_SELECT;

        foreach(Player player in players){
            player.Init();
            worldObjects.Add(new WorldObject(){
                player = player.id,
                faded = false,
                renderers = player.launcher.meshes.ToArray()
            });
            
            worldObjects.Add(new WorldObject(){
                player = player.id,
                faded = false,
                renderers = player.launcher.rocket.bodyMeshes.ToArray()
            });
        }

        playerChange += (player) => {
            // UpdateFocus(focusingPlayer);
        };
    }

    #region Stage-specific code

    public void SetStage(Stage stage) => Game.stage = stage;

    public void UpdateStage(){
        if(stage == Stage.MCQ)
        {
            questionsThisRound = 0;
            MCQ.question = GetRandomQuestion();
            MCQ.ResetQuestion();
        }

        if(stage == Stage.LAUNCH){
            prelaunch = true;
            foreach(Player player in players)
                player.launcher.rocket.Reset();
        }
    }

    public void MCQComplete(){
        if(stage == Stage.MCQ){
            usedQuestions.Add(MCQ.question);
            allQuestions.Remove(MCQ.question);
            
            
            questionsThisRound++;
            if(questionsThisRound < questionsPerRound){
                MCQ.question = GetRandomQuestion(); // constrict by difficulty?
                MCQ.ResetQuestion();
            }else{
                questionsThisRound = 0;
                stage = Stage.SHOP;
            }  
        }
    }

    public static Question GetRandomQuestion(int difficulty = -1){
        if(difficulty != -1){
            List<Question> questions = (List<Question>)Game.game.allQuestions.Where((q) => q.difficulty == difficulty);
            return questions[UnityEngine.Random.Range(0, questions.Count)];
        }else
            return game.allQuestions[UnityEngine.Random.Range(0, game.allQuestions.Count)];
    }

    public void LaunchAllRockets(){
        for(int i = 0; i < players.Length; i++){
            players[i].launcher.Fire();
        }
    }

    public void SetStageMCQ() => stage = Stage.MCQ;
    public void SetStageSHOP() => stage = Stage.SHOP;
    public void SetStageLAUNCH() => stage = Stage.LAUNCH;

    

    #endregion

    public void Awake(){
        Game.game = this;
        MCQController.questionComplete += MCQComplete;
    }

    public void Update(){
        f_currentPlayer = currPlayer.id;
        f_focusingPlayer = focusingPlayer;
        f_stage = _stage;
    }


    #region Focus

    public void UpdateFocus(int focusingPlayer){
        // mainCamera.target = focusingPlayer;
        float fadeTime = 1f;

        if(focusingPlayer == -1){
            StopAllCoroutines();
            foreach(WorldObject obj in worldObjects){
                if(obj.faded)
                    StartCoroutine(FadeInCoroutine(obj, fadeTime));
            }
        }else{
            StopAllCoroutines();
            foreach(WorldObject obj in worldObjects){
                if((obj.player != focusingPlayer && obj.player != currPlayer.id) || obj.player == -1){
                    if(!obj.faded)
                        StartCoroutine(FadeOutCoroutine(obj, fadeTime));
                }if(obj.player == focusingPlayer){
                    if(obj.faded)
                        StartCoroutine(FadeInCoroutine(obj, fadeTime));
                }
            }
        }

        focusChange?.Invoke(focusingPlayer);
    }

    public IEnumerator FadeOutCoroutine(WorldObject obj, float time, float minAlpha = 0.2f){
        float currTime = 0f;
        while(currTime < time){
            foreach(MeshRenderer renderer in obj.renderers){
                if(renderer == null)
                    continue;
                Color color = renderer.material.color;
                renderer.material.color = new Color(color.r, color.g, color.b, (time - currTime)/time * (1 - minAlpha) + minAlpha);
            }
            yield return new WaitForEndOfFrame();
            currTime += Time.deltaTime;
        }
        obj.faded = true;
    }

    public IEnumerator FadeInCoroutine(WorldObject obj, float time, float minAlpha = 0.2f){
        float currTime = 0f;
        while(currTime < time){
            foreach(MeshRenderer renderer in obj.renderers){
                if(renderer == null)
                    continue;
                Color color = renderer.material.color;
                renderer.material.color = new Color(color.r, color.g, color.b, (1 - (time - currTime)/time) * (1 - minAlpha) + minAlpha);
            }
            yield return new WaitForEndOfFrame();
            currTime += Time.deltaTime;
        }
        obj.faded = false;
    }

    #endregion 

}

[Serializable]
public class Player{
    public string name;
    public int id;
    public Color color;
    public Launcher launcher;

    [SerializeField]
    public Upgrades upgrades;

    public int currency = 0;

    public void Init(){
        launcher.Init(id);
    }
}

[Flags]
public enum Upgrades{
    FUEL_1 = (1 << 0),
    FUEL_2 = (1 << 1),
    FUEL_3 = (1 << 2),

    THRUST_1 = (1 << 3),
    THRUST_2 = (1 << 4),
    THRUST_3 = (1 << 5),

    CONTROL_1 = (1 << 6),
    CONTROL_2 = (1 << 7),

    RADIO_1 = (1 << 8),
    RADIO_2 = (1 << 9)
}
