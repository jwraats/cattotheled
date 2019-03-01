using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ws281x.Net;

namespace CATToTheLED.Web.Api.Extensions
{
    partial class TheTheater
    {
        public async Task Alarm()
        {
            int brightnessNumber = NeoPixelStatic.Neopixel.GetBrightness();
            brightnessNumber = (int)Math.Floor((double)(brightnessNumber / 5)) * 5;

            //Color up
            for (int brightness = brightnessNumber; brightness <= 255; brightness += 5)
            {
                this.SetBrightnessShow(brightness);
                NeoPixelStatic.Neopixel.Show();
                await Task.Delay(50);
            }

            //Color down
            for (int brightness = 255; brightness >= 0; brightness -= 5)
            {
                this.SetBrightnessShow(brightness);
                NeoPixelStatic.Neopixel.Show();
                await Task.Delay(50);
            }
        }

        public async Task ColorSwipe()
        {
            this.SetBrightnessShow(255);
            for (int i = 0; i < this.numberOfPixels; i++)
            {
                this.SetColorShow(i, Color.Red);
                NeoPixelStatic.Neopixel.Show();
            }
            await Task.Delay(1000);
            for (int i = 0; i < this.numberOfPixels; i++)
            {
                this.SetColorShow(i, Color.Blue);
                NeoPixelStatic.Neopixel.Show();
            }
            await Task.Delay(1000);
        }

        public async Task Rainbow()
        {
            this.SetBrightnessShow(255);
            for (int i = 0; i < this.numberOfPixels; i++)
            {
                this.SetColorShow(i, Color.Purple);
            }
            NeoPixelStatic.Neopixel.Show();
            await Task.Delay(1000);
            for (int i = 0; i < this.numberOfPixels; i++)
            {
                this.SetColorShow(i, Color.Green);
            }
            NeoPixelStatic.Neopixel.Show();
            await Task.Delay(1000);
        }
    }
}