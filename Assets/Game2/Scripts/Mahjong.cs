using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mahjong : MonoBehaviour
{
    private SpriteRenderer _sr;
    private BoxCollider2D _boxCollider2D;
    private MahjongSO _data;
    public SpriteRenderer SpriteRenderer { get => _sr; }
    public MahjongSO Data { get => _data; }
    public int Layer;
    private Vector3 _selectedOffestPosition = new Vector3(0f, 0.15f, 0);
    public bool CanSelect { get; private set; }
    [SerializeField] private LayerMask _mahjongLayer;
    private Collider2D[] _colliders = new Collider2D[5];


    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {

        if (CheckBoundsIntersection())
        {
            CanSelect = false;
            _sr.material.SetColor("_Color", Color.white * 0.7f);

        }
        else
        {
            CanSelect = true;
            _sr.material.SetColor("_Color", Color.white);
        }
    }

    public void SetData(MahjongSO data, int layer)
    {
        this._data = data;
        this.Layer = layer;
        _sr.sprite = this._data.Icon;
        _sr.sortingOrder = layer;

        transform.position = new Vector3(transform.position.x, transform.position.y, layer);
    }

    public void SelectEffect(bool select)
    {
        if (select)
        {
            transform.position += _selectedOffestPosition;
            transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
            _sr.sortingOrder = 100;
        }
        else
        {
            transform.position -= _selectedOffestPosition;
            transform.localScale = new Vector3(1, 1, 1);
            _sr.sortingOrder = Layer;
        }
    }

    public bool CheckBoundsIntersection()
    {
        // Calculate the bounds of the BoxCollider2D in world space
        Bounds bounds = _boxCollider2D.bounds;

        int hit = Physics2D.OverlapBoxNonAlloc(bounds.center, bounds.size, 0f, _colliders, _mahjongLayer);
        // Check if any colliders on the specified layer intersect with the bounds
        if (hit > 0)
        {
            for (int i = 0; i < hit; i++)
            {
                if (Layer < _colliders[i].transform.position.z)
                {
                    return true;
                }
            }
        }
        return false;
    }


    public void SetMatchPhysics()
    {
        gameObject.AddComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(-90, 90));
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().gravityScale = 1.55f;

        _boxCollider2D.enabled = false;
    }
}
