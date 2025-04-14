<!-- Home Page Template - Custom Sneaker Theme -->
<template>
  <div class="home-wrapper">
    <!-- Header Component -->
    <Header />

    <!-- Hero Section -->
    <section class="hero-section text-center">
      <div class="container py-5">
        <h1 class="display-3 fw-bold text-sneaker-orange animate__animated animate__fadeInDown">
          Tạo Nên Phong Cách Riêng Của Bạn
        </h1>
        <p class="lead text-white mt-3 animate__animated animate__fadeInUp">
          Thiết kế giày độc đáo với công nghệ AI - Phong cách của bạn, câu chuyện của bạn.
        </p>
        <div class="mt-4 d-flex justify-content-center gap-3">
          <NuxtLink to="/customPage" class="btn btn-sneaker px-5 py-3 fw-bold text-uppercase animate__animated animate__zoomIn">
            Thiết Kế Ngay
          </NuxtLink>
          <NuxtLink to="/gallery" class="btn btn-outline-light px-5 py-3 fw-bold text-uppercase animate__animated animate__zoomIn">
            Khám Phá Bộ Sưu Tập
          </NuxtLink>
        </div>
        <div class="hero-img-container mt-5">
          <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s" alt="Giày Tùy Chỉnh" class="img-fluid sneaker-hero-img animate__animated animate__pulse" />
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="container py-5">
      <h2 class="text-center fw-bold mb-5 text-sneaker-orange">Tại Sao Chọn Chúng Tôi?</h2>
      <div class="row g-4 text-center">
        <div class="col-md-4">
          <div class="feature-box p-4 rounded">
            <i class="bi bi-gear-fill feature-icon"></i>
            <h4 class="mt-3 fw-bold">Thiết Kế Bằng AI</h4>
            <p class="text-muted">Công nghệ thông minh tạo ra đôi giày phù hợp hoàn hảo với bạn.</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="feature-box p-4 rounded">
            <i class="bi bi-brush-fill feature-icon"></i>
            <h4 class="mt-3 fw-bold">Sáng Tạo Không Giới Hạn</h4>
            <p class="text-muted">Tùy chỉnh mọi chi tiết - màu sắc, chất liệu và nhiều hơn nữa.</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="feature-box p-4 rounded">
            <i class="bi bi-truck feature-icon"></i>
            <h4 class="mt-3 fw-bold">Giao Hàng Nhanh</h4>
            <p class="text-muted">Nhận giày tùy chỉnh của bạn trong thời gian ngắn nhất.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- How It Works Section -->
    <section class="how-it-works-section py-5 bg-light">
      <div class="container">
        <h2 class="text-center fw-bold mb-5 text-sneaker-orange">Quy Trình Hoạt Động</h2>
        <div class="row g-4 text-center">
          <div class="col-md-3">
            <div class="step-box p-4">
              <span class="step-number">1</span>
              <h5 class="mt-3 fw-bold">Chọn Mẫu Cơ Bản</h5>
              <p class="text-muted">Lựa chọn kiểu giày phù hợp với phong cách của bạn.</p>
            </div>
          </div>
          <div class="col-md-3">
            <div class="step-box p-4">
              <span class="step-number">2</span>
              <h5 class="mt-3 fw-bold">Tùy Chỉnh</h5>
              <p class="text-muted">Sử dụng công cụ AI để thiết kế mọi chi tiết.</p>
            </div>
          </div>
          <div class="col-md-3">
            <div class="step-box p-4">
              <span class="step-number">3</span>
              <h5 class="mt-3 fw-bold">Xem Trước & Đặt Hàng</h5>
              <p class="text-muted">Xem thiết kế 3D và đặt hàng.</p>
            </div>
          </div>
          <div class="col-md-3">
            <div class="step-box p-4">
              <span class="step-number">4</span>
              <h5 class="mt-3 fw-bold">Mang Tác Phẩm Của Bạn</h5>
              <p class="text-muted">Nhận giày độc đáo và tự tin bước đi.</p>
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
      <h2 class="text-center fw-bold mb-5 text-sneaker-orange">Giày Bán Chạy Nhất</h2>
      
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
          Xem Tất Cả Sản Phẩm
        </NuxtLink>
      </div>
    </section>

    <!-- Testimonials Section -->
    <section class="testimonials-section py-5 bg-light">
      <div class="container">
        <h2 class="text-center fw-bold mb-5 text-sneaker-orange">Khách Hàng Nói Gì?</h2>
        <div class="row g-4">
          <div class="col-md-4">
            <div class="testimonial-box p-4 rounded">
              <p class="text-muted">"Công cụ tùy chỉnh thật tuyệt vời! Đôi giày của tôi hoàn toàn như mong đợi."</p>
              <h6 class="mt-3 fw-bold">Nguyễn Văn A</h6>
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
        <h2 class="fw-bold text-white">Sẵn Sàng Nâng Tầm Phong Cách?</h2>
        <p class="text-light mt-3">Tham gia cùng hàng nghìn người yêu giày đã biến ước mơ thành hiện thực.</p>
        <NuxtLink to="/customPage" class="btn btn-sneaker px-5 py-3 mt-4 fw-bold text-uppercase">
          Bắt Đầu Thiết Kế
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

import { useCartStore } from '~/stores/cart'
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
  
  notificationMessage.value = `Đã thêm ${product.name} vào giỏ hàng!`
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
  /* Hero Section Styles */
  .hero-section {
    background: linear-gradient(rgba(0, 0, 0, 0.6), rgba(0, 0, 0, 0.6)), url('https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s');
    background-size: cover;
    background-position: center;
    color: #fff;
    padding: 120px 0;
    position: relative;
    overflow: hidden;
  }

 .text-sneaker-orange {
    background: linear-gradient(45deg, #2c3e50, #3498db);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
  }

  .text-sneaker-star {
  background: linear-gradient(45deg, #f39c12, #f1c40f);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}


  .sneaker-hero-img {
    max-width: 70%;
    filter: drop-shadow(0 15px 30px rgba(0, 0, 0, 0.4));
    transition: transform 0.4s ease;
  }

  .sneaker-hero-img:hover {
    transform: scale(1.08) rotate(2deg);
  }

  .btn-sneaker {
    background: linear-gradient(45deg, #2c3e50, #3498db);
    color: #fff;
    border: none;
    border-radius: 50px;
    transition: all 0.3s ease;
  }

  .btn-sneaker:hover {
    background: linear-gradient(45deg, #2c3e50, #3498db);
    transform: scale(1.1);
  }

  .btn-outline-light {
    border-radius: 50px;
    transition: all 0.3s ease;
  }

  .btn-outline-light:hover {
    background: linear-gradient(45deg, #2c3e50, #3498db);
    transform: scale(1.1);
  }

  /* Features Section Styles */
  .feature-box {
    background: #fff;
    box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
    transition: all 0.3s ease;
  }

  .feature-box:hover {
    transform: translateY(-15px);
    box-shadow: 0 20px 40px rgba(0, 0, 0, 0.2);
  }

  .feature-icon {
    font-size: 2.8rem;
    color: #8bc34a;
  }

  /* How It Works Section Styles */
  .how-it-works-section {
    background: #f8f9fa;
  }

  .step-box {
    position: relative;
    background: #fff;
    border-radius: 10px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
    transition: all 0.3s ease;
  }

  .step-box:hover {
    transform: translateY(-10px);
  }

  .step-number {
    position: absolute;
    top: -20px;
    left: 50%;
    transform: translateX(-50%);
    background: linear-gradient(45deg, #2c3e50, #3498db);
    color: #fff;
    width: 40px;
    height: 40px;
    line-height: 40px;
    border-radius: 50%;
    font-size: 1.2rem;
  }

  /* Featured Designs Section Styles */
  .design-card img {
    transition: transform 0.3s ease;
  }

  .design-card:hover img {
    transform: scale(1.05);
  }

  /* Testimonials Section Styles */
  .testimonial-box {
    background: #fff;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
    transition: all 0.3s ease;
  }

  .testimonial-box:hover {
    transform: translateY(-10px);
    box-shadow: 0 15px 30px rgba(0, 0, 0, 0.15);
  }

  /* Blog Teaser Section Styles */
  .blog-teaser-section {
    background: #fff;
  }

  .blog-card img {
    transition: transform 0.3s ease;
  }

  .blog-card:hover img {
    transform: scale(1.05);
  }

  /* CTA Section Styles */
  .cta-section {
    background: linear-gradient(45deg, #2c3e50, #3498db);
    color: #fff;
    padding: 80px 0;
  }

  /* Best Seller Section Styles */
  .best-seller-swiper {
    padding: 20px 40px;
    margin: 0 -20px;
  }

  .product-card {
    cursor: pointer;
    transition: transform 0.3s ease, box-shadow 0.3s ease;
    padding: 1rem;
    border-radius: 8px;
  }

  .product-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
  }

  .product-image {
    overflow: hidden;
    border-radius: 8px;
  }

  .product-image img {
    transition: transform 0.3s ease;
  }

  .product-card:hover .product-image img {
    transform: scale(1.05);
  }

  .btn-add-cart {
    z-index: 2;
    position: relative;
  }

  .product-info {
    padding: 1rem 0.5rem;
  }

  .price {
    font-size: 1.2rem;
    font-weight: 700;
    color: #2c3e50;
    background: linear-gradient(45deg, #2c3e50, #3498db);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
  }

  .rating {
    display: flex;
    align-items: center;
    gap: 4px;
  }

  .rating i {
    color: #f1c40f;
  }

  /* Swiper Navigation Styles */
  .swiper-button-next,
  .swiper-button-prev {
    color: #2c3e50;
    background: white;
    width: 44px;
    height: 44px;
    border-radius: 50%;
    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
    transition: all 0.3s ease;
  }

  .swiper-button-next:hover,
  .swiper-button-prev:hover {
    background: #f8f9fa;
    transform: scale(1.1);
  }

  .swiper-button-next:after,
  .swiper-button-prev:after {
    font-size: 18px;
    font-weight: bold;
  }

  /* Swiper Pagination Styles */
  .swiper-pagination {
    margin-top: 20px;
  }

  .swiper-pagination-bullet {
    width: 10px;
    height: 10px;
    background: #2c3e50;
    opacity: 0.5;
    transition: all 0.3s ease;
  }

  .swiper-pagination-bullet-active {
    background: #3498db;
    opacity: 1;
    width: 24px;
    border-radius: 5px;
  }

  /* Loading Effect */
  .image-skeleton {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
    background-size: 200% 100%;
    animation: loading 1.5s infinite;
    border-radius: 10px;
  }

  @keyframes loading {
    0% { background-position: 200% 0; }
    100% { background-position: -200% 0; }
  }

  /* Responsive Adjustments */
  @media (max-width: 768px) {
    .hero-section {
      padding: 80px 0;
    }
    .display-3 {
      font-size: 2.2rem;
    }
    .sneaker-hero-img {
      max-width: 85%;
    }
    .btn-sneaker,
    .btn-outline-light {
      padding: 10px 20px;
      font-size: 0.9rem;
    }
    .feature-box,
    .step-box,
    .testimonial-box {
      padding: 20px;
    }
    .design-card,
    .blog-card {
      text-align: center;
    }
    .blog-card img {
      max-width: 100%;
      margin-bottom: 15px;
    }
    .blog-card .ms-3 {
      margin-left: 0 !important;
    }
    .product-card {
      margin-bottom: 20px;
    }
    
    .product-overlay {
      opacity: 1;
      background: rgba(0, 0, 0, 0.5);
    }
    
    .btn-add-cart {
      padding: 8px 15px;
      font-size: 0.9rem;
    }
  }

  /* Slider Styles */
  .best-seller-swiper {
    padding: 20px;
  }

  .swiper-button-next,
  .swiper-button-prev {
    display: none;
  }

  .swiper-pagination-bullet {
    background: #2c3e50;
  }

  .swiper-pagination-bullet-active {
    background: #3498db;
  }

  /* Responsive adjustments */
  @media (max-width: 768px) {
    .best-seller-swiper {
      padding: 20px;
    }
    
    .swiper-button-next,
    .swiper-button-prev {
      display: none;
    }
  }

  /* Notification Toast Styles */
  .notification-toast {
    position: fixed;
    bottom: 20px;
    right: 20px;
    background: linear-gradient(45deg, #2c3e50, #3498db);
    color: white;
    padding: 15px 25px;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
    transform: translateY(100px);
    opacity: 0;
    transition: all 0.3s ease;
    z-index: 1000;
  }

  .notification-toast.show {
    transform: translateY(0);
    opacity: 1;
  }

  /* Product Title Hover Effect */
  .product-title {
    color: #2c3e50;
    transition: color 0.3s ease;
  }

  .product-title:hover {
    color: #3498db;
  }

  /* Image Loading Animation */
  .image-loaded {
    animation: fadeIn 0.5s ease;
  }

  @keyframes fadeIn {
    from { opacity: 0; }
    to { opacity: 1; }
  }
</style>