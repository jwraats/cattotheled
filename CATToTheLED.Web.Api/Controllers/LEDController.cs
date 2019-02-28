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
        [HttpGet("color")]
        public Dictionary<int, string> GetColors()
        {
            // You can also choose a custom color order
            var neopixel = new ws281x.Net.Neopixel(ledCount: _ledCount, pin: _pin);

            // Always initialize the wrapper first
            neopixel.Begin();

            Dictionary<int, string> colorIndex = new Dictionary<int, string>();
            for (var i = 0; i < neopixel.GetNumberOfPixels(); i++)
            {
                colorIndex.Add(i, $"R:{neopixel.LedList.GetColor(i).G}, G:{neopixel.LedList.GetColor(i).B}, {neopixel.LedList.GetColor(i).R}");
            }

            return colorIndex;
        }

        [HttpPut("color")]
        public void SetColors(string colorString)
        {

            // You can also choose a custom color order
            var neopixel = new ws281x.Net.Neopixel(ledCount: _ledCount, pin: _pin);

            // Always initialize the wrapper first
            neopixel.Begin();

            for (var i = 0; i < neopixel.GetNumberOfPixels(); i++)
            {
                Color desiredColor = System.Drawing.Color.FromName(colorString);
                neopixel.SetPixelColor(i, System.Drawing.Color.FromArgb(desiredColor.G, desiredColor.B, desiredColor.R));
                neopixel.LedList.SetColor(i, System.Drawing.Color.FromArgb(desiredColor.G, desiredColor.B, desiredColor.R));
            }

            // Apply changes to the led
            neopixel.Show();

        }

        [HttpGet("ping")]
        public string GetPing()
        {
            return $"Pong {DateTime.Now}";
        }

        [HttpGet("led/{ledIndex}/color")]
        public string GetColor(int ledIndex)
        {
            var neopixel = new ws281x.Net.Neopixel(ledCount: _ledCount, pin: _pin);

            if (!(ledIndex >= 0 && ledIndex < neopixel.GetNumberOfPixels()))
            {
                throw new Exception("Led is not existing");
            }


            // Always initialize the wrapper first
            neopixel.Begin();

            //Get the attribute
            var color = neopixel.LedList.GetColor(ledIndex).Name;

            return color;
        }

        [HttpPut("led/{ledIndex}/color")]
        public void SetColor(int ledIndex, string colorString)
        {

            // You can also choose a custom color order
            var neopixel = new ws281x.Net.Neopixel(ledCount: _ledCount, pin: _pin);


            if (!(ledIndex >= 0 && ledIndex < neopixel.GetNumberOfPixels()))
            {
                throw new Exception("Led is not existing");
            }
            // Always initialize the wrapper first
            neopixel.Begin();
            Color desiredColor = System.Drawing.Color.FromName(colorString);
            neopixel.SetPixelColor(ledIndex, System.Drawing.Color.FromArgb(desiredColor.G, desiredColor.B, desiredColor.R));
            neopixel.LedList.SetColor(ledIndex, System.Drawing.Color.FromArgb(desiredColor.G, desiredColor.B, desiredColor.R));

            // Apply changes to the led
            neopixel.Show();

        }

    }
}