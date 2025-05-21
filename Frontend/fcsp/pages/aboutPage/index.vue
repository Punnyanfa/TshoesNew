<template>
  <div class="about-wrapper">
    <div class="big-image-bg"></div>
    <!-- Header Component (Assumed Imported) -->
    <Header/>

    <!-- Main Content Section -->
    <section class="container py-6">

      <!-- Introduction Section -->
      <div class="row mb-6 intro-section">
        <div class="col-12">
          <div class="intro-content text-center">
            <h2 class="intro-title">Introduction to Our Custom Shoe Innovation</h2>
            <p class="intro-text">
              At our core, we specialize in crafting personalized footwear experiences. Combining innovation, AI-driven customization, and expert craftsmanship, we empower you to design unique shoes that reflect your style and personality.
            </p>
          </div>
          <div class="intro-features d-flex justify-content-center flex-wrap gap-4">
            <div class="feature-card">
              <div class="feature-icon"><i class="fas fa-dollar-sign" aria-hidden="true"></i></div>
              <h3>Custom Shoe Innovation</h3>
              <p>Personalized pricing models for unique designs.</p>
            </div>
            <div class="feature-card">
              <div class="feature-icon"><i class="fas fa-chart-line" aria-hidden="true"></i></div>
              <h3>AI-Driven Insights</h3>
              <p>Data-powered customization to match your style.</p>
            </div>
            <div class="feature-card">
              <div class="feature-icon"><i class="fas fa-users" aria-hidden="true"></i></div>
              <h3>Expert Craftsmanship</h3>
              <p>A skilled team ensuring top-tier quality and precision.</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Image Gallery Section with Text and Video Side by Side -->
      <div class="row mb-6 image-gallery align-items-center">
        <div class="col-md-6">
          <div class="gallery-text">
            <h3 class="gallery-title">Our Journey in Custom Shoe Innovation</h3>
            <p class="gallery-description">
              Explore how we merge AI-powered customization with expert craftsmanship to create one-of-a-kind footwear tailored to your style. Watch our video to see the magic unfold!
            </p>
          </div>
        </div>
        <div class="col-md-6">
          <div class="gallery-video">
            <iframe
              class="gallery-iframe w-100"
              src="https://www.youtube.com/embed/LRgpZpW7raU?si=hfu1SnE2Lyu7c0Jy&autoplay=1&mute=1"
              title="Our Team Introduction Video"
              frameborder="0"
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
              referrerpolicy="strict-origin-when-cross-origin"
              allowfullscreen
            ></iframe>
          </div>
        </div>
      </div>

      <!-- Hero Section with Full-Width Background Image -->
      <div class="hero-section">
        <div class="hero-content text-left position-relative">
          <div class="hero-bg-overlay"></div>
          <div class="payment-hero-content">
            <h1 class="payment-hero-title">Get Paid Faster and More Securely with Our Payment Gateway Solution</h1>
            <p class="payment-hero-text">Simplify your transactions and boost customer trust with our fast, secure, and reliable payment processing system.</p>
            <NuxtLink to="/contactPage" class="btn btn-primary contact-btn">Contact Us</NuxtLink>
          </div>
        </div>
      </div>

      <!-- Stats Section -->
      <div class="stats-section">
        <div class="stat-item">
          <h3 class="stat-number">2M+</h3>
          <p class="stat-label">Member Active</p>
        </div>
        <div class="stat-divider"></div>
        <div class="stat-item">
          <h3 class="stat-number">98%</h3>
          <p class="stat-label">Satified Customer</p>
        </div>
        <div class="stat-divider"></div>
        <div class="stat-item">
          <h3 class="stat-number">4.7</h3>
          <p class="stat-label">User Rating</p>
        </div>
        <div class="stat-divider"></div>
        <div class="stat-item">
          <h3 class="stat-number">15+</h3>
          <p class="stat-label">Years of Experience</p>
        </div>
      </div>

      <!-- Team Members Section -->
      <div class="row mb-6 team-section">
        <div class="col-12 text-center">
          <h2 class="section-title">Teamwork makes the dream work</h2>
          <p class="section-subtitle">
            Discover the skilled artisans and innovators bringing your custom shoe designs to life with precision and creativity.
          </p>
        </div>
        <div class="col-12">
          <div class="team-members d-flex justify-content-center flex-wrap gap-4">
            <div class="team-card">
              <img
                src="https://th.bing.com/th/id/OIP.8zxb_q87POL2XN03FhvQ1gHaHa?w=187&h=187&c=7&r=0&o=5&dpr=1.3&pid=1.7"
                alt="Ba Tung, FE & Leader"
                class="team-photo"
              />
              <h4>Bá Tùng</h4>
              <div class="social-icons">
                <a href="#" class="social-icon" aria-label="Ba Tung's LinkedIn"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon" aria-label="Ba Tung's Twitter"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon" aria-label="Ba Tung's Instagram"><i class="fab fa-instagram"></i></a>
              </div>
              <p class="team-role">FE & Leader</p>
            </div>
            <div class="team-card">
              <img
                src="https://th.bing.com/th/id/OIP.8zxb_q87POL2XN03FhvQ1gHaHa?w=187&h=187&c=7&r=0&o=5&dpr=1.3&pid=1.7"
                alt="Harry Worth, Branch Manager"
                class="team-photo"
              />
              <h4>Triệu Ân</h4>
              <div class="social-icons">
                <a href="#" class="social-icon" aria-label="Harry Worth's LinkedIn"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon" aria-label="Harry Worth's Twitter"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon" aria-label="Harry Worth's Instagram"><i class="fab fa-instagram"></i></a>
              </div>
              <p class="team-role">BE & Database</p>
            </div>
            <div class="team-card">
              <img
                src="https://th.bing.com/th/id/OIP.8zxb_q87POL2XN03FhvQ1gHaHa?w=187&h=187&c=7&r=0&o=5&dpr=1.3&pid=1.7"
                alt="Jenny Hobb, Head Manager"
                class="team-photo"
              />
              <h4>Hoàng Đức</h4>
              <div class="social-icons">
                <a href="#" class="social-icon" aria-label="Jenny Hobb's LinkedIn"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon" aria-label="Jenny Hobb's Twitter"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon" aria-label="Jenny Hobb's Instagram"><i class="fab fa-instagram"></i></a>
              </div>
              <p class="team-role">FE & Docs</p>
            </div>
            <div class="team-card">
              <img
                src="https://th.bing.com/th/id/OIP.8zxb_q87POL2XN03FhvQ1gHaHa?w=187&h=187&c=7&r=0&o=5&dpr=1.3&pid=1.7"
                alt="Johnny Smith, Senior Supervisor"
                class="team-photo"
              />
              <h4>Triệu Thuận</h4>
              <div class="social-icons">
                <a href="#" class="social-icon" aria-label="Johnny Smith's LinkedIn"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon" aria-label="Johnny Smith's Twitter"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon" aria-label="Johnny Smith's Instagram"><i class="fab fa-instagram"></i></a>
              </div>
              <p class="team-role">BE & 3D Model</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Footer Note -->
      <div class="text-center footer-note py-4">
        <p class="text-muted">© {{ currentYear }} www.YourCompany.com | All Rights Reserved</p>
      </div>
    </section>

    <!-- Footer Component (Assumed Imported) -->
    <Footer />
  </div>
</template>

<script setup>
import Header from '~/components/Header.vue';

// Dynamic year for footer
const currentYear = new Date().getFullYear();
</script>

<style scoped>
/* Import Font Awesome for icons */
@import url('https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css');
/* Import Google Fonts */
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

.about-wrapper {
  background: linear-gradient(135deg, #f5f7fa 0%, #e4e9f2 100%);
  min-height: 100vh;
  font-family: 'Poppins', sans-serif;
  position: relative;
  display: flex;
  flex-direction: column;
}

.container {
  position: relative;
  z-index: 1;
  color: #000000;
  max-width: 1300px;
  margin: 0 auto;
  flex: 1;
  padding-bottom: 60px;
  min-height: 400px;
}

/* Hero Section with Full-Width Background Image */
.hero-section {
  position: relative;
  width: 100%;
  background: linear-gradient(rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0.7)), url('https://kingshoes.vn/data/upload/media/af1-2.jpg');
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
  height: 90vh;
  min-height: 600px;
  max-height: 900px;
  display: flex;
  align-items: center;
  justify-content: center;
  animation: fadeIn 1.5s ease-in-out;
}

.hero-content {
  max-width: 1300px;
  width: 100%;
  margin: 0 auto;
  padding: 0 20px;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: flex-start;
  position: relative;
  z-index: 2;
}

.hero-bg-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-size: cover;
  filter: brightness(70%) contrast(110%);
  z-index: 0;
  border-radius: 0;
}

.payment-hero-content {
  max-width: 800px;
  color: white;
  position: relative;
  z-index: 2;
  text-align: left;
  padding-left: 20px;
}

.payment-hero-title {
  font-size: 3.5rem;
  font-weight: 700;
  margin-bottom: 1.5rem;
  line-height: 1.2;
  text-align: left;
  color: #ffffff;
}

.payment-hero-text {
  font-size: 1.2rem;
  margin-bottom: 2rem;
  line-height: 1.6;
  text-align: left;
  color: #ffffff;
}

.contact-btn {
  padding: 12px 30px;
  font-size: 1.1rem;
  font-weight: 500;
  background-color: #AAAAAA;
  border: none;
  border-radius: 5px;
  transition: all 0.3s ease;
  display: inline-block;
  margin: 0;
}

.contact-btn:hover {
  background-color: #888888;
  transform: translateY(-2px);
}

/* Introduction Section */
.intro-section {
  padding: 60px 0;
  background: #ffffff;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.05);
  margin-bottom: 60px;
  width: 100vw;
  position: relative;
  left: 50%;
  right: 50%;
  margin-left: -50vw;
  margin-right: -50vw;
}

.intro-content {
  margin-bottom: 40px;
}

.intro-title {
  font-size: 2.5rem;
  font-weight: 600;
  color: #000000;
  margin-bottom: 20px;
  letter-spacing: 1px;
}

.intro-text {
  font-size: 1.1rem;
  color: #000000;
  line-height: 1.8;
  max-width: 900px;
  margin: 0 auto;
}

.intro-features {
  padding: 0 20px;
}

.feature-card {
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 6px 15px rgba(0, 0, 0, 0.08);
  padding: 30px;
  text-align: center;
  width: 300px;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.feature-card:hover {
  transform: translateY(-10px);
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.12);
}

.feature-icon {
  font-size: 2.5rem;
  color: #000000;
  margin-bottom: 15px;
}

.feature-card h3 {
  font-size: 1.4rem;
  font-weight: 500;
  color: #000000;
  margin-bottom: 10px;
}

.feature-card p {
  font-size: 1rem;
  color: #000000;
  line-height: 1.6;
}

/* Image Gallery Section */
.image-gallery {
  padding: 80px 0;
  min-height: 500px;
}

.gallery-text {
  padding: 20px;
}

.gallery-title {
  font-size: 1.8rem;
  font-weight: 600;
  color: #000000;
  margin-bottom: 15px;
}

.gallery-description {
  font-size: 1.1rem;
  color: #000000;
  line-height: 1.8;
}

.gallery-video {
  position: relative;
  padding-bottom: 56.25%;
  height: 0;
  overflow: hidden;
  border-radius: 10px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.1);
  min-height: 400px;
}

.gallery-iframe {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  border-radius: 10px;
}

/* Team Members Section */
.team-section {
  padding: 80px 0;
  margin-top: 40px;
  min-height: 600px;
}

.section-title {
  font-size: 2.5rem;
  color: #000000;
  margin-bottom: 20px;
  letter-spacing: 1px;
}

.section-subtitle {
  font-size: 1.1rem;
  color: #000000;
  max-width: 700px;
  margin: 0 auto 40px;
}

.team-members {
  padding: 0 20px;
}

.team-card {
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 6px 15px rgba(0, 0, 0, 0.08);
  padding: 20px;
  text-align: center;
  width: 250px;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.team-card:hover {
  transform: translateY(-10px);
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.12);
}

.team-photo {
  width: 220px;
  height: 220px;
  border-radius: 50%;
  object-fit: cover;
  margin-bottom: 20px;
  border: 4px solid transparent;
  background-clip: border-box;
}

.team-card h4 {
  font-size: 1.4rem;
  font-weight: 500;
  color: #000000;
  margin-bottom: 5px;
}

.team-role {
  font-size: 1rem;
  color: #000000;
  margin-bottom: 15px;
}

.social-icons {
  display: flex;
  justify-content: center;
  gap: 10px;
}

.social-icon {
  font-size: 1.2rem;
  color: #000000;
  transition: transform 0.3s ease;
}

.social-icon:hover {
  transform: scale(1.2);
}

/* Footer Note */
.footer-note {
  border-top: 1px solid #e2e8f0;
  margin-top: auto;
  padding: 20px 0;
  background: white;
}

.footer-note p {
  font-size: 0.9rem;
  color: #000000;
  margin: 0;
}

/* Responsive Design */
@media (max-width: 768px) {
  .hero-section {
    height: 60vh;
    min-height: 400px;
  }

  .payment-hero-title {
    font-size: 2rem;
  }

  .stats-section {
    flex-wrap: wrap;
    padding: 1rem 0;
  }

  .stat-item {
    width: 50%;
    padding: 1rem 0;
  }

  .stat-divider {
    display: none;
  }

  .stat-number {
    font-size: 2rem;
  }

  .intro-title {
    font-size: 2rem;
  }

  .intro-features {
    flex-direction: column;
    align-items: center;
  }

  .feature-card {
    width: 100%;
    max-width: 300px;
    margin-bottom: 20px;
  }

  .image-gallery .col-md-6 {
    margin-bottom: 20px;
  }

  .gallery-video {
    padding-bottom: 56.25%; /* Giữ tỷ lệ 16:9 */
  }

  .section-title {
    font-size: 2rem;
  }

  .team-members {
    flex-direction: column;
    align-items: center;
  }

  .team-card {
    width: 100%;
    max-width: 300px;
    margin-bottom: 20px;
  }
}

/* Stats Section */
.stats-section {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 2rem 0;
  background: white;
  width: 100%;
  border-bottom: 1px solid #eee;
}

.stat-item {
  text-align: center;
  flex: 1;
}

.stat-divider {
  width: 1px;
  height: 50px;
  background-color: #e0e0e0;
  margin: 0 15px;
}

.stat-number {
  font-size: 2.5rem;
  font-weight: 700;
  color: #AAAAAA;
  margin-bottom: 0.5rem;
}

.stat-label {
  font-size: 1rem;
  color: #000000;
  margin: 0;
}

.big-image-bg {
  width: 100vw;
  height: 550px;
  min-height: 250px;
  max-height: 500px;
  background: url('https://kingshoes.vn/data/upload/media/af1-2.jpg') center center/cover no-repeat;
  position: absolute;
  top: 0;
  left: 0;
  z-index: 0;
}

/* Cập nhật màu cho các phần tử khác */
.text-muted {
  color: #000000 !important;
}

.lead {
  color: #000000;
}

/* Cập nhật màu cho các phần tử có gradient */
.intro-title,
.gallery-title,
.section-title,
.feature-icon,
.social-icon {
  background: none;
  -webkit-background-clip: initial;
  background-clip: initial;
  color: #AAAAAA;
}
</style>