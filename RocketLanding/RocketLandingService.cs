using System;
using System.Drawing;

namespace RocketLanding
{
  public class RocketLandingService
    {

        internal static LandingSettings Settings { get; set; }
        public Rectangle LandingPlatform { get; set; }
        public Point PreviousCheckedPoint { get; set; }
        public const int SafetyUnit = 1;
        /// <summary>
        /// Initialize RocketLandingService by setting the LandingPlatform
        /// platform size can vary and should be configurable
        /// </summary>
        /// <param name="landingPlatform">Rectangle defining the coordinates and size of the LandingPlatform</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the landingPlatform is invalid</exception>
        public void Initialize(Rectangle landingPlatform)
        {
            Settings = new LandingSettings();

            if (!Settings.LandingArea.Contains(landingPlatform))
            {
                throw new ArgumentOutOfRangeException(nameof(landingPlatform), "Landing Platform is not inside the given Landing Area");
            }

            if (landingPlatform.Height == 0 || landingPlatform.Width == 0 )
            {
                throw new ArgumentOutOfRangeException(nameof(landingPlatform), "Landing Platform area is invalid");
            }

            PreviousCheckedPoint = Point.Empty;
            LandingPlatform = landingPlatform;
        }


        /// <summary>
        /// Check if a given point it's on a correct trajectory to safely land on a platform.
        /// <param name="point">Check if a specific point x,y is inside a rectangle</param>
        /// <returns></returns>
        public string  ValidateTtrajectory(Point point)
        {
          
            if (!LandingPlatform.Contains(point))
                  return Status.OutOfPlatform;
           
            if (IsClashed(point))
            {
                PreviousCheckedPoint = point;
                return Status.Clash;
            }

            if (Settings.LandingArea.Contains(point))
            {
                PreviousCheckedPoint = point;
                return Status.OkForLanding;
            }

            return Status.OutOfPlatform;

        }
        ///<Summary>
        ///Check If the rocket asks for a position that is located next to a position that has previously been checked 
        /// <param name="point">Check if a specific point x,y is inside a rectangle</param>
        /// <returns></returns>
        bool IsClashed(Point point)
        {
          return (SafetyExtensions.SetObjectPosition(PreviousCheckedPoint, SafetyUnit).Contains(point))  ?  true: false;
             
        }


    }
}
