<!-- Home Page Template - Custom Sneaker Theme -->
<template>
  <div class="home-wrapper">
    <!-- Header Component -->
    <Header />

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
          <img src="data:image/jpeg;base64,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" alt="Custom Sneaker" class="img-fluid sneaker-hero-img" />
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
.home-wrapper {
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  min-height: 100vh;
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