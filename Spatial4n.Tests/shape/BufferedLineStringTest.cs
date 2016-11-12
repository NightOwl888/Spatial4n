﻿using NetTopologySuite.Geometries;
using Spatial4n.Core.Context;
using Spatial4n.Core.Shapes;
using Spatial4n.Core.Shapes.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Spatial4n.Tests.shape
{
    public class BufferedLineStringTest
    {


        private readonly SpatialContext ctx = new SpatialContextFactory()
        { geo = false, worldBounds = new RectangleImpl(-100, 100, -50, 50, null) }.NewSpatialContext();

        private class RectIntersectionAnonymousHelper : RectIntersectionTestHelper /*<BufferedLineString>*/
        {
            protected readonly Random random = new Random(RandomSeed.Seed());

            public RectIntersectionAnonymousHelper(SpatialContext ctx)
                : base(ctx)
            {
            }

            protected override /*BufferedLineString*/ Shape GenerateRandomShape(Core.Shapes.Point nearP)
            {
                Rectangle nearR = RandomRectangle(nearP);
                int numPoints = 2 + random.Next(3 + 1);//2-5 points

                List<Core.Shapes.Point> points = new List<Core.Shapes.Point>(numPoints);
                while (points.Count < numPoints)
                {
                    points.Add(RandomPointIn(nearR));
                }
                double maxBuf = Math.Max(nearR.GetWidth(), nearR.GetHeight());
                double buf = Math.Abs(RandomGaussian()) * maxBuf / 4;
                buf = random.Next((int)Divisible(buf));
                return new BufferedLineString(points, buf, ctx);
            }

            protected override Core.Shapes.Point RandomPointInEmptyShape(/*BufferedLineString*/ Shape shape)
            {
                IList<Core.Shapes.Point> points = ((BufferedLineString)shape).GetPoints();
                return points.Count == 0 ? null : points[random.Next(points.Count/* - 1*/)];
            }
        }


        [Fact]
        public virtual void TestRectIntersect()
        {
            new RectIntersectionAnonymousHelper(ctx).TestRelateWithRectangle();

            //        new RectIntersectionTestHelper<BufferedLineString>(ctx) {


            //  protected override BufferedLineString GenerateRandomShape(Point nearP)
            //    {
            //        Rectangle nearR = randomRectangle(nearP);
            //        int numPoints = 2 + randomInt(3);//2-5 points

            //        ArrayList<Point> points = new ArrayList<Point>(numPoints);
            //        while (points.size() < numPoints)
            //        {
            //            points.add(randomPointIn(nearR));
            //        }
            //        double maxBuf = Math.max(nearR.getWidth(), nearR.getHeight());
            //        double buf = Math.abs(randomGaussian()) * maxBuf / 4;
            //        buf = randomInt((int)divisible(buf));
            //        return new BufferedLineString(points, buf, ctx);
            //    }

            //    protected Point randomPointInEmptyShape(BufferedLineString shape)
            //    {
            //        List<Point> points = shape.getPoints();
            //        return points.get(randomInt(points.size() - 1));
            //    }
            //}.testRelateWithRectangle();
        }
    }
}