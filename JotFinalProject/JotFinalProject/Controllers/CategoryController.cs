﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JotFinalProject.Data;
using JotFinalProject.Models;
using JotFinalProject.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JotFinalProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategory _context;
        private readonly JotDbContext _jotdbcontext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ICategory context, JotDbContext jotdbcontext, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _jotdbcontext = jotdbcontext;
            _userManager = userManager;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var getUser = await _userManager.GetUserAsync(User);
            string id = getUser.Email;
            var cat = await _context.GetCategories(id);
            return View(cat);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            category.Notes = await _context.GetAllNotesFromCategory(id);
            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                var getUser = await _userManager.GetUserAsync(User);
                category.UserID = getUser.Email;
                await _context.AddCategory(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Category category)
        {
            if (id != category.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var getUser = await _userManager.GetUserAsync(User);
                    category.UserID = getUser.Email;
                    await _context.UpdateCategory(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var getUser = await _userManager.GetUserAsync(User);
            //var userId = getUser.Email;
            await _context.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.GetCategory(id) != null;
        }
    }
}