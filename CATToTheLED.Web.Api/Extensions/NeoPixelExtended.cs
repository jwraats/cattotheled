using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ws281x.Net;

namespace CATToTheLED.Web.Api.Extensions
{

    public class NeoPixelExtended : Neopixel
    {
        public Dictionary<int, Color> Info { get; set; }

        public NeoPixelExtended(int ledCount, int pin) : base(ledCount: ledCount, pin: pin)
        {
            Info = new Dictionary<int, Color>();
            for (int i = 0; i < this.GetNumberOfPixels(); i++){
                Info.Add(i, Color.Black);
            }
            //Reset everything to black
            this.Show();
        }

        public Color SetColor(int i, Color color)
        {
            if(i < 0 && i > this.GetNumberOfPixels()){
                throw new Exception("Only the pixels that we have..");
            }
            if (!Info.TryAdd(i, color))
            {
                Info[i] = color;
            }
            return color;
        }

        public Color GetColor(int i)
        {
            return this.Info.GetValueOrDefault(i);
        }

        public new void Show()
        {
            for (int i = 0; i < this.GetNumberOfPixels(); i++)
            {
                if (Info.TryGetValue(i, out Color color))
                {
                    LedList.SetColor(i, Color.FromArgb(color.G, color.B, color.R));
                }
            }
            base.Show();
        }
    }

    public static class NeoPixelStatic
    {
        public static NeoPixelExtended Neopixel { get; }

        static NeoPixelStatic()
        {
            int _ledCount = 60;
            int _pin = 18;
            Neopixel = new NeoPixelExtended(_ledCount, _pin);
        }
    }
}
