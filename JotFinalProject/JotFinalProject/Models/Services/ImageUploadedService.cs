﻿using JotFinalProject.Data;
using JotFinalProject.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JotFinalProject.Models.Services
{
    public class ImageUploadedService : IImageUploaded
    {
        private JotDbContext _context;

        public ImageUploadedService(JotDbContext context)
        {
            _context = context;
        }

        public async Task CreateImageUploaded(ImageUploaded imageUploaded)
        {
            _context.ImageUploadeds.Add(imageUploaded);
            await _context.SaveChangesAsync();
        }

        public List<ImageUploaded> GetImageUploaded(string userId)
        {
            return  _context.ImageUploadeds.Where(x => x.UserId.Equals(userId)).ToList();
        }
    }
}
