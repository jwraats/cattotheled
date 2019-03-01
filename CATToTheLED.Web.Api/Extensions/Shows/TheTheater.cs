using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ws281x.Net;

namespace CATToTheLED.Web.Api.Extensions
{
    public enum Shows
    {
        None = 0,
        Rainbow = 1,
        ColorSwipe = 2,
        Alarm = 3
    }

    public sealed partial class TheTheater
    {
        private int numberOfPixels;

        public TheTheater(int numberOfPixels)
        {
            this.numberOfPixels = numberOfPixels;
        }

        private void SetColorShow(int i, Color color)
        {
            if (NeoPixelStatic.Neopixel.GiveShow == Shows.None)
            {
                throw new OperationCanceledException("Show is canceled :(");
            }
            NeoPixelStatic.Neopixel.SetColor(i, color);
        }

        private void SetBrightnessShow(int brightness)
        {
            if (NeoPixelStatic.Neopixel.GiveShow == Shows.None)
            {
                throw new OperationCanceledException("Show is canceled :(");
            }
            if (brightness > 0 && brightness <= 255)
                NeoPixelStatic.Neopixel.SetBrightness((byte)brightness);
        }
    }
}