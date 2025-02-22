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
            />
            <button class="btn btn-success">Subscribe</button>
          </div>
          <p class="mt-3 text-secondary">
            Get the latest news and updates directly to your inbox.
          </p>
        </div>
      </div>

      <!-- Quick Links Section -->
      <div class="row mb-4">
        <div class="col-md-3">
          <h6 class="fw-bold mb-3">CUSTOM LINKS</h6>
          <ul class="list-unstyled">
            <li><a href="#" class="text-light">Privacy Policy</a></li>
            <li><a href="#" class="text-light">Order Status</a></li>
            <li><a href="#" class="text-light">Returns & Exchanges</a></li>
            <li><a href="#" class="text-light">Size Guide</a></li>
            <li><a href="#" class="text-light">Cart</a></li>
            <li><a href="#" class="text-light">FAQ</a></li>
          </ul>
        </div>

        <div class="col-md-3">
          <h6 class="fw-bold mb-3">PRODUCTS</h6>
          <ul class="list-unstyled">
            <li><a href="#" class="text-light">Prices drop</a></li>
            <li><a href="#" class="text-light">New products</a></li>
            <li><a href="#" class="text-light">Best sales</a></li>
            <li><a href="#" class="text-light">Contact us</a></li>
            <li><a href="#" class="text-light">Sitemap</a></li>
            <li><a href="#" class="text-light">Stores</a></li>
          </ul>
        </div>

        <div class="col-md-3">
          <h6 class="fw-bold mb-3">OUR COMPANY</h6>
          <ul class="list-unstyled">
            <li><a href="#" class="text-light">Delivery</a></li>
            <li><a href="#" class="text-light">Legal Notice</a></li>
            <li><a href="#" class="text-light">About us</a></li>
            <li><a href="#" class="text-light">Secure payment</a></li>
            <li><a href="#" class="text-light">Contact us</a></li>
            <li><a href="#" class="text-light">Sitemap</a></li>
          </ul>
        </div>

        <div class="col-md-3">
          <h6 class="fw-bold mb-3">YOUR ACCOUNT</h6>
          <ul class="list-unstyled">
            <li><a href="#" class="text-light">Personal info</a></li>
            <li><a href="#" class="text-light">Orders</a></li>
            <li><a href="#" class="text-light">Credit slips</a></li>
            <li><a href="#" class="text-light">Addresses</a></li>
            <li><a href="#" class="text-light">Stores</a></li>
            <li><a href="#" class="text-light">FAQ</a></li>
          </ul>
        </div>
      </div>

      <!-- Contact Information & Copyright Section -->
      <div class="text-center border-top pt-4">
        <p>
          ADDRESS: 123 Main Street, New York | EMAIL: demo@example.com | PHONE: (123) 456-7890
        </p>
        <small>&copy; {{ currentYear }} Pullman. Made with ❤️ by HasThemes</small>
      </div>
    </div>
  </footer>
</template>

<script setup>
const currentYear = new Date().getFullYear();
</script>

<style scoped>
/* Footer Links Hover Effect */
.footer a {
  transition: color 0.3s ease;
}
.footer a:hover {
  color: #8bc34a;
  text-decoration: underline;
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
  background-color: #8bc34a;
  color: white;
  transition: background-color 0.3s ease;
}
.newsletter-group .btn:hover {
  background-color: #7cb342;
}
</style>
