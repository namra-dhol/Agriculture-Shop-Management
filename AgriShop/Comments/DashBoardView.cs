//@model DashboardViewModel
//@{
//    ViewData["Title"] = "AgriMart - Premium Agricultural Products";
//    var userName = Context.Session.GetString("Username");
//    var isLoggedIn = Context.Session.GetString("IsLoggedIn") == "true";
//}

//<main id="main" class="main">
//    <!-- Hero Section -->
//    <section class="hero-section py-5">
//        <div class="container">
//            <div class="row align-items-center">
//                <div class="col-lg-6">
//                    <div class="py-5">
//                        @if (isLoggedIn)
//                        {
//                            <span class="badge bg-primary-custom mb-3">Welcome back, @userName!</span>
//                        }
//                        else
//                        {
//                            <span class="badge bg-primary-custom mb-3">🌾 India's #1 Agriculture Marketplace</span>
//                        }
//                        <h1 class="display-4 fw-bold mb-4">
//                            Premium Agricultural Products for
//                            <span class="text-primary-custom">Modern Farmers</span>
//                        </h1>
//                        <p class="lead text-muted mb-4">
//                            Get quality seeds, fertilizers, tools, and expert advice delivered to your farm.
//                            Trusted by over 50,000 farmers across India.
//                        </p>
//                        <div class="d-flex flex-column flex-sm-row gap-3">
//                            <button class="btn bg-primary-custom text-white btn-lg px-4" onclick="scrollToProducts()">
//                                Shop Now <i class="bi bi-chevron-right ms-2"></i>
//                            </button>
//                            <button class="btn btn-outline-primary btn-lg px-4">
//                                Download App
//                            </button>
//                        </div>
//                    </div>
//                </div>
//                <div class="col-lg-6 d-none d-lg-block">
//                    <img src="https://images.unsplash.com/photo-1574323347407-f5e1ad6d020b?w=600&h=400"
//                         alt="Agriculture" class="img-fluid rounded shadow">
//                </div>
//            </div>
//        </div>
//    </section>

//    <!-- Dynamic Statistics Section -->
//    <section class="py-4 bg-light">
//        <div class="container">
//            <div class="row">
//                <div class="col-md-3 col-6 mb-3">
//                    <div class="d-flex align-items-center gap-3 p-3">
//                        <i class="bi bi-box-seam feature-icon"></i>
//                        <div>
//                            <h6 class="mb-1 fw-semibold">@Model.TotalProducts Products</h6>
//                            <small class="text-muted">Available in stock</small>
//                        </div>
//                    </div>
//                </div>
//                <div class="col-md-3 col-6 mb-3">
//                    <div class="d-flex align-items-center gap-3 p-3">
//                        <i class="bi bi-grid-3x3-gap feature-icon"></i>
//                        <div>
//                            <h6 class="mb-1 fw-semibold">@Model.TotalCategories Categories</h6>
//                            <small class="text-muted">Product varieties</small>
//                        </div>
//                    </div>
//                </div>
//                <div class="col-md-3 col-6 mb-3">
//                    <div class="d-flex align-items-center gap-3 p-3">
//                        <i class="bi bi-truck feature-icon"></i>
//                        <div>
//                            <h6 class="mb-1 fw-semibold">Free Delivery</h6>
//                            <small class="text-muted">Orders above ₹999</small>
//                        </div>
//                    </div>
//                </div>
//                <div class="col-md-3 col-6 mb-3">
//                    <div class="d-flex align-items-center gap-3 p-3">
//                        <i class="bi bi-award feature-icon"></i>
//                        <div>
//                            <h6 class="mb-1 fw-semibold">@Model.RecentProductsCount New</h6>
//                            <small class="text-muted">Added this month</small>
//                        </div>
//                    </div>
//                </div>
//            </div>
//        </div>
//    </section>

//    <!-- Dynamic Categories Section -->
//    <section class="py-5" id="categories">
//        <div class="container">
//            <div class="text-center mb-5">
//                <h2 class="display-5 fw-bold mb-3">Shop by Category</h2>
//                <p class="text-muted">Find everything you need for your farm in one place</p>
//            </div>
//            <div class="row">
//                @if (Model.ProductCategories.Any())
//                {
//                    @foreach (var category in Model.ProductCategories.Take(6))
//                    {
//                        <div class="col-lg-2 col-md-4 col-6 mb-4">
//                            <div class="card category-card h-100 border-0 shadow-sm" style="cursor: pointer;" 
//                                 onclick="filterByCategory(@category.ProductTypeId, '@category.TypeName')">
//                                <div class="card-body text-center p-4">
//                                    <img src="@category.DefaultCategoryImage"
//                                         alt="@category.TypeName" class="rounded-circle mb-3" 
//                                         width="64" height="64" style="object-fit: cover;">
//                                    <h6 class="fw-semibold">@category.TypeName</h6>
//                                    <small class="text-muted">@category.ProductCount Products</small>
//                                </div>
//                            </div>
//                        </div>
//                    }
//                }
//                else
//                {
//                    <div class="col-12 text-center">
//                        <p class="text-muted">No categories available at the moment.</p>
//                    </div>
//                }
//            </div>
//        </div>
//    </section>

//    <!-- Dynamic Featured Products -->
//    <section class="py-5 bg-light" id="products">
//        <div class="container">
//            <div class="d-flex justify-content-between align-items-center mb-5">
//                <div>
//                    <h2 class="display-5 fw-bold mb-3">Featured Products</h2>
//                    <p class="text-muted">Best selling products chosen by farmers</p>
//                </div>
//                <button class="btn btn-outline-primary" onclick="loadAllProducts()">
//                    View All Products <i class="bi bi-chevron-right ms-2"></i>
//                </button>
//            </div>
            
//            <div class="row" id="productGrid">
//                @if (Model.FeaturedProducts.Any())
//                {
//                    @foreach (var product in Model.FeaturedProducts)
//                    {
//                        <div class="col-lg-3 col-md-6 mb-4">
//                            <div class="card product-card h-100 border-0 shadow-sm">
//                                <div class="position-relative">
//                                    <img src="@product.ImageUrl"
//                                         class="card-img-top" alt="@product.ProductName" 
//                                         style="height: 200px; object-fit: cover;">
                                    
//                                    @if (product.IsOutOfStock)
//                                    {
//                                        <div class="position-absolute inset-0 bg-dark bg-opacity-50 d-flex align-items-center justify-content-center" style="top: 0; left: 0; right: 0; bottom: 80px;">
//                                            <span class="text-white fw-medium">Out of Stock</span>
//                                        </div>
//                                    }
//                                    else if (product.IsLowStock)
//                                    {
//                                        <span class="badge bg-warning position-absolute top-0 start-0 m-2">Low Stock</span>
//                                    }
                                    
//                                    <button class="btn btn-light position-absolute top-0 end-0 m-2 rounded-circle">
//                                        <i class="bi bi-heart"></i>
//                                    </button>
//                                </div>
//                                <div class="card-body">
//                                    <h6 class="card-title">@product.ProductName</h6>
//                                    <div class="d-flex align-items-center mb-2">
//                                        <div class="star-rating me-2">
//                                            <i class="bi bi-star-fill"></i>
//                                            <i class="bi bi-star-fill"></i>
//                                            <i class="bi bi-star-fill"></i>
//                                            <i class="bi bi-star-fill"></i>
//                                            <i class="bi bi-star"></i>
//                                        </div>
//                                        <small class="text-muted">(★ 4.2)</small>
//                                    </div>
//                                    <div class="mb-2">
//                                        <small class="text-muted">Stock: @product.Stock units</small>
//                                    </div>
//                                    <div class="d-flex align-items-center gap-2 mb-3">
//                                        <span class="h5 text-primary-custom mb-0">₹@(Random.Shared.Next(199, 2999))</span>
//                                        <span class="text-muted text-decoration-line-through">₹@(Random.Shared.Next(299, 3999))</span>
//                                    </div>
//                                </div>
//                                <div class="card-footer bg-transparent border-0">
//                                    @if (product.IsOutOfStock)
//                                    {
//                                        <button class="btn btn-secondary w-100" disabled>
//                                            <i class="bi bi-cart me-2"></i>Out of Stock
//                                        </button>
//                                    }
//                                    else
//                                    {
//                                        <button class="btn bg-primary-custom text-white w-100" onclick="addToCart(@product.ProductId)">
//                                            <i class="bi bi-cart me-2"></i>Add to Cart
//                                        </button>
//                                    }
//                                </div>
//                            </div>
//                        </div>
//                    }
//                }
//                else
//                {
//                    <div class="col-12 text-center">
//                        <p class="text-muted">No featured products available at the moment.</p>
//                        <button class="btn btn-primary" onclick="window.location.reload()">Refresh</button>
//                    </div>
//                }
//            </div>
            
//            <!-- Loading indicator -->
//            <div id="loadingIndicator" class="text-center d-none">
//                <div class="spinner-border text-primary" role="status">
//                    <span class="visually-hidden">Loading...</span>
//                </div>
//                <p class="mt-2">Loading products...</p>
//            </div>
//        </div>
//    </section>

//    <!-- Newsletter Section -->
//    <section class="py-5 bg-primary-custom text-white">
//        <div class="container text-center">
//            <h2 class="display-5 fw-bold mb-4">Stay Updated with AgriMart</h2>
//            <p class="lead mb-4">Get farming tips, product updates, and exclusive offers delivered to your inbox</p>
//            <div class="row justify-content-center">
//                <div class="col-md-6">
//                    <div class="input-group">
//                        <input type="email" class="form-control" placeholder="Enter your email" id="newsletterEmail">
//                        <button class="btn btn-light text-dark fw-medium" onclick="subscribeNewsletter()">Subscribe</button>
//                    </div>
//                </div>
//            </div>
//        </div>
//    </section>
//</main>

//<!-- Custom Styles -->
//<style>
//    .feature-icon {
//        font-size: 2rem;
//        color: var(--primary-color, #22c55e);
//    }

//    .product-card {
//        transition: transform 0.2s ease, box-shadow 0.2s ease;
//    }

//    .product-card:hover {
//        transform: translateY(-5px);
//        box-shadow: 0 8px 25px rgba(0,0,0,0.15);
//    }

//    .category-card {
//        transition: transform 0.2s ease, box-shadow 0.2s ease;
//    }

//    .category-card:hover {
//        transform: translateY(-3px);
//        box-shadow: 0 6px 20px rgba(0,0,0,0.1);
//    }

//    .star-rating i {
//        color: #ffd700;
//        font-size: 0.9rem;
//    }

//    .bg-primary-custom {
//        background-color: #22c55e !important;
//    }

//    .text-primary-custom {
//        color: #22c55e !important;
//    }

//    .btn-outline-primary {
//        border-color: #22c55e;
//        color: #22c55e;
//    }

//    .btn-outline-primary:hover {
//        background-color: #22c55e;
//        border-color: #22c55e;
//    }
//</style>

//<!-- JavaScript for Dynamic Features -->
//<script>
//    // Scroll to products section
//    function scrollToProducts() {
//        document.getElementById('products').scrollIntoView({ behavior: 'smooth' });
//    }

//    // Filter products by category
//    async function filterByCategory(categoryId, categoryName) {
//        const loadingIndicator = document.getElementById('loadingIndicator');
//        const productGrid = document.getElementById('productGrid');
        
//        try {
//            loadingIndicator.classList.remove('d-none');
//            productGrid.style.opacity = '0.5';

//            const response = await fetch(`/Home/ProductsByCategory?categoryId=${categoryId}`);
//            const products = await response.json();

//            if (products && products.length > 0) {
//                updateProductGrid(products, `${categoryName} Products`);
//            } else {
//                showNoProductsMessage(categoryName);
//            }
//        } catch (error) {
//            console.error('Error filtering products:', error);
//            showErrorMessage();
//        } finally {
//            loadingIndicator.classList.add('d-none');
//            productGrid.style.opacity = '1';
//        }
//    }

//    // Update product grid with new data
//    function updateProductGrid(products, title) {
//        const productGrid = document.getElementById('productGrid');
//        const sectionTitle = document.querySelector('#products h2');
        
//        sectionTitle.textContent = title;
        
//        productGrid.innerHTML = products.map(product => `
//            <div class="col-lg-3 col-md-6 mb-4">
//                <div class="card product-card h-100 border-0 shadow-sm">
//                    <div class="position-relative">
//                        <img src="${product.imageUrl || 'https://images.unsplash.com/photo-1574323347407-f5e1ad6d020b?w=400'}"
//                             class="card-img-top" alt="${product.productName}" 
//                             style="height: 200px; object-fit: cover;">
                        
//                        ${product.isOutOfStock ? `
//                        <div class="position-absolute inset-0 bg-dark bg-opacity-50 d-flex align-items-center justify-content-center" style="top: 0; left: 0; right: 0; bottom: 80px;">
//                            <span class="text-white fw-medium">Out of Stock</span>
//                        </div>` : ''}
                        
//                        ${product.isLowStock && !product.isOutOfStock ? `
//                        <span class="badge bg-warning position-absolute top-0 start-0 m-2">Low Stock</span>` : ''}
                        
//                        <button class="btn btn-light position-absolute top-0 end-0 m-2 rounded-circle">
//                            <i class="bi bi-heart"></i>
//                        </button>
//                    </div>
//                    <div class="card-body">
//                        <h6 class="card-title">${product.productName}</h6>
//                        <div class="d-flex align-items-center mb-2">
//                            <div class="star-rating me-2">
//                                <i class="bi bi-star-fill"></i>
//                                <i class="bi bi-star-fill"></i>
//                                <i class="bi bi-star-fill"></i>
//                                <i class="bi bi-star-fill"></i>
//                                <i class="bi bi-star"></i>
//                            </div>
//                            <small class="text-muted">(★ 4.2)</small>
//                        </div>
//                        <div class="mb-2">
//                            <small class="text-muted">Stock: ${product.stock} units</small>
//                        </div>
//                        <div class="d-flex align-items-center gap-2 mb-3">
//                            <span class="h5 text-primary-custom mb-0">₹${Math.floor(Math.random() * 2800) + 199}</span>
//                            <span class="text-muted text-decoration-line-through">₹${Math.floor(Math.random() * 3800) + 299}</span>
//                        </div>
//                    </div>
//                    <div class="card-footer bg-transparent border-0">
//                        ${product.isOutOfStock ? `
//                        <button class="btn btn-secondary w-100" disabled>
//                            <i class="bi bi-cart me-2"></i>Out of Stock
//                        </button>` : `
//                        <button class="btn bg-primary-custom text-white w-100" onclick="addToCart(${product.productId})">
//                            <i class="bi bi-cart me-2"></i>Add to Cart
//                        </button>`}
//                    </div>
//                </div>
//            </div>
//        `).join('');
        
//        // Scroll to products section
//        document.getElementById('products').scrollIntoView({ behavior: 'smooth' });
//    }

//    // Show message when no products found
//    function showNoProductsMessage(categoryName) {
//        const productGrid = document.getElementById('productGrid');
//        const sectionTitle = document.querySelector('#products h2');
        
//        sectionTitle.textContent = `${categoryName} Products`;
//        productGrid.innerHTML = `
//            <div class="col-12 text-center py-5">
//                <i class="bi bi-inbox display-1 text-muted mb-3"></i>
//                <h4 class="text-muted">No products found in ${categoryName}</h4>
//                <p class="text-muted">Try browsing other categories or check back later.</p>
//                <button class="btn btn-primary" onclick="loadAllProducts()">View All Products</button>
//            </div>
//        `;
//    }

//    // Show error message
//    function showErrorMessage() {
//        const productGrid = document.getElementById('productGrid');
//        productGrid.innerHTML = `
//            <div class="col-12 text-center py-5">
//                <i class="bi bi-exclamation-triangle display-1 text-warning mb-3"></i>
//                <h4 class="text-muted">Unable to load products</h4>
//                <p class="text-muted">Please check your connection and try again.</p>
//                <button class="btn btn-primary" onclick="window.location.reload()">Refresh Page</button>
//            </div>
//        `;
//    }

//    // Load all products
//    function loadAllProducts() {
//        window.location.reload();
//    }

//    // Add to cart functionality
//    function addToCart(productId) {
//        if (!@Json.Serialize(isLoggedIn)) {
//            alert('Please login to add items to cart');
//            window.location.href = '/Login';
//            return;
//        }
        
//        // Simulate add to cart
//        const button = event.target.closest('button');
//        const originalText = button.innerHTML;
        
//        button.innerHTML = '<i class="bi bi-check-circle me-2"></i>Added!';
//        button.disabled = true;
//        button.classList.remove('bg-primary-custom');
//        button.classList.add('btn-success');
        
//        setTimeout(() => {
//            button.innerHTML = originalText;
//            button.disabled = false;
//            button.classList.add('bg-primary-custom');
//            button.classList.remove('btn-success');
//        }, 2000);
        
//        console.log(`Product ${productId} added to cart`);
//    }

//    // Newsletter subscription
//    function subscribeNewsletter() {
//        const email = document.getElementById('newsletterEmail').value;
//        if (!email) {
//            alert('Please enter your email address');
//            return;
//        }
        
//        // Simulate newsletter subscription
//        alert('Thank you for subscribing! You will receive farming tips and updates.');
//        document.getElementById('newsletterEmail').value = '';
//    }

//    // Load products on page load
//    document.addEventListener('DOMContentLoaded', function() {
//        console.log('Dashboard loaded with:');
//        console.log('- Products:', @Json.Serialize(Model.TotalProducts));
//        console.log('- Categories:', @Json.Serialize(Model.TotalCategories));
//        console.log('- Featured Products:', @Json.Serialize(Model.FeaturedProducts.Count));
        
//        @if (TempData["Success"] != null)
//        {
//            <text>
//            // Show success message with animation
//            const successAlert = document.createElement('div');
//            successAlert.className = 'alert alert-success alert-dismissible fade show position-fixed';
//            successAlert.style.cssText = 'top: 20px; right: 20px; z-index: 1050; min-width: 300px;';
//            successAlert.innerHTML = `
//                <i class="bi bi-check-circle-fill me-2"></i>@TempData["Success"]
//                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
//            `;
//            document.body.appendChild(successAlert);
            
//            setTimeout(() => {
//                if (successAlert.parentNode) {
//                    successAlert.remove();
//                }
//            }, 5000);
//            </text>
//        }
//    });
//</script>@model DashboardViewModel
//@{
//    ViewData["Title"] = "AgriMart - Premium Agricultural Products";
//    var userName = Context.Session.GetString("Username");
//    var isLoggedIn = Context.Session.GetString("IsLoggedIn") == "true";
//}

//<main id="main" class="main">
//    <!-- Hero Section -->
//    <section class="hero-section py-5">
//        <div class="container">
//            <div class="row align-items-center">
//                <div class="col-lg-6">
//                    <div class="py-5">
//                        @if (isLoggedIn)
//                        {
//                            <span class="badge bg-primary-custom mb-3">Welcome back, @userName!</span>
//                        }
//                        else
//                        {
//                            <span class="badge bg-primary-custom mb-3">🌾 India's #1 Agriculture Marketplace</span>
//                        }
//                        <h1 class="display-4 fw-bold mb-4">
//                            Premium Agricultural Products for
//                            <span class="text-primary-custom">Modern Farmers</span>
//                        </h1>
//                        <p class="lead text-muted mb-4">
//                            Get quality seeds, fertilizers, tools, and expert advice delivered to your farm.
//                            Trusted by over 50,000 farmers across India.
//                        </p>
//                        <div class="d-flex flex-column flex-sm-row gap-3">
//                            <button class="btn bg-primary-custom text-white btn-lg px-4" onclick="scrollToProducts()">
//                                Shop Now <i class="bi bi-chevron-right ms-2"></i>
//                            </button>
//                            <button class="btn btn-outline-primary btn-lg px-4">
//                                Download App
//                            </button>
//                        </div>
//                    </div>
//                </div>
//                <div class="col-lg-6 d-none d-lg-block">
//                    <img src="https://images.unsplash.com/photo-1574323347407-f5e1ad6d020b?w=600&h=400"
//                         alt="Agriculture" class="img-fluid rounded shadow">
//                </div>
//            </div>
//        </div>
//    </section>

//    <!-- Dynamic Statistics Section -->
//    <section class="py-4 bg-light">
//        <div class="container">
//            <div class="row">
//                <div class="col-md-3 col-6 mb-3">
//                    <div class="d-flex align-items-center gap-3 p-3">
//                        <i class="bi bi-box-seam feature-icon"></i>
//                        <div>
//                            <h6 class="mb-1 fw-semibold">@Model.TotalProducts Products</h6>
//                            <small class="text-muted">Available in stock</small>
//                        </div>
//                    </div>
//                </div>
//                <div class="col-md-3 col-6 mb-3">
//                    <div class="d-flex align-items-center gap-3 p-3">
//                        <i class="bi bi-grid-3x3-gap feature-icon"></i>
//                        <div>
//                            <h6 class="mb-1 fw-semibold">@Model.TotalCategories Categories</h6>
//                            <small class="text-muted">Product varieties</small>
//                        </div>
//                    </div>
//                </div>
//                <div class="col-md-3 col-6 mb-3">
//                    <div class="d-flex align-items-center gap-3 p-3">
//                        <i class="bi bi-truck feature-icon"></i>
//                        <div>
//                            <h6 class="mb-1 fw-semibold">Free Delivery</h6>
//                            <small class="text-muted">Orders above ₹999</small>
//                        </div>
//                    </div>
//                </div>
//                <div class="col-md-3 col-6 mb-3">
//                    <div class="d-flex align-items-center gap-3 p-3">
//                        <i class="bi bi-award feature-icon"></i>
//                        <div>
//                            <h6 class="mb-1 fw-semibold">@Model.RecentProductsCount New</h6>
//                            <small class="text-muted">Added this month</small>
//                        </div>
//                    </div>
//                </div>
//            </div>
//        </div>
//    </section>

//    <!-- Dynamic Categories Section -->
//    <section class="py-5" id="categories">
//        <div class="container">
//            <div class="text-center mb-5">
//                <h2 class="display-5 fw-bold mb-3">Shop by Category</h2>
//                <p class="text-muted">Find everything you need for your farm in one place</p>
//            </div>
//            <div class="row">
//                @if (Model.ProductCategories.Any())
//                {
//                    @foreach (var category in Model.ProductCategories.Take(6))
//                    {
//                        <div class="col-lg-2 col-md-4 col-6 mb-4">
//                            <div class="card category-card h-100 border-0 shadow-sm" style="cursor: pointer;" 
//                                 onclick="filterByCategory(@category.ProductTypeId, '@category.TypeName')">
//                                <div class="card-body text-center p-4">
//                                    <img src="@category.DefaultCategoryImage"
//                                         alt="@category.TypeName" class="rounded-circle mb-3" 
//                                         width="64" height="64" style="object-fit: cover;">
//                                    <h6 class="fw-semibold">@category.TypeName</h6>
//                                    <small class="text-muted">@category.ProductCount Products</small>
//                                </div>
//                            </div>
//                        </div>
//                    }
//                }
//                else
//                {
//                    <div class="col-12 text-center">
//                        <p class="text-muted">No categories available at the moment.</p>
//                    </div>
//                }
//            </div>
//        </div>
//    </section>

//    <!-- Dynamic Featured Products -->
//    <section class="py-5 bg-light" id="products">
//        <div class="container">
//            <div class="d-flex justify-content-between align-items-center mb-5">
//                <div>
//                    <h2 class="display-5 fw-bold mb-3">Featured Products</h2>
//                    <p class="text-muted">Best selling products chosen by farmers</p>
//                </div>
//                <button class="btn btn-outline-primary" onclick="loadAllProducts()">
//                    View All Products <i class="bi bi-chevron-right ms-2"></i>
//                </button>
//            </div>
            
//            <div class="row" id="productGrid">
//                @if (Model.FeaturedProducts.Any())
//                {
//                    @foreach (var product in Model.FeaturedProducts)
//                    {
//                        <div class="col-lg-3 col-md-6 mb-4">
//                            <div class="card product-card h-100 border-0 shadow-sm">
//                                <div class="position-relative">
//                                    <img src="@product.ImageUrl"
//                                         class="card-img-top" alt="@product.ProductName" 
//                                         style="height: 200px; object-fit: cover;">
                                    
//                                    @if (product.IsOutOfStock)
//                                    {
//                                        <div class="position-absolute inset-0 bg-dark bg-opacity-50 d-flex align-items-center justify-content-center" style="top: 0; left: 0; right: 0; bottom: 80px;">
//                                            <span class="text-white fw-medium">Out of Stock</span>
//                                        </div>
//                                    }
//                                    else if (product.IsLowStock)
//                                    {
//                                        <span class="badge bg-warning position-absolute top-0 start-0 m-2">Low Stock</span>
//                                    }
                                    
//                                    <button class="btn btn-light position-absolute top-0 end-0 m-2 rounded-circle">
//                                        <i class="bi bi-heart"></i>
//                                    </button>
//                                </div>
//                                <div class="card-body">
//                                    <h6 class="card-title">@product.ProductName</h6>
//                                    <div class="d-flex align-items-center mb-2">
//                                        <div class="star-rating me-2">
//                                            <i class="bi bi-star-fill"></i>
//                                            <i class="bi bi-star-fill"></i>
//                                            <i class="bi bi-star-fill"></i>
//                                            <i class="bi bi-star-fill"></i>
//                                            <i class="bi bi-star"></i>
//                                        </div>
//                                        <small class="text-muted">(★ 4.2)</small>
//                                    </div>
//                                    <div class="mb-2">
//                                        <small class="text-muted">Stock: @product.Stock units</small>
//                                    </div>
//                                    <div class="d-flex align-items-center gap-2 mb-3">
//                                        <span class="h5 text-primary-custom mb-0">₹@(Random.Shared.Next(199, 2999))</span>
//                                        <span class="text-muted text-decoration-line-through">₹@(Random.Shared.Next(299, 3999))</span>
//                                    </div>
//                                </div>
//                                <div class="card-footer bg-transparent border-0">
//                                    @if (product.IsOutOfStock)
//                                    {
//                                        <button class="btn btn-secondary w-100" disabled>
//                                            <i class="bi bi-cart me-2"></i>Out of Stock
//                                        </button>
//                                    }
//                                    else
//                                    {
//                                        <button class="btn bg-primary-custom text-white w-100" onclick="addToCart(@product.ProductId)">
//                                            <i class="bi bi-cart me-2"></i>Add to Cart
//                                        </button>
//                                    }
//                                </div>
//                            </div>
//                        </div>
//                    }
//                }
//                else
//                {
//                    <div class="col-12 text-center">
//                        <p class="text-muted">No featured products available at the moment.</p>
//                        <button class="btn btn-primary" onclick="window.location.reload()">Refresh</button>
//                    </div>
//                }
//            </div>
            
//            <!-- Loading indicator -->
//            <div id="loadingIndicator" class="text-center d-none">
//                <div class="spinner-border text-primary" role="status">
//                    <span class="visually-hidden">Loading...</span>
//                </div>
//                <p class="mt-2">Loading products...</p>
//            </div>
//        </div>
//    </section>

//    <!-- Newsletter Section -->
//    <section class="py-5 bg-primary-custom text-white">
//        <div class="container text-center">
//            <h2 class="display-5 fw-bold mb-4">Stay Updated with AgriMart</h2>
//            <p class="lead mb-4">Get farming tips, product updates, and exclusive offers delivered to your inbox</p>
//            <div class="row justify-content-center">
//                <div class="col-md-6">
//                    <div class="input-group">
//                        <input type="email" class="form-control" placeholder="Enter your email" id="newsletterEmail">
//                        <button class="btn btn-light text-dark fw-medium" onclick="subscribeNewsletter()">Subscribe</button>
//                    </div>
//                </div>
//            </div>
//        </div>
//    </section>
//</main>

//<!-- Custom Styles -->
//<style>
//    .feature-icon {
//        font-size: 2rem;
//        color: var(--primary-color, #22c55e);
//    }

//    .product-card {
//        transition: transform 0.2s ease, box-shadow 0.2s ease;
//    }

//    .product-card:hover {
//        transform: translateY(-5px);
//        box-shadow: 0 8px 25px rgba(0,0,0,0.15);
//    }

//    .category-card {
//        transition: transform 0.2s ease, box-shadow 0.2s ease;
//    }

//    .category-card:hover {
//        transform: translateY(-3px);
//        box-shadow: 0 6px 20px rgba(0,0,0,0.1);
//    }

//    .star-rating i {
//        color: #ffd700;
//        font-size: 0.9rem;
//    }

//    .bg-primary-custom {
//        background-color: #22c55e !important;
//    }

//    .text-primary-custom {
//        color: #22c55e !important;
//    }

//    .btn-outline-primary {
//        border-color: #22c55e;
//        color: #22c55e;
//    }

//    .btn-outline-primary:hover {
//        background-color: #22c55e;
//        border-color: #22c55e;
//    }
//</style>

//<!-- JavaScript for Dynamic Features -->
//<script>
//    // Scroll to products section
//    function scrollToProducts() {
//        document.getElementById('products').scrollIntoView({ behavior: 'smooth' });
//    }

//    // Filter products by category
//    async function filterByCategory(categoryId, categoryName) {
//        const loadingIndicator = document.getElementById('loadingIndicator');
//        const productGrid = document.getElementById('productGrid');
        
//        try {
//            loadingIndicator.classList.remove('d-none');
//            productGrid.style.opacity = '0.5';

//            const response = await fetch(`/Home/ProductsByCategory?categoryId=${categoryId}`);
//            const products = await response.json();

//            if (products && products.length > 0) {
//                updateProductGrid(products, `${categoryName} Products`);
//            } else {
//                showNoProductsMessage(categoryName);
//            }
//        } catch (error) {
//            console.error('Error filtering products:', error);
//            showErrorMessage();
//        } finally {
//            loadingIndicator.classList.add('d-none');
//            productGrid.style.opacity = '1';
//        }
//    }

//    // Update product grid with new data
//    function updateProductGrid(products, title) {
//        const productGrid = document.getElementById('productGrid');
//        const sectionTitle = document.querySelector('#products h2');
        
//        sectionTitle.textContent = title;
        
//        productGrid.innerHTML = products.map(product => `
//            <div class="col-lg-3 col-md-6 mb-4">
//                <div class="card product-card h-100 border-0 shadow-sm">
//                    <div class="position-relative">
//                        <img src="${product.imageUrl || 'https://images.unsplash.com/photo-1574323347407-f5e1ad6d020b?w=400'}"
//                             class="card-img-top" alt="${product.productName}" 
//                             style="height: 200px; object-fit: cover;">
                        
//                        ${product.isOutOfStock ? `
//                        <div class="position-absolute inset-0 bg-dark bg-opacity-50 d-flex align-items-center justify-content-center" style="top: 0; left: 0; right: 0; bottom: 80px;">
//                            <span class="text-white fw-medium">Out of Stock</span>
//                        </div>` : ''}
                        
//                        ${product.isLowStock && !product.isOutOfStock ? `
//                        <span class="badge bg-warning position-absolute top-0 start-0 m-2">Low Stock</span>` : ''}
                        
//                        <button class="btn btn-light position-absolute top-0 end-0 m-2 rounded-circle">
//                            <i class="bi bi-heart"></i>
//                        </button>
//                    </div>
//                    <div class="card-body">
//                        <h6 class="card-title">${product.productName}</h6>
//                        <div class="d-flex align-items-center mb-2">
//                            <div class="star-rating me-2">
//                                <i class="bi bi-star-fill"></i>
//                                <i class="bi bi-star-fill"></i>
//                                <i class="bi bi-star-fill"></i>
//                                <i class="bi bi-star-fill"></i>
//                                <i class="bi bi-star"></i>
//                            </div>
//                            <small class="text-muted">(★ 4.2)</small>
//                        </div>
//                        <div class="mb-2">
//                            <small class="text-muted">Stock: ${product.stock} units</small>
//                        </div>
//                        <div class="d-flex align-items-center gap-2 mb-3">
//                            <span class="h5 text-primary-custom mb-0">₹${Math.floor(Math.random() * 2800) + 199}</span>
//                            <span class="text-muted text-decoration-line-through">₹${Math.floor(Math.random() * 3800) + 299}</span>
//                        </div>
//                    </div>
//                    <div class="card-footer bg-transparent border-0">
//                        ${product.isOutOfStock ? `
//                        <button class="btn btn-secondary w-100" disabled>
//                            <i class="bi bi-cart me-2"></i>Out of Stock
//                        </button>` : `
//                        <button class="btn bg-primary-custom text-white w-100" onclick="addToCart(${product.productId})">
//                            <i class="bi bi-cart me-2"></i>Add to Cart
//                        </button>`}
//                    </div>
//                </div>
//            </div>
//        `).join('');
        
//        // Scroll to products section
//        document.getElementById('products').scrollIntoView({ behavior: 'smooth' });
//    }

//    // Show message when no products found
//    function showNoProductsMessage(categoryName) {
//        const productGrid = document.getElementById('productGrid');
//        const sectionTitle = document.querySelector('#products h2');
        
//        sectionTitle.textContent = `${categoryName} Products`;
//        productGrid.innerHTML = `
//            <div class="col-12 text-center py-5">
//                <i class="bi bi-inbox display-1 text-muted mb-3"></i>
//                <h4 class="text-muted">No products found in ${categoryName}</h4>
//                <p class="text-muted">Try browsing other categories or check back later.</p>
//                <button class="btn btn-primary" onclick="loadAllProducts()">View All Products</button>
//            </div>
//        `;
//    }

//    // Show error message
//    function showErrorMessage() {
//        const productGrid = document.getElementById('productGrid');
//        productGrid.innerHTML = `
//            <div class="col-12 text-center py-5">
//                <i class="bi bi-exclamation-triangle display-1 text-warning mb-3"></i>
//                <h4 class="text-muted">Unable to load products</h4>
//                <p class="text-muted">Please check your connection and try again.</p>
//                <button class="btn btn-primary" onclick="window.location.reload()">Refresh Page</button>
//            </div>
//        `;
//    }

//    // Load all products
//    function loadAllProducts() {
//        window.location.reload();
//    }

//    // Add to cart functionality
//    function addToCart(productId) {
//        if (!@Json.Serialize(isLoggedIn)) {
//            alert('Please login to add items to cart');
//            window.location.href = '/Login';
//            return;
//        }
        
//        // Simulate add to cart
//        const button = event.target.closest('button');
//        const originalText = button.innerHTML;
        
//        button.innerHTML = '<i class="bi bi-check-circle me-2"></i>Added!';
//        button.disabled = true;
//        button.classList.remove('bg-primary-custom');
//        button.classList.add('btn-success');
        
//        setTimeout(() => {
//            button.innerHTML = originalText;
//            button.disabled = false;
//            button.classList.add('bg-primary-custom');
//            button.classList.remove('btn-success');
//        }, 2000);
        
//        console.log(`Product ${productId} added to cart`);
//    }

//    // Newsletter subscription
//    function subscribeNewsletter() {
//        const email = document.getElementById('newsletterEmail').value;
//        if (!email) {
//            alert('Please enter your email address');
//            return;
//        }
        
//        // Simulate newsletter subscription
//        alert('Thank you for subscribing! You will receive farming tips and updates.');
//        document.getElementById('newsletterEmail').value = '';
//    }

//    // Load products on page load
//    document.addEventListener('DOMContentLoaded', function() {
//        console.log('Dashboard loaded with:');
//        console.log('- Products:', @Json.Serialize(Model.TotalProducts));
//        console.log('- Categories:', @Json.Serialize(Model.TotalCategories));
//        console.log('- Featured Products:', @Json.Serialize(Model.FeaturedProducts.Count));
        
//        @if (TempData["Success"] != null)
//        {
//            <text>
//            // Show success message with animation
//            const successAlert = document.createElement('div');
//            successAlert.className = 'alert alert-success alert-dismissible fade show position-fixed';
//            successAlert.style.cssText = 'top: 20px; right: 20px; z-index: 1050; min-width: 300px;';
//            successAlert.innerHTML = `
//                <i class="bi bi-check-circle-fill me-2"></i>@TempData["Success"]
//                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
//            `;
//            document.body.appendChild(successAlert);
            
//            setTimeout(() => {
//                if (successAlert.parentNode) {
//                    successAlert.remove();
//                }
//            }, 5000);
//            </text>
//        }
//    });
//</script>