using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using ws281x.Net;

namespace CATToTheLED.Web.Api.Controllers
{

    public class NeoPixelExtended :Neopixel {
        public Dictionary<int, Color> Info { get; set; }
        public NeoPixelExtended(int ledCount, int pin) : base(ledCount: ledCount, pin: pin)
        {
            Info = new Dictionary<int, Color>();
        }

        public Color SetColor(int i, Color color){
            if(!this.Info.TryAdd(i, color))
            {
                this.Info[i] = color;
            }
            return color;
        }

        public Color GetColor(int i)
        {
            return this.Info.GetValueOrDefault(i);
        }

        new public void Show(){
            for (int i = 0; i < this.GetNumberOfPixels(); i++){
                if(this.Info.TryGetValue(i, out Color color)){
                    this.LedList.SetColor(i, System.Drawing.Color.FromArgb(color.G, color.B, color.R));
                }
            }
            base.Show();
        }
    }

    public static class NeoPixelStatic
    {
        public static NeoPixelExtended neopixel { get; }

        static NeoPixelStatic(){
            int _ledCount = 60;
            int _pin = 18;
            neopixel = new NeoPixelExtended(_ledCount, _pin);
        }
    }


    [Route("ledstrip/v1")]
    [ApiController]
    public class LedController : ControllerBase
    {

        private readonly NeoPixelExtended _neopixel;

        public LedController(){
            _neopixel = NeoPixelStatic.neopixel;
            _neopixel.Begin();
        }

        [HttpGet("color")]
        public Dictionary<int, Color> GetColors()
        {
            return _neopixel.Info;
        }

        [HttpPut("color")]
        public void SetColors(string colorString)
        {
            for (var i = 0; i < _neopixel.GetNumberOfPixels(); i++)
            {
                _neopixel.SetColor(i, System.Drawing.Color.FromName(colorString));
            }

            // Apply changes to the led
            _neopixel.Show();

        }

        [HttpGet("ping")]
        public string GetPing()
        {
            return $"Pong {DateTime.Now}";
        }

        [HttpGet("led/{ledIndex}/color")]
        public string GetColor(int ledIndex)
        {
            if (!(ledIndex >= 0 && ledIndex < _neopixel.GetNumberOfPixels()))
            {
                throw new Exception("Led is not existing");
            }

            //Get the attribute
            var color = _neopixel.GetColor(ledIndex).Name;

            return color;
        }

        [HttpPut("led/{ledIndex}/color")]
        public void SetColor(int ledIndex, string colorString)
        {
        
            if (!(ledIndex >= 0 && ledIndex < _neopixel.GetNumberOfPixels()))
            {
                throw new Exception("Led is not existing");
            }
            _neopixel.SetColor(ledIndex, System.Drawing.Color.FromName(colorString));

            // Apply changes to the led
            _neopixel.Show();

        }

    }
}