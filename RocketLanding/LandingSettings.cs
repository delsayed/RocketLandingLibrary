using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RocketLanding
{
  public   class LandingSettings
    {
        public const int LandingAreaWidth = 100;
        public const int LandingAreaHeight = 100;

        // Set top left corner of landing area at 0,0
        public const int LandingAreaX = 0;
        public const int LandingAreaY = 0;
        
        public Rectangle LandingArea { get; private set; }= 
                                     new Rectangle(LandingAreaX, LandingAreaY, LandingAreaWidth, LandingAreaHeight);
        
       
    }
}
