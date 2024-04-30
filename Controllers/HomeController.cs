using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormsApp.Controllers;

public class HomeController : Controller
{

    
    public HomeController()
    {
       
    }

    public IActionResult Index(string searchString, string category)
    {
        var products =Repository.Products;
        if(!String.IsNullOrEmpty(searchString))
        {
            ViewBag.Search=searchString;
            products= products.Where(p => p.Name.ToLower().Contains(searchString)).ToList();
        }
        if(!String.IsNullOrEmpty(category) && category != "0"){
            products=products.Where(p=> p.CategoryId==int.Parse(category)).ToList();
        }

        // ViewBag.Categories= new SelectList(Repository.Categories,"CategoryId","Name",category);
        var model = new ProductViewModel{
            Products= products,
            Categories=Repository.Categories,
            SelectedCategory=category
        };
        return View(model);

        
    }
    

    [HttpGet]
    public IActionResult Create() //kullanıcıya bilgi vermek
    {
        ViewBag.Categories= new SelectList(Repository.Categories,"CategoryId","Name"); // Select'e kategoriler aktarılıyor
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product model, IFormFile imageFile)//kullanıcıdan bilgi almak
    {
        
        var extension="";
        if(imageFile != null)
        {
            var allowedExtensions = new [] {".jpg",".jpeg",".png"};
            extension =Path.GetExtension(imageFile.FileName); //Abc.jpg uzantıyı almak için 
            if(!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("","Geçerli bir resim seçiniz.");
            }
        }

        if(ModelState.IsValid) // Verilen bilginin doğruluğu teyit ediliyor
        {
            if(imageFile != null)
            {
                var randomFileName =string.Format($"{Guid.NewGuid().ToString()}{extension}"); //Rastgele bir isim gir ve Extension'a aktardığımız uzantıyı ekle
                var path= Path.Combine(Directory.GetCurrentDirectory(),"wwroot/img",imageFile.FileName);

                using(var stream= new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                model.Image=randomFileName;
                model.ProductId=Repository.Products.Count; //PrpductId oto veriyor - Count + 1; diyerek bir fazlasını verebilirsin.
                Repository.CreateProduct(model);
                return RedirectToAction("Index");  //finalde verilen bilgiler doğru iste Index'e aktarıyor
            }
            
        }
        ViewBag.Categories= new SelectList(Repository.Categories,"CategoryId","Name"); // Select'e kategoriler aktarılıyor
        return View(model); //Model'e aktarılan bilgilerle sayfa yenileniyor.

        
    }

    public IActionResult Edit (int? id)
    {
    
    if(id == null){
        return NotFound();
    }

    var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);
    if (entity == null){
        return NotFound();
    }
    ViewBag.Categories= new SelectList(Repository.Categories,"CategoryId","Name"); // Select'e kategoriler aktarılıyor
    return View(entity);
    
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile)
    {
        if(id != model.ProductId)
        {
           return NotFound();
        }
        
        if(ModelState.IsValid)
        {
            if(imageFile != null)
            {
                var extension =Path.GetExtension(imageFile.FileName); //Abc.jpg uzantıyı almak için 
                var randomFileName =string.Format($"{Guid.NewGuid().ToString()}{extension}"); //Rastgele bir isim gir ve Extension'a aktardığımız uzantıyı ekle
                var path= Path.Combine(Directory.GetCurrentDirectory(),"wwroot/img",imageFile.FileName);

                using(var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                model.Image = randomFileName; 
            }
            
            Repository.EditProduct(model);
            return RedirectToAction("Index");

        }
        
        ViewBag.Categories= new SelectList(Repository.Categories,"CategoryId","Name"); // Select'e kategoriler aktarılıyor
        return View(model);
    }

    public IActionResult Delete(int? id)
    {
        if(id == null)
        {
            return NotFound();
        }
        var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);
        if (entity == null){
            return NotFound();
        }
        return View("DeleteConfirm",entity);
    }
    [HttpPost]
    public IActionResult Delete(int id, int ProductId)
    {
        if(id != ProductId)
        {
           return NotFound();
        }
        var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);
        if (entity == null){
            return NotFound();
        }
        Repository.DeleteProduct(entity);
        return RedirectToAction("Index");

    }
}
