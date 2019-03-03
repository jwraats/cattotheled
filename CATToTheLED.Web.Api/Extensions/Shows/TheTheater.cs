using System;
using System.Drawing;

namespace CATToTheLED.Web.Api.Extensions
{
    public enum Shows
    {
        None = 0,
        Rainbow = 1,
        ColorSwipe = 2,
        Alarm = 3,
        ShowColors = 4,
        Snake = 5
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