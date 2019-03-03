using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ws281x.Net;

namespace CATToTheLED.Web.Api.Extensions
{
    partial class TheTheater
    {
        private List<Color> rainbowColors = new List<Color>(){
                Color.FromArgb(0x201000),
                Color.FromArgb(0x202000),
                Color.Green,
                Color.FromArgb(0x002020),
                Color.Blue,
                Color.FromArgb(0x100010),
                Color.FromArgb(0x200010)
            };

        public async Task Snake(){
            //W.I.P! Age mar leut et..
            int colorOffset = 0;
            NeoPixelStatic.Neopixel.Info.Clear();
            while (NeoPixelStatic.Neopixel.GiveShow == Shows.Snake)
            {
                for (int skip = 2; skip < 4; skip++){
                    for (int i = 0; i < this.numberOfPixels; i+=skip)
                    {
                        var colorIndex = (i + colorOffset) % rainbowColors.Count;
                        this.SetColorShow(i, rainbowColors[colorIndex]);
                    }
                    NeoPixelStatic.Neopixel.Show();
                    await Task.Delay(50);

                    NeoPixelStatic.Neopixel.Info.Clear();
                    colorOffset++;
                    if (colorOffset < int.MaxValue)
                    {
                        colorOffset = 0;
                    }
                }
            }
        }

        public async Task Swipe(Color color){
            for (int i = 0; i < this.numberOfPixels; i++)
            {
                this.SetColorShow(i, color);
                NeoPixelStatic.Neopixel.Show();
                await Task.Delay(50);
            }
        }

        public async Task ColorSwipe()
        {
            await this.Swipe(Color.Red);
            await this.Swipe(Color.Green);
            await this.Swipe(Color.Blue);
        }

        public async Task Rainbow()
        {
           
            int colorOffset = 0;

            while (NeoPixelStatic.Neopixel.GiveShow == Shows.Rainbow)
            {
                for (int i = 0; i < this.numberOfPixels; i++)
                {
                    var colorIndex = (i + colorOffset) % rainbowColors.Count;
                    this.SetColorShow(i, rainbowColors[colorIndex]);
                }
                colorOffset++;

                NeoPixelStatic.Neopixel.Show();
                if (colorOffset < int.MaxValue)
                {
                    colorOffset = 0;
                }

                await Task.Delay(50);
            }
        }

        public async Task ShowColors(){
            foreach (KnownColor knowColor in Enum.GetValues(typeof(KnownColor)))
            {
                Color color = Color.FromKnownColor(knowColor);
                for (int i = 0; i < this.numberOfPixels; i++)
                {
                    this.SetColorShow(i, color);
                }
                NeoPixelStatic.Neopixel.Show();
                NeoPixelStatic.Logger.LogInformation($"Color shown is: {knowColor}.");
                await Task.Delay(5000);
            }
        }

    }
}