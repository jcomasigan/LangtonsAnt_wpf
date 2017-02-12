using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangtonsAnt_wpf
{
    public enum Facing { North, South, East, West };
    
    public enum TurnDirection { Clockwise, AntiClockwise};

    public class ColorValues
    {
        public static Color OriginalColor { get; set; }
        public static Color ReplacementColor { get; set; }
    }
    public class AntVector
    {
        public enum MoveTo { X, Y };
        public enum Direction { Forward, Backward }
        public MoveTo moveto { get; set; }
        public Direction direction {get; set;}

        AntVector(MoveTo _moveto, Direction _dir)
        {
            moveto = _moveto;
            direction = _dir;
        }
    }
    public class AntFacing
    {

        public Facing direction { get; set; }
        AntFacing(Facing _facing)
        {
            direction = _facing;
        }
    }

    public class Ant
    {
        public AntCoordinates coord { get; set; }
        public Facing facing { get; set; }
        public Ant(int x, int y)
        {
            coord = new AntCoordinates(x, y);
        }
    }
    public class AntCoordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
        public AntCoordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public struct ColourAndRotation
    {
        public Color Colour;
        public TurnDirection Direction;
    }
    public static class LangtonAnt
    {
        public static int _canvasSize;
        public static Bitmap Ant(Bitmap canvas, ref Ant ant, int canvasSize, Dictionary<Color, ColourAndRotation> colDic, int sleepTime = 500)
        {
            _canvasSize = canvasSize;
            AntCoordinates newCoordinates = ant.coord;
            Color antPixel = canvas.GetPixel(ant.coord.X, ant.coord.Y);
            foreach(KeyValuePair<Color, ColourAndRotation> kv in colDic)
            {
                Color original = kv.Key;
                Color replacement = kv.Value.Colour;
                TurnDirection dir = kv.Value.Direction;
                if(antPixel == kv.Key)
                {
                    canvas = ChangePixel(canvas, ant.coord, replacement);
                    ant = TurnAndMove(ant, dir);
                }
            }
            /*
            if (antPixel.R == 0 && antPixel.G == 0 && antPixel.B == 0)
            {

                canvas = ChangePixel(canvas, ant.coord, Color.FromArgb(50, 50, 50));
                ant = TurnAndMove(ant, TurnDirection.Clockwise);
            }
            else if (antPixel.R == 50 && antPixel.G == 50 && antPixel.B == 50)
            {
                canvas = ChangePixel(canvas, ant.coord, Color.FromArgb(0, 255, 0));
                ant = TurnAndMove(ant, TurnDirection.AntiClockwise);
            }
            else if (antPixel.R == 0 && antPixel.G == 255 && antPixel.B == 0)
            {

                canvas = ChangePixel(canvas, ant.coord, Color.FromArgb(0, 0, 255));
                ant = TurnAndMove(ant, TurnDirection.Clockwise);
            }
            else if (antPixel.R == 0 && antPixel.G == 0 && antPixel.B == 255)
            {

                canvas = ChangePixel(canvas, ant.coord, Color.FromArgb(100, 255, 20));
                ant = TurnAndMove(ant, TurnDirection.Clockwise);
            }
            else if (antPixel.R == 100 && antPixel.G == 255 && antPixel.B == 20)
            {
                canvas = ChangePixel(canvas, ant.coord, Color.FromArgb(255, 255, 255));
                ant = TurnAndMove(ant, TurnDirection.Clockwise);
            }
            else
            {

                canvas = ChangePixel(canvas, ant.coord, Color.FromArgb(0, 0, 0));
                ant = TurnAndMove(ant, TurnDirection.AntiClockwise);
            }
            */
            return canvas;
        }


        private static Ant TurnAndMove(Ant ant, TurnDirection dir)
        {
            AntCoordinates newCoordinates = new AntCoordinates(ant.coord.X, ant.coord.Y);
            if (dir == TurnDirection.Clockwise)
            {
                switch(ant.facing)
                {
                    case Facing.North:
                        {
                            newCoordinates.X = ant.coord.X + 1;
                            newCoordinates = CheckBounds(newCoordinates, _canvasSize);
                            ant.facing = Facing.East;
                            break;
                        }
                    case Facing.East:
                        {
                            newCoordinates.Y = ant.coord.Y + 1;
                            newCoordinates = CheckBounds(newCoordinates, _canvasSize);
                            ant.facing = Facing.South;
                            break;
                        }
                    case Facing.South:
                        {
                            newCoordinates.X = ant.coord.X - 1;
                            newCoordinates = CheckBounds(newCoordinates, _canvasSize);
                            ant.facing = Facing.West;
                            break;
                        }
                    case Facing.West:
                        {
                            newCoordinates.Y = ant.coord.Y - 1;
                            newCoordinates = CheckBounds(newCoordinates, _canvasSize);
                            ant.facing = Facing.North;
                            break;
                        }
                }
            }
            else
            {
                switch (ant.facing)
                {
                    case Facing.North:
                        {
                            newCoordinates.X = ant.coord.X - 1;
                            newCoordinates = CheckBounds(newCoordinates, _canvasSize);
                            ant.facing = Facing.West;

                            break;
                        }
                    case Facing.East:
                        {
                            newCoordinates.Y = ant.coord.Y - 1;
                            newCoordinates = CheckBounds(newCoordinates, _canvasSize);
                            ant.facing = Facing.North;
                            break;
                        }
                    case Facing.South:
                        {
                            newCoordinates.X = ant.coord.X + 1;
                            newCoordinates = CheckBounds(newCoordinates, _canvasSize);
                            ant.facing = Facing.East;
                            break;
                        }
                    case Facing.West:
                        {
                            newCoordinates.Y = ant.coord.Y + 1;
                            newCoordinates = CheckBounds(newCoordinates, _canvasSize);
                            ant.facing = Facing.South;
                            break;
                        }
                }
            }
            ant.coord = newCoordinates;
            return ant;
        }
        private static AntCoordinates CheckBounds(AntCoordinates coord, int canvasSize)
        {
            int upperBounds = canvasSize - 1;
            if (coord.X < 0)
            {
                coord.X = upperBounds;
            }
            if (coord.Y < 0)
            {
                coord.Y = upperBounds;
            }
            if (coord.X > upperBounds)
            {
                coord.X = 0;
            }
            if (coord.Y > upperBounds)
            {
                coord.Y = 0;
            }
            return coord;
        }
        private static Bitmap ChangePixel(Bitmap canvas, AntCoordinates coord, Color col)
        {
            Color color = canvas.GetPixel(coord.X, coord.Y);
            bool isSyscol = color.IsSystemColor;
            canvas.SetPixel(coord.X, coord.Y, col);
            return canvas;
        }


    }
}
