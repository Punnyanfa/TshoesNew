<template>
  <footer class="footer bg-dark text-light py-5">
    <div class="container">
      <!-- Newsletter Section -->
      <div class="row">
        <div class="col-md-12 text-center mb-5">
          <h5 class="fw-bold mb-3">JOIN OUR NEWSLETTER</h5>
          <div class="input-group newsletter-group mx-auto">
            <input 
              type="email" 
              class="form-control" 
              placeholder="Enter your email address" 
              aria-label="Email address"
              v-model="email"
              @keyup.enter="subscribe"
            />
            <button 
              class="btn btn-success" 
              @click="subscribe"
              :disabled="isLoading"
            >
              <span v-if="isLoading" class="spinner-border spinner-border-sm me-2"></span>
              {{ isLoading ? 'Subscribing...' : 'Subscribe' }}
            </button>
          </div>
          <p class="mt-3 text-secondary">
            Get the latest news and updates directly to your inbox.
          </p>
        </div>
      </div>

      <!-- Quick Links Section -->
      <div class="row mb-4">
        <div class="col-md-3">
          <h6 class="fw-bold mb-3">CUSTOM SHOES COLLECTIONS</h6>
          <ul class="list-unstyled footer-links">
            <li><NuxtLink to="/productPage" class="text-light" aria-label="View our products">Products</NuxtLink></li>
            <li><NuxtLink to="/customPage" class="text-light" aria-label="Customize your shoes">Customize Products</NuxtLink></li>
            <li><NuxtLink to="/mycustomPage" class="text-light" aria-label="View my customizations">My Customize</NuxtLink></li>
          </ul>
        </div>

        <div class="col-md-3">
          <h6 class="fw-bold mb-3">SERVICES</h6>
          <ul class="list-unstyled footer-links">
            <li><NuxtLink to="/aboutPage" class="text-light" aria-label="Learn about us">About</NuxtLink></li>
            <li><NuxtLink to="/contactPage" class="text-light" aria-label="Contact us">Contact</NuxtLink></li>
            <li><NuxtLink to="/blogPage" class="text-light" aria-label="Read our blog">Blog</NuxtLink></li>
          </ul>
        </div>

        <div class="col-md-3">
          <h6 class="fw-bold mb-3">NEWSLETTER</h6>
          <ul class="list-unstyled footer-links">
            <li><a href="https://www.facebook.com" target="_blank" class="text-light" aria-label="Follow us on Facebook">Facebook</a></li>
            <li><a href="https://www.instagram.com" target="_blank" class="text-light" aria-label="Follow us on Instagram">Instagram</a></li>
            <li><a href="https://www.twitter.com" target="_blank" class="text-light" aria-label="Follow us on Twitter">Twitter</a></li>
          </ul>
        </div>
      </div>

      <!-- Contact Information & Copyright Section -->
      <div class="text-center border-top pt-4">
        <p>
          ADDRESS: 290 Nơ Trang Long Street, HCM city | EMAIL: shoedesignwpy@gmail.com | PHONE: (090) 946-1569
        </p>
        <small>&copy; {{ currentYear }} CustomShoes. Made with ❤️ by WPY</small>
      </div>
    </div>
  </footer>
</template>

<script setup>
const currentYear = new Date().getFullYear();
const email = ref('');
const isLoading = ref(false);

const subscribe = async () => {
  if (!email.value) return;
  
  isLoading.value = true;
  try {
    // TODO: Implement newsletter subscription logic
    await new Promise(resolve => setTimeout(resolve, 1000)); // Simulate API call
    email.value = '';
  } finally {
    isLoading.value = false;
  }
};
</script>

<style scoped>
/* Footer Links Hover Effect */
.footer a {
  transition: color 0.3s ease;
  text-decoration: none;
}
.footer a:hover {
  color: #8bc34a;
}

/* Newsletter Input Group */
.newsletter-group {
  max-width: 400px;
  border-radius: 20px;
  overflow: hidden;
}
.newsletter-group .form-control {
  border-radius: 20px 0 0 20px;
  padding: 10px 15px;
}
.newsletter-group .btn {
  border-radius: 0 20px 20px 0;
  background: linear-gradient(45deg, #2c3e50, #3498db);
  color: white;
  transition: all 0.3s ease;
  padding: 10px 20px;
}

.newsletter-group .btn:hover {
  background: linear-gradient(45deg, #1f2d3d, #2980b9);
  transform: translateY(-1px);
}

.newsletter-group .btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

/* Footer Links */
.footer-links li {
  margin-bottom: 8px;
  padding: 4px 0;
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .footer .col-md-3 {
    margin-bottom: 2rem;
  }
}
</style>
