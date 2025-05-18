<template>
  <footer class="footer bg-white text-dark py-5">
    <div class="container">
      <!-- Newsletter Section -->
      <div class="row">
        <div class="col-md-12 text-center mb-5">
          <h5 class="fw-bold mb-3 text-primary">JOIN OUR NEWSLETTER</h5>
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
              class="btn btn-primary" 
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
          <h6 class="fw-bold mb-3 text-primary">CUSTOM SHOES COLLECTIONS</h6>
          <ul class="list-unstyled footer-links">
            <li><NuxtLink to="/productPage" class="text-dark" aria-label="View our products">Products</NuxtLink></li>
            <li><NuxtLink to="/customPage" class="text-dark" aria-label="Customize your shoes">Customize Products</NuxtLink></li>
            <li><NuxtLink to="/mycustomPage" class="text-dark" aria-label="View my customizations">My Customize</NuxtLink></li>
          </ul>
        </div>

        <div class="col-md-3">
          <h6 class="fw-bold mb-3 text-primary">SERVICES</h6>
          <ul class="list-unstyled footer-links">
            <li><NuxtLink to="/aboutPage" class="text-dark" aria-label="Learn about us">About</NuxtLink></li>
            <li><NuxtLink to="/contactPage" class="text-dark" aria-label="Contact us">Contact</NuxtLink></li>
            <li><NuxtLink to="/blogPage" class="text-dark" aria-label="Read our blog">Blog</NuxtLink></li>
          </ul>
        </div>

        <div class="col-md-3">
          <h6 class="fw-bold mb-3 text-primary">NEWSLETTER</h6>
          <ul class="list-unstyled footer-links">
            <li><a href="https://www.facebook.com" target="_blank" class="text-dark" aria-label="Follow us on Facebook">Facebook</a></li>
            <li><a href="https://www.instagram.com" target="_blank" class="text-dark" aria-label="Follow us on Instagram">Instagram</a></li>
            <li><a href="https://www.twitter.com" target="_blank" class="text-dark" aria-label="Follow us on Twitter">Twitter</a></li>
          </ul>
        </div>
      </div>

      <!-- Contact Information & Copyright Section -->
      <div class="text-center border-top pt-4">
        <p class="text-secondary">
          ADDRESS: 290 Nơ Trang Long Street, HCM city | EMAIL: shoedesignwpy@gmail.com | PHONE: (090) 946-1569
        </p>
        <small class="text-secondary">&copy; {{ currentYear }} CustomShoes. Made with ❤️ by WPY</small>
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
  color: #007bff;
}

/* Newsletter Input Group */
.newsletter-group {
  max-width: 400px;
  border-radius: 20px;
  overflow: hidden;
  box-shadow: 0 4px 20px rgba(0, 123, 255, 0.1);
}
.newsletter-group .form-control {
  border-radius: 20px 0 0 20px;
  padding: 10px 15px;
  border: 1px solid rgba(0, 123, 255, 0.2);
}
.newsletter-group .btn {
  border-radius: 0 20px 20px 0;
  background: linear-gradient(135deg, #007bff, #0056b3);
  color: white;
  transition: all 0.3s ease;
  padding: 10px 20px;
  border: none;
}

.newsletter-group .btn:hover {
  background: linear-gradient(135deg, #0056b3, #004494);
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(0, 123, 255, 0.2);
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

.footer {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
  box-shadow: 0 -4px 20px rgba(0, 123, 255, 0.1);
}
</style>
