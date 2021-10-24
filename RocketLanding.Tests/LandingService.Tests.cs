using System;
using System.Collections.Generic;
using System.Drawing;
using Xunit;
namespace RocketLanding.Tests
{
           public class LandingServiceTests
        {
            private readonly RocketLandingService sut;

            public LandingServiceTests()
            {
                sut = new RocketLandingService();
            }

            public static IEnumerable<object[]> LandingPlatformsOK()
            {
                yield return new object[] { new Rectangle(LandingSettings.LandingAreaX, LandingSettings.LandingAreaY, 1, 1) };
                yield return new object[] { new Rectangle(LandingSettings.LandingAreaX, LandingSettings.LandingAreaX, 100,100) };
                yield return new object[] { new Rectangle(LandingSettings.LandingAreaX, LandingSettings.LandingAreaY, LandingSettings.LandingAreaWidth, LandingSettings.LandingAreaHeight) };
                yield return new object[] { new Rectangle(3, 12, 45, 62) };
            }

            [Theory]
            [MemberData(nameof(LandingPlatformsOK))]
            public void LandingService_canConfigureLandingPlatform_OK(Rectangle landingPlatform)
            {
                sut.Initialize(landingPlatform);

                Assert.Equal(landingPlatform, sut.LandingPlatform);
            }

            public static IEnumerable<object[]> LandingPlatformsOutOfRange()
            {
                yield return new object[] { new Rectangle(-3, 1, 20, 12) };
                yield return new object[] { new Rectangle(100, 100, 20, 12) };
                yield return new object[] { new Rectangle(100, 100, 20, 12) };
                yield return new object[] { new Rectangle(100, 100, 2, 2) };
                yield return new object[] { new Rectangle(5, 5, 72, 0) };
                yield return new object[] { new Rectangle(5, 5, 0, 0) };
            }

            [Theory]
            [MemberData(nameof(LandingPlatformsOutOfRange))]
            public void LandingService_LandingPlatform_outsideLandingArea_throws_OutOfRangeException(Rectangle landingPlatform)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.Initialize(landingPlatform));
            }

            public static IEnumerable<object[]> OkForLanding()
            {
                yield return new object[] { new Rectangle(5, 5, 10, 10), new Point(5, 5) };
                yield return new object[] { new Rectangle(5, 5, 10, 10), new Point(5, 14) };
                yield return new object[] { new Rectangle(5, 5, 10, 10), new Point(14, 14) };
                yield return new object[] { new Rectangle(5, 5, 10, 10), new Point(14, 5) };
            }

            [Theory]
            [MemberData(nameof(OkForLanding))]
            public void LandingService_checkTrajectory_OkForLanding(Rectangle landingPlatform, Point point)
            {
                sut.Initialize(landingPlatform);
                var message = sut.ValidateTtrajectory(point);

                Assert.Equal(Status.OkForLanding, message);
            }

            public static IEnumerable<object[]> OutOfPlatform()
            {
                yield return new object[] { new Rectangle(5, 5, 10, 10), new Point(16, 15) };

                yield return new object[] { new Rectangle(10, 10, 7, 3), new Point(LandingSettings.LandingAreaX - 1, LandingSettings.LandingAreaY - 1) };
                yield return new object[] { new Rectangle(10, 10, 7, 3), new Point(LandingSettings.LandingAreaWidth + 1, LandingSettings.LandingAreaHeight + 1) };
            }

            [Theory]
            [MemberData(nameof(OutOfPlatform))]
            public void LandingService_checkTrajectory_OutOfPlatform(Rectangle landingPlatform, Point point)
            {
                sut.Initialize(landingPlatform);
                var message = sut.ValidateTtrajectory(point);

                Assert.Equal(Status.OutOfPlatform, message);
            }

            public static IEnumerable<object[]> Clash()
            {
                yield return new object[] {
                new Rectangle(5, 5, 10, 10),
                new Point(7, 7),
                new Point[] {
                    new Point(7,7),
                    new Point(7,8),
                    new Point(6,7),
                    new Point(6,6),
                }
            };
            }

            [Theory]
            [MemberData(nameof(Clash))]
            public void LandingService_checkTrajectory_Clash(Rectangle landingPlatform, Point point, Point[] clashingPoints)
            {
                sut.Initialize(landingPlatform);

                Assert.Equal(Status.OkForLanding, sut.ValidateTtrajectory(point));

                Assert.All(clashingPoints, clashingPoint =>
                    Assert.Equal(Status.Clash, sut.ValidateTtrajectory(clashingPoint))
                );
            }

            [Fact]
            public void LandingService_remember_only_last_checked_Point()
            {
                var landingPlatform = new Rectangle(5, 5, 10, 10);
                var point1 = new Point(7, 7);
                var point2 = new Point(7, 8);
               
                sut.Initialize(landingPlatform);

                sut.ValidateTtrajectory(point1);
                sut.ValidateTtrajectory(point2);
               
            Assert.Equal(point2, sut.PreviousCheckedPoint);
            }
        }
    }


