using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using ws281x.Net;

namespace CATToTheLED.Web.Api.Controllers
{
    [Route("ledstrip/v1")]
    [ApiController]
    public class LedController : ControllerBase
    {
        private readonly int _ledCount = 60;
        private readonly int _pin = 18;
        private readonly Neopixel _neopixel;

        public LedController(){
            _neopixel = new ws281x.Net.Neopixel(ledCount: _ledCount, pin: _pin);
            _neopixel.Begin();
        }

        [HttpGet("color")]
        public Dictionary<int, string> GetColors()
        {
            Dictionary<int, string> colorIndex = new Dictionary<int, string>();
            for (var i = 0; i < _neopixel.GetNumberOfPixels(); i++)
            {
                colorIndex.Add(i, _neopixel.LedList.GetColor(i).Name);
            }
            return colorIndex;
        }

        [HttpPut("color")]
        public void SetColors(string colorString)
        {
            for (var i = 0; i < _neopixel.GetNumberOfPixels(); i++)
            {
                Color desiredColor = System.Drawing.Color.FromName(colorString);
                _neopixel.LedList.SetColor(i, System.Drawing.Color.FromArgb(desiredColor.G, desiredColor.B, desiredColor.R));
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
            var color = _neopixel.LedList.GetColor(ledIndex).Name;

            return color;
        }

        [HttpPut("led/{ledIndex}/color")]
        public void SetColor(int ledIndex, string colorString)
        {
        
            if (!(ledIndex >= 0 && ledIndex < _neopixel.GetNumberOfPixels()))
            {
                throw new Exception("Led is not existing");
            }
            Color desiredColor = System.Drawing.Color.FromName(colorString);
            _neopixel.LedList.SetColor(ledIndex, System.Drawing.Color.FromArgb(desiredColor.G, desiredColor.B, desiredColor.R));

            // Apply changes to the led
            _neopixel.Show();

        }

    }
}