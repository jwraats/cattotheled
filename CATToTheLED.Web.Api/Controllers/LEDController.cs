using System;
using System.Collections.Generic;
using System.Drawing;
using CATToTheLED.Web.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CATToTheLED.Web.Api.Controllers
{
    [Route("ledstrip/v1")]
    [ApiController]
    public class LedController : ControllerBase
    {
        private readonly NeoPixelExtended _neopixel;

        public LedController(){
            _neopixel = NeoPixelStatic.Neopixel;
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
                _neopixel.SetColor(i, Color.FromName(colorString));
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
            _neopixel.SetColor(ledIndex, Color.FromName(colorString));

            // Apply changes to the led
            _neopixel.Show();
        }
    }
}