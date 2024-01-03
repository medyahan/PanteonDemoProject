using System;
using System.Collections;
using System.Collections.Generic;
using MilitaryGame.Building;
using MilitaryGame.Factory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace MilitaryGame.GridBuilding
{
    public class GridBuildingSystem : Singleton<GridBuildingSystem>
    {
        #region Variable Fields

        [SerializeField] private GridLayout _gridLayout;
    
        [Header("TILE MAPS")]
        [SerializeField] private Tilemap _mainTilemap;
        [SerializeField] private Tilemap _tempTilemap;

        [Header("TILE BASES")]
        [SerializeField] private TileBase _whiteTileBase;
        [SerializeField] private TileBase _greenTileBase;
        [SerializeField] private TileBase _redTileBase;
        [SerializeField] private TileBase _fillTileBase;

        private static Dictionary<TileType, TileBase> _tileBases = new Dictionary<TileType, TileBase>();

        private BaseBuilding _tempBaseBuilding;
        private Vector3 _prevBuildingPos;
        private BoundsInt _prevArea;

        public GridLayout GridLayout => _gridLayout;
        public Tilemap MainTilemap => _mainTilemap;
        public BaseBuilding TempBaseBuilding => _tempBaseBuilding;
    
        #endregion // Variable Fields
    
        #region UNITY METHODS

        private void Start()
        {
            _tileBases.Add(TileType.Empty, null);
            _tileBases.Add(TileType.White, _whiteTileBase);
            _tileBases.Add(TileType.Green, _greenTileBase);
            _tileBases.Add(TileType.Red, _redTileBase);
            _tileBases.Add(TileType.Fill, _fillTileBase);
        }

        private void Update()
        {
            if(!_tempBaseBuilding)
                return;
            
            // Check if the pointer is over a UI element.
            if(EventSystem.current.IsPointerOverGameObject(0))
                return;
                
            if (!_tempBaseBuilding.Placed)
            {
                // Get the touch position in world coordinates.
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellpos = _gridLayout.LocalToCell(touchPos);

                // Move the temporary building to the current cell position.
                if (_prevBuildingPos != cellpos)
                {
                    _tempBaseBuilding.transform.localPosition =
                        _gridLayout.CellToLocalInterpolated(cellpos);

                    _prevBuildingPos = cellpos; 
                    FollowBuilding();
                }
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                // Check if the temporary building is already placed.
                if(_tempBaseBuilding.Placed)
                    return;
                
                // Check if the temporary building can be placed.
                if (_tempBaseBuilding.CanBePlaced())
                {
                    _tempBaseBuilding.Place();
                }
            }

            // Check for the escape key to clear the temporary building.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClearTempBuilding();
            }
        }

        #endregion

        #region TILEMAP MANAGEMENT
        
        /// <summary>
        /// Retrieves an array of tiles within the specified area from the given tilemap.
        /// </summary>
        /// <param name="area">The bounds of the area to retrieve tiles from.</param>
        /// <param name="tilemap">The tilemap containing the tiles.</param>
        /// <returns>An array of tiles within the specified area.</returns>
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

        /// <summary>
        /// Sets tiles of the specified type within the given area on the provided tilemap.
        /// </summary>
        /// <param name="area">The bounds of the area to set tiles.</param>
        /// <param name="tileType">The type of tiles to set.</param>
        /// <param name="tilemap">The tilemap to set tiles on.</param>
        private static void SetTilesBlock(BoundsInt area, TileType tileType, Tilemap tilemap)//
        {
            int size = area.size.x * area.size.y * area.size.z;
        
            TileBase[] tileBaseArray = new TileBase[size];

            FillTiles(tileBaseArray, tileType);
            tilemap.SetTilesBlock(area, tileBaseArray);
        }
        
        /// <summary>
        /// Fills the specified array of tile bases with tiles of the given type.
        /// </summary>
        /// <param name="tileBaseArray">The array to fill with tiles.</param>
        /// <param name="tileType">The type of tiles to fill the array with.</param>
        private static void FillTiles(TileBase[] tileBaseArray, TileType tileType)//
        {
            for (int i = 0; i < tileBaseArray.Length; i++)
            {
                tileBaseArray[i] = _tileBases[tileType];
            }
        }
        
        /// <summary>
        /// Clears the area on the temporary tilemap by setting tiles to the Empty type.
        /// </summary>
        private void ClearArea()
        {
            TileBase[] toClearTileBaseArray = new TileBase[_prevArea.size.x * _prevArea.size.y * _prevArea.size.z];
            FillTiles(toClearTileBaseArray, TileType.Empty);
            _tempTilemap.SetTilesBlock(_prevArea, toClearTileBaseArray);
        }
        
        /// <summary>
        /// Updates the temporary tilemap to visualize the area the building will occupy.
        /// </summary>
        private void FollowBuilding()
        {
            ClearArea();

            BoundsInt tempBuildingArea = _tempBaseBuilding.Area;
            tempBuildingArea.position = _gridLayout.WorldToCell(_tempBaseBuilding.gameObject.transform.position);
            
            // Get the tile types in the base area from the main tilemap.
            TileBase[] baseArray = GetTilesBlock(tempBuildingArea, _mainTilemap);
            int size = baseArray.Length;
            TileBase[] tileArray = new TileBase[size];

            // Check each tile in the base area and update the temporary tilemap accordingly.   
            for (int i = 0; i < baseArray.Length; i++)
            {
                if (baseArray[i] == _tileBases[TileType.White])
                {
                    tileArray[i] = _tileBases[TileType.Green];
                }
                else
                {
                    // If any tile is not of type White, set the entire area to Red and exit the loop.
                    FillTiles(tileArray, TileType.Red);
                    break;
                }
            
            }
            
            _tempTilemap.SetTilesBlock(tempBuildingArea, tileArray);
            _prevArea = tempBuildingArea;
        }
        
        /// <summary>
        /// Checks if the specified area on the main tilemap can be taken (all tiles are of type White).
        /// </summary>
        /// <param name="area">The bounds of the area to check.</param>
        /// <returns>True if the area can be taken, false otherwise.</returns>
        public bool CanTakeArea(BoundsInt area)
        {
            TileBase[] baseArray = GetTilesBlock(area, _mainTilemap);

            foreach (TileBase tileBase in baseArray)
            {
                if (tileBase != _tileBases[TileType.White])
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Takes the specified area by setting tiles to Empty on the temporary tilemap and Fill on the main tilemap.
        /// </summary>
        /// <param name="area">The bounds of the area to take.</param>
        public void TakeArea(BoundsInt area)
        {
            SetTilesBlock(area, TileType.Empty, _tempTilemap);
            SetTilesBlock(area, TileType.Fill, _mainTilemap);
        }
        
        /// <summary>
        /// Clears the visualization of the temporary building, ends its placement, and destroys it.
        /// </summary>
        public void ClearTempBuilding()
        {
            ClearArea();
            _tempBaseBuilding.End();
            BuildingFactory.Instance.DestroyBuilding(_tempBaseBuilding);
            _tempBaseBuilding = null;
        }
        
        #endregion

        #region BUILDING PLACEMENT
        
        /// <summary>
        /// Initializes the building placement with a temporary building of the specified type.
        /// </summary>
        /// <param name="buildingType">The type of the building to place.</param>
        public void InitializeWithBuilding(BuildingData.BuildingType buildingType)
        {
            _tempBaseBuilding = BuildingFactory.Instance.CreateBuilding(buildingType, Vector3.zero, Quaternion.identity);
        
            FollowBuilding();   
        }
        
        #endregion
    }

    public enum TileType
    {
        Empty,
        White,
        Green,
        Red,
        Fill
    }
}