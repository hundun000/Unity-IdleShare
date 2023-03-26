using hundun.unitygame.gamelib;
using System.Collections.Generic;
using System.Linq;


namespace hundun.idleshare.gamelib
{
    public class GridPosition
    {
        public int x;
        public int y;

        public GridPosition()
        {
        }

        public GridPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is GridPosition))
            {
                return false;
            }
            return (this.x == ((GridPosition)obj).x)
                && (this.y == ((GridPosition)obj).y);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        internal string toShowText()
        {
            return JavaFeatureForGwt.stringFormat("({0},{1})", x, y);
        }
    }

    public interface ITileNode<T> where T : ITileNode<T>
    {
        GridPosition position { get; set; }
        Dictionary<TileNeighborDirection, T> neighbors { get; set; }

    }

    public interface ITileNodeMap<T> where T : ITileNode<T>
    {
        T getValidNodeOrNull(GridPosition position);


    }

  

    public enum TileNeighborDirection
    {
        LEFT_UP,
        LEFT_MID,
        LEFT_DOWN,
        RIGHT_UP,
        RIGHT_MID,
        RIGHT_DOWN
    }


    public class TileNodeUtils
    {
        public static TileNeighborDirection[] values =
        {
            TileNeighborDirection.LEFT_UP,
            TileNeighborDirection.LEFT_MID,
            TileNeighborDirection.LEFT_DOWN,
            TileNeighborDirection.RIGHT_UP,
            TileNeighborDirection.RIGHT_MID,
            TileNeighborDirection.RIGHT_DOWN
        };

        public static void updateNeighborsAllStep<T>(ITileNode<T> target, ITileNodeMap<T> map) where T : ITileNode<T>
        {
            // update self
            updateNeighborsOneStep(target, map);
            // update new neighbors
            target.neighbors.Values.ToList()
                    .Where(it => it != null)
                    .ToList()
                    .ForEach(it => updateNeighborsOneStep(it, map));
        }

        public static void updateNeighborsOneStep<T>(ITileNode<T> target, ITileNodeMap<T> map) where T : ITileNode<T>
        {
            GridPosition position;
            T neighbor;

            target.neighbors = new Dictionary<TileNeighborDirection, T>();

            values.ToList().ForEach(it => {

                position = tileNeighborPosition(target, map, it);
                neighbor = map.getValidNodeOrNull(position);
                target.neighbors.Add(it, neighbor);

            });

        }


        public static GridPosition tileNeighborPosition<T>(ITileNode<T> target, ITileNodeMap<T> map, TileNeighborDirection direction) where T : ITileNode<T>
        {
            switch (direction) { 
                case TileNeighborDirection.LEFT_UP:
                    return tileLeftUpNeighbor(target, map);
                case TileNeighborDirection.LEFT_MID:
                    return tileLeftMidNeighbor(target, map);
                case TileNeighborDirection.LEFT_DOWN:
                    return tileLeftDownNeighbor(target, map);
                case TileNeighborDirection.RIGHT_UP:
                    return tileRightUpNeighbor(target, map);
                case TileNeighborDirection.RIGHT_MID:
                    return tileRightMidNeighbor(target, map);
                case TileNeighborDirection.RIGHT_DOWN:
                    return tileRightDownNeighbor(target, map);
            }
            return null;
        }



        public static GridPosition tileRightMidNeighbor<T>(ITileNode<T> target, ITileNodeMap<T> map) where T : ITileNode<T>
        {
            int y;
            int x;
            y = target.position.y;
            x = target.position.x + 1;
            return new GridPosition(x, y);
        }


        public static GridPosition tileRightUpNeighbor<T>(ITileNode<T> target, ITileNodeMap<T> map) where T : ITileNode<T>
        {
            int y;
            int x;
            if (target.position.y % 2 == 0)
            {
                y = target.position.y + 1;
                x = target.position.x + 1;
            }
            else
            {
                y = target.position.y + 1;
                x = target.position.x;
            }
            return new GridPosition(x, y);
        }


        public static GridPosition tileRightDownNeighbor<T>(ITileNode<T> target, ITileNodeMap<T> map) where T : ITileNode<T>
        {
            int y;
            int x;
            if (target.position.y % 2 == 0)
            {
                y = target.position.y - 1;
                x = target.position.x + 1;
            }
            else
            {
                y = target.position.y - 1;
                x = target.position.x;
            }
            return new GridPosition(x, y);
        }

        public static GridPosition tileLeftUpNeighbor<T>(ITileNode<T> target, ITileNodeMap<T> map) where T : ITileNode<T>
        {
            int y;
            int x;
            if (target.position.y % 2 == 0)
            {
                y = target.position.y + 1;
                x = target.position.x;
            }
            else
            {
                y = target.position.y + 1;
                x = target.position.x - 1;
            }
            return new GridPosition(x, y);
        }

        public static GridPosition tileLeftMidNeighbor<T>(ITileNode<T> target, ITileNodeMap<T> map) where T : ITileNode<T>
        {
            int y;
            int x;
            y = target.position.y;
            x = target.position.x - 1;
            return new GridPosition(x, y);
        }

        public static GridPosition tileLeftDownNeighbor<T>(ITileNode<T> target, ITileNodeMap<T> map) where T : ITileNode<T>
        {
            int y;
            int x;
            if (target.position.y % 2 == 0)
            {
                y = target.position.y - 1;
                x = target.position.x;
            }
            else
            {
                y = target.position.y - 1;
                x = target.position.x - 1;
            }
            return new GridPosition(x, y);
        }




    }

}


