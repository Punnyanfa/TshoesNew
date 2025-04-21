<!-- Home Page Template - Custom Sneaker Theme -->
<template>
  <div class="home-wrapper">
    <!-- Header Component -->
    <Header />

    <!-- Hero Section -->
    <section class="hero-section text-center">
      <div class="container py-5">
        <h1 class="display-3 fw-bold text-sneaker-orange animate__animated animate__fadeInDown">
          Create Your Unique Style
        </h1>
        <p class="lead text-white mt-3 animate__animated animate__fadeInUp">
          Design unique sneakers with AI technology - Your style, your story.
        </p>
        <div class="mt-4 d-flex justify-content-center gap-3">
          <NuxtLink to="/customPage" class="btn btn-sneaker px-5 py-3 fw-bold text-uppercase animate__animated animate__zoomIn">
            Design Now
          </NuxtLink>
          <NuxtLink to="/gallery" class="btn btn-outline-light px-5 py-3 fw-bold text-uppercase animate__animated animate__zoomIn">
            Explore Collection
          </NuxtLink>
        </div>
        <div class="hero-img-container mt-5">
          <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s" alt="Custom Sneakers" class="img-fluid sneaker-hero-img animate__animated animate__pulse" />
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="container py-5">
      <h2 class="text-center fw-bold mb-5 text-sneaker-orange">Why Choose Us?</h2>
      <div class="row g-4 text-center">
        <div class="col-md-4">
          <div class="feature-box p-4 rounded">
            <i class="bi bi-gear-fill feature-icon"></i>
            <h4 class="mt-3 fw-bold">AI-Powered Design</h4>
            <p class="text-muted">Smart technology creates sneakers that perfectly match your style.</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="feature-box p-4 rounded">
            <i class="bi bi-brush-fill feature-icon"></i>
            <h4 class="mt-3 fw-bold">Unlimited Creativity</h4>
            <p class="text-muted">Customize every detail - colors, materials, and more.</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="feature-box p-4 rounded">
            <i class="bi bi-truck feature-icon"></i>
            <h4 class="mt-3 fw-bold">Fast Delivery</h4>
            <p class="text-muted">Receive your custom sneakers in the shortest time.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- How It Works Section -->
    <section class="how-it-works-section py-5 bg-light">
      <div class="container">
        <h2 class="text-center fw-bold mb-5 text-sneaker-orange">How It Works</h2>
        <div class="row g-4 text-center">
          <div class="col-md-3">
            <div class="step-box p-4">
              <span class="step-number">1</span>
              <h5 class="mt-3 fw-bold">Choose Base Design</h5>
              <p class="text-muted">Select a sneaker style that matches your taste.</p>
            </div>
          </div>
          <div class="col-md-3">
            <div class="step-box p-4">
              <span class="step-number">2</span>
              <h5 class="mt-3 fw-bold">Customize</h5>
              <p class="text-muted">Use AI tools to design every detail.</p>
            </div>
          </div>
          <div class="col-md-3">
            <div class="step-box p-4">
              <span class="step-number">3</span>
              <h5 class="mt-3 fw-bold">Preview & Order</h5>
              <p class="text-muted">View 3D design and place your order.</p>
            </div>
          </div>
          <div class="col-md-3">
            <div class="step-box p-4">
              <span class="step-number">4</span>
              <h5 class="mt-3 fw-bold">Wear Your Creation</h5>
              <p class="text-muted">Receive your unique sneakers and step out in style.</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Featured Designs Section -->
    <section class="featured-designs-section container py-5">
      <h2 class="text-center fw-bold mb-5 text-sneaker-orange">Featured Designs</h2>
      <div class="row g-4">
        <div class="col-md-4">
          <div class="design-card">
            <img src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=363&h=182&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Sneaker Design 1" class="img-fluid rounded" />
            <h5 class="mt-3 fw-bold">Neon Nights</h5>
            <p class="text-muted">Bold colors for the fearless.</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="design-card">
            <img src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=363&h=182&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Sneaker Design 2" class="img-fluid rounded" />
            <h5 class="mt-3 fw-bold">Classic Redux</h5>
            <p class="text-muted">Timeless style, modern twist.</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="design-card">
            <img src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=363&h=182&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Sneaker Design 3" class="img-fluid rounded" />
            <h5 class="mt-3 fw-bold">Urban Jungle</h5>
            <p class="text-muted">Rugged yet refined.</p>
          </div>
        </div>
      </div>
      <div class="text-center mt-4">
        <NuxtLink to="/productPage" class="btn btn-sneaker px-5 py-3 fw-bold text-uppercase">
          See More Designs
        </NuxtLink>
      </div>
    </section>

    <!-- Best Seller Sneakers Section -->
    <section class="best-sellers-section container py-5">
      <h2 class="text-center fw-bold mb-5 text-sneaker-orange">Best Selling Sneakers</h2>
      
      <Swiper
        v-bind="swiperOptions"
        class="best-seller-swiper"
      >
        <SwiperSlide v-for="product in products" :key="product.id">
          <div class="product-card" @click="navigateToProduct(product.id)">
            <div class="product-image position-relative">
              <img 
                :src="product.image" 
                :alt="product.name" 
                class="img-fluid rounded"
                @load="handleImageLoad(product.id)"
                :class="{ 'image-loaded': imageLoaded[product.id] }"
              />
              <div class="image-skeleton" v-if="!imageLoaded[product.id]"></div>
              <div class="product-overlay">
                <button 
                  class="btn btn-add-cart"
                  @click.stop="addToCart(product)"
                >
                </button>
              </div>
            </div>
            <div class="product-info mt-3">
              <h5 class="fw-bold mb-2">{{ product.name }}</h5>
              <div class="d-flex justify-content-between align-items-center">
                <span class="price fw-bold">{{ product.price }}</span>
                <span class="rating">
                  <i class="bi bi-star-fill text-warning"></i>
                  <span class="ms-1">{{ product.rating }} ({{ product.reviews }})</span>
                </span>
              </div>
            </div>
          </div>
        </SwiperSlide>

        <div class="swiper-button-next"></div>
        <div class="swiper-button-prev"></div>
        <div class="swiper-pagination"></div>
      </Swiper>

      <!-- Notification Toast -->
      <div 
        class="notification-toast"
        :class="{ 'show': showNotification }"
      >
        {{ notificationMessage }}
      </div>

      <div class="text-center mt-5">
        <NuxtLink to="/productPage" class="btn btn-sneaker px-5 py-3 fw-bold text-uppercase">
          View All Products
        </NuxtLink>
      </div>
    </section>

    <!-- Testimonials Section -->
    <section class="testimonials-section py-5 bg-light">
      <div class="container">
        <h2 class="text-center fw-bold mb-5 text-sneaker-orange">What Our Customers Say</h2>
        <div class="row g-4">
          <div class="col-md-4">
            <div class="testimonial-box p-4 rounded">
              <p class="text-muted">"The customization tool is amazing! My sneakers turned out exactly as I wanted."</p>
              <h6 class="mt-3 fw-bold">John D.</h6>
              <span class="text-sneaker-star">★★★★★</span>
            </div>
          </div>
          <div class="col-md-4">
            <div class="testimonial-box p-4 rounded">
              <p class="text-muted">"Fast delivery and perfect fit. I'm already designing my next pair!"</p>
              <h6 class="mt-3 fw-bold">Sarah K.</h6>
              <span class="text-sneaker-star">★★★★★</span>
            </div>
          </div>
          <div class="col-md-4">
            <div class="testimonial-box p-4 rounded">
              <p class="text-muted">"Hands down the best sneaker experience. Worth every penny."</p>
              <h6 class="mt-3 fw-bold">Mike L.</h6>
              <span class="text-sneaker-star">★★★★★</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Blog Teaser Section -->
    <section class="blog-teaser-section container py-5">
      <h2 class="text-center fw-bold mb-5 text-sneaker-orange">Sneaker Stories</h2>
      <div class="row g-4">
        <div class="col-md-6">
          <div class="blog-card d-flex">
            <img src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=363&h=182&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Blog Post 1" class="img-fluid rounded" style="max-width: 200px;" />
            <div class="ms-3">
              <h5 class="fw-bold">Custom Shoes: Company Swag That Gets People Talking</h5>
              <p class="text-muted">Your browser does not support our video. When it comes to company swag, businesses are always on the lookout for something unique, memorable, and practical. Custom shoes have emerged as a stan...</p>
              <NuxtLink to="/blog/custom-shoes-swag" class="text-sneaker-orange">Read More</NuxtLink>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="blog-card d-flex">
            <img src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=363&h=182&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Blog Post 2" class="img-fluid rounded" style="max-width: 200px;" />
            <div class="ms-3">
              <h5 class="fw-bold">Best Company Branded Shoes in 2025</h5>
              <p class="text-muted">As we step into 2025, branded shoes continue to dominate the market with their innovation, style, and functionality. The footwear industry has expanded its offerings, introducing products that cate...</p>
              <NuxtLink to="/blog/best-branded-shoes-2025" class="text-sneaker-orange">Read More</NuxtLink>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="blog-card d-flex">
            <img src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=363&h=182&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Blog Post 3" class="img-fluid rounded" style="max-width: 200px;" />
            <div class="ms-3">
              <h5 class="fw-bold">How Custom Shoes Can Reflect Company Values</h5>
              <p class="text-muted">We're proud to announce a bold direction in footwear that goes beyond simple style. When it comes to expressing who we are, custom shoes speak volumes. They help us show the world the things our co...</p>
              <NuxtLink to="/blog/company-values-shoes" class="text-sneaker-orange">Read More</NuxtLink>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="blog-card d-flex">
            <img src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=363&h=182&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Blog Post 4" class="img-fluid rounded" style="max-width: 200px;" />
            <div class="ms-3">
              <h5 class="fw-bold">The Rise of Sustainable Custom Shoes in 2025</h5>
              <p class="text-muted">Explore how eco-friendly materials and sustainable manufacturing are shaping the custom shoe industry. Highlight companies leading the charge and how businesses can align their brand with sustainability trends...</p>
              <NuxtLink to="/blog/sustainable-shoes-2025" class="text-sneaker-orange">Read More</NuxtLink>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="blog-card d-flex">
            <img src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=363&h=182&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Blog Post 5" class="img-fluid rounded" style="max-width: 200px;" />
            <div class="ms-3">
              <h5 class="fw-bold">Top 10 Companies Revolutionizing Custom Footwear Design</h5>
              <p class="text-muted">Showcase innovative companies using AI, 3D printing, or unique design techniques to create custom shoes. Include visuals of standout designs and interviews or quotes from industry leaders...</p>
              <NuxtLink to="/blog/top-companies-2025" class="text-sneaker-orange">Read More</NuxtLink>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="blog-card d-flex">
            <img src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=363&h=182&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Blog Post 6" class="img-fluid rounded" style="max-width: 200px;" />
            <div class="ms-3">
              <h5 class="fw-bold">How to Build Brand Loyalty with Custom Shoe Campaigns</h5>
              <p class="text-muted">Discuss strategies for companies to use custom shoes as a marketing tool to boost customer engagement and loyalty. Include case studies or examples of successful campaigns...</p>
              <NuxtLink to="/blog/brand-loyalty-campaigns" class="text-sneaker-orange">Read More</NuxtLink>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="blog-card d-flex">
            <img src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=363&h=182&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Blog Post 7" class="img-fluid rounded" style="max-width: 200px;" />
            <div class="ms-3">
              <h5 class="fw-bold">The Future of Custom Shoes: Predictions for 2026 and Beyond</h5>
              <p class="text-muted">Offer a forward-looking perspective on where the custom shoe industry is headed, including advancements in technology, design trends, and consumer preferences...</p>
              <NuxtLink to="/blog/future-shoes-2026" class="text-sneaker-orange">Read More</NuxtLink>
            </div>
          </div>
        </div>
      </div>
      <div class="text-center mt-4">
        <NuxtLink to="/aboutPage" class="btn btn-sneaker px-5 py-3 fw-bold text-uppercase">
          View All Blogs
        </NuxtLink>
      </div>
    </section>

    <!-- Call to Action Section -->
    <section class="cta-section text-center py-5">
      <div class="container">
        <h2 class="fw-bold text-white">Ready to Elevate Your Style?</h2>
        <p class="text-light mt-3">Join thousands of sneaker enthusiasts who have turned their dreams into reality.</p>
        <NuxtLink to="/customPage" class="btn btn-sneaker px-5 py-3 mt-4 fw-bold text-uppercase">
          Start Designing
        </NuxtLink>
      </div>
    </section>

    <!-- Footer Component -->
    <Footer />
  </div>
</template>

<script setup>
// Import Swiper Vue.js components
import { Swiper, SwiperSlide } from 'swiper/vue'

// Import required modules
import { Navigation, Pagination, Autoplay } from 'swiper/modules'
import {  GetBestSelling } from '~/server/BestSelling-service'
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

// Define reactive state
const products = ref([])
const showNotification = ref(false)
const notificationMessage = ref('')
const imageLoaded = ref({})
const router = useRouter()

// Fetch products on component mount
onMounted(async () => {
  try {
    const response = await GetBestSelling()
    console.log("shshhs",response.data.designs)
    if (response && response.data.designs) {
      products.value = response.data.designs.map(product => ({
        id: product.id,
        name: product.name,
        image: product.previewImageUrl,
        price: product.price,
        rating: product.rating,
        reviews: product.ratingCount
      }))
    }
  } catch (error) {
    console.error('Error fetching products:', error)
  }
})

const swiperModules = [Navigation, Pagination, Autoplay]
const swiperOptions = {
  modules: swiperModules,
  slidesPerView: 1,
  spaceBetween: 20,
  loop: true,
  autoplay: {
    delay: 3000,
    disableOnInteraction: false,
  },
  navigation: {
    nextEl: '.swiper-button-next',
    prevEl: '.swiper-button-prev',
  },
  pagination: {
    el: '.swiper-pagination',
    clickable: true,
  },
  breakpoints: {
    640: {
      slidesPerView: 2,
    },
    768: {
      slidesPerView: 3,
    },
    1024: {
      slidesPerView: 4,
    },
  },
}

import { useCartStore } from '@/server/cart'
const cartStore = useCartStore()

// Hàm thêm vào giỏ hàng
const addToCart = (product) => {
  cartStore.addItem({
    id: product.id,
    name: product.name,
    price: product.price,
    image: product.image,
    quantity: 1
  })
  
  notificationMessage.value = `${product.name} has been added to cart!`
  showNotification.value = true
  
  setTimeout(() => {
    showNotification.value = false
  }, 3000)
}

// Handle image loading
const handleImageLoad = (productId) => {
  imageLoaded.value[productId] = true
}

const navigateToProduct = (productId) => {
  router.push(`/productPage/${productId}`);
};
</script>

<style scoped>
  /* Global Styles */
  .home-wrapper {
    background-color: #f8f9fa;
  }

  /* Hero Section Styles */
  .hero-section {
    background: linear-gradient(rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0.7)), url('https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s');
    background-size: cover;
    background-position: center;
    background-attachment: fixed;
    color: #fff;
    padding: 150px 0;
    position: relative;
    overflow: hidden;
  }

  .hero-section::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: linear-gradient(45deg, rgba(52, 152, 219, 0.1), rgba(0, 188, 212, 0.1));
    z-index: 1;
  }

  .hero-section .container {
    position: relative;
    z-index: 2;
  }

  .text-sneaker-orange {
    color: #00bcd4;
    font-weight: bold;
  }

  .text-sneaker-star {
    color: #ffd700;
  }

  .sneaker-hero-img {
    max-width: 70%;
    filter: drop-shadow(0 10px 20px rgba(0, 0, 0, 0.2));
    transition: all 0.4s ease;
  }

  .sneaker-hero-img:hover {
    transform: scale(1.05);
    filter: drop-shadow(0 15px 30px rgba(0, 0, 0, 0.3));
  }

  .btn-sneaker {
    background: #00bcd4;
    color: #fff;
    border: none;
    border-radius: 8px;
    padding: 12px 30px;
    font-weight: 600;
    letter-spacing: 0.5px;
    transition: all 0.3s ease;
  }

  .btn-sneaker:hover {
    background: #008ba3;
    transform: translateY(-2px);
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
  }

  .btn-outline-light {
    border: 2px solid #fff;
    border-radius: 8px;
    padding: 12px 30px;
    font-weight: 600;
    letter-spacing: 0.5px;
    transition: all 0.3s ease;
  }

  .btn-outline-light:hover {
    background: rgba(255, 255, 255, 0.1);
    transform: translateY(-2px);
  }

  /* Features Section Styles */
  .feature-box {
    background: #fff;
    border-radius: 12px;
    padding: 30px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
    transition: all 0.3s ease;
  }

  .feature-box:hover {
    transform: translateY(-5px);
    box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1);
  }

  .feature-icon {
    font-size: 2.5rem;
    color: #00bcd4;
    margin-bottom: 15px;
  }

  /* How It Works Section */
  .step-box {
    background: #fff;
    border-radius: 12px;
    padding: 30px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
    transition: all 0.3s ease;
  }

  .step-number {
    display: inline-block;
    width: 40px;
    height: 40px;
    line-height: 40px;
    text-align: center;
    background: #00bcd4;
    color: #fff;
    border-radius: 50%;
    font-size: 1.2rem;
    font-weight: bold;
    margin-bottom: 15px;
  }

  .step-box:hover {
    transform: translateY(-5px);
    box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1);
  }

  /* Best Seller Section */
  .product-card {
    background: #fff;
    border-radius: 12px;
    padding: 15px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
    transition: all 0.3s ease;
  }

  .product-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1);
  }

  .product-image {
    border-radius: 8px;
    overflow: hidden;
  }

  .product-image img {
    transition: transform 0.3s ease;
  }

  .product-card:hover .product-image img {
    transform: scale(1.05);
  }

  .price {
    font-size: 1.1rem;
    font-weight: bold;
    color: #00bcd4;
  }

  /* CTA Section */
  .cta-section {
    background: #00bcd4;
    color: #fff;
    padding: 80px 0;
  }

  /* Notification Toast */
  .notification-toast {
    position: fixed;
    bottom: 20px;
    right: 20px;
    background: #00bcd4;
    color: white;
    padding: 12px 25px;
    border-radius: 8px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
    transform: translateY(100px);
    opacity: 0;
    transition: all 0.3s ease;
  }

  .notification-toast.show {
    transform: translateY(0);
    opacity: 1;
  }

  /* Swiper Navigation */
  .swiper-button-next,
  .swiper-button-prev {
    background: #fff;
    width: 40px;
    height: 40px;
    border-radius: 50%;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    transition: all 0.3s ease;
  }

  .swiper-button-next:hover,
  .swiper-button-prev:hover {
    background: #00bcd4;
    color: #fff;
  }

  .swiper-pagination-bullet {
    width: 8px;
    height: 8px;
    background: #ccc;
    opacity: 0.5;
  }

  .swiper-pagination-bullet-active {
    background: #00bcd4;
    opacity: 1;
  }

  /* Loading Effect */
  .image-skeleton {
    background: #f5f5f5;
  }

  /* Testimonials */
  .testimonial-box {
    background: #fff;
    border-radius: 12px;
    padding: 30px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
    transition: all 0.3s ease;
  }

  .testimonial-box:hover {
    transform: translateY(-5px);
    box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1);
  }

  /* Blog Cards */
  .blog-card {
    background: #fff;
    border-radius: 12px;
    padding: 15px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
    transition: all 0.3s ease;
  }

  .blog-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1);
  }

  .blog-card img {
    border-radius: 8px;
    transition: transform 0.3s ease;
  }

  .blog-card:hover img {
    transform: scale(1.03);
  }

  /* Responsive Styles */
  @media (max-width: 768px) {
    .hero-section {
      padding: 80px 0;
    }

    .sneaker-hero-img {
      max-width: 90%;
    }

    .feature-box,
    .step-box,
    .product-card,
    .testimonial-box,
    .blog-card {
      margin-bottom: 20px;
    }

    .btn-sneaker,
    .btn-outline-light {
      width: 100%;
      margin-bottom: 10px;
    }
  }
</style>