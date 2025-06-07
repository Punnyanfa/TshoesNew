<!-- Home Page Template - Custom Sneaker Theme -->
<template>
  <div class="home-wrapper">
    <!-- Header Component -->
    <Header />

    <!-- Hero Section -->
    <section class="hero-section new-arrivals-section d-flex align-items-center justify-content-center">
      <div class="container">
        <div class="row align-items-center">
          <!-- Text bên trái -->
          <div class="col-md-6 text-start">
            <h1 class="display-3 fw-bold text-new-arrivals mb-3">
              {{ heroSlides[currentSlide].title }}
            </h1>
            <p class="lead text-muted mb-4">
              {{ heroSlides[currentSlide].description }}
            </p>
            <NuxtLink to="/productPage" class="btn btn-view-collection fw-bold">
              View Collection <span class="ms-2">&#8594;</span>
            </NuxtLink>
          </div>
          <!-- Ảnh giày bên phải -->
          <div class="col-md-6 text-center">
            <img :src="heroSlides[currentSlide].image" alt="New Arrivals Shoes" class="img-fluid hero-shoe-img" />
          </div>
        </div>
        <!-- Nút chuyển slide -->
        <button class="hero-arrow hero-arrow-left" @click="prevSlide">
          <span>&#8592;</span>
        </button>
        <button class="hero-arrow hero-arrow-right" @click="nextSlide">
          <span>&#8594;</span>
        </button>
      </div>
    </section>
    <section class="best-sellers-section container py-5">
      <h2 class="collection-title text-center">3D Custom Shoe Collection</h2>
      <p class="collection-desc text-center">
        Personalize your perfect pair with our 3D custom shoe collection. Choose colors, materials, and styles to match your unique taste. Step into comfort and creativity—designed by you, made for you.
      </p>
      
      <Swiper
        v-bind="swiperOptions"
        class="best-seller-swiper"
      >
        <SwiperSlide v-for="product in products" :key="product.id">
          <div class="product-card">
            <div class="product-image position-relative">
              <img 
                :src="product.previewImageUrl" 
                :alt="product.name" 
                class="img-fluid rounded"
                @load="handleImageLoad(product.id)"
                :class="{ 'image-loaded': imageLoaded[product.id] }"
              />
              <div class="image-skeleton" v-if="!imageLoaded[product.id]"></div>
              <div class="product-overlay">
                
              </div>
            </div>
            <div class="product-info mt-3">
              <NuxtLink 
                :to="`/productPage/${product.id}`" 
                class="product-title text-decoration-none"
              >
                <h5 class="fw-bold mb-2">{{ product.name }}</h5>
              </NuxtLink>
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

    <!-- How It Works Section -->
    <section class="how-it-works-section py-5 bg-light">
      <div class="container">
        <h2 class="text-center fw-bold mb-5 text-sneaker-orange">How It Works</h2>
        <div class="row align-items-center">
          <!-- Text on the left -->
          <div class="col-md-6 text-start">
            <h1 class="display-3 fw-bold text-new-arrivals mb-3">
              Design Your Own
            </h1>
            <p class="lead text-muted mb-4">
              Discover our custom shoe design process. From your first idea to the final product, we'll be with you every step of the creative journey.
            </p>
            <div class="steps-list">
              <div class="step-item mb-3">
                <i class="bi bi-1-circle-fill text-sneaker-orange me-2"></i>
                <span>Select a base shoe model</span>
              </div>
              <div class="step-item mb-3">
                <i class="bi bi-2-circle-fill text-sneaker-orange me-2"></i>
                <span>Customize colors and materials</span>
              </div>
              <div class="step-item mb-3">
                <i class="bi bi-3-circle-fill text-sneaker-orange me-2"></i>
                <span>Add graphics and personal details</span>
              </div>
              <div class="step-item">
                <i class="bi bi-4-circle-fill text-sneaker-orange me-2"></i>
                <span>Preview and place your order</span>
              </div>
            </div>
            <NuxtLink to="/customPage" class="btn btn-view-purchase fw-bold mt-4">
              Start Designing
            </NuxtLink>
          </div>
          <!-- Image on the right -->
          <div class="col-md-6 text-center">
            <img src="https://i.pinimg.com/736x/1a/9b/31/1a9b3156a00590fa03ac2220850640ca.jpg" alt="Custom Shoe Design Process" class="img-fluid hero-shoe-img" />
          </div>
        </div>
      </div>
    </section>

    <!-- Featured Designs Section -->
    <section class="featured-designs-bg">
      <div class="container py-5">
        <div class="row g-4">
          <div class="col-md-4" v-for="(item, idx) in featuredDesigns" :key="idx">
            <div class="design-card-custom">
              <div class="design-img-wrap">
                <img :src="item.image" :alt="item.title" class="img-fluid rounded" />
                <div class="design-date-badge">
                  <span class="date-day">{{ item.dateDay }}</span>
                  <span class="date-month">{{ item.dateMonth }}</span>
                </div>
              </div>
              <h5 class="design-title">{{ item.title }}</h5>
              <p class="design-desc">{{ item.desc }}</p>
            </div>
          </div>
        </div>
        <div class="text-center mt-4">
          <NuxtLink to="/blogPage" class="btn btn-sneaker px-5 py-3 fw-bold text-uppercase">
            View All Blogs
          </NuxtLink>
        </div>
      </div>
    </section>

    <!-- Best Seller Sneakers Section -->
    <section class="best-seller-highlight-section py-5">
      <div class="container">
        <div class="row align-items-center justify-content-center">
          <!-- Ảnh giày bên trái -->
          <div class="col-md-6 text-center">
            <img :src="bestSeller.image" :alt="bestSeller.name" class="img-fluid best-seller-img" />
          </div>
          <!-- Thông tin bên phải -->
          <div class="col-md-6">
            <h2 class="best-seller-title">{{ bestSeller.name }}</h2>
            <div class="best-seller-rating mb-2">
              <span v-for="star in 5" :key="star">
                <i class="bi" :class="star <= bestSeller.rating ? 'bi-star-fill text-warning' : 'bi-star text-muted'"></i>
              </span>
            </div>
            <p class="best-seller-desc">{{ bestSeller.desc }}</p>
            <div class="best-seller-colors mb-4">
              <span
                v-for="(color, idx) in bestSeller.colors"
                :key="idx"
                :style="{ background: color }"
                class="color-dot" 
              ></span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Testimonials Section -->
    <section class="testimonials-section py-5 bg-light">
      <div class="container">
        <h2 class="text-center fw-bold mb-5 text-sneaker-orange">What Our Customers Say</h2>
        <div class="row g-4">
          <div class="col-md-4" v-for="(item, idx) in testimonials" :key="idx">
            <div class="testimonial-box p-4 rounded">
              <p class="text-muted">"{{ item.content }}"</p>
              <h6 class="mt-3 fw-bold">{{ item.name }}</h6>
              <span class="text-sneaker-star">
                <template v-for="star in item.stars">★</template>
              </span>
            </div>
          </div>
        </div>
      </div>
    </section>


    <!-- Readmore Section -->
    <section class="readmore-section container py-5">
      <h2 class="text-center fw-bold mb-5 text-sneaker-orange">The Story Behind the Shoes</h2>
      <div class="row g-4">
        <div class="col-md-6" v-for="article in readmoreArticles" :key="article.id">
          <div class="readmore-card d-flex">
            <img :src="article.image" :alt="article.title" class="img-fluid rounded" style="max-width: 200px;" />
            <div class="ms-3">
              <h5 class="fw-bold">{{ article.title }}</h5>
              <p class="text-muted">{{ article.excerpt }}</p>
            </div>
          </div>
        </div>
      </div>
    </section>


    <!-- Call to Action Section -->
    <section class="cta-section text-center py-5">
      <div class="container">
        <h2 class="fw-bold" style="color: #777777;">Ready to Elevate Your Style?</h2>
        <p class="mt-3" style="color: #000000;">Join thousands of sneaker enthusiasts who've turned their ideas into one-of-a-kind custom shoes.</p>
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
import 'swiper/css'
import 'swiper/css/navigation'
import 'swiper/css/pagination'
import { Swiper, SwiperSlide } from 'swiper/vue'

// Import required modules
import { Navigation, Pagination, Autoplay } from 'swiper/modules'
import { GetBestSelling } from '~/server/BestSelling-service'
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useCartStore } from '@/server/cart'

// Define reactive state
const showNotification = ref(false)
const notificationMessage = ref('')
const imageLoaded = ref({})
const router = useRouter()
const hoverIdx = ref(null)
const cartStore = useCartStore()
const isLoading = ref(false)
const error = ref(null)
const products = ref([])

const handleImageLoad = (productId) => {
  imageLoaded.value[productId] = true;
};

// Fetch products on component mount
onMounted(async () => {
  isLoading.value = true
  error.value = null
  try {
    const response = await GetBestSelling()
    if (response && response.data.designs) {
      products.value = response.data.designs.map(product => ({
        id: product.id,
        name: product.name,
        previewImageUrl: product.previewImageUrl,
        price: product.price,
        rating: product.rating,
        reviews: product.ratingCount
      }))
    }
  } catch (err) {
    error.value = err.message || 'Có lỗi xảy ra khi tải dữ liệu'
    console.error('Error fetching products:', err)
  } finally {
    isLoading.value = false
  }
})

// Hàm thêm vào giỏ hàng
const addToCart = (product) => {
  cartStore.addItem({
    id: product.id,
    name: product.name,
    price: product.price,
    image: product.previewImageUrl,
    quantity: 1
  })
}

const navigateToProduct = (productId) => {
  router.push(`/productPage/${productId}`);
};

// Slider cho Hero Section
const heroSlides = [
  {
    title: 'Latest Designs',
    description: 'Step into the future of footwear with 3D custom shoes. Create your own unique style by choosing colors, patterns, and details that match your personality. With advanced 3D previews, you can see your creation in real time before making it a reality. Stand out, step up, and walk in your truly one-of-a-kind shoes.',
    image: 'https://i.pinimg.com/736x/5a/3f/f9/5a3ff9bc476206eef0a072cfe24544c8.jpg',
    link: '/collection'
  },
  {
    title: '2025 Hot Trends',
    description: 'Discover the hottest custom shoe styles of 2025 — youthful, dynamic, and full of personality.',
    image: 'https://i.pinimg.com/736x/75/e0/8d/75e08dc38be9e3e97ddf2c71a940de81.jpg',
    link: '/collection'
  }
]
const currentSlide = ref(0)
function prevSlide() {
  currentSlide.value = (currentSlide.value - 1 + heroSlides.length) % heroSlides.length
}
function nextSlide() {
  currentSlide.value = (currentSlide.value + 1) % heroSlides.length
}

const collections = [
  {
    id: 1,
    name: 'Jungle Moc Slip-on',
    image: 'https://static.nike.com/a/images/t_PDP_864_v1/f_auto,q_auto:eco/2b0b2e2c-2e7e-4e2e-8e2e-2e7e2e7e2e7e/nike-air-zoom-pegasus-33.jpg',
    price: '$300.00',
    rating: 4
  },
  {
    id: 2,
    name: 'Jungle Moc Slip-on',
    image: 'https://static.nike.com/a/images/t_PDP_864_v1/f_auto,q_auto:eco/1b0b2e2c-2e7e-4e2e-8e2e-2e7e2e7e2e7e/nike-air-max-2024.jpg',
    price: '$300.00',
    rating: 3
  },
  {
    id: 3,
    name: 'Cloudfoam Race Sneaker',
    image: 'https://static.nike.com/a/images/t_PDP_864_v1/f_auto,q_auto:eco/3b0b2e2c-2e7e-4e2e-8e2e-2e7e2e7e2e7e/nike-cloudfoam.jpg',
    price: '$120.00',
    rating: 5
  },
  {
    id: 4,
    name: 'Low waterproof Boot',
    image: 'https://static.nike.com/a/images/t_PDP_864_v1/f_auto,q_auto:eco/4b0b2e2c-2e7e-4e2e-8e2e-2e7e2e7e2e7e/nike-waterproof.jpg',
    price: '$400.00',
    rating: 4
  },
  {
    id: 5,
    name: 'Garren Cap Toe Brogue',
    image: 'https://static.nike.com/a/images/t_PDP_864_v1/f_auto,q_auto:eco/5b0b2e2c-2e7e-4e2e-8e2e-2e7e2e7e2e7e/nike-garren.jpg',
    price: '$290.00',
    rating: 3
  },
  {
    id: 6,
    name: 'Dunham Lexington',
    image: 'https://static.nike.com/a/images/t_PDP_864_v1/f_auto,q_auto:eco/6b0b2e2c-2e7e-4e2e-8e2e-2e7e2e7e2e7e/nike-dunham.jpg',
    price: '<span style="text-decoration:line-through;color:#bbb;">$550</span> <span style="color:#3ec6a7;">$450.00</span>',
    rating: 5
  },
  {
    id: 7,
    name: 'Merrell Encore Bypass',
    image: 'https://static.nike.com/a/images/t_PDP_864_v1/f_auto,q_auto:eco/7b0b2e2c-2e7e-4e2e-8e2e-2e7e2e7e2e7e/nike-merrell.jpg',
    price: '$300.00',
    rating: 4
  },
  {
    id: 8,
    name: 'Sperry Top-Sider Lanyard',
    image: 'https://static.nike.com/a/images/t_PDP_864_v1/f_auto,q_auto:eco/8b0b2e2c-2e7e-4e2e-8e2e-2e7e2e7e2e7e/nike-sperry.jpg',
    price: '$250.00',
    rating: 3
  }
]

const featuredDesigns = [
  {
    image: 'https://i.pinimg.com/736x/04/57/9e/04579e0fb83da096f0e0fbad5f4fcfa7.jpg',
    dateDay: '22',
    dateMonth: 'Mar',
    title: '20% Off Custom Nike Air Shoes',
    desc: 'Create your own signature look with personalized Nike Air shoes, crafted using cutting-edge 3D customization technology.',
    link: '/blog/1'
  },
  {
    image: 'https://i.pinimg.com/736x/de/a2/24/dea224f643ddb837ca7c9584df346ce2.jpg',
    dateDay: '10',
    dateMonth: 'Apr',
    title: 'Exclusive Sneaker Design Experience',
    desc: 'Explore custom-designed Nike sneakers in 3D, where you become the creator of your very own pair.',
    link: '/blog/2'
  },
  {
    image: 'https://i.pinimg.com/736x/66/e0/4a/66e04a329510d77f65bc12c7e6efabd9.jpg',
    dateDay: '11',
    dateMonth: 'May',
    title: 'Top 20 Hottest Custom Running Shoes',
    desc: 'Check out the most popular custom running shoes with full personalization options—from color to style.',
    link: '/blog/3'
  }
]

const bestSeller = {
  name: 'Air Force 1 Custom',
  image: 'https://i.pinimg.com/736x/07/21/53/0721530efeb6ae4f880178a49a4a237e.jpg',
  rating: 5,
  desc: 'The custom Air Force 1 brings a bold, personal touch to a classic icon. Choose your own colors, materials, and patterns to match your style using advanced 3D design technology.',
  colors: ['#bdbba7', '#c2b8a3', '#b6b6c7', '#aeb8c6'],
}

const readmoreArticles = [
  {
    id: 1,
    title: 'Giày Tùy Chỉnh: Món Quà Thương Hiệu Khiến Mọi Người Bàn Tán',
    excerpt: 'Khi nói đến quà tặng thương hiệu, các doanh nghiệp luôn tìm kiếm thứ gì đó độc đáo, đáng nhớ và hữu ích. Giày tùy chỉnh đang nổi lên như một lựa chọn nổi bật...',
    image: 'https://i.pinimg.com/736x/59/78/d1/5978d13d6ac017bacce2d5d9a6d36310.jpg'
  },
  {
    id: 2,
    title: 'Top Giày Thương Hiệu Doanh Nghiệp Tốt Nhất 2025',
    excerpt: 'Bước vào năm 2025, những đôi giày mang thương hiệu doanh nghiệp vẫn đang dẫn đầu xu hướng nhờ sự đổi mới, phong cách và tính ứng dụng cao...',
    image: 'https://i.pinimg.com/736x/b1/bc/60/b1bc60366d6ef337fba6116460679316.jpg'
  },
  {
    id: 3,
    title: 'Giày Tùy Chỉnh – Phản Chiếu Giá Trị Thương Hiệu',
    excerpt: 'Chúng tôi tự hào giới thiệu một hướng đi táo bạo trong ngành giày dép – nơi phong cách không còn là tất cả. Giày tùy chỉnh giúp doanh nghiệp thể hiện rõ nét bản sắc và giá trị cốt lõi...',
    image: 'https://i.pinimg.com/736x/a2/52/ec/a252ec9a07367fe21c1ad0dc9976ece6.jpg'
  },
  {
    id: 4,
    title: 'Xu Hướng Giày Tùy Chỉnh Bền Vững Đang Lên Ngôi 2025',
    excerpt: 'Khám phá cách vật liệu thân thiện với môi trường và quy trình sản xuất bền vững đang định hình lại ngành giày tùy chỉnh. Cùng điểm mặt những thương hiệu tiên phong trong xu hướng này...',
    image: 'https://i.pinimg.com/736x/07/21/53/0721530efeb6ae4f880178a49a4a237e.jpg'
  },
  {
    id: 5,
    title: 'Top 10 Công Ty Đột Phá Trong Thiết Kế Giày Tùy Chỉnh',
    excerpt: 'Giới thiệu những công ty sáng tạo ứng dụng AI, in 3D và các kỹ thuật thiết kế độc đáo để tạo ra những mẫu giày tùy chỉnh ấn tượng. Kèm hình ảnh nổi bật và chia sẻ từ các chuyên gia trong ngành...',
    image: 'https://i.pinimg.com/736x/1a/62/0c/1a620c6c7b9bb6689426d13088b29c98.jpg'
  },
  {
    id: 6,
    title: 'Xây Dựng Lòng Trung Thành Với Thương Hiệu Qua Chiến Dịch Giày Tùy Chỉnh',
    excerpt: 'Tìm hiểu cách các doanh nghiệp sử dụng giày tùy chỉnh như một công cụ tiếp thị hiệu quả để tăng tương tác và giữ chân khách hàng. Có dẫn chứng từ những chiến dịch thành công thực tế...',
    image: 'https://i.pinimg.com/736x/da/6d/ef/da6defa9b113bc30fbcda1dc48bc68f9.jpg'
  }
]

const testimonials = [
  {
    content: 'Công cụ tùy chỉnh thật tuyệt vời! Đôi giày của tôi hoàn toàn đúng như ý muốn.',
    name: 'Hoàng Khôi',
    stars: 5
  },
  {
    content: 'Giao hàng nhanh, vừa chân hoàn hảo. Tôi đang thiết kế đôi tiếp theo rồi!',
    name: 'Trúc Linh',
    stars: 5
  },
  {
    content: 'Trải nghiệm giày tốt nhất từ trước đến nay. Rất đáng đồng tiền.',
    name: 'Tấn Tài',
    stars: 5
  }
]

const swiperOptions = {
  modules: [Navigation, Pagination, Autoplay],
  slidesPerView: 4,
  spaceBetween: 30,
  navigation: true,
  pagination: { clickable: true },
  autoplay: { delay: 3000, disableOnInteraction: false },
  breakpoints: {
    1200: { slidesPerView: 4 },
    992: { slidesPerView: 3 },
    768: { slidesPerView: 2 },
    0: { slidesPerView: 1 }
  }
}
</script>

<style scoped>
  /* Global Styles */
  :root {
    --primary-color: #AAAAAA;
    --primary-dark: #888888;
    --primary-light: #CCCCCC;
    --accent-color: #AAAAAA;
    --accent-dark: #888888;
    --text-dark: #000000;
    --text-light: #333333;
    --white: #fff;
    --light-bg: #f8f9fa;
  }

  .home-wrapper {
    background-color: var(--light-bg);
  }

  /* Hero Section Styles */
  .hero-section {
    background: linear-gradient(rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0.7)), url('https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s');
    background-size: cover;
    background-position: center;
    background-attachment: fixed;
    color: var(--white);
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
    background: linear-gradient(45deg, rgba(0, 188, 212, 0.1), rgba(0, 188, 212, 0.1));
    z-index: 1;
  }

  .hero-section .container {
    position: relative;
    z-index: 2;
  }

  .text-sneaker-orange {
    color: var(--text-dark);
    font-weight: bold;
    color: #777777;
  }

  .text-sneaker-star {
    color: #d4c603;
  }

  .btn-sneaker {
    color: var(--text-dark);
    background: none;
    border: none;
    font-size: 1.1rem;
    transition: color 0.2s;
  }

  .btn-sneaker:hover {
    color: var(--text-dark);
  }

  /* Best Seller Section */
  .best-seller-highlight-section {
    background: #f5f8fa;
    padding: 60px 0 40px 0;
  }
  .best-seller-img {
    max-width: 420px;
    width: 100%;
    filter: drop-shadow(0 10px 20px rgba(0,0,0,0.15));
  }
  .best-seller-title {
    font-size: 2.3rem;
    font-weight: 700;
    color: var(--text-dark);
    margin-bottom: 10px;
    letter-spacing: 0;
  }
  .best-seller-rating i {
    font-size: 1.2rem;
    margin-right: 2px;
  }
  .best-seller-desc {
    color: var(--text-dark);
    font-size: 1.08rem;
    margin-bottom: 18px;
    margin-top: 8px;
    max-width: 500px;
  }
  .best-seller-colors {
    display: flex;
    gap: 12px;
    margin-bottom: 24px;
  }
  .color-dot {
    width: 28px;
    height: 28px;
    border-radius: 50%;
    border: 2px solid #e0e0e0;
    display: inline-block;
    cursor: pointer;
    transition: border 0.2s;
  }
  .color-dot:hover {
    border: 2px solid #3ec6a7;
  }
  .btn-best-seller-buy {
    background: var(--accent-color);
    color: var(--white) !important;
    border: none;
    border-radius: 4px;
    padding: 10px 38px;
    font-weight: 700;
    font-size: 1.1rem;
    letter-spacing: 0.5px;
    box-shadow: 0 4px 16px rgba(62,198,167,0.08);
    transition: background 0.2s, transform 0.2s;
    outline: none;
    display: inline-block;
  }
  .btn-best-seller-buy:hover {
    background: var(--accent-dark);
    color: var(--white);
    transform: translateY(-2px) scale(1.04);
    box-shadow: 0 8px 24px rgba(62,198,167,0.15);
    text-decoration: none;
  }

  /* CTA Section */
  .cta-section {
    background: var(--primary-color);
    color: var(--text-dark);
    padding: 80px 0;
  }

  .cta-section h2 {
    color: var(--text-dark);
  }

  .cta-section p {
    color: var(--text-dark);
  }

  /* Testimonials */
  .testimonial-box {
    background: var(--white);
    border-radius: 12px;
    padding: 30px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
    transition: all 0.3s ease;
  }

  .testimonial-box:hover {
    transform: translateY(-5px);
    box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1);
  }

  /* Readmore Cards */
  .readmore-card {
    background: var(--white);
    border-radius: 12px;
    padding: 15px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
    transition: all 0.3s ease;
  }

  .readmore-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1);
  }

  .readmore-card img {
    border-radius: 8px;
    transition: transform 0.3s ease;
  }

  .readmore-card:hover img {
    transform: scale(1.03);
  }

  /* New Arrivals Section Styles */
  .new-arrivals-section {
    background: #f5f8fa;
    min-height: 500px;
    position: relative;
    padding: 60px 0;
  }
  .text-new-arrivals {
    color: var(--accent-color);
    font-size: 3rem;
    font-weight: bold;
    color: #777777;
  }
  .btn-view-collection {
    color: var(--text-dark);
    background: none;
    border: none;
    font-size: 1.1rem;
    transition: color 0.2s;
  }
  .btn-view-collection:hover {
    color: var(--text-dark);
  }

  .btn-view-purchase {
    color: var(--text-dark);
    background: none;
    border: none;
    font-size: 1.1rem;
    transition: color 0.2s;
  }
  .btn-view-purchase:hover {
    color: var(--text-dark);
  }

  .hero-shoe-img {
    max-width: 420px;
    width: 100%;
    filter: drop-shadow(0 10px 20px rgba(0,0,0,0.15));
  }
  .hero-arrow {
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    background: var(--white);
    border: 1px solid #e0e0e0;
    background-color: #ffffff;
    border-radius: 50%;
    width: 38px;
    height: 38px;
    display: flex;
    justify-content: center;
    font-size: 1.3rem;
    color: var(--accent-color);
    cursor: pointer;
    transition: background 0.2s;
    z-index: 2;
  }
  .hero-arrow-left {
    left: -60px;
  }
  .hero-arrow-right {
    right: -60px;
  }
  .hero-arrow:hover {
    background: var(--primary-light);
  }

  /* Collection Section Styles */
  .collection-section {
    background: #fff;
    padding: 60px 0 40px 0;
  }
  .collection-title {
    font-size: 2.2rem;
    font-weight: bold;
    color: var(--text-dark);
    margin-bottom: 10px;
    color: #777777;
  }
  .collection-desc {
    color: var(--text-dark);
    font-size: 1.1rem;
    margin-bottom: 40px;
    max-width: 700px;
    margin-left: auto;
    margin-right: auto;
  }
  .product-card-collection {
    background: none;
    border: none;
    box-shadow: none;
    padding: 0;
    position: relative;
    transition: box-shadow 0.2s;
  }
  .product-img-wrap {
    position: relative;
    overflow: hidden;
    background: #f8f8f8;
    border-radius: 4px;
  }
  .product-img-wrap img {
    width: 100%;
    display: block;
    transition: transform 0.3s;
  }
  .product-card-collection:hover img {
    transform: scale(1.04);
  }
  .product-overlay {
    position: absolute;
    top: 0; left: 0; right: 0; bottom: 0;
    background: rgba(40,40,40,0.6);
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 16px;
    z-index: 2;
  }
  .overlay-btn {
    background: #fff;
    border: none;
    border-radius: 4px;
    width: 38px;
    height: 38px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.2rem;
    color: #222;
    margin: 0 2px;
    transition: background 0.2s;
  }
  .overlay-btn:hover {
    background: #e0e0e0;
  }
  .product-info {
    text-align: center;
    padding: 10px 0 0 0;
  }
  .product-info .product-name {
    font-size: 1.08rem;
    font-weight: 600;
    color: var(--text-dark);
    margin-bottom: 4px;
    line-height: 1.3;
    min-height: 38px;
    display: flex;
    align-items: center;
    justify-content: center;
  }
  .product-info .product-price {
    color: var(--text-dark);
    font-size: 1.08rem;
    font-weight: 500;
    margin-bottom: 4px;
  }
  .product-info .product-rating {
    margin-top: 2px;
    margin-bottom: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 2px;
  }
  .product-info .product-rating i {
    font-size: 1rem;
    margin: 0 1px;
  }
  .btn-outline-collection {
    border: 1.5px solid var(--text-dark);
    color: var(--text-dark);
    background: var(--white);
    border-radius: 4px;
    padding: 8px 32px;
    font-weight: 600;
    transition: all 0.2s;
  }
  .btn-outline-collection:hover {
    background: var(--text-dark);
    color: var(--white);
  }

  .btn-view-collection .ms-2 {
    transition: transform 0.2s;
  }
  .btn-view-collection:hover .ms-2 {
    transform: translateX(4px);
  }

  .btn-view-purchase .ms-2 {
    transition: transform 0.2s;
  }
  .btn-view-purchase:hover .ms-2 {
    transform: translateX(4px);
  }

  /* Featured Designs Section Styles */
  .featured-designs-bg {
    background: #fff;
    width: 100vw;
    position: relative;
    left: 50%;
    right: 50%;
    margin-left: -50vw;
    margin-right: -50vw;
  }
  .featured-designs-section {
    padding: 60px 0 40px 0;
  }
  .design-card-custom {
    background: none;
    border: none;
    box-shadow: none;
    padding: 0;
    text-align: left;
  }
  .design-img-wrap {
    position: relative;
    border-radius: 8px;
    overflow: hidden;
  }
  .design-img-wrap img {
    width: 100%;
    border-radius: 8px;
    display: block;
  }
  .design-date-badge {
    position: absolute;
    top: 12px;
    left: 12px;
    background: var(--accent-color);
    color: var(--white);
    border-radius: 6px;
    padding: 4px 10px 2px 10px;
    display: flex;
    flex-direction: column;
    align-items: center;
    font-size: 0.95rem;
    font-weight: 600;
    min-width: 38px;
    box-shadow: 0 2px 8px rgba(62,198,167,0.08);
  }
  .date-day {
    font-size: 1.1rem;
    font-weight: 700;
    line-height: 1;
  }
  .date-month {
    font-size: 0.85rem;
    font-weight: 400;
    line-height: 1;
    margin-top: 1px;
    letter-spacing: 0.5px;
  }
  .design-title {
    font-size: 1.13rem;
    font-weight: 700;
    color: var(--text-dark);
    margin: 16px 0 6px 0;
    line-height: 1.3;
  }
  .design-desc {
    color: var(--text-dark);
    font-size: 1rem;
    margin-bottom: 6px;
    min-height: 38px;
  }
  .design-readmore {
    color: var(--text-dark);
    font-size: 1rem;
    font-weight: 500;
    text-decoration: none;
    transition: color 0.2s;
  }
  .design-readmore:hover {
    color: var(--text-dark);
    text-decoration: underline;
  }

  /* Responsive Styles */
  @media (max-width: 768px) {
    .hero-section {
      padding: 80px 0;
    }

    .new-arrivals-section .row {
      flex-direction: column-reverse;
    }
    .hero-shoe-img {
      max-width: 80vw;
      margin-bottom: 30px;
    }

    .best-seller-title {
      font-size: 1.5rem;
    }
    .best-seller-img {
      max-width: 90vw;
      margin-bottom: 30px;
    }
  }

  /* Best Seller Section Styles */
  .best-seller-swiper {
    padding: 20px 40px;
    margin: 0 -20px;
  }

  .product-card {
    background: #fff;
    border-radius: 15px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
    transition: all 0.4s ease;
    overflow: hidden;
    height: 100%;
    padding: 15px;
  }

  .product-card:hover {
    transform: translateY(-8px);
    box-shadow: 0 15px 30px rgba(0, 0, 0, 0.15);
  }

  .product-image {
    overflow: hidden;
    position: relative;
    padding-top: 100%;
    border-radius: 10px;
  }

  .product-image img {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    object-fit: cover;
    transition: transform 0.4s ease;
  }

  .product-card:hover .product-image img {
    transform: scale(1.1);
  }

  .product-overlay {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.7);
    display: flex;
    justify-content: center;
    align-items: center;
    opacity: 0;
    transition: all 0.3s ease;
    border-radius: 10px;
  }

  .product-card:hover .product-overlay {
    opacity: 1;
  }

  .product-info {
    padding: 1rem 0.5rem;
  }

  .btn-add-cart {
    background: linear-gradient(45deg, #AAAAAA, #888888);
    color: #fff;
    border: none;
    padding: 12px 24px;
    border-radius: 25px;
    font-weight: 600;
    letter-spacing: 0.5px;
    transition: all 0.3s ease;
  }

  .btn-add-cart:hover {
    transform: scale(1.1);
    background: linear-gradient(45deg, #888888, #AAAAAA);
  }

  .price {
    font-size: 1.2rem;
    font-weight: 700;
    color: #2c3e50;
    background: linear-gradient(45deg, #AAAAAA, #888888);
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
    color: #AAAAAA;
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
    background: #AAAAAA;
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

  /* Notification Toast Styles */
  .notification-toast {
    position: fixed;
    bottom: 20px;
    right: 20px;
    background: linear-gradient(45deg, #AAAAAA, #888888);
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
    color: #AAAAAA;
  }

  /* Image Loading Animation */
  .image-loaded {
    animation: fadeIn 0.5s ease;
  }

  @keyframes fadeIn {
    from { opacity: 0; }
    to { opacity: 1; }
  }

  /* Responsive Adjustments */
  @media (max-width: 768px) {
    .best-seller-swiper {
      padding: 20px;
    }
    
    .swiper-button-next,
    .swiper-button-prev {
      display: none;
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
</style>