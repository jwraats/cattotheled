using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ws281x.Net;

namespace CATToTheLED.Web.Api.Extensions
{
    public enum Shows {
        None = 0,
        Rainbow = 1,
        ColorSwipe = 2
    }

    public class NeoPixelExtended : Neopixel
    {
        public Dictionary<int, Color> Info { get; set; }

        private Shows _giveShow = Shows.None;
        public Shows GiveShow
        {
            get{
                return this._giveShow;
            }
            set{
                this._giveShow = value;
                Task.Run(() => this.DoWorkAsync());
            } 
        }


        public NeoPixelExtended(int ledCount, int pin) : base(ledCount: ledCount, pin: pin)
        {
            //Otherwise the whole world crash
            this.Begin();

            //Set list to Black
            Info = new Dictionary<int, Color>();
            for (int i = 0; i < this.GetNumberOfPixels(); i++){
                Info.Add(i, Color.Black);
            }

            //Show it
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

        private bool SetColorShow(int i, Color color){
            if(this.GiveShow != Shows.None){
                this.SetColor(i, color);
                return true;
            }
            return false;
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

        private async Task DoWorkAsync(){
            while(GiveShow != Shows.None)
            {
                switch (GiveShow){
                    case Shows.ColorSwipe:
                        await this.ColorSwipe();
                        break;
                    case Shows.Rainbow:
                        await this.Rainbow();
                        break;
                    default:
                        return;
                }
            }
        }

        private async Task ColorSwipe(){
            for (int i = 0; i < this.GetNumberOfPixels(); i++)
            {
                if (!this.SetColorShow(i, Color.Red))
                    return;
                this.Show();
            }
            await Task.Delay(1000);
            for (int i = 0; i < this.GetNumberOfPixels(); i++)
            {
                if (!this.SetColorShow(i, Color.Blue))
                    return;
                this.Show();
            }
            await Task.Delay(1000);
        }

        private async Task Rainbow()
        {
            for (int i = 0; i < this.GetNumberOfPixels(); i++)
            {
                if (!this.SetColorShow(i, Color.Purple))
                    return;
            }
            this.Show();
            await Task.Delay(1000);
            for (int i = 0; i < this.GetNumberOfPixels(); i++)
            {
                if (!this.SetColorShow(i, Color.Green))
                    return;
            }
            this.Show();
            await Task.Delay(1000);
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
