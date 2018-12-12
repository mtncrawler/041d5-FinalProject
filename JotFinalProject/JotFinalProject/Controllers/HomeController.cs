﻿using System;
using System.Text;
using System.Threading.Tasks;
using JotFinalProject.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JotFinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IImageUploaded _imageUploaded;
        private readonly ICognitive _cognitive;

        public HomeController(IImageUploaded imageUploaded, ICognitive cognitive)
        {
            _imageUploaded = imageUploaded;
            _cognitive = cognitive;
        } 

        public async Task<IActionResult> Index()
        {
            var imageUploaded = await _cognitive.AnalyzeImage();
            //await _imageUploaded.CreateImageUploaded(imageUploaded);
            var imageUploadeds = _imageUploaded.GetImageUploadeds("1");
            return View(imageUploadeds);
        }

        public async Task<IActionResult> Details(int id)
        {
            var imageUploaded = _imageUploaded.GetImageUploaded(id);
            if (imageUploaded == null)
            {
                return NotFound();
            }

            var apiReponseBody = await _cognitive.GetContentFromOperationLocation(imageUploaded);
            StringBuilder output = new StringBuilder();
            foreach (var item in apiReponseBody.RecognitionResult.Lines)
            {
                output.Append(item.Text);
                output.Append(Environment.NewLine);
            }
            //ViewData["data"] = output;

            return View(apiReponseBody);
        }
    }
}