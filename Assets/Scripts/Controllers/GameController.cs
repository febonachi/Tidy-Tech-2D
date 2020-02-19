using UnityEngine;
using UnityEngine.SceneManagement;

using Utilsf;

public class GameController : MonoBehaviour {
    #region singleton
    static public GameController instance;
    #endregion

    #region editor
    public UIController ui = default;
    public GameObject winEffect = default;
    public SwipeController swipeController = default;
    public AudioController audioController = default;

    public int sceneToLoadFirst = 1;
    #endregion

    private Maze maze;
    private PlayerController player;
    private Score score = new Score();

    #region private
    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(this);

        //GameAnalytics.Initialize();

        swipeController.swipe += onSwiped;

        ui.addNextLevelButtonListener(nextLevel);
        ui.addRestartButtonListener(restartLevel);
        SceneManager.sceneLoaded += onSceneLoaded;

        if (sceneToLoadFirst == 0) { 
            int currentSceneIndex = PlayerPrefs.GetInt("currentSceneIndex");
            SceneManager.LoadScene(currentSceneIndex != 0 ? currentSceneIndex : 1);
        } else SceneManager.LoadScene(sceneToLoadFirst);
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "main") return;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("currentSceneIndex", currentSceneIndex);

        maze = Maze.instance;
        player = FindObjectOfType<PlayerController>();

        addDelegates();

        ui.indicatorsCount = maze.repairTilesTotalCount;
        ui.reset(currentSceneIndex);
        score.reset();

        audioController.playRandomBackground();
    }

    private void addDelegates() {
        maze.tileRepaired += onTileRepaired;
        maze.tileBrokeDown += onTileBrokeDown;

        player.moved += onPlayerMoved;
        player.exited += onPlayerExited;
        player.trapped += onPlayerTrapped;
        player.enemyKilled += onEnemyKilled;
        player.repairing += onPlayerRepairing;
        player.obstacleEntered += onPlayerObstacleEntered;
    }

    private void removeDelegates() {
        maze.tileRepaired -= onTileRepaired;
        maze.tileBrokeDown -= onTileBrokeDown;

        player.moved -= onPlayerMoved;
        player.exited -= onPlayerExited;
        player.trapped -= onPlayerTrapped;
        player.enemyKilled -= onEnemyKilled;
        player.repairing -= onPlayerRepairing;
        player.obstacleEntered -= onPlayerObstacleEntered;
    }

    private void onPlayerMoved() {
        audioController.play("playerMove");
    }

    private void onPlayerObstacleEntered() {
        audioController.play("obstacle");
    }

    private void onPlayerExited() {
        ui.show(UIController.UIStyle.UINext);

        Destroy(Instantiate(winEffect), 2f);
    }

    private void onPlayerTrapped() {
        audioController.play("trap");
        ui.showMessage("Ooops...", Utility.colorByString("#aa00ae"));
        ui.show(UIController.UIStyle.UIRestart);
    }

    private void onPlayerRepairing() {
        audioController.play("repair");
    }

    private void onTileRepaired(int count) {
        if (count == maze.repairTilesTotalCount) {
            audioController.play("spiderDie");
            audioController.play("doorsOpen");
        }
        ui.showMessage("Repaired!", Utility.colorByString("#FFE900"));
        ui.highlightIndicators(count);

        score.tileRepaired();
    }

    private void onTileBrokeDown(int count) {
        audioController.play("spiderDie");
        ui.showMessage("Broken...", Utility.colorByString("#d61212"));
        ui.highlightIndicators(count);
    }

    private void onEnemyKilled() {
        audioController.play("spiderDie");
        ui.showMessage("Squashed!", Utility.colorByString("#ffffff"));
        ui.enemyPanel.enemyKilled();

        score.enemyKilled();
    }

    private void onSwiped() {
        player.hideHint();
        ui.hideSwipeHint();
    }

    private void restartLevel() {
        removeDelegates();        

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        ui.hide();
    }

    private void nextLevel() {
        removeDelegates();

        int currentSceneIndex = PlayerPrefs.GetInt("currentSceneIndex");

        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex < SceneManager.sceneCountInBuildSettings ? nextSceneIndex : 1);

        ui.hide();
    }
    #endregion
}
