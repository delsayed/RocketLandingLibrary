using System.Drawing;

namespace RocketLanding
{
   public static class SafetyExtensions
    {
        // more than one rocket can land on the same platform at the same time and rockets
        // need to have at least one unit separation between their landing positions
        static Point offset = new Point();

        /// <summary>
        /// Creates a safety perimeter Rectangle from a Point
        /// </summary>
        /// <param name="point">Point to use as center of the safetyPerimeter</param>
        /// <param name="unit">Points separation for safetyPerimeter</param>
        /// <returns></returns>
        public static Rectangle SetObjectPosition(Point point,int unit=0)
        {          
            if (point.IsEmpty)
                return Rectangle.Empty;

            offset = new Point(unit,unit);
            int xOffset = point.X - offset.X;
            int yOffset = point.Y - offset.Y;

            var diameter = (unit * 2) + 1;
            return  new Rectangle(xOffset,yOffset,diameter,diameter);
           
        }

    }
}
