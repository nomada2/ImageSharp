﻿// <copyright file="ColorConversionTests.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests.Drawing
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Xunit;
    using Drawing;
    using ImageSharp.Drawing;
    using System.Numerics;

    using ImageSharp.PixelFormats;
    using SixLabors.Primitives;

    public class PolygonTests : FileTestBase
    {
        [Fact]
        public void ImageShouldBeOverlayedByPolygonOutline()
        {
            string path = this.CreateOutputDirectory("Drawing", "Polygons");

            using (Image<Rgba32> image = new Image<Rgba32>(500, 500))
            {
                    image
                        .BackgroundColor(Rgba32.Blue)
                        .DrawPolygon(Rgba32.HotPink, 5,
                        new SixLabors.Primitives.PointF[] {
                            new Vector2(10, 10),
                            new Vector2(200, 150),
                            new Vector2(50, 300)
                        })
                        .Save($"{path}/Simple.png");

                using (PixelAccessor<Rgba32> sourcePixels = image.Lock())
                {
                    Assert.Equal(Rgba32.HotPink, sourcePixels[9, 9]);

                    Assert.Equal(Rgba32.HotPink, sourcePixels[199, 149]);

                    Assert.Equal(Rgba32.Blue, sourcePixels[50, 50]);

                    Assert.Equal(Rgba32.Blue, sourcePixels[2, 2]);
                }
            }
        }

        [Fact]
        public void ImageShouldBeOverlayedPolygonOutlineWithOpacity()
        {
            string path = this.CreateOutputDirectory("Drawing", "Polygons");
            SixLabors.Primitives.PointF[] simplePath = new SixLabors.Primitives.PointF[] {
                            new Vector2(10, 10),
                            new Vector2(200, 150),
                            new Vector2(50, 300)
            };

            Rgba32 color = new Rgba32(Rgba32.HotPink.R, Rgba32.HotPink.G, Rgba32.HotPink.B, 150);

            using (Image<Rgba32> image = new Image<Rgba32>(500, 500))
            {
                    image
                        .BackgroundColor(Rgba32.Blue)
                        .DrawPolygon(color, 10, simplePath)
                        .Save($"{path}/Opacity.png");

                //shift background color towards forground color by the opacity amount
                Rgba32 mergedColor = new Rgba32(Vector4.Lerp(Rgba32.Blue.ToVector4(), Rgba32.HotPink.ToVector4(), 150f / 255f));

                using (PixelAccessor<Rgba32> sourcePixels = image.Lock())
                {
                    Assert.Equal(mergedColor, sourcePixels[9, 9]);

                    Assert.Equal(mergedColor, sourcePixels[199, 149]);

                    Assert.Equal(Rgba32.Blue, sourcePixels[50, 50]);

                    Assert.Equal(Rgba32.Blue, sourcePixels[2, 2]);
                }
            }
        }

        [Fact]
        public void ImageShouldBeOverlayedByRectangleOutline()
        {
            string path = this.CreateOutputDirectory("Drawing", "Polygons");

            using (Image<Rgba32> image = new Image<Rgba32>(500, 500))
            {
                    image
                        .BackgroundColor(Rgba32.Blue)
                        .Draw(Rgba32.HotPink, 10, new Rectangle(10, 10, 190, 140))
                        .Save($"{path}/Rectangle.png");

                using (PixelAccessor<Rgba32> sourcePixels = image.Lock())
                {
                    Assert.Equal(Rgba32.HotPink, sourcePixels[8, 8]);

                    Assert.Equal(Rgba32.HotPink, sourcePixels[198, 10]);

                    Assert.Equal(Rgba32.HotPink, sourcePixels[10, 50]);

                    Assert.Equal(Rgba32.Blue, sourcePixels[50, 50]);

                    Assert.Equal(Rgba32.Blue, sourcePixels[2, 2]);
                }
            }
        }
    }
}