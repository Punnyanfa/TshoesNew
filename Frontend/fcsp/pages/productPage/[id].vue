<template>
  <div class="product-detail">
    <Header />
    <section class="container my-5">
      <div v-if="loading" class="text-center py-5">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>

      <!-- Product details -->
      <div v-else class="row">
        <div class="col-md-6">
          <!-- Product images/textures -->
          <div v-if="product.previewImages && product.previewImages.length > 0" class="product-image-section mb-4">
            <div class="d-flex gap-3">
              <!-- Texture thumbnails -->
              <div class="texture-thumbnail-list d-flex flex-column gap-2">
                <div v-for="(url, index) in product.previewImages" :key="index" 
                     class="texture-thumbnail" 
                     :class="{'active-thumbnail': selectedTextureIndex === index}"
                     @click="selectedTextureIndex = index">
                  <img :src="url" :alt="'Texture ' + (index + 1)" class="img-thumbnail">
                </div>
              </div>
              <!-- Main product image -->
              <div class="card shadow-sm flex-grow-1">
                <img :src="product.previewImages[selectedTextureIndex]" 
                     :alt="product.name" 
                     class="card-img-top img-fluid texture-image">
              </div>
            </div>
          </div>
          <div v-else class="text-center p-5 bg-light rounded">
            <p class="mb-0">No images available for this product.</p>
          </div>
        </div>

        <div class="col-md-6">
          <div class="card border-0">
            <div class="card-body">
              <h1 class="card-title fw-bold">{{ product.name }}</h1>
              <p class="card-text text-muted">
                {{ [undefined, null, '', 'undefined'].includes(product?.description) ? 'stylish comfort that keeps you moving with confidence' : product?.description }}
              </p>
              <h3 class="mb-4 fw-bold text-primary">
                {{ product.price !== undefined ?  formatPrice(product.price) + ' đ' : 'N/A' }}
              </h3>
              
              <!-- Size selection -->
              <div class="mb-4">
                <h5 class="mb-3">Size:</h5>
                <div class="d-flex flex-wrap gap-2">
                  <div v-for="size in product.sizes" :key="size.id" class="form-check form-check-inline">
                    <input class="form-check-input visually-hidden" type="radio" 
                           :id="'size-' + size.id" 
                           :value="size.sizeValue" 
                           v-model="selectedSize"
                           name="productSize">
                    <label class="form-check-label size-label" :class="{'active': selectedSize === size.sizeValue}" :for="'size-' + size.id">
                      {{ size.sizeValue }}
                    </label>
                  </div>
                </div>
                <div v-if="selectedSize" class="mt-2 text-success">
                  Selected size: {{ selectedSize }}
                </div>
              </div>
              <div class="mb-4">
                <h5 class="mb-3">Quantity:</h5>
                <input type="number" v-model="selectedQuantity" min="1" class="form-control w-25" />
              </div>
              <!-- Textures section (moved to sidebar in mobile view) -->
              <div class="d-md-none mb-4">
                <h5>Textures:</h5>
                <div v-if="product.previewImages && product.previewImages.length > 0" class="small text-muted">
                  Click on an image above to view different textures.
                </div>
                <div v-else>
                  <p class="small text-muted">No textures available for this product.</p>
                </div>
              </div>
              
              <!-- Add to cart button -->
              <div class="d-grid gap-2">
                <button class="btn btn-sneaker py-3 fw-bold text-uppercase" 
                        @click="addToCart"
                        :disabled="!selectedSize">
                  Add to Cart
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Reviews Section -->
    <section class="container mb-5">
      <div class="card border-0">
        <div class="card-body">
          <h2 class="card-title mb-4">Customer Reviews</h2>
          
          <!-- Reviews List -->
          <div class="reviews-list">
            <div v-if="reviews.length === 0" class="text-center py-4">
              <p class="text-muted">No reviews yet.</p> <!-- Simplified message -->
            </div>
            <div v-else v-for="review in reviews" :key="review.id" class="review-item mb-4">
              <div class="d-flex justify-content-between align-items-start">
                <div>
                  <h5 class="mb-1">{{ review.userName }}</h5> <!-- Display user name as title -->
                  <div class="rating-stars mb-2">
                    <span v-for="star in 5" :key="star" 
                          class="star" 
                          :class="{ 'active': review.userRating >= star }">★</span> <!-- Use userRating -->
                  </div>
                </div>
                <small class="text-muted">{{ formatDate(review.createdAt) }}</small> <!-- Use createdAt -->
              </div>
              <p class="review-content mb-2">{{ review.comment }}</p> <!-- Use comment -->
              <!-- Remove review-meta as it's redundant with user name displayed -->
              <!-- <div class="review-meta">
                <small class="text-muted">By {{ review.userName }}</small>
              </div> -->
            </div>
          </div>
        </div>
      </div>
    </section>
    <Footer />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { getProductById } from '~/server/product-service';
import { useCart } from '~/composables/useCart';
import { getRatingStatsByShoeDesignId } from '~/server/rating-service';

const route = useRoute();
const router = useRouter();
const { updateCartCount } = useCart();
const product = ref(null);
const loading = ref(true);
const selectedSize = ref('');
const selectedTextureIndex = ref(3);
const selectedQuantity = ref(1);

const reviews = ref([]);

// Fetch product details and reviews from API
const fetchProduct = async () => {
  try {
    const productId = route.params.id;
    const productResponse = await getProductById(productId);
    product.value = productResponse;
    console.log("Product value after assignment:", product.value);
    console.log("Description value:", product.value?.description);

    // Fetch reviews after product details are loaded, using the product ID from the route
    if (productId) { // Ensure productId exists
      const reviewsResponse = await getRatingStatsByShoeDesignId(productId); // Use productId directly
      console.log("Fetched reviews response using product ID:", reviewsResponse); // Updated log
      // Assuming reviews are in reviewsResponse.rating based on the image
      if (reviewsResponse && reviewsResponse.rating) {
         reviews.value = reviewsResponse.rating;
      } else {
         console.error("Reviews data not found in response or response format unexpected:", reviewsResponse); // Updated error message
         reviews.value = []; // Ensure reviews is an empty array if data is missing
      }
    } else {
       console.warn("Product ID is undefined, cannot fetch reviews.");
       reviews.value = [];
    }

  } catch (error) {
    console.error('Error fetching product or reviews:', error);
  } finally {
    loading.value = false;
  }
};

// Fetch data when component is mounted
onMounted(fetchProduct);

const formatPrice = (price) => {
  if (typeof price !== 'number') return 'N/A';
  return price.toLocaleString('vi-VN');
};

const formatDate = (dateString) => {
  if (!dateString) return '';
  return new Date(dateString).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });
};

const addToCart = () => {
  if (product.value && selectedSize.value && selectedQuantity.value > 0) {
    let imageUrl = '/placeholder.png';
    if (product.value.previewImages && product.value.previewImages.length > 0) {
      imageUrl = product.value.previewImages[selectedTextureIndex.value];
    } else if (product.value.previewImageUrl) { 
      imageUrl = product.value.previewImageUrl;
    }

    const productToAdd = {
      id: product.value.id,
      name: product.value.name,
      description: product.value.description||'stylish comfort that keeps you moving with confidence',
      price: product.value.price,
      selectedSize: selectedSize.value,
      selectedQuantity: selectedQuantity.value,
      previewImageUrl: imageUrl
    };

    try {
      const userId = localStorage.getItem("userId");
      let cart = JSON.parse(localStorage.getItem(`cart_${userId}`) || '[]');
    
      const existingItemIndex = cart.findIndex(item =>
        item.id === productToAdd.id && item.selectedSize === productToAdd.selectedSize
      );

      if (existingItemIndex > -1) {
        cart[existingItemIndex].selectedQuantity += productToAdd.selectedQuantity;
      } else {
        cart.push(productToAdd);
      }

      localStorage.setItem(`cart_${userId}`, JSON.stringify(cart));

      updateCartCount(cart.length);

      console.log('Product added to cart:', productToAdd);
      console.log('Updated cart stored in localStorage:', cart);

      router.push('/shoppingCartPage');

    } catch (error) {
      console.error('Error adding product to cart:', error);
    }
  } else {
     console.warn('Please select size and quantity before adding to cart.');
  }
};
</script>

<style scoped>
.product-detail {
  background: linear-gradient(135deg, #f5f7fa 0%, #e4e9f2 100%);
  min-height: 100vh;
  font-family: 'Poppins', sans-serif;
  position: relative;
}

.product-detail::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: radial-gradient(circle, rgba(255, 255, 255, 0.1) 10%, transparent 10%);
  background-size: 30px 30px;
  opacity: 0.5;
  z-index: 0;
}

.container {
  position: relative;
  z-index: 1;
}

.card {
  background: #ffffff;
  border-radius: 15px;
  box-shadow: 0 15px 40px rgba(0, 0, 0, 0.15);
  border: 1px solid rgba(0, 0, 0, 0.05);
  animation: fadeInUp 0.5s ease;
}

.card-title {
  font-size: 2.5rem;
  font-weight: 800;
  background: #AAAAAA;
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  letter-spacing: 1px;
  transition: transform 0.3s ease;
}

/* Product images/textures section */
.product-image-section {
  margin-bottom: 1.5rem; /* Add some space below the image section */
}

.card.shadow-sm {
  padding: 15px;
  position: relative;
  width: 100%;
  height: 545px;
  padding-top: 70%; /* Duy trì tỷ lệ khung hình ban đầu hoặc điều chỉnh nếu cần */
  overflow: hidden;
  border-radius: 15px;
  box-shadow: 0 15px 40px rgba(0, 0, 0, 0.15);
}

.card.shadow-sm .card-body {
  /* This element seems redundant with absolute positioned image, can potentially simplify HTML */
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  padding: 0;
  display: flex;
  justify-content: center;
  align-items: center;
}

.texture-image {
  border-radius: 12px;
  object-fit: cover;
  position: absolute;
  top: 15px;
  left: 15px;
  bottom: auto;
  right: auto;
  width: calc(100% - 30px);
  height: calc(100% - 30px);
  transform: none;
  max-width: none;
  max-height: none;
}

/* Thumbnail section */
.texture-thumbnail-list {
    display: flex;
    flex-direction: column;
    gap: 8px;
    width: 80px;
}

.texture-thumbnail {
  width: 80px;
  height: 80px;
  cursor: pointer;
  overflow: hidden;
  border-radius: 8px;
  transition: all 0.3s ease;
  box-shadow: 0 3px 10px rgba(0, 0, 0, 0.1);
  border: 1px solid #e0e0e0;
  flex-shrink: 0;
}

.texture-thumbnail img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.active-thumbnail {
  border-color: #AAAAAA;
  box-shadow: 0 5px 15px rgba(170, 170, 170, 0.3);
}

.size-label {
  display: inline-block;
  padding: 8px 16px;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
  text-align: center;
  min-width: 45px;
  background: #f9f9f9;
  margin-right: 5px; /* Added spacing between size labels */
  margin-bottom: 5px; /* Added spacing for wrapped items */
}

.size-label:hover {
  background: #f0f0f0;
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
}

.size-label.active {
  background: #AAAAAA;
  color: white;
  border-color: #AAAAAA;
  box-shadow: 0 5px 15px rgba(170, 170, 170, 0.3);
}

.form-control {
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 12px 15px;
  background: #f9f9f9;
  font-size: 1rem;
  transition: all 0.3s ease;
  width: 100px; /* Set specific width for quantity input */
}

.form-control:focus {
  border-color: #AAAAAA;
  background: #fff;
  box-shadow: 0 0 10px rgba(170, 170, 170, 0.2);
  outline: none;
}

.btn-sneaker {
  background: #AAAAAA;
  color: #fff;
  border: none;
  border-radius: 8px;
  padding: 12px;
  font-weight: 600;
  font-size: 1.1rem;
  transition: all 0.3s ease;
  box-shadow: 0 5px 15px rgba(170, 170, 170, 0.3);
  width: 100%; /* Make button full width */
  margin-top: 20px; /* Added space above button */
}

.btn-sneaker:hover {
  background: #888888;
  transform: translateY(-3px);
  box-shadow: 0 8px 20px rgba(170, 170, 170, 0.4);
}

.btn-sneaker:active {
  transform: translateY(0);
  box-shadow: 0 3px 10px rgba(170, 170, 170, 0.2);
}

.btn-sneaker:disabled {
  background: #6c757d;
  transform: none;
  box-shadow: none;
}

.text-success {
  color: #AAAAAA !important;
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@media (max-width: 992px) {
  .card-title {
    font-size: 2rem;
  }

  .texture-thumbnail {
    width: 60px;
    height: 60px;
  }
  
  .texture-thumbnail-list {
    width: 60px;
  }
}

@media (max-width: 576px) {
  .card-title {
    font-size: 1.8rem;
  }

  .texture-thumbnail {
    width: 50px;
    height: 50px;
  }
  
  .texture-thumbnail-list {
    width: 50px;
  }
}

.d-flex.flex-wrap.mt-3.gap-2 {
    gap: 8px !important; /* Adjust gap for thumbnails */
}

.d-flex.flex-wrap.gap-2 {
    gap: 5px !important; /* Adjust gap for size labels */
}

.mb-4 {
    margin-bottom: 1.5rem !important; /* Standardize margin below sections */
}

/* Ensure column padding is consistent */
.row > .col-md-6 {
    padding: 0 15px;
}

.product-detail .row > .col-md-6:first-child > div:first-child,
.product-detail .row > .col-md-6:last-child > div:first-child {
  margin-top: 0 !important;
}

/* Reviews Section Styles */
.review-form {
  background: #f8f9fa;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
}

.rating-stars {
  display: flex;
  gap: 5px;
}

.star {
  font-size: 24px;
  color: #ddd;
  cursor: pointer;
  transition: color 0.2s ease;
}

.star.active {
  color: #ffd700;
}

.review-item {
  padding: 1.5rem;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
  transition: transform 0.2s ease;
}

.review-item:hover {
  transform: translateY(-2px);
}

.review-content {
  color: #555;
  line-height: 1.6;
}

.review-meta {
  border-top: 1px solid #eee;
  padding-top: 0.5rem;
  margin-top: 0.5rem;
}

.form-label {
  font-weight: 500;
  color: #555;
}

.form-control {
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 12px 15px;
  background: #fff;
  transition: all 0.3s ease;
}

.form-control:focus {
  border-color: #AAAAAA;
  box-shadow: 0 0 0 0.2rem rgba(170, 170, 170, 0.25);
}

@media (max-width: 768px) {
  .review-form {
    padding: 1.5rem;
  }
  
  .review-item {
    padding: 1rem;
  }
  
  .star {
    font-size: 20px;
  }
}
</style>