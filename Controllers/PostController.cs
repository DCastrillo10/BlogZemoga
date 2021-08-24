using BlogZemoga.Data;
using BlogZemoga.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogZemoga.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PostController(ApplicationDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? Id)
        {
            CommentVM comment = new CommentVM();
            comment.Post = new Post();
            comment.Comment = new Comment();

            comment.Comment.PostId = comment.Post.Id;
            comment.Comment.Status = true;
            comment.Comment.RegisterDate = DateTime.Now;

            if (Id == null)
            {
                return View(comment);
            }
            else
            {
                comment.Comment.PostId = comment.Post.Id;
                comment.Post = await _db.Posts.FindAsync(Id);
                return View(comment);
            }
            
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CommentVM commentVM)
        {
            commentVM.Comment.PostId = commentVM.Post.Id;
            commentVM.Comment.Status = true;
            commentVM.Comment.RegisterDate = DateTime.Now;

            //if (ModelState.IsValid)
            //{
                await _db.Comments.AddAsync(commentVM.Comment);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = 0 });
            //}
            //return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> SeeComments(int? id)
        {
            
            if (id == null)
            {
                id = 0;
            }
            var ListComments = await _db.Comments.Where(p => p.PostId == id).ToListAsync();
            return View(ListComments);
        }

        //Metodo Get, cuando no se define el metodo, implicitamente lo toma como un HttpGet
        public async Task<IActionResult> MyPost(int? id)
        {
            Post post = new Post(); //Inicializo una variable de tipo Post, que es el modelo
            
            post.RegisterDate = DateTime.Now;
            //post.ApprovalDate = DateTime.Now;
            post.Status = "Pending";
            //Capturamos los datos
            var claimuserId = (ClaimsIdentity)User.Identity;
            var claim = claimuserId.FindFirst(ClaimTypes.NameIdentifier);
            post.UserId = claim.Value;

            if (id == null)
            {
                return View(post);
            }
            else
            {
                post = await _db.Posts.FindAsync(id);
                return View(post);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyPost(Post post)
        {
            post.RegisterDate = DateTime.Now;
            //post.ApprovalDate = DateTime.Now;
            post.Status ="Pending";
            //Capturamos los datos
            var claimuserId = (ClaimsIdentity)User.Identity;
            var claim = claimuserId.FindFirst(ClaimTypes.NameIdentifier);
            post.UserId = claim.Value;

            //if (ModelState.IsValid)
            //{
                if (post.Id == 0)
                {
                    await _db.Posts.AddAsync(post);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(MyPost));
                }
                else
                {
                    _db.Posts.Update(post);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(MyPost), new { id=0 });
                }
            //}
            //return View(post);
        }

        public async Task<IActionResult> PostPending()
        {
            var postpend = await _db.Posts.Include(u=>u.ApplicationUser).Where(s => s.Status == "Pending").ToListAsync();
            return View(postpend);
            
        }

        //[HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Approved(int id)
        {

            var result = await  _db.Posts.FirstOrDefaultAsync(p=>p.Id==id);

            if (result is null)
            {
                return NotFound();
            }

            try
            {
                result.Status = "Approved";
                result.ApprovalDate = DateTime.Now;
                var claimuserId = (ClaimsIdentity)User.Identity;
                var claim = claimuserId.FindFirst(ClaimTypes.NameIdentifier);
                result.UserId = claim.Value;

                _db.Posts.Update(result);
                await _db.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!PostExists(result.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(PostPending));
        }

        public async Task<IActionResult> Reject(int id)
        {

            var result = await _db.Posts.FirstOrDefaultAsync(p => p.Id == id);

            if (result is null)
            {
                return NotFound();
            }

            try
            {
                result.Status = "Reject";
                result.ApprovalDate = DateTime.Now;
                var claimuserId = (ClaimsIdentity)User.Identity;
                var claim = claimuserId.FindFirst(ClaimTypes.NameIdentifier);
                result.UserId = claim.Value;

                _db.Posts.Update(result);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(result.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(PostPending));
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _db.Posts.Include(u=>u.ApplicationUser).FirstOrDefaultAsync(p => p.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var result = await _db.Posts.FindAsync(id);
            _db.Posts.Remove(result);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(PostPending));
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {
            //Ojo que te toca cambiar el status a "Approved"
            var all = await _db.Posts.Include(u=>u.ApplicationUser).Where(p=>p.Status== "Approved").ToListAsync();
            return Json(new { data = all });
        }

        public async Task<ActionResult> GetPostByUser(string user)
        {
            var claimuserId = (ClaimsIdentity)User.Identity;
            var claim = claimuserId.FindFirst(ClaimTypes.NameIdentifier);
            user = claim.Value;
            var list = await _db.Posts.Include(u => u.ApplicationUser).Where(u => u.UserId == user).ToListAsync();
            return Json(new { data = list });
        }

        public async Task<ActionResult> GetPostPending()
        {
            var all = await _db.Posts.Include(u=>u.ApplicationUser).Where(s=>s.Status=="Pending").ToListAsync();
            return Json(new { data = all });
        }

        private bool PostExists(int id)
        {
            return _db.Posts.Any(e => e.Id == id);
        }

    }
}
