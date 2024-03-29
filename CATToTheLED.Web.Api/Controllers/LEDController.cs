﻿using System;
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
        }

        [HttpGet("color")]
        public Dictionary<int, Color> GetColors()
        {
            return _neopixel.Info;
        }

        [HttpPost("color/list")]
        //Set the whole matrix the way you want it
        public void SetColorList([FromBody]Dictionary<int, string> coloursDictionary)
        {
            _neopixel.GiveShow = Shows.None;
            foreach (var colorIndexCombination in coloursDictionary)
            {
                _neopixel.SetColor(colorIndexCombination.Key, Color.FromName(colorIndexCombination.Value));
            }

            // Apply changes to the led
            _neopixel.Show();

        }

        [HttpPost("brightness")]
        public void SetBrightness(byte brightness)
        {
            if (brightness >= 0 && brightness <= 255)
            {
                _neopixel.SetBrightness(brightness);
            }
            // Apply changes to the led
            _neopixel.Show();
        }

        [HttpPost("color")]
        public void SetColors(string colorString, byte brightness = 255)
        {
            _neopixel.GiveShow = Shows.None;
            for (var i = 0; i < _neopixel.GetNumberOfPixels(); i++)
            {
                _neopixel.SetColor(i, Color.FromName(colorString));
            }
            if(brightness >= 0 && brightness <= 255)
            {
                _neopixel.SetBrightness(brightness);
            }
            // Apply changes to the led
            _neopixel.Show();
        }

        [HttpPost("show")]
        public Shows SetShow(string show, int time)
        {
            if (time < 0)
                throw new Exception("We can't time travel!");

            //First cancel the show 
            _neopixel.GiveShow = Shows.None;
            //Prepare new show
            _neopixel.GiveShowForMS = time;
            _neopixel.GiveShow = (Shows)Enum.Parse(typeof(Shows), show);
            return _neopixel.GiveShow;
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
            _neopixel.GiveShow = Shows.None;
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