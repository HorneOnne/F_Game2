using UnityEngine;


public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }
    public static event System.Action OnStateChanged;
    public static event System.Action OnPlaying;
    public static event System.Action OnWin;
    public static event System.Action OnGameOver;
    public static event System.Action OnRoundFinished;
    public static event System.Action OnStartNextRound;

    public GameObject Match3Prefab;
    private GameObject _match3Instance;

    public enum GameState
    {
        PLAYING,
        WAITING,
        WIN,
        GAMEOVER,
        PAUSE,
        UNPAUSE,
        EXIT,
    }


    [Header("Properties")]
    [SerializeField] private GameState _currentState;
    private GameState _gameStateWhenPause;


    #region Properties
    public GameState CurrentState { get => _currentState; }
    #endregion


    #region Init & Events
    private void Awake()
    {
        Instance = this;

    }

    private void OnEnable()
    {
        OnStateChanged += SwitchState;
    }

    private void OnDisable()
    {
        OnStateChanged -= SwitchState;
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
        ChangeGameState(GameState.WAITING);

    }
    #endregion



    public void ChangeGameState(GameState state)
    {
        _currentState = state;
        OnStateChanged?.Invoke();
    }

    public void CacheGameStateWhenPause(GameState state)
    {
        _gameStateWhenPause = state;
    }

    private void SwitchState()
    {
        switch (_currentState)
        {
            default: break;
            case GameState.WAITING:


                break;
            case GameState.PLAYING:
                Time.timeScale = 1.0f;
            
                OnPlaying?.Invoke();
                break;
            case GameState.WIN:
              
                OnWin?.Invoke();
                break;
            case GameState.GAMEOVER:
              
                OnGameOver?.Invoke();
                break;
            case GameState.PAUSE:
                Time.timeScale = 0.0f;
                break;
            case GameState.UNPAUSE:
                Time.timeScale = 1.0f;
                _currentState = GameState.PLAYING;
                break;
            case GameState.EXIT:
                Time.timeScale = 1.0f;
                break;
        }
    }

    public void CreateMatch3()
    {
        if(_match3Instance == null)
        {
            _match3Instance = Instantiate(Match3Prefab);
            _match3Instance.GetComponent<Canvas>().worldCamera = Camera.main;
        }
            
    }


    public void RemoveMatch3()
    {
        if (_match3Instance != null)
        {
            Destroy(_match3Instance.gameObject);
        }
    }
}


