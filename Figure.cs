using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figures
{
    public enum FigureType
    {
        Round,
        Closed,
        Opened
    }

    class Figure
    {
        public List<PointF> points;
        public FigureType type;

        public Figure(List<PointF> p, FigureType type)
        {
            points =p;
            this.type = type;
        }
        public Figure()
        {
            points = new List<PointF>();
        }
    }
}
