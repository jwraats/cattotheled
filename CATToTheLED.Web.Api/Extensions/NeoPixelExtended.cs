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
        ColorSwipe = 2,
        Alarm = 3
    }

    public class NeoPixelExtended : Neopixel
    {
        public Dictionary<int, Color> Info { get; set; }
        private Dictionary<int, Color> BeforeShow { get; set; }
        private int BrightnessBeforeShow { get; set; }

        private int _giveShowForMS = 0;
        public int GiveShowForMS {
            get{
                return this._giveShowForMS;
            }
            set{
                this._giveShowForMS = value;
                Task.Run(() => this.TimeShowAsync());
            }
        }
        private Shows _giveShow = Shows.None;
        public Shows GiveShow
        {
            get{
                return this._giveShow;
            }
            set{
                if(this._giveShow == Shows.None && value != Shows.None)
                {
                    BeforeShow = Info.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);
                    BrightnessBeforeShow = this.GetBrightness();
                }
                this._giveShow = value;
                if(this._giveShow != Shows.None)
                {
                    Task.Run(() => this.DoWorkAsync());
                }
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

        private void SetColorShow(int i, Color color){
            if(this.GiveShow == Shows.None){
                throw new OperationCanceledException("Show is canceled :(");
            }
            this.SetColor(i, color);
        }

        private void SetBrightnessShow(int brightness){
            if (this.GiveShow == Shows.None)
            {
                throw new OperationCanceledException("Show is canceled :(");
            }
            if (brightness > 0 && brightness <= 255)
                this.SetBrightness((byte)brightness);
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

        private async Task TimeShowAsync(){
            if (GiveShowForMS == 0) // 0 = forever
                return;

            await Task.Delay(GiveShowForMS);
            GiveShow = Shows.None;
            return;
        }

        private async Task DoWorkAsync()
        {
            try { 
                while (GiveShow != Shows.None)
                {
                    switch (GiveShow)
                    {
                        case Shows.ColorSwipe:
                            await this.ColorSwipe();
                            break;
                        case Shows.Rainbow:
                            await this.Rainbow();
                            break;
                        case Shows.Alarm:
                            await this.Alarm();
                            break;
                        default:
                            return;
                    }
                }
                throw new OperationCanceledException("Show is none :(.. but luckely just finished");
            }
            catch{
                //Reset everything
                Info = BeforeShow.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);
                BeforeShow = new Dictionary<int, Color>();
                this.SetBrightness((byte)BrightnessBeforeShow);
                this.Show();
            }
        }

        private async Task Alarm(){
            int brightnessNumber = this.GetBrightness();
            brightnessNumber = (int)Math.Floor((double)(brightnessNumber / 5)) * 5;

            //Color up
            for (int brightness = brightnessNumber; brightness <= 255; brightness+= 5){
                this.SetBrightnessShow(brightness);
                this.Show();
                await Task.Delay(50);
            }

            //Color down
            for (int brightness = 255; brightness >= 0; brightness-=5)
            {
                this.SetBrightnessShow(brightness);
                this.Show();
                await Task.Delay(50);
            }

        }

        private async Task ColorSwipe(){
            for (int i = 0; i < this.GetNumberOfPixels(); i++)
            {
                this.SetColorShow(i, Color.Red);
                this.Show();
            }
            await Task.Delay(1000);
            for (int i = 0; i < this.GetNumberOfPixels(); i++)
            {
                this.SetColorShow(i, Color.Blue);
                this.Show();
            }
            await Task.Delay(1000);
        }

        private async Task Rainbow()
        {
            for (int i = 0; i < this.GetNumberOfPixels(); i++)
            {
                this.SetColorShow(i, Color.Purple);
            }
            this.Show();
            await Task.Delay(1000);
            for (int i = 0; i < this.GetNumberOfPixels(); i++)
            {
                this.SetColorShow(i, Color.Green);
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
