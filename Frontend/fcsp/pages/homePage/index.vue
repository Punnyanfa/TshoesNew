<!-- Home Page Template - Custom Sneaker Theme -->
<template>
  <div class="home-wrapper">
    <!-- Header Component -->
    <Header />

    <!-- Hero Section -->
    <section class="hero-section text-center">
      <div class="container py-5">
        <h1 class="display-3 fw-bold text-sneaker-orange animate__animated animate__fadeInDown">
          Create Your Sneaker Legacy
        </h1>
        <p class="lead text-white mt-3 animate__animated animate__fadeInUp">
          Craft one-of-a-kind sneakers with AI precision—your style, your story.
        </p>
        <div class="mt-4 d-flex justify-content-center gap-3">
          <NuxtLink to="/customPage" class="btn btn-sneaker px-5 py-3 fw-bold text-uppercase animate__animated animate__zoomIn">
            Design Now
          </NuxtLink>
          <NuxtLink to="/gallery" class="btn btn-outline-light px-5 py-3 fw-bold text-uppercase animate__animated animate__zoomIn">
            Explore Gallery
          </NuxtLink>
        </div>
        <div class="hero-img-container mt-5">
          <img src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=363&h=182&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Custom Sneaker" class="img-fluid sneaker-hero-img animate__animated animate__pulse" />
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
            <h4 class="mt-3 fw-bold">AI-Driven Design</h4>
            <p class="text-muted">Smart technology crafts sneakers that fit you perfectly.</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="feature-box p-4 rounded">
            <i class="bi bi-brush-fill feature-icon"></i>
            <h4 class="mt-3 fw-bold">Unlimited Creativity</h4>
            <p class="text-muted">Personalize every detail—colors, textures, and more.</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="feature-box p-4 rounded">
            <i class="bi bi-truck feature-icon"></i>
            <h4 class="mt-3 fw-bold">Swift Shipping</h4>
            <p class="text-muted">Get your custom sneakers delivered in record time.</p>
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
              <h5 class="mt-3 fw-bold">Choose Your Base</h5>
              <p class="text-muted">Pick a sneaker style that suits your vibe.</p>
            </div>
          </div>
          <div class="col-md-3">
            <div class="step-box p-4">
              <span class="step-number">2</span>
              <h5 class="mt-3 fw-bold">Customize It</h5>
              <p class="text-muted">Use our AI tool to design every detail.</p>
            </div>
          </div>
          <div class="col-md-3">
            <div class="step-box p-4">
              <span class="step-number">3</span>
              <h5 class="mt-3 fw-bold">Preview & Order</h5>
              <p class="text-muted">See your creation in 3D, then place your order.</p>
            </div>
          </div>
          <div class="col-md-3">
            <div class="step-box p-4">
              <span class="step-number">4</span>
              <h5 class="mt-3 fw-bold">Wear Your Art</h5>
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

    <!-- Testimonials Section -->
    <section class="testimonials-section py-5 bg-light">
      <div class="container">
        <h2 class="text-center fw-bold mb-5 text-sneaker-orange">What Our Customers Say</h2>
        <div class="row g-4">
          <div class="col-md-4">
            <div class="testimonial-box p-4 rounded">
              <p class="text-muted">"The customization tool is insane! My sneakers are exactly what I dreamed of."</p>
              <h6 class="mt-3 fw-bold">Alex T.</h6>
              <span class="text-sneaker-star">★★★★★</span>
            </div>
          </div>
          <div class="col-md-4">
            <div class="testimonial-box p-4 rounded">
              <p class="text-muted">"Fast delivery and perfect fit. I’m already designing my next pair!"</p>
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
        <h2 class="fw-bold text-white">Ready to Step Up Your Game?</h2>
        <p class="text-light mt-3">Join thousands of sneaker enthusiasts who’ve turned their vision into reality.</p>
        <NuxtLink to="/customPage" class="btn btn-sneaker px-5 py-3 mt-4 fw-bold text-uppercase">
          Start Your Design
        </NuxtLink>
      </div>
    </section>

    <!-- Footer Component -->
    <Footer />
  </div>
</template>

<script setup>
  // No complex logic needed for the home page, just import necessary components
</script>

<style scoped>
  /* Hero Section Styles */
  .hero-section {
    background: linear-gradient(rgba(0, 0, 0, 0.6), rgba(0, 0, 0, 0.6)), url('https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=338&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7');
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
  }
</style>