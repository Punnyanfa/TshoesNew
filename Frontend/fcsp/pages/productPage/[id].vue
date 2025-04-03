<!-- Product Detail Page -->
<template>
  <div class="product-detail-container">
    <Header />
    
    <div class="container py-5">
      <div v-if="loading" class="text-center">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>
      
      <div v-else class="row">
        <!-- Product Image Section -->
        <div class="col-md-6">
          <div class="product-image-container">
            <img 
              :src="product.previewImageUrl" 
              :alt="product.name" 
              class="main-product-image"
            />
          </div>
          
          <!-- Thumbnail Gallery -->
          <div class="thumbnail-gallery mt-4">
            <div class="row g-2">
              <div class="col-3" v-for="(img, index) in product.gallery" :key="index">
                <img 
                  :src="img" 
                  :alt="`${product.name} view ${index + 1}`"
                  class="thumbnail-image"
                  @click="changeMainImage(img)"
                />
              </div>
            </div>
          </div>
        </div>

        <!-- Product Info Section -->
        <div class="col-md-6">
          <div class="product-info">
            <h1 class="product-title">{{ product.name }}</h1>
            <div class="product-meta">
              <span class="product-rating">
                <i class="bi bi-star-fill text-warning"></i>
                {{ product.rating }} ({{ product.ratingCount }} reviews)
              </span>
              <span class="product-stock" :class="{ 'in-stock': product.inStock }">
                {{ product.inStock ? 'In Stock' : 'Out of Stock' }}
              </span>
            </div>
            
            <div class="product-price">
              <span class="current-price">${{ product.price }}</span>
              <span class="original-price" v-if="product.originalPrice">{{ product.originalPrice }}</span>
              <span class="discount-badge" v-if="product.discount">{{ product.discount }}% OFF</span>
            </div>

            <div class="product-description mt-4">
              <h3>Description</h3>
              <p>{{ product.description }}</p>
            </div>

            <div class="product-options mt-4">
              <div class="size-options">
                <h4>Select Size</h4>
                <div class="size-buttons">
                  <button 
                    v-for="size in product.sizes" 
                    :key="size"
                    class="size-btn"
                    :class="{ 'selected': selectedSize === size }"
                    @click="selectedSize = size"
                  >
                    {{ size }}
                  </button>
                </div>
              </div>

              <div class="color-options mt-4">
                <h4>Select Color</h4>
                <div class="color-buttons">
                  <button 
                    v-for="color in product.colors" 
                    :key="color"
                    class="color-btn"
                    :class="{ 'selected': selectedColor === color }"
                    :style="{ backgroundColor: color }"
                    @click="selectedColor = color"
                  ></button>
                </div>
              </div>
            </div>

            <div class="product-actions mt-4">
              <div class="quantity-selector">
                <button 
                  class="quantity-btn"
                  @click="decreaseQuantity"
                  :disabled="quantity <= 1"
                >-</button>
                <input 
                  type="number" 
                  v-model="quantity" 
                  min="1" 
                  class="quantity-input"
                />
                <button 
                  class="quantity-btn"
                  @click="increaseQuantity"
                  :disabled="quantity >= product.maxQuantity"
                >+</button>
              </div>

              <button 
                class="add-to-cart-btn"
                @click="addToCart"
                :disabled="!product.inStock"
              >
                <i class="bi bi-cart-plus"></i> Add to Cart
              </button>
            </div>

            <div class="product-additional mt-4">
              <div class="delivery-info">
                <i class="bi bi-truck"></i>
                <span>Free shipping on orders over $100</span>
              </div>
              <div class="return-info">
                <i class="bi bi-arrow-repeat"></i>
                <span>30-day easy returns</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Product Details Tabs -->
      <div class="product-tabs mt-5">
        <ul class="nav nav-tabs" id="productTabs" role="tablist">
          <li class="nav-item" role="presentation">
            <button 
              class="nav-link active" 
              id="details-tab" 
              data-bs-toggle="tab" 
              data-bs-target="#details" 
              type="button" 
              role="tab"
            >
              Product Details
            </button>
          </li>
          <li class="nav-item" role="presentation">
            <button 
              class="nav-link" 
              id="specs-tab" 
              data-bs-toggle="tab" 
              data-bs-target="#specs" 
              type="button" 
              role="tab"
            >
              Specifications
            </button>
          </li>
          <li class="nav-item" role="presentation">
            <button 
              class="nav-link" 
              id="reviews-tab" 
              data-bs-toggle="tab" 
              data-bs-target="#reviews" 
              type="button" 
              role="tab"
            >
              Reviews
            </button>
          </li>
        </ul>

        <div class="tab-content" id="productTabsContent">
          <div class="tab-pane fade show active" id="details" role="tabpanel">
            <div class="p-4">
              <h4>Product Details</h4>
              <p>{{ product.detailedDescription }}</p>
            </div>
          </div>
          <div class="tab-pane fade" id="specs" role="tabpanel">
            <div class="p-4">
              <h4>Specifications</h4>
              <ul class="specs-list">
                <li v-for="(value, key) in product.specifications" :key="key">
                  <strong>{{ key }}:</strong> {{ value }}
                </li>
              </ul>
            </div>
          </div>
          <div class="tab-pane fade" id="reviews" role="tabpanel">
            <div class="p-4">
              <h4>Customer Reviews</h4>
              <div class="reviews-list">
                <div v-for="review in product.reviews" :key="review.id" class="review-item">
                  <div class="review-header">
                    <span class="reviewer-name">{{ review.name }}</span>
                    <span class="review-date">{{ review.date }}</span>
                  </div>
                  <div class="review-rating">
                    <i v-for="n in 5" :key="n" 
                       class="bi" 
                       :class="n <= review.rating ? 'bi-star-fill text-warning' : 'bi-star'">
                    </i>
                  </div>
                  <p class="review-content">{{ review.content }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Related Products -->
      <div class="related-products mt-5">
        <h3>Related Products</h3>
        <div class="row g-4">
          <div class="col-md-3" v-for="relatedProduct in relatedProducts" :key="relatedProduct.id">
            <div class="related-product-card">
              <img :src="relatedProduct.image" :alt="relatedProduct.name" class="img-fluid" />
              <h5>{{ relatedProduct.name }}</h5>
              <p class="price">{{ relatedProduct.price }}</p>
              <NuxtLink :to="`/productPage/${relatedProduct.id}`" class="btn btn-outline-primary">
                View Details
              </NuxtLink>
            </div>
          </div>
        </div>
      </div>
    </div>

    <Footer />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useCartStore } from '~/stores/cart';
import { ElNotification } from 'element-plus';
import { getProductById, mockGetRelatedProducts } from '~/server/product-service';

const route = useRoute();
const router = useRouter();
const cartStore = useCartStore();

// State
const product = ref({
  id: null,
  name: '',
  previewImageUrl: '',
  rating: 0,
  ratingCount: 0,
  price: 0
});

const relatedProducts = ref([]);
const selectedSize = ref('');
const selectedColor = ref('');
const quantity = ref(1);
const loading = ref(true);

// Methods
const changeMainImage = (newImage) => {
  product.value.previewImageUrl = newImage;
};

const increaseQuantity = () => {
  if (quantity.value < product.value.maxQuantity) {
    quantity.value++;
  }
};

const decreaseQuantity = () => {
  if (quantity.value > 1) {
    quantity.value--;
  }
};

const addToCart = () => {
  if (!selectedSize.value || !selectedColor.value) {
    ElNotification({
      title: 'Warning',
      message: 'Please select size and color',
      type: 'warning'
    });
    return;
  }

  const productToAdd = {
    id: product.value.id,
    name: product.value.name,
    price: product.value.price,
    image: product.value.previewImageUrl,
    size: selectedSize.value,
    color: selectedColor.value,
    quantity: quantity.value
  };

  console.log('Adding to cart:', productToAdd);
  cartStore.addItem(productToAdd);

  ElNotification({
    title: 'Success',
    message: 'Product added to cart successfully!',
    type: 'success'
  });
};

const fetchProductDetails = async () => {
  try {
    loading.value = true;
    const productId = route.params.id;
    console.log('Fetching product with ID:', productId);
    
    const response = await getProductById(productId);
    if (response.data) {
      const productData = response.data;
      product.value = {
        id: productData.id,
        name: productData.name || 'Unknown Product',
        previewImageUrl: productData.previewImageUrl || '/default-shoe.jpg',
        gallery: productData.gallery || [],
        rating: productData.rating || 0,
        ratingCount: productData.ratingCount || 0,
        price: productData.price || 0,
        description: productData.description || 'No description available',
        inStock: productData.inStock !== undefined ? productData.inStock : true,
        sizes: productData.sizes || ['36', '37', '38', '39', '40', '41', '42', '43', '44'],
        colors: productData.colors || ['#000000', '#FFFFFF', '#FF0000', '#0000FF'],
        maxQuantity: productData.maxQuantity || 10,
        specifications: productData.specifications || {},
        detailedDescription: productData.detailedDescription || 'No detailed description available',
        reviews: productData.reviews || []
      };
      console.log('Product data:', product.value);
    } else {
      throw new Error('Invalid product data received');
    }
  } catch (error) {
    console.error('Error fetching product:', error);
    ElNotification({
      title: 'Error',
      message: 'Failed to load product details',
      type: 'error'
    });
    router.push('/productPage');
  } finally {
    loading.value = false;
  }
};

const fetchRelatedProducts = async () => {
  try {
    const productId = route.params.id;
    const data = await mockGetRelatedProducts(productId);
    relatedProducts.value = data.map(product => ({
      ...product,
      id: product._id,
      image: product.previewImageUrl
    }));
  } catch (error) {
    console.error('Error fetching related products:', error);
  }
};

// Lifecycle hooks
onMounted(async () => {
  await Promise.all([
    fetchProductDetails(),
    fetchRelatedProducts()
  ]);
});
</script>

<style scoped>
.product-detail-container {
  min-height: 100vh;
  background-color: #f8f9fa;
}

.product-image-container {
  position: relative;
  border-radius: 10px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
}

.main-product-image {
  width: 100%;
  height: auto;
  transition: transform 0.3s ease;
}

.main-product-image:hover {
  transform: scale(1.05);
}

.thumbnail-gallery {
  margin-top: 20px;
}

.thumbnail-image {
  width: 100%;
  height: 80px;
  object-fit: cover;
  border-radius: 5px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.thumbnail-image:hover {
  transform: scale(1.1);
  box-shadow: 0 4px 8px rgba(0,0,0,0.2);
}

.product-info {
  padding: 20px;
  background: white;
  border-radius: 10px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
}

.product-title {
  font-size: 2rem;
  font-weight: bold;
  margin-bottom: 15px;
}

.product-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.product-rating {
  display: flex;
  align-items: center;
  gap: 5px;
}

.product-stock {
  padding: 5px 10px;
  border-radius: 20px;
  font-size: 0.9rem;
}

.in-stock {
  background-color: #e8f5e9;
  color: #2e7d32;
}

.product-price {
  display: flex;
  align-items: center;
  gap: 15px;
  margin-bottom: 20px;
}

.current-price {
  font-size: 1.8rem;
  font-weight: bold;
  color: #2c3e50;
}

.original-price {
  text-decoration: line-through;
  color: #999;
}

.discount-badge {
  background-color: #ff5722;
  color: white;
  padding: 5px 10px;
  border-radius: 20px;
  font-size: 0.9rem;
}

.size-buttons, .color-buttons {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.size-btn {
  padding: 8px 16px;
  border: 1px solid #ddd;
  border-radius: 5px;
  background: white;
  cursor: pointer;
  transition: all 0.3s ease;
}

.size-btn.selected {
  background: #2c3e50;
  color: white;
  border-color: #2c3e50;
}

.color-btn {
  width: 30px;
  height: 30px;
  border-radius: 50%;
  border: 2px solid #ddd;
  cursor: pointer;
  transition: all 0.3s ease;
}

.color-btn.selected {
  border-color: #2c3e50;
  transform: scale(1.1);
}

.product-actions {
  display: flex;
  gap: 20px;
  margin-top: 30px;
}

.quantity-selector {
  display: flex;
  align-items: center;
  border: 1px solid #ddd;
  border-radius: 5px;
}

.quantity-btn {
  padding: 8px 16px;
  border: none;
  background: none;
  cursor: pointer;
}

.quantity-input {
  width: 60px;
  text-align: center;
  border: none;
  border-left: 1px solid #ddd;
  border-right: 1px solid #ddd;
  padding: 8px;
}

.add-to-cart-btn {
  flex: 1;
  padding: 12px 24px;
  background: linear-gradient(45deg, #2c3e50, #3498db);
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.add-to-cart-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0,0,0,0.2);
}

.add-to-cart-btn:disabled {
  background: #ccc;
  cursor: not-allowed;
}

.product-additional {
  display: flex;
  gap: 20px;
  margin-top: 30px;
  padding-top: 20px;
  border-top: 1px solid #ddd;
}

.delivery-info, .return-info {
  display: flex;
  align-items: center;
  gap: 10px;
}

.product-tabs {
  background: white;
  border-radius: 10px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
  margin-top: 40px;
}

.nav-tabs {
  border-bottom: 1px solid #ddd;
}

.nav-tabs .nav-link {
  color: #666;
  border: none;
  padding: 15px 25px;
  font-weight: 500;
}

.nav-tabs .nav-link.active {
  color: #2c3e50;
  border-bottom: 2px solid #2c3e50;
}

.tab-content {
  padding: 20px;
}

.specs-list {
  list-style: none;
  padding: 0;
}

.specs-list li {
  padding: 10px 0;
  border-bottom: 1px solid #eee;
}

.review-item {
  padding: 20px 0;
  border-bottom: 1px solid #eee;
}

.review-header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 10px;
}

.reviewer-name {
  font-weight: bold;
}

.review-date {
  color: #666;
}

.review-rating {
  margin-bottom: 10px;
}

.related-product-card {
  background: white;
  border-radius: 10px;
  padding: 15px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
  transition: all 0.3s ease;
}

.related-product-card:hover {
  transform: translateY(-5px);
}

.related-product-card img {
  width: 100%;
  height: 200px;
  object-fit: cover;
  border-radius: 5px;
  margin-bottom: 15px;
}

.related-product-card h5 {
  margin-bottom: 10px;
}

.related-product-card .price {
  color: #2c3e50;
  font-weight: bold;
  margin-bottom: 15px;
}

@media (max-width: 768px) {
  .product-actions {
    flex-direction: column;
  }
  
  .quantity-selector {
    width: 100%;
  }
  
  .add-to-cart-btn {
    width: 100%;
  }
  
  .product-additional {
    flex-direction: column;
  }
}
</style> 