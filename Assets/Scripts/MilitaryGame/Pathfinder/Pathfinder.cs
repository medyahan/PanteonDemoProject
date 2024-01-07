using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MilitaryGame.Pathfinder
{
    public class Pathfinder : Singleton<Pathfinder>
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _whiteTile;

        // A* pathfinding 
        public List<Vector3Int> FindPath(Vector3Int start, Vector3Int end)
        {
            List<Vector3Int> path = new List<Vector3Int>();
            List<Vector3Int> openSet = new List<Vector3Int>();
            List<Vector3Int> closedSet = new List<Vector3Int>();

            openSet.Add(start);

            Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
            Dictionary<Vector3Int, int> gScore = new Dictionary<Vector3Int, int>();
            gScore[start] = 0;

            while (openSet.Count > 0)
            {
                Vector3Int current = GetLowestFScore(openSet, gScore, end);

                if (current == end)
                {
                    path = ReconstructPath(cameFrom, current);
                    break;
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (Vector3Int neighbor in GetNeighbors(current))
                {
                    if (closedSet.Contains(neighbor) || !IsWalkable(neighbor))
                        continue;

                    int tentativeGScore = gScore[current] + 1;

                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            return path;
        }

        private Vector3Int GetLowestFScore(List<Vector3Int> openSet, Dictionary<Vector3Int, int> gScore, Vector3Int end)
        {
            int lowestFScore = int.MaxValue;
            Vector3Int lowestFScoreNode = Vector3Int.zero;

            foreach (Vector3Int node in openSet)
            {
                int fScore = gScore[node] + HeuristicCostEstimate(node, end);
                if (fScore < lowestFScore)
                {
                    lowestFScore = fScore;
                    lowestFScoreNode = node;
                }
            }

            return lowestFScoreNode;
        }

        private int HeuristicCostEstimate(Vector3Int start, Vector3Int end)
        {
            return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
        }

        private List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
        {
            List<Vector3Int> path = new List<Vector3Int>();
            while (cameFrom.ContainsKey(current))
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Reverse();
            return path;
        }

        private bool IsWalkable(Vector3Int position)
        {
            TileBase tile = _tilemap.GetTile(position);
            return tile == _whiteTile; 
        }

        private List<Vector3Int> GetNeighbors(Vector3Int position)
        {
            List<Vector3Int> neighbors = new List<Vector3Int>();
            neighbors.Add(new Vector3Int(position.x + 1, position.y, position.z));
            neighbors.Add(new Vector3Int(position.x - 1, position.y, position.z));
            neighbors.Add(new Vector3Int(position.x, position.y + 1, position.z));
            neighbors.Add(new Vector3Int(position.x, position.y - 1, position.z));
            
            return neighbors;
        }
    }
}