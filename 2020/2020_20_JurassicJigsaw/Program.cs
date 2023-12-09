using AdventCodeExtension;
using System.Diagnostics;
using _2020_20_JurassicJigsaw.Helpers;
using AdventCodeExtension.Models;

var input = File.ReadAllText($"Resources\\Input.txt");
var shapeInput = File.ReadAllText($"Resources\\ShapeInput.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input);
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part2(input, shapeInput);
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Drawing
//Draw(input);

object Part1(string input)
{
    var tiles = input.Split($"{Environment.NewLine}{Environment.NewLine}")
                     .Select(TakeTile)
                     .ToList();

    //SelectMany Edges*Tiles -> 8*9 -> 72
    //GroupBy Edge
    return tiles.SelectMany(x => x.EdgeIds.Select(y => (EdgeId: y, TileId: x.Id))) //Take all combinations: Edges*Tiles -> 8*9 -> 72
                .GroupBy(x => x.EdgeId) 
                .Where(x => x.Count() == 1) //Take edges without neighbours -> External edges
                .SelectMany(x => x)
                .GroupBy(x => x.TileId)
                .Where(x => x.Count() == 4) //Take corner tiles -> External Tile have 2 External Edges (each external edge have view front/back)
                .SelectMany(x => x.Select(y => y.TileId))
                .Distinct()
                .Product();
}

object Part2(string input, string shapeInput)
{
    var tiles = input.Split($"{Environment.NewLine}{Environment.NewLine}")
                     .Select(TakeTile)
                     .ToList();
    var monster = TakeShape(shapeInput);

    //Build score
    var map = TakeMap(tiles); //Build boolean map the whole area
    return CountSafeFields(map, monster); //Count fields what have 1 and are not part of monsters
}

void Draw(string input)
{
    var tiles = input.Split($"{Environment.NewLine}{Environment.NewLine}")
                        .Select(TakeTile)
                        .ToList();

    var map = TakeMap(tiles);
    DrawingHelper.DrawPlane(map);
}

bool[,] TakeMap(IList<Tile> tiles)
{
    ArrangeTiles(tiles); //Arrange tiles (Rotate/Flip)
    var cornerTiles = TakeCornerTiles(tiles); //Is returning 4 corner tiles
    var leftTopTile = TakeLeftTopTile(tiles, cornerTiles); //Is returning 1 corner tile (left-top)
    var tilesWithConnection = ConnectTiles(tiles); //is returning the relation of tile and neighbour tiles
    var mapTiles = MapTiles(tilesWithConnection, leftTopTile.Id); //is returning 2D map of tiles

    //Build boolean map from map tiles
    var width = mapTiles.GetLength(0);
    var heigh = mapTiles.GetLength(1);

    var result = new bool[width * 8, heigh * 8];

    for (int rowId = 0; rowId < width; rowId++)
        for (int columnId = 0; columnId < heigh; columnId++)
            result.InsertRange((rowId * 8, columnId * 8), mapTiles[rowId, columnId].TakeContentInside());

    return result;
}

void ArrangeTiles(IList<Tile> tiles)
{
    var tileRandom = tiles.First();
    var queue = new Queue<Tile>(new[] { tileRandom });
    var used = new HashSet<int>();
    while (queue.Any())
    {
        var nextTile = queue.Dequeue();
        if (used.Contains(nextTile.Id))
            continue;
        else
            used.Add(nextTile.Id);

        var nextTilesRelation = tiles.Where(x => x.EdgeIds.Intersect(nextTile.EdgeIds).Count() == 2).ToList();
        foreach (var nextTileRelation in nextTilesRelation)
        {
            var edgeId = nextTile.EdgeIds.Take(4)
                                         .Intersect(nextTileRelation.EdgeIds)
                                         .Single();
            var nteIndex = nextTile.TakeEdgeIndex(edgeId);
            var edgeBackId = nextTile.EdgeIds[nteIndex + 4];

            var ntreIndex = nextTileRelation.TakeEdgeIndex(edgeBackId);
            if (ntreIndex >= 4)
            {
                nextTileRelation.Flip();
                ntreIndex = nextTileRelation.TakeEdgeIndex(edgeBackId);
            }

            while (true)
            {
                if (ntreIndex == ((nteIndex + 2) & 3))
                    break;

                nextTileRelation.Rotate();
                ntreIndex = nextTileRelation.TakeEdgeIndex(edgeBackId);
            }
        }

        foreach (var nextTileRelation in nextTilesRelation)
            queue.Enqueue(nextTileRelation);
    }
}

List<Tile> TakeCornerTiles(IList<Tile> tiles)
{
    IEnumerable<int> TakeCandidateEdges(Tile tile)
    {
        foreach (var nextTile in tiles.Where(x => x != tile))
            foreach (var nextEdge in nextTile.EdgeIds.TakeLast(4))
                yield return nextEdge;
    }

    return tiles.Where(x => x.EdgeIds.Take(4).Intersect(TakeCandidateEdges(x)).Count() == 2)
                .ToList();
}

Tile TakeLeftTopTile(IList<Tile> tiles, IList<Tile> cornerTiles)
{
    var leftEdges = tiles.Select(x => x.EdgeIds[7])
                         .ToHashSet();
    var topEdges = tiles.Select(x => x.EdgeIds[4])
                        .ToHashSet();

    return cornerTiles.Single(x => leftEdges.Contains(x.EdgeIds[1]) && topEdges.Contains(x.EdgeIds[2]));
}

Dictionary<Tile, HashSet<Tile>> ConnectTiles(IList<Tile> tiles)
{
    var result = new Dictionary<Tile, HashSet<Tile>>();
    foreach (var tile in tiles)
    {
        var neighbours = tiles.Where(x => x.EdgeIds.Intersect(tile.EdgeIds).Count() == 2)
                              .ToHashSet();

        result.Add(tile, neighbours);
    }

    return result;
}

Tile[,] MapTiles(Dictionary<Tile, HashSet<Tile>> tilesWithConnection, int tileId)
{
    var size = (int)Math.Sqrt(tilesWithConnection.Count);
    var map = new Tile[size, size];

    //Build First Row
    var tileNext = tilesWithConnection.FirstAndRemove(x => x.Key.Id == tileId); //Take first corner tile and remove from list
    map[0, 0] = tileNext.Key;
    for (int columnId = 1; columnId < size; columnId++)
    {
        tileNext = tilesWithConnection.FirstAndRemove(x => tileNext.Value.Contains(x.Key) && x.Value.Count <= 3); //Take first neighbour edge/corner tile and remove from list
        map[0, columnId] = tileNext.Key;
    }

    //Build Next Rows
    for (int rowId = 0; rowId < size - 1; rowId++)
    {
        var columnId = 0;
        foreach (var item in map.TakeRow(rowId))
        {
            tileNext = tilesWithConnection.FirstAndRemove(x => x.Value.Contains(item));
            map[rowId + 1, columnId++] = tileNext.Key;
        }
    }

    return map;
}

int CountSafeFields(bool[,] map, Shape monster)
{
    var maxRowId = map.GetLength(0) - 1;
    var maxColumnId = map.GetLength(1) - 1;

    var candidates = new HashSet<PointStruct>();
    for (int rowId = 0; rowId <= maxRowId; rowId++)
        for (int columnId = 0; columnId <= maxColumnId; columnId++)
            if (map[rowId, columnId])
                candidates.Add(new PointStruct(columnId, rowId));

    var counter = 0;
    foreach (var candidate in candidates.ToList())
    {
        foreach (var points in monster.TakePointsStructTransformations(candidate.ToPoint()))
        {
            if (points.Min(m => m.X) < 0 || points.Max(m => m.X) > maxColumnId || points.Min(m => m.Y) < 0 || points.Max(m => m.Y) > maxRowId)
                continue;

            if (candidates.Intersect(points).Count() == points.Count)
            {
                foreach (var point in points)
                    candidates.Remove(point);
                //points.ForEach(x => candidates.Remove(x));
                counter++;
                break;
            }
        }

    }

    return candidates.Count;
}

Shape TakeShape(string input)
{
    var points = input.Split(Environment.NewLine)
                      .Select(x => x.Select(y => y is '#'))
                      .SelectMany((x, rowId) => x.Select((y, columnId) => (Point: new Point(rowId, columnId), Load: y))
                                                 .ToArray())
                      .Where(x => x.Load)
                      .Select(x => x.Point)
                      .ToArray();

    return new Shape(points);
}

Tile TakeTile(string input)
{
    var lines = input.Split(Environment.NewLine);

    var id = int.Parse(lines[0].Replace("Tile ", string.Empty).Replace(":", string.Empty));
    var array2D = lines.Skip(1)
                       .Select(x => x.Select(y => y is '#').ToArray())
                       .ToArray()
                       .To2DArray();

    return new Tile(id, array2D);  
}

class Shape
{
    public IList<Point> Points { get; }

    public Shape(IList<Point> points)
    {
        Points = points;
    }

    public IEnumerable<IList<PointStruct>> TakePointsStructTransformations(Point relativePoint)
    {
        //Calculate (move) shape points to relative point
        var firstPoint = Points.First();
        var delta = firstPoint.Delta(relativePoint);

        var result = Points.Select(p => new Point(p.X + delta.DeltaX, p.Y + delta.DeltaY))
                           .ToList();

        //Calculate (rotate/flip) shape points to all possible transformations
        //Result is always 8 transformations
        for (int i = 1; i <= 2; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                yield return result.Select(x => new PointStruct(x)).ToList();
                result.ForEach(x => x.Rotate(relativePoint, PointRotate.Right));
            }
            result.ForEach(x => x.Flip(relativePoint, PointFlip.Horizontal));
        }
    }
}

class Tile
{
    public int Id { get; }
    public bool[,] Content { get; private set; }
    public List<int> EdgeIds { get; private set; } //Always 8 items

    public Tile(int id, bool[,] content)
    {
        Id = id;
        Content = content;

        EdgeIds = TakeEdgeIds();
    }

    public int TakeEdgeIndex(int edgeId) => EdgeIds.IndexOf(edgeId);

    public bool[,] TakeContentInside()
    {
        var lastRowId = Content.GetLength(0) - 1;
        var lastColumnId = Content.GetLength(1) - 1;
        return Content.TakeRange((1, 1), (lastRowId - 1, lastColumnId - 1));
    }

    public void Flip()
    {
        Content = Content.FlipHorizontal();

        EdgeIds = TakeEdgeIds();
    }

    public void Rotate()
    {
        Content = Content.RotateRight();

        EdgeIds = TakeEdgeIds();
    }

    private List<int> TakeEdgeIds()
    {
        static int TakeId(bool[] bools, bool reverse)
        {
            var bitArray = bools.ToBitArray();
            if (reverse)
                bitArray = bitArray.Reverse();

            return bitArray.ToInt();
        }

        var lastRowId = Content.GetLength(0) - 1;
        var lastColumnId = Content.GetLength(1) - 1;

        return new List<int>
        {
            TakeId(Content.TakeRow(0), false),
            TakeId(Content.TakeColumn(lastColumnId), false),
            TakeId(Content.TakeRow(lastRowId), true),
            TakeId(Content.TakeColumn(0), true),
            TakeId(Content.TakeRow(0), true),
            TakeId(Content.TakeColumn(lastColumnId), true),
            TakeId(Content.TakeRow(lastRowId), false),
            TakeId(Content.TakeColumn(0), false),
        };
    }
}