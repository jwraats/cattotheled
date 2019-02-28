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
        private readonly Neopixel _neopixel;

        public LedController()
        {
            _neopixel = new Neopixel(60, 18);
        }

        [HttpGet("color")]
        public ActionResult<Dictionary<int, Color>> Get()
        {
            Dictionary<int, Color> colorIndex = new Dictionary<int, Color>();

            for(int i = 0; i <= 60; i++)
                colorIndex.Add(i, _neopixel.LedList.GetColor(i));

            return colorIndex;
        }

        [HttpGet("led/{ledIndex}/color")]
        public ActionResult<Color> Get(int ledIndex)
        {
            return _neopixel.LedList.GetColor(ledIndex);
        }

        [HttpPut("led/{ledIndex}/setColor")]
        public void SetColor(int ledIndex, Color color)
        {
            _neopixel.SetPixelColor(ledIndex, color);
        }
    }
}