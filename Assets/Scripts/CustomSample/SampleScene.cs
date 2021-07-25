using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    public enum ESampleType
    {
        Polygon,
        Edge,
    }

    public Sprite[] sprites;
    public Button setPolygonButton;
    public Button setEdgeButton;
    public Button setChangeSpriteButton;
    
    public Button refreshButton;
    public Button resetButton;

    public GameObject polygonSample;
    public GameObject edgeSample;
    
    public ColorPicker colorPicker;
    public Slider toleranceSlider;

    private LineRenderer _line;
    private PolygonSimplication _polygonSimplication;
    private EdgeSimplication _edgeSimplication;

    private ESampleType sampleType = ESampleType.Polygon;

    private int spriteIndex = 0;
    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _polygonSimplication = FindObjectOfType<PolygonSimplication>();
        _edgeSimplication = FindObjectOfType<EdgeSimplication>();
    }

    private void Start()
    {
        setPolygonButton.onClick.AddListener(OnClickSetPolygon);
        setEdgeButton.onClick.AddListener(OnClickSetEdge);
        setChangeSpriteButton.onClick.AddListener(OnClickChangeSprite);
        
        SwitchState(sampleType);
    }

    private void Update()
    {
        _line.startColor = colorPicker.curColor;
        _line.endColor = colorPicker.curColor;
        
        if (sampleType == ESampleType.Polygon)
        {
            if (_polygonSimplication == null)
            {
                return;
            }

            _polygonSimplication.tolerance = toleranceSlider.value;

            _line.positionCount = _polygonSimplication.Path.Length;
            for (var i = 0; i < _polygonSimplication.Path.Length; i++)
            {
                _line.SetPosition(i, _polygonSimplication.Path[i]);
            }
        }
        else
        {
            if (_edgeSimplication == null)
            {
                return;
            }
            
            _edgeSimplication.tolerance = toleranceSlider.value;

            _line.positionCount = _edgeSimplication.EdgePath.Length;
            for (var i = 0; i < _edgeSimplication.EdgePath.Length; i++)
            {
                _line.SetPosition(i, _edgeSimplication.EdgePath[i]);
            }
        }
    }

    private void SwitchState(ESampleType type)
    {
        sampleType = type;
        switch (type)
        {
            case ESampleType.Polygon:
                AcitvePolygon(true);
                ActiveSetEdge(false);
                break;
            case ESampleType.Edge:
                AcitvePolygon(false);
                ActiveSetEdge(true);
                break;
        }
    }

    private void AcitvePolygon(bool enable)
    {
        polygonSample.gameObject.SetActive(enable);

        if (enable)
        {
            _polygonSimplication = polygonSample.GetComponent<PolygonSimplication>();
            refreshButton.onClick.RemoveAllListeners();
            resetButton.onClick.RemoveAllListeners();
            refreshButton.onClick.AddListener(OnClickPolygonRefresh);
            resetButton.onClick.AddListener(OnClickPolygonReset);
        }
        else
        {
            _polygonSimplication = null;
        }
    }
    
    private void ActiveSetEdge(bool enable)
    {
        edgeSample.gameObject.SetActive(enable);

        if (enable)
        {
            _edgeSimplication = edgeSample.GetComponent<EdgeSimplication>();
            refreshButton.onClick.RemoveAllListeners();
            resetButton.onClick.RemoveAllListeners();
            refreshButton.onClick.AddListener(OnClickEdgeRefresh);
        }
        else
        {
            _edgeSimplication = null;
        }
    }

    private void OnClickSetPolygon()
    {
        SwitchState(ESampleType.Polygon);
    }
    
    private void OnClickSetEdge()
    {
        SwitchState(ESampleType.Edge);
    }
    
    private void OnClickPolygonRefresh()
    {
        _polygonSimplication.Refresh();
    }
    
    private void OnClickPolygonReset()
    {
        _polygonSimplication.Reset();
    }
    
    private void OnClickEdgeRefresh()
    {
        _edgeSimplication.Refresh();
    }
    
    private void OnClickChangeSprite()
    {
        if (sampleType == ESampleType.Polygon)
        {
            if (_polygonSimplication == null)
            {
                return;
            }
            _polygonSimplication.GetComponent<SpriteRenderer>().sprite = sprites[(spriteIndex++) % sprites.Length];
        }
        else
        {
            if (_edgeSimplication == null)
            {
                _edgeSimplication.GetComponent<SpriteRenderer>().sprite = sprites[(spriteIndex++) % sprites.Length];
            }
        }
    }
    
}
