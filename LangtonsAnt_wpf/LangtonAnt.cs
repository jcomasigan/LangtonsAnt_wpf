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


    public static class LangtonAnt
    {
        public static int _canvasSize;
        public static Bitmap Ant(Bitmap canvas, ref Ant ant, int canvasSize, int sleepTime = 500)
        {
            _canvasSize = canvasSize;
            AntCoordinates newCoordinates = ant.coord;
            Color antPixel = canvas.GetPixel(ant.coord.X, ant.coord.Y);
            if(antPixel.R == 0)
            {
                switch(ant.facing)
                {
                    //Ant on black pixel
                    case Facing.North:
                        {
                            canvas = ChangePixel(canvas, newCoordinates);
                            newCoordinates.X = ant.coord.X - 1;
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            ant.facing = Facing.West;

                            break;
                        }
                    case Facing.East:
                        {
                            canvas = ChangePixel(canvas, newCoordinates);
                            newCoordinates.Y = ant.coord.Y - 1;
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            ant.facing = Facing.North;
                            break;
                        }
                    case Facing.South:
                        {
                            canvas = ChangePixel(canvas, newCoordinates);
                            newCoordinates.X = ant.coord.X + 1;
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            ant.facing = Facing.East;
                            break;
                        }
                    case Facing.West:
                        {
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            canvas = ChangePixel(canvas, newCoordinates);
                            newCoordinates.Y = ant.coord.Y + 1;
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            ant.facing = Facing.South;
                            break;
                        }
                }
            }
            else
            {
                //Ant on white pixel
                switch (ant.facing)
                {
                    case Facing.North:
                        {
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            canvas = ChangePixel(canvas, newCoordinates);
                            newCoordinates.X = ant.coord.X + 1;
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            ant.facing = Facing.East;
                            break;
                        }
                    case Facing.East:
                        {
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            canvas = ChangePixel(canvas, newCoordinates);
                            newCoordinates.Y = ant.coord.Y + 1;
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            ant.facing = Facing.South;
                            break;
                        }
                    case Facing.South:
                        {
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            canvas = ChangePixel(canvas, newCoordinates);
                            newCoordinates.X = ant.coord.X - 1;
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            ant.facing = Facing.West;
                            break;
                        }
                    case Facing.West:
                        {
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            canvas = ChangePixel(canvas, newCoordinates);
                            newCoordinates.Y = ant.coord.Y - 1;
                            newCoordinates = CheckBounds(newCoordinates, canvasSize);
                            ant.facing = Facing.North;
                            break;
                        }
                }
            }
            ant.coord = newCoordinates;
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

            }
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
        private static Bitmap ChangePixel(Bitmap canvas, AntCoordinates coord)
        {
            Color color = canvas.GetPixel(coord.X, coord.Y);
            bool isSyscol = color.IsSystemColor;
            if(color.R == 0)
            {
                canvas.SetPixel(coord.X, coord.Y, Color.White);
            }
            else
            {
                canvas.SetPixel(coord.X, coord.Y, Color.Black);
            }


            return canvas;
        }


    }
}
