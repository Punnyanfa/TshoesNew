<template>
  <div class="about-wrapper">
    <!-- Header Component (Assumed Imported) -->
    <Header/>

    <!-- Breadcrumb -->
    <nav class="container py-3" aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li class="breadcrumb-item">
          <NuxtLink to="/homePage" class="text-decoration-none">Home</NuxtLink>
        </li>
        <li class="breadcrumb-item active" aria-current="page">About Uss</li>
      </ol>
    </nav>

    <!-- Main Content Section -->
    <section class="container py-6">
      <!-- Hero Section with Full-Width Background Image -->
      <div class="hero-section">
        <div class="hero-content text-center position-relative">
          <div class="hero-bg-overlay"></div>
          <h1 class="hero-title">About Us</h1>
        </div>
      </div>

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

      <!-- Team Members Section -->
      <div class="row mb-6 team-section">
        <div class="col-12 text-center">
          <h2 class="section-title">Meet Our Craftsmanship Team</h2>
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
              <h4>Ba Tung</h4>
              <p class="team-role">FE & Leader</p>
              <div class="social-icons">
                <a href="#" class="social-icon" aria-label="Ba Tung's LinkedIn"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon" aria-label="Ba Tung's Twitter"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon" aria-label="Ba Tung's Instagram"><i class="fab fa-instagram"></i></a>
              </div>
            </div>
            <div class="team-card">
              <img
                src="https://via.placeholder.com/200x200?text=Harry+Worth"
                alt="Harry Worth, Branch Manager"
                class="team-photo"
              />
              <h4>Harry Worth</h4>
              <p class="team-role">Branch Manager</p>
              <div class="social-icons">
                <a href="#" class="social-icon" aria-label="Harry Worth's LinkedIn"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon" aria-label="Harry Worth's Twitter"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon" aria-label="Harry Worth's Instagram"><i class="fab fa-instagram"></i></a>
              </div>
            </div>
            <div class="team-card">
              <img
                src="https://via.placeholder.com/200x200?text=Jenny+Hobb"
                alt="Jenny Hobb, Head Manager"
                class="team-photo"
              />
              <h4>Jenny Hobb</h4>
              <p class="team-role">Head Manager</p>
              <div class="social-icons">
                <a href="#" class="social-icon" aria-label="Jenny Hobb's LinkedIn"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon" aria-label="Jenny Hobb's Twitter"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon" aria-label="Jenny Hobb's Instagram"><i class="fab fa-instagram"></i></a>
              </div>
            </div>
            <div class="team-card">
              <img
                src="https://via.placeholder.com/200x200?text=Johnny+Smith"
                alt="Johnny Smith, Senior Supervisor"
                class="team-photo"
              />
              <h4>Johnny Smith</h4>
              <p class="team-role">Senior Supervisor</p>
              <div class="social-icons">
                <a href="#" class="social-icon" aria-label="Johnny Smith's LinkedIn"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon" aria-label="Johnny Smith's Twitter"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon" aria-label="Johnny Smith's Instagram"><i class="fab fa-instagram"></i></a>
              </div>
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
}

.container {
  position: relative;
  z-index: 1;
  color: #1e293b;
  max-width: 1300px;
  margin: 0 auto;
}

/* Breadcrumb Styles */
.breadcrumb {
  background: transparent;
  padding: 0;
  margin-bottom: 0;
}

.breadcrumb-item a {
  color: #3498db;
  text-decoration: none;
  transition: color 0.3s ease;
}

.breadcrumb-item a:hover {
  color: #2c3e50;
  text-decoration: underline;
}

.breadcrumb-item.active {
  color: #1e293b;
  font-weight: 500;
}

/* Hero Section with Full-Width Background Image */
.hero-section {
  position: relative;
  margin-bottom: 60px;
  width: 100vw;
  max-width: 1300px;
  left: 50%;
  transform: translateX(-50%);
}

.hero-content {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  max-width: 1300px;
  margin: 0 auto;
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

.hero-title {
  font-size: 4.5rem;
  font-weight: 700;
  color: #fff;
  text-transform: uppercase;
  position: relative;
  z-index: 1;
  padding: 20px;
  text-shadow: 3px 3px 15px rgba(0, 0, 0, 0.5);
  border-radius: 8px;
  background: linear-gradient(45deg, #2c3e50, #3498db);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent; 
}

/* Introduction Section */
.intro-section {
  padding: 60px 0;
  background: #ffffff;
  border-radius: 15px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.05);
  margin-bottom: 60px;
}

.intro-content {
  margin-bottom: 40px;
}

.intro-title {
  font-size: 2.5rem;
  font-weight: 600;
  background: linear-gradient(45deg, #2c3e50, #3498db);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
  margin-bottom: 20px;
  letter-spacing: 1px;
}

.intro-text {
  font-size: 1.1rem;
  color: #64748b;
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
  background: linear-gradient(45deg, #2c3e50, #3498db);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
  margin-bottom: 15px;
}

.feature-card h3 {
  font-size: 1.4rem;
  font-weight: 500;
  color: #1e293b;
  margin-bottom: 10px;
}

.feature-card p {
  font-size: 1rem;
  color: #64748b;
  line-height: 1.6;
}

/* Image Gallery Section */
.image-gallery {
  padding: 60px 0;
}

.gallery-text {
  padding: 20px;
}

.gallery-title {
  font-size: 1.8rem;
  font-weight: 600;
  background: linear-gradient(45deg, #2c3e50, #3498db);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
  margin-bottom: 15px;
}

.gallery-description {
  font-size: 1.1rem;
  color: #64748b;
  line-height: 1.8;
}

.gallery-video {
  position: relative;
  padding-bottom: 56.25%; /* Tỷ lệ 16:9 */
  height: 0;
  overflow: hidden;
  border-radius: 10px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.1);
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
  padding: 60px 0;
  background: #ffffff;
  border-radius: 15px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.05);
}

.section-title {
  font-size: 2.5rem;
  font-weight: 600;
  background: linear-gradient(45deg, #2c3e50, #3498db);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
  margin-bottom: 20px;
  letter-spacing: 1px;
}

.section-subtitle {
  font-size: 1.1rem;
  color: #64748b;
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
  width: 200px;
  height: 200px;
  border-radius: 50%;
  object-fit: cover;
  margin-bottom: 15px;
  border: 4px solid transparent;
  background-clip: border-box;
}

.team-card h4 {
  font-size: 1.4rem;
  font-weight: 500;
  color: #1e293b;
  margin-bottom: 5px;
}

.team-role {
  font-size: 1rem;
  color: #64748b;
  margin-bottom: 15px;
}

.social-icons {
  display: flex;
  justify-content: center;
  gap: 10px;
}

.social-icon {
  font-size: 1.2rem;
  background: linear-gradient(45deg, #2c3e50, #3498db);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
  transition: transform 0.3s ease;
}

.social-icon:hover {
  transform: scale(1.2);
}

/* Footer Note */
.footer-note {
  border-top: 1px solid #e2e8f0;
}

.footer-note p {
  font-size: 0.9rem;
  color: #64748b;
}

/* Responsive Design */
@media (max-width: 768px) {
  .hero-title {
    font-size: 2.5rem;
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
</style>