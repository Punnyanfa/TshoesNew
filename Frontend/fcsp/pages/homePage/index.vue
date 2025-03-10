<!-- Home Page Template - Custom Sneaker Theme -->
<template>
  <div class="home-wrapper">
    <!-- Header Component -->
    <Header />

    <!-- Breadcrumb -->
    <nav class="container py-3" aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li class="breadcrumb-item active" aria-current="page">Home</li>
      </ol>
    </nav>

    <!-- Hero Section -->
    <section class="hero-section text-center">
      <div class="container py-5">
        <h1 class="display-3 fw-bold text-sneaker-orange animate__animated animate__fadeInDown">
          Unleash Your Sneaker Soul
        </h1>
        <p class="lead text-white mt-3 animate__animated animate__fadeInUp">
          Design your perfect pair with AI-driven customization—unique, stylish, and all you.
        </p>
        <div class="mt-4">
          <NuxtLink to="/customPage" class="btn btn-sneaker px-5 py-3 fw-bold text-uppercase animate__animated animate__zoomIn">
            Start Designing
          </NuxtLink>
        </div>
        <div class="hero-img-container mt-5">
          <img src="data:image/jpeg;base64,/9j/..." alt="Custom Sneaker" class="img-fluid sneaker-hero-img" />
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="container py-5">
      <div class="row g-4 text-center">
        <div class="col-md-4">
          <div class="feature-box p-4 rounded">
            <i class="bi bi-gear-fill feature-icon"></i>
            <h4 class="mt-3 fw-bold">AI Customization</h4>
            <p class="text-muted">Precision-fit designs tailored to your feet.</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="feature-box p-4 rounded">
            <i class="bi bi-brush-fill feature-icon"></i>
            <h4 class="mt-3 fw-bold">Endless Styles</h4>
            <p class="text-muted">Mix colors, patterns, and materials your way.</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="feature-box p-4 rounded">
            <i class="bi bi-truck feature-icon"></i>
            <h4 class="mt-3 fw-bold">Fast Delivery</h4>
            <p class="text-muted">Your custom kicks, shipped straight to you.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Footer Component -->
    <Footer />
  </div>
</template>

<script setup>
// Không cần logic phức tạp cho trang chủ
</script>

<style scoped>
.breadcrumb {
  background: transparent;
  padding: 0;
  margin-bottom: 0;
}

.breadcrumb-item {
  font-size: 1rem;
  color: #8bc34a;
}

.breadcrumb-item.active {
  color: #fff;
  font-weight: 500;
}

.hero-section {
  background: linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url('/images/sneaker-bg.jpg');
  background-size: cover;
  background-position: center;
  color: #fff;
  padding: 100px 0;
}

.contact-wrapper {
  background: linear-gradient(135deg, #f5f7fa 0%, #e4e9f2 100%);
  min-height: 100vh;
  font-family: 'Poppins', sans-serif;
  position: relative;
}

.hero-section {
  background: linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url('/images/sneaker-bg.jpg');
  background-size: cover;
  background-position: center;
  color: #fff;
  padding: 100px 0;
}

.text-sneaker-orange {
  color: #8bc34a; /* Sneaker-inspired orange */
}

.sneaker-hero-img {
  max-width: 70%;
  filter: drop-shadow(0 10px 20px rgba(0, 0, 0, 0.3));
  transition: transform 0.3s ease;
}

.sneaker-hero-img:hover {
  transform: scale(1.05);
}

.btn-sneaker {
  background: #8bc34a;
  color: #fff;
  border: none;
  border-radius: 10px;
  transition: background 0.3s ease, transform 0.2s ease;
}

.btn-sneaker:hover {
  background: #8bc34a;
  transform: scale(1.05);
}

.feature-box {
  background: #fff;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.feature-box:hover {
  transform: translateY(-10px);
  box-shadow: 0 15px 30px rgba(0, 0, 0, 0.15);
}

.feature-icon {
  font-size: 2.5rem;
  color: #8bc34a;
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .hero-section {
    padding: 60px 0;
  }
  .display-3 {
    font-size: 2.5rem;
  }
  .sneaker-hero-img {
    max-width: 90%;
  }
}
</style>