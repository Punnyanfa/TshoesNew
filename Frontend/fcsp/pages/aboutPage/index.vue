<template>
  <div class="about-wrapper">
    <!-- Header Component (Assumed Imported) -->
    <Header />

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
            <h2 class="intro-title">Introduction to Our Premier Digital Agency</h2>
            <p class="intro-text">
              At our core, we are a forward-thinking digital agency dedicated to delivering innovative solutions. With a
              commitment to excellence, we craft strategies that empower brands to thrive in a competitive landscape.
            </p>
          </div>
          <div class="intro-features d-flex justify-content-center flex-wrap gap-4">
            <div class="feature-card">
              <div class="feature-icon"><i class="fas fa-dollar-sign" aria-hidden="true"></i></div>
              <h3>Competitive Pricing</h3>
              <p>Ensuring top value with tailored pricing models.</p>
            </div>
            <div class="feature-card">
              <div class="feature-icon"><i class="fas fa-chart-line" aria-hidden="true"></i></div>
              <h3>Expert Analysis</h3>
              <p>Data-driven insights to optimize your success.</p>
            </div>
            <div class="feature-card">
              <div class="feature-icon"><i class="fas fa-users" aria-hidden="true"></i></div>
              <h3>Elite Team</h3>
              <p>A seasoned team delivering unparalleled expertise.</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Image Gallery Section with Autoplay Video -->
      <div class="row mb-6 image-gallery">
        <div class="col-12 position-relative">
          <div class="gallery-container">
            <img
              src="https://th.bing.com/th/id/OIP.DcH1ZzFxnyIPGVfcYF1SMQHaE7?w=306&h=202&c=7&r=0&o=5&dpr=1.3&pid=1.7"
              alt="Team collaborating in a modern office"
              class="gallery-image main-image"
            />
            <iframe
              class="gallery-iframe sub-image top-right"
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
          <h2 class="section-title">Meet Our Team</h2>
          <p class="section-subtitle">
            Discover the talented individuals driving our success with dedication and innovation.
          </p>
        </div>
        <div class="col-12">
          <div class="team-members d-flex justify-content-center flex-wrap gap-4">
            <div class="team-card">
              <img
                src="https://via.placeholder.com/200x200?text=Sony+Madison"
                alt="Sony Madison, CEO & Founder"
                class="team-photo"
              />
              <h4>Sony Madison</h4>
              <p class="team-role">CEO & Founder</p>
              <div class="social-icons">
                <a href="#" class="social-icon" aria-label="Sony Madison's LinkedIn"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon" aria-label="Sony Madison's Twitter"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon" aria-label="Sony Madison's Instagram"><i class="fab fa-instagram"></i></a>
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
import Header from './components/Header.vue'; // Import Header component
import Footer from './components/Footer.vue'; // Import Footer component

// Dynamic year for footer
const currentYear = new Date().getFullYear();
</script>

<style scoped>
/* Import Font Awesome for icons */
@import url('https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css');
/* Import Google Fonts */
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

.about-wrapper {
  background-color: #f8fafc;
  font-family: 'Poppins', sans-serif;
  position: relative;
  overflow: hidden;
}

.container {
  position: relative;
  z-index: 1;
  color: #1e293b;
  max-width: 1300px;
  margin: 0 auto;
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
  background: url('https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ-rdOm9Yea2nfWcFHxGyQLDbPJSyyOr885rA&s') no-repeat center;
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
  background: linear-gradient(45deg, #2c3e50, #3498db); /* Thêm gradient */
  -webkit-background-clip: text; /* Cho text hiển thị gradient */
  background-clip: text;
  color: transparent; /* Để gradient hiện lên thay màu trắng */
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
  background: linear-gradient(45deg, #2c3e50, #3498db); /* Thay đổi màu */
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
  background: linear-gradient(45deg, #2c3e50, #3498db); /* Thay đổi màu */
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

.gallery-container {
  position: relative;
  overflow: hidden;
  border-radius: 15px;
}

.gallery-image {
  border-radius: 10px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease;
}

.main-image {
  width: 100%;
  height: auto;
}

.gallery-iframe {
  position: absolute;
  width: 300px;
  height: 200px;
  border-radius: 10px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.1);
}

.top-right {
  top: 20px;
  right: 20px;
}

.gallery-image:hover {
  transform: scale(1.02);
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
  background: linear-gradient(45deg, #2c3e50, #3498db); /* Thay đổi màu */
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
  border: 4px solid transparent; /* Thay đổi border */
  background: linear-gradient(45deg, #2c3e50, #3498db); /* Gradient cho border */
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
  background: linear-gradient(45deg, #2c3e50, #3498db); /* Thay đổi màu */
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
  transition: transform 0.3s ease; /* Thay đổi từ color sang transform */
}

.social-icon:hover {
  transform: scale(1.2); /* Hiệu ứng phóng to khi hover */
}

/* Footer Note */
.footer-note {
  background: #f8fafc;
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

  .gallery-iframe {
    position: static;
    width: 100%;
    height: 200px;
    margin-top: 20px;
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