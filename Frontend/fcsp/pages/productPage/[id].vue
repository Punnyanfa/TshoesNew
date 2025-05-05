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
          <div v-if="product.texturesUrls && product.texturesUrls.length > 0" class="mb-4">
            <div class="card shadow-sm">
              <img :src="product.texturesUrls[selectedTextureIndex]" 
                   :alt="product.name" 
                   class="card-img-top img-fluid texture-image">
            </div>
            <!-- Texture thumbnails -->
            <div class="d-flex flex-wrap mt-3 gap-2">
              <div v-for="(url, index) in product.texturesUrls" :key="index" 
                   class="texture-thumbnail" 
                   :class="{'active-thumbnail': selectedTextureIndex === index}"
                   @click="selectedTextureIndex = index">
                <img :src="url" :alt="'Texture ' + (index + 1)" class="img-thumbnail">
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
              <p class="card-text text-muted">{{ product.description }}</p>
              <h3 class="mb-4 fw-bold text-primary">
                {{ product.price !== undefined ? '$' + formatPrice(product.price) : 'N/A' }}
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
                <div v-if="product.texturesUrls && product.texturesUrls.length > 0" class="small text-muted">
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
    <Footer />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { getProductById } from '~/server/product-service';
import { useCart } from '~/composables/useCart';

const route = useRoute();
const router = useRouter();
const { updateCartCount } = useCart();
const product = ref(null);
const loading = ref(true);
const selectedSize = ref('');
const selectedTextureIndex = ref(0);
const selectedQuantity = ref(1);

// Fetch product details from API
const fetchProduct = async () => {
  try {
    const productId = route.params.id;
    const response = await getProductById(productId);
    product.value = response;
    console.log('Template data:', product.value);
  } catch (error) {
    console.error('Error fetching product:', error);
  } finally {
    loading.value = false;
  }
};

// Fetch product data when component is mounted
onMounted(fetchProduct);

// Modified addToCart function to save to sessionStorage
const addToCart = () => {
  if (product.value && selectedSize.value && selectedQuantity.value > 0) {
    // Determine the image URL to store
    let imageUrl = '/placeholder.png'; // Default placeholder
    if (product.value.texturesUrls && product.value.texturesUrls.length > 0) {
      imageUrl = product.value.texturesUrls[selectedTextureIndex.value];
    } else if (product.value.previewImageUrl) { 
      imageUrl = product.value.previewImageUrl;
    }

    const productToAdd = {
      id: product.value.id,
      name: product.value.name,
      description: product.value.description,
      price: product.value.price,
      selectedSize: selectedSize.value,
      selectedQuantity: selectedQuantity.value,
      previewImageUrl: imageUrl // Use the determined image URL
    };

    try {
      // Get existing cart from sessionStorage
      let cart = JSON.parse(sessionStorage.getItem('cart') || '[]');

      // Check if item with same ID and size already exists
      const existingItemIndex = cart.findIndex(item =>
        item.id === productToAdd.id && item.selectedSize === productToAdd.selectedSize
      );

      if (existingItemIndex > -1) {
        // Update quantity if item exists
        cart[existingItemIndex].selectedQuantity += productToAdd.selectedQuantity;
      } else {
        // Add new item if it doesn't exist
        cart.push(productToAdd);
      }

      // Save updated cart back to sessionStorage
      sessionStorage.setItem('cart', JSON.stringify(cart));

      // Update cart count
      updateCartCount(cart.length);

      console.log('Product added to cart:', productToAdd);
      console.log('Updated cart stored in session:', cart);

      // Optional: Navigate to the shopping cart page
      router.push('/shoppingCartPage');

    } catch (error) {
      console.error('Error adding product to cart:', error);
      // Optional: Show error message to user
    }
  } else {
     console.warn('Please select size and quantity before adding to cart.');
     // Optional: Show warning message to user
  }
};

const formatPrice = (price) => {
  if (typeof price !== 'number') return 'N/A';
  return price.toFixed(2);
};
</script>

<style scoped>
.product-detail {
  background: #fff;
  min-height: 100vh;
  font-family: 'Poppins', sans-serif;
}

.texture-image {
  border-radius: 8px;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.texture-thumbnail {
  width: 60px;
  height: 60px;
  cursor: pointer;
  overflow: hidden;
  border-radius: 4px;
  transition: all 0.2s ease;
}

.texture-thumbnail img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.active-thumbnail {
  border: 2px solid #2c3e50;
  transform: scale(1.05);
}

.size-label {
  display: inline-block;
  padding: 8px 16px;
  border: 1px solid #dee2e6;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s ease;
  text-align: center;
  min-width: 45px;
}

.size-label:hover {
  background-color: #f8f9fa;
}

.size-label.active {
  background-color: #2c3e50;
  color: white;
  border-color: #2c3e50;
}

.btn-sneaker {
  background: linear-gradient(45deg, #2c3e50, #3498db);
  color: #fff;
  border: none;
  border-radius: 8px;
  transition: background 0.3s ease, transform 0.2s ease;
}

.btn-sneaker:hover {
  background: linear-gradient(45deg, #2c3e50 20%, #2980b9);
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.btn-sneaker:disabled {
  background: #6c757d;
  transform: none;
}
</style>