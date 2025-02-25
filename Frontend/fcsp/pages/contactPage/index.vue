<!-- Contact Us Template - Custom Sneaker Theme -->
<template>
  <div class="contact-wrapper">
    <!-- Header Component -->
    <Header />

    <!-- Main Content Section -->
    <section class="container mt-5 py-5">
      <h1 class="text-center display-4 fw-bold text-sneaker-orange animate__animated animate__fadeIn">
        Design Your Dream Kicks
      </h1>
      <p class="text-center text-muted mb-5">
        Got a wild idea for your next pair? Drop us a line, and let’s make it happen!
      </p>

      <div class="row justify-content-center">
        <div class="col-md-8 col-lg-6">
          <form @submit.prevent="sendMessage" class="contact-form shadow-lg p-4 rounded">
            <div class="mb-4 position-relative">
              <label class="form-label fw-bold">Your Name</label>
              <input 
                v-model="form.name" 
                class="form-control form-control-lg sneaker-input" 
                placeholder="Who’s rocking these kicks?" 
                required 
              />
              <span class="input-icon"><i class="bi bi-person-fill"></i></span>
            </div>
            <div class="mb-4 position-relative">
              <label class="form-label fw-bold">Email</label>
              <input 
                v-model="form.email" 
                type="email" 
                class="form-control form-control-lg sneaker-input" 
                placeholder="Where should we lace you up?" 
                required 
              />
              <span class="input-icon"><i class="bi bi-envelope-fill"></i></span>
            </div>
            <div class="mb-4 position-relative">
              <label class="form-label fw-bold">Your Vision</label>
              <textarea 
                v-model="form.message" 
                class="form-control form-control-lg sneaker-input" 
                rows="5" 
                placeholder="Tell us about your dream design..." 
                required
              ></textarea>
              <span class="input-icon"><i class="bi bi-brush-fill"></i></span>
            </div>
            <button 
              type="submit" 
              class="btn btn-sneaker w-100 py-3 fw-bold text-uppercase"
            >
              Send Your Style
            </button>
          </form>
        </div>
      </div>
    </section>

    <!-- Footer Component -->
    <Footer />
  </div>
</template>

<script setup>
import { ref } from 'vue';

const form = ref({ name: '', email: '', message: '' });

const sendMessage = () => {
  alert('Your sneaker vibe has been sent!');
  form.value = { name: '', email: '', message: '' };
};
</script>

<style scoped>
.contact-wrapper {
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  min-height: 100vh;
}

.text-sneaker-orange {
  color: #8bc34a;
}

.contact-form {
  background: #fff;
  border-radius: 15px;
  transition: transform 0.3s ease;
}

.contact-form:hover {
  transform: translateY(-5px);
}

.sneaker-input {
  border: 2px solid #8bc34a;
  border-radius: 10px;
  padding-left: 40px; /* Space for icon */
  transition: border-color 0.3s ease;
}

.sneaker-input:focus {
  border-color:#8bc34a;
  box-shadow: 0 0 10px #8bc34a;
}

.input-icon {
  position: absolute;
  left: 15px;
  top: 50%;
  transform: translateY(-50%);
  color:#8bc34a;
}

.btn-sneaker {
  background:#8bc34a;
  color: #fff;
  border: none;
  border-radius: 10px;
  transition: background 0.3s ease, transform 0.2s ease;
}

.btn-sneaker:hover {
  background:#8bc34a;
  transform: scale(1.05);
}

/* Responsive adjustments */
@media (max-width: 768px) {
  .contact-form {
    padding: 20px;
  }
  .display-4 {
    font-size: 2.5rem;
  }
}
</style>