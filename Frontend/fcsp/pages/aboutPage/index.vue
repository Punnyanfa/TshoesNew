<template>
  <div class="about-wrapper">
    <!-- Header Component -->
    <Header />

    <!-- Main Content Section -->
    <section class="container py-6">
      <!-- Hero Section -->
      <div class="row align-items-center mb-6 hero-section">
        <div class="col-12 text-center position-relative">
          <h1 class="hero-title">About Us</h1>
          <div class="hero-bg-overlay"></div>
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
              <div class="feature-icon"><i class="fas fa-dollar-sign"></i></div>
              <h3>Competitive Pricing</h3>
              <p>Ensuring top value with tailored pricing models.</p>
            </div>
            <div class="feature-card">
              <div class="feature-icon"><i class="fas fa-chart-line"></i></div>
              <h3>Expert Analysis</h3>
              <p>Data-driven insights to optimize your success.</p>
            </div>
            <div class="feature-card">
              <div class="feature-icon"><i class="fas fa-users"></i></div>
              <h3>Elite Team</h3>
              <p>A seasoned team delivering unparalleled expertise.</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Image Gallery Section -->
      <div class="row mb-6 image-gallery">
        <div class="col-12 position-relative">
          <div class="gallery-container">
            <img src="https://via.placeholder.com/1200x600?text=Team+Collaboration" alt="Team Collaboration" class="gallery-image main-image" />
            <img src="https://via.placeholder.com/300x300?text=Video+Presentation" alt="Video Presentation" class="gallery-image sub-image top-right" />
            <img src="https://via.placeholder.com/300x300?text=Team+Meeting" alt="Team Meeting" class="gallery-image bottom-left" />
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
              <img src="https://via.placeholder.com/200x200?text=Sony+Madison" alt="Sony Madison" class="team-photo" />
              <h4>Sony Madison</h4>
              <p class="team-role">CEO & Founder</p>
              <div class="social-icons">
                <a href="#" class="social-icon"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon"><i class="fab fa-instagram"></i></a>
              </div>
            </div>
            <div class="team-card">
              <img src="https://via.placeholder.com/200x200?text=Harry+Worth" alt="Harry Worth" class="team-photo" />
              <h4>Harry Worth</h4>
              <p class="team-role">Branch Manager</p>
              <div class="social-icons">
                <a href="#" class="social-icon"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon"><i class="fab fa-instagram"></i></a>
              </div>
            </div>
            <div class="team-card">
              <img src="https://via.placeholder.com/200x200?text=Jenny+Hobb" alt="Jenny Hobb" class="team-photo" />
              <h4>Jenny Hobb</h4>
              <p class="team-role">Head Manager</p>
              <div class="social-icons">
                <a href="#" class="social-icon"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon"><i class="fab fa-instagram"></i></a>
              </div>
            </div>
            <div class="team-card">
              <img src="https://via.placeholder.com/200x200?text=Johnny+Smith" alt="Johnny Smith" class="team-photo" />
              <h4>Johnny Smith</h4>
              <p class="team-role">Senior Supervisor</p>
              <div class="social-icons">
                <a href="#" class="social-icon"><i class="fab fa-linkedin"></i></a>
                <a href="#" class="social-icon"><i class="fab fa-twitter"></i></a>
                <a href="#" class="social-icon"><i class="fab fa-instagram"></i></a>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Footer Note -->
      <div class="text-center footer-note py-4">
        <p class="text-muted">© 2025 www.YourCompany.com | All Rights Reserved</p>
      </div>
    </section>

    <!-- Footer Component -->
    <Footer />
  </div>
</template>

<script setup>
// Không cần thêm logic trong trường hợp này
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
}

/* Hero Section */
.hero-section {
  min-height: 250px;
  position: relative;
  margin-bottom: 60px;
}

.hero-title {
  font-size: 4rem;
  font-weight: 700;
  color: #fff;
  text-transform: uppercase;
  position: relative;
  z-index: 2;
  padding: 20px;
  text-shadow: 2px 2px 10px rgba(0, 0, 0, 0.3);
}

.hero-bg-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: url('https://via.placeholder.com/1300x250?text=Professional+Background') no-repeat center;
  background-size: cover;
  filter: grayscale(40%) brightness(60%);
  z-index: 1;
  border-radius: 10px;
  overflow: hidden;
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
  color: #ef4444;
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
  color: #ef4444;
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

.sub-image {
  position: absolute;
  width: 300px;
  height: 300px;
  object-fit: cover;
}

.top-right {
  top: 20px;
  right: 20px;
}

.bottom-left {
  bottom: 20px;
  left: 20px;
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
  color: #1e293b;
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
  border: 4px solid #ef4444;
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
  color: #ef4444;
  transition: color 0.3s ease;
}

.social-icon:hover {
  color: #dc2626;
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
    margin-bottom: 20px;
  }

  .gallery-image.sub-image {
    position: static;
    width: 100%;
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
    width: 80%;
    margin-bottom: 20px;
  }
}
</style>