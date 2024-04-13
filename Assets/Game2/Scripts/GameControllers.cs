using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllers : MonoBehaviour
{
    public static GameControllers Instance { get; private set; }
    public static event System.Action<MahjongSO> OnMatched;
    public static event System.Action OnCompleteRequest;
    public Transform Center;

    [SerializeField] private List<MahjongSO> _allMahjongData;
    [SerializeField] private LayerMask _mahjongLayer;


    private Vector2 _mahjongOffset = new Vector2(1, 1.35f);
    private List<Mahjong> Mahjongs = new();
    private List<MahjongSO> _initializeData;
    private int _currentGetDataIndex = 0;


    private Vector3Int layer1;
    private Vector3Int layer2;
    private Vector3Int layer3;


    public int Layer1Size { get => layer1.x + layer1.y + layer1.z; }
    public int Layer2Size { get => layer2.x + layer2.y + layer2.z; }
    public int Layer3Size { get => layer3.x + layer3.y + layer3.z; }

    private System.Random rng;



    public enum PlayState
    {
        CheckingCanplayable,
        Default,
        Checking
    }
    public PlayState State;
    private float _checkTimer = 0.0f;

    // Selection
    public Mahjong SelectionA;
    public Mahjong SelectionB;


    // game event
    public MahjongSO RequestMahjong { get; private set; }
    public bool CountDown { get; private set; } = false;
    private float _eventTime = 15.0f; // time in seconds
    public float EventTimer { get; private set; } = 0.0f;


    private void Awake()
    {
        Instance = this;
        long seed = System.DateTime.Now.Ticks;
        rng = new System.Random((int)seed);
    }

    private void Start()
    {
        TimerManager.OnEventTimeReached += AddGameplayEvent;
        OnMatched += CheckRequest;

        layer1 = GameManager.Instance.CurrentLevel.Layer1;
        layer2 = GameManager.Instance.CurrentLevel.Layer2;
        layer3 = GameManager.Instance.CurrentLevel.Layer3;


        State = PlayState.CheckingCanplayable;
        int size = layer1.x + layer1.y + layer1.z + layer2.x + layer2.y + layer2.z + layer3.x + layer3.y + layer3.z;
        if (size % 2 != 0)
        {
            Debug.Log("Size not a even number.");
        }

        _initializeData = new();
        _currentGetDataIndex = 0;
        for (int i = 0; i < size; i++)
        {
            if (i < size / 2)
            {
                _initializeData.Add(_allMahjongData[Random.Range(0, _allMahjongData.Count)]);
            }
            else
            {
                _initializeData.Add(_initializeData[i % (size / 2)]);
            }
        }
        ShuffleList(_initializeData);

        GenerateLayerOfMahjong(layer1.x, layer1.y, layer1.z, 0);
        GenerateLayerOfMahjong(layer2.x, layer2.y, layer2.z, 1);
        GenerateLayerOfMahjong(layer3.x, layer3.y, layer3.z, 2);
    }


    private void OnDestroy()
    {
        TimerManager.OnEventTimeReached -= AddGameplayEvent;
        OnMatched -= CheckRequest;
    }

    public void AddGameplayEvent()
    {
        RequestMahjong = Mahjongs[0].Data;
        EventTimer = _eventTime;
        CountDown = true;
    }
    public void RecreateTable()
    {
        _currentGetDataIndex = 0;
        _initializeData.Clear();
        for(int i = 0; i < Mahjongs.Count; i++)
        {
            _initializeData.Add(Mahjongs[i].Data);
        }
        ShuffleList(_initializeData);
        for (int i = 0; i < Mahjongs.Count; i++)
        {
            Destroy(Mahjongs[i].gameObject);
        }
        Mahjongs.Clear();

        GenerateLayerOfMahjong(layer1.x, layer1.y, layer1.z, 0);
        GenerateLayerOfMahjong(layer2.x, layer2.y, layer2.z, 1);
        GenerateLayerOfMahjong(layer3.x, layer3.y, layer3.z, 2);

    }


    private void Update()
    {
        if (GameplayManager.Instance.CurrentState != GameplayManager.GameState.PLAYING) return;
  
        switch (State)
        {
            case PlayState.CheckingCanplayable:
                if(CanPlayble())
                {
                    State = PlayState.Default;
                }
                else
                {
                    RecreateTable();
                }
                break;
            default:
            case PlayState.Default:
                if (Input.GetMouseButtonDown(0))
                {
                    // Cast a ray from the mouse position
                    Vector2 raycastOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, Vector2.zero, Mathf.Infinity, _mahjongLayer);

                    // Check if any objects are hit
                    if (hits.Length > 0)
                    {
                        // Find the object with the highest sorting order
                        GameObject highestOrderObject = FindObjectWithHighestSortingOrder(hits);

                        // Handle the highest order object
                        if (highestOrderObject != null)
                        {
                            //Debug.Log("Object with highest sorting order hit: " + highestOrderObject.name);
                            // You can perform additional actions here, such as accessing properties or calling methods on the highest order object

                            if (highestOrderObject.gameObject.TryGetComponent<Mahjong>(out Mahjong selectedMahjong))
                            {
                                if (selectedMahjong.CanSelect == false) break;
                                Select(selectedMahjong);

                                if (SelectionA != null && SelectionB != null)
                                {
                                    State = PlayState.Checking;

                                }
                            }


                        }
                    }
                    else
                    {
                        // If the ray doesn't hit any object
                        Debug.Log("No object hit.");
                    }

                }
                break;
            case PlayState.Checking:
                _checkTimer += Time.deltaTime;
                if (_checkTimer > 0.25f)
                {
                    //Debug.Log(Mahjongs.Count);
                    _checkTimer = 0.0f;
                    if (CheckMatch(SelectionA, SelectionB))
                    {
                        SoundManager.Instance.PlaySound(SoundType.Button, false);
                        // Match
                        OnMatched?.Invoke(SelectionA.Data);

                        SelectionA.SetMatchPhysics();
                        SelectionB.SetMatchPhysics();


                        var a = SelectionA.gameObject;
                        var b = SelectionB.gameObject;



                        Mahjongs.Remove(SelectionA);
                        Mahjongs.Remove(SelectionB);

                        Destroy(a.gameObject, 2.0f);
                        Destroy(b.gameObject, 2.0f);

                        SelectionA = null;
                        SelectionB = null;
                    }
                    else
                    {
                        SoundManager.Instance.PlaySound(SoundType.HitBlock, false);
                        // Not match
                        //Debug.Log("No match");

                        // reset selection
                        SelectionA.SelectEffect(false);
                        SelectionB.SelectEffect(false);
                        SelectionA = null;
                        SelectionB = null;
                    }

                    State = PlayState.CheckingCanplayable;
                }
                break;
        }


        if(CountDown)
        {
            EventTimer -= Time.deltaTime;
            if(EventTimer < 0.0f)
            {
                EventTimer = 0.0f;
                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.GAMEOVER);
            }
        }
    }
    private void CheckRequest(MahjongSO data)
    {
        if (CountDown)
        {
            if (RequestMahjong.ID == data.ID)
            {
                CountDown = false;
                EventTimer = 0.0f;
                OnCompleteRequest?.Invoke();
            }
        }
    }

    private void GenerateLayerOfMahjong(int line1Width, int line2Width, int line3Width, int layer)
    {
        float startLine1 = Center.position.x - ((line1Width - 1) / 2.0f * _mahjongOffset.x);
        for (int x = 0; x < line1Width; x++)
        {
            Vector2 position = new Vector2(startLine1 + x * _mahjongOffset.x, Center.position.y + _mahjongOffset.y) + new Vector2(layer * 0.1f, layer * 0.1f);
          
            if(_currentGetDataIndex < _initializeData.Count)
            {
                var data = _initializeData[_currentGetDataIndex];
                _currentGetDataIndex++;
                Mahjongs.Add(CreateMahjong(data, position, layer));
            }       
        }

        float startLine2 = Center.position.x - ((line2Width - 1) / 2.0f * _mahjongOffset.x);
        for (int x = 0; x < line2Width; x++)
        {
            Vector2 position = new Vector2(startLine2 + x * _mahjongOffset.x, Center.position.y) + new Vector2(layer * 0.1f, layer * 0.1f);

            if (_currentGetDataIndex < _initializeData.Count)
            {
                var data = _initializeData[_currentGetDataIndex];
                _currentGetDataIndex++;
                Mahjongs.Add(CreateMahjong(data, position, layer));
            }
        }

        float startLine3 = Center.position.x - ((line3Width - 1) / 2.0f * _mahjongOffset.x);
        for (int x = 0; x < line3Width; x++)
        {
            Vector2 position = new Vector2(startLine3 + x * _mahjongOffset.x, Center.position.y - _mahjongOffset.y) + new Vector2(layer * 0.1f, layer * 0.1f);
            if (_currentGetDataIndex < _initializeData.Count)
            {
                var data = _initializeData[_currentGetDataIndex];
                _currentGetDataIndex++;
                Mahjongs.Add(CreateMahjong(data, position, layer));
            }
        }
    }

    public Vector2 GetPosition(int line, int lineWidth, int layer)
    {
        float startLine = Center.position.x - ((lineWidth - 1) / 2.0f * _mahjongOffset.x);
        if (line == 1)
        {
            return new Vector2(line + Random.Range(0, startLine) * _mahjongOffset.x, Center.position.y + _mahjongOffset.y) + new Vector2(layer * 0.1f, layer * 0.1f);
        }
        else if (line == 2)
        {
            return new Vector2(line + Random.Range(0, startLine) * _mahjongOffset.x, Center.position.y) + new Vector2(layer * 0.1f, layer * 0.1f);
        }
        else
        {
            return new Vector2(line + Random.Range(0, startLine) * _mahjongOffset.x, Center.position.y - _mahjongOffset.y) + new Vector2(layer * 0.1f, layer * 0.1f);
        }
    }

    public Mahjong CreateMahjong(MahjongSO data, Vector2 position, int layer)
    {
        var prefab = Resources.Load<Mahjong>("Mahjong");
        if (prefab != null)
        {
            var mahjongInstance = Instantiate(prefab, position, Quaternion.identity).GetComponent<Mahjong>();
            mahjongInstance.SetData(data, layer);
            return mahjongInstance;
        }
        return null;
    }




    GameObject FindObjectWithHighestSortingOrder(RaycastHit2D[] hits)
    {
        GameObject highestOrderObject = null;
        int highestSortingOrder = int.MinValue;

        foreach (RaycastHit2D hit in hits)
        {
            // Check if the hit object has a sprite renderer
            SpriteRenderer renderer = hit.collider.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                // Check if the sorting order of the sprite renderer is higher than the current highest sorting order
                if (renderer.sortingOrder > highestSortingOrder)
                {
                    highestSortingOrder = renderer.sortingOrder;
                    highestOrderObject = hit.collider.gameObject;
                }
            }
        }

        return highestOrderObject;
    }


    #region Gameplay
    public void Select(Mahjong mahjong)
    {
        if (mahjong == SelectionA) return;

        if (SelectionA == null)
        {
            SelectionA = mahjong;
            SelectionA.SelectEffect(true);

        }
        else if (SelectionB == null)
        {
            SelectionB = mahjong;
            SelectionB.SelectEffect(true);
        }
    }

    public bool CheckMatch(Mahjong a, Mahjong b)
    {
        return a.Data.ID == b.Data.ID;
    }


   
    private bool CanPlayble()
    {
        if (Mahjongs.Count == 0)
        {
            Debug.Log("Win");
            GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.WIN);
            return false;
        }
        for (int i = 0; i < Mahjongs.Count; i++)
        {
            for (int j = 1; j < Mahjongs.Count; j++)
            {
                if (i == j) continue;

                if (Mahjongs[i].CanSelect && Mahjongs[j].CanSelect)
                {
                    if (Mahjongs[i].Data.ID == Mahjongs[j].Data.ID)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
 
    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    #endregion
}
