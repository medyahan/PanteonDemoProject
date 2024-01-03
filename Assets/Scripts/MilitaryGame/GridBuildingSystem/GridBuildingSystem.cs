using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace MilitaryGame.GridBuildingSystem
{
    public class GridBuildingSystem : MonoBehaviour
    {
        #region Variable Fields
    
        public static GridBuildingSystem Current;//

        [SerializeField] private GridLayout _gridLayout;//
    
        [Header("TILE MAPS")]
        [SerializeField] private Tilemap _mainTilemap;//
        [SerializeField] private Tilemap _tempTilemap;//    

        [Header("TILE BASES")]
        [SerializeField] private TileBase _whiteTileBase;
        [SerializeField] private TileBase _greenTileBase;
        [SerializeField] private TileBase _redTileBase;

        private static Dictionary<TileType, TileBase> _tileBases = new Dictionary<TileType, TileBase>();

        private Buildings.Building _tempBuilding;//
        private Vector3 _prevBuildingPos;//
        private BoundsInt _prevArea;//

        public GridLayout GridLayout => _gridLayout;//
        public Buildings.Building TempBuilding => _tempBuilding;
    
        #endregion // Variable Fields
    
        #region UNITY METHODS

        private void Awake()//
        {
            Current = this;
        }

        private void Start()//
        {
            _tileBases.Add(TileType.Empty, null);
            _tileBases.Add(TileType.White, _whiteTileBase);
            _tileBases.Add(TileType.Green, _greenTileBase);
            _tileBases.Add(TileType.Red, _redTileBase);
        }

        private void Update()
        {
            if(!_tempBuilding)
                return;
            
            if(EventSystem.current.IsPointerOverGameObject(0))
                return;
                
            if (!_tempBuilding.Placed)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellpos = _gridLayout.LocalToCell(touchPos);

                if (_prevBuildingPos != cellpos)
                {
                    _tempBuilding.transform.localPosition =
                        _gridLayout.CellToLocalInterpolated(cellpos);

                    _prevBuildingPos = cellpos; 
                    FollowBuilding();
                }
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if(_tempBuilding.Placed)
                    return;
                
                if (_tempBuilding.CanBePlaced())
                {
                    Debug.Log("placed: " + _tempBuilding.gameObject.name);
                    _tempBuilding.Place();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClearTempBuilding();
            }
        }

        #endregion

        #region TILEMAP MANAGEMENT
        
        private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)//
        {
            TileBase[] tileBaseArray = new TileBase[area.size.x * area.size.y * area.size.z];

            int counter = 0;

            foreach (Vector3Int vector3 in area.allPositionsWithin)
            {
                Vector3Int pos = new Vector3Int(vector3.x, vector3.y, 0);
                tileBaseArray[counter] = tilemap.GetTile(pos);
                counter++;
            }

            return tileBaseArray;
        }

        private static void SetTilesBlock(BoundsInt area, TileType tileType, Tilemap tilemap)//
        {
            int size = area.size.x * area.size.y * area.size.z;
        
            TileBase[] tileBaseArray = new TileBase[size];

            FillTiles(tileBaseArray, tileType);
            tilemap.SetTilesBlock(area, tileBaseArray);
        }
        
        private static void FillTiles(TileBase[] tileBaseArray, TileType tileType)//
        {
            for (int i = 0; i < tileBaseArray.Length; i++)
            {
                tileBaseArray[i] = _tileBases[tileType];
            }
        }
        
        private void ClearArea()
        {
            TileBase[] toClearTileBaseArray = new TileBase[_prevArea.size.x * _prevArea.size.y * _prevArea.size.z];
            FillTiles(toClearTileBaseArray, TileType.Empty);
            _tempTilemap.SetTilesBlock(_prevArea, toClearTileBaseArray);
        }
        
        private void FollowBuilding()
        {
            ClearArea();

            BoundsInt tempBuildingArea = _tempBuilding.Area;
            tempBuildingArea.position = _gridLayout.WorldToCell(_tempBuilding.gameObject.transform.position);
            //tempBuildingArea.position = _gridLayout.WorldToCell(_tempBuilding.gameObject.transform.position);

            //_tempBuilding.Area = tempBuildingArea;

            TileBase[] baseArray = GetTilesBlock(tempBuildingArea, _mainTilemap);

            int size = baseArray.Length;

            TileBase[] tileArray = new TileBase[size];

            for (int i = 0; i < baseArray.Length; i++)
            {
                if (baseArray[i] == _tileBases[TileType.White])
                {
                    tileArray[i] = _tileBases[TileType.Green];
                }
                else
                {
                    FillTiles(tileArray, TileType.Red);
                    break;
                }
            
            }
            
            _tempTilemap.SetTilesBlock(tempBuildingArea, tileArray);
            _prevArea = tempBuildingArea;
        }
        
        public bool CanTakeArea(BoundsInt area)
        {
            TileBase[] baseArray = GetTilesBlock(area, _mainTilemap);

            foreach (TileBase tileBase in baseArray)
            {
                if (tileBase != _tileBases[TileType.White])
                {
                    Debug.Log("Cannot place here");
                    return false;
                }
            }

            return true;
        }
        
        public void TakeArea(BoundsInt area)
        {
            SetTilesBlock(area, TileType.Empty, _tempTilemap);
            SetTilesBlock(area, TileType.Green, _mainTilemap);
        }
        
        public void ClearTempBuilding()
        {
            ClearArea();
            Destroy(_tempBuilding.gameObject);
        }
        
        #endregion

        #region BUILDING PLACEMENT
        
        public void InitializeWithBuilding(Buildings.Building building)
        {
            _tempBuilding = Instantiate(building, Vector3.zero, Quaternion.identity);
        
            FollowBuilding();   
        }
        
        #endregion
    }

    public enum TileType//
    {
        Empty,
        White,
        Green,
        Red
    }
}