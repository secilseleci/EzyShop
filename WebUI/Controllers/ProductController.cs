 

//        #region Create
 //        [HttpGet]
//        public async Task<IActionResult> Create()
//        {
//            var user = await GetCurrentUserAsync();
//            if (user == null) return RedirectToAction("Login", "Account");

//            var shopResult = await _shopService.GetShopBySellerIdAsync(user.Id);
//            if (!shopResult.Success || shopResult.Data == null)
//            {
//                TempData["ErrorMessage"] = "You don't have an active shop. Please contact admin.";
//                return RedirectToAction(nameof(Index));
//            }

//            var model = new ProductCreateViewModel
//            {
//                ShopId = shopResult.Data.Id  
//            };

//            return View(model);
//        }
 //    
 
//        #region API
//        [HttpGet]
//        public async Task<IActionResult> GetSellerProducts()
//        {
//            var user = await GetCurrentUserAsync();
//            if (user == null) return Unauthorized();

//            var shopResult = await _shopService.GetShopBySellerIdAsync(user.Id);
//            if (!shopResult.Success || shopResult.Data == null)
//            {
//                return Json(new { data = new List<object>() });  
//            }

//            var products = await _productService.GetAllProductsWithCategoryAsync(p => p.ShopId == shopResult.Data.Id);
//            if (products.Data == null || !products.Data.Any())
//            {
//                return Json(new { data = new List<object>() });
//            }
//            var sortedProducts = products.Data.OrderByDescending(p => p.CreatedDate).ToList(); // ✅ En yeni ürünler başta

//            return Json(new { data = sortedProducts });
//        }


//       

//        [AllowAnonymous]
//        [HttpGet]
//        public async Task<IActionResult> GetFilteredProducts(string? name, string? category, string? color, decimal? minPrice, decimal? maxPrice)
//        {
//            var result = await _productService.GetFilteredProductsAsync(name, category, color, minPrice, maxPrice);

//            if (!result.Success || result.Data == null || !result.Data.Any())
//            {
//                return NotFound(new { success = false, message = "No products found matching the filters." });
//            }

//            return Json(new { success = true, data = result.Data });
//        }
//        #endregion

//        #region Product Details
//        [Authorize(Roles = "Customer")]
//        [HttpGet]
//        public async Task<IActionResult> Details(Guid productId)
//        {
//            var result = await _productService.GetProductByIdWithCategoryAsync(productId);
//            if (!result.Success || result.Data == null)
//            {
//                TempData["ErrorMessage"] = "Product not found.";
//                return RedirectToAction("Shop", "Home");  
//            }

//            return View(result.Data); 
//        }
//        #endregion
//    }
//}
