<template>
  <div class="contact-wrapper">
    <Header />
    
    <!-- Main Section -->
    <section class="container mt-5 py-5">
      <div class="row g-3 align-items-center">
        <!-- Left Column: Contact Info and Map -->
        <div class="col-lg-6 mb-0">
          <h1 class="title mb-4">Say Hello.</h1>
            <p class="subtitle mb-4">
              If there are any mistakes or questions, please contact us, and we will assist you.
            </p>
            <p class="contact-info mb-4">
              290 Nơ Trang Long, P12, Quận Bình Thạnh, Thành phố Hồ Chí Minh<br />
              <a href="mailto:tunghoang5161997@gmail.com">tunghoang5161997@gmail.com</a>
            </p>
          <div class="map-container">
            <iframe
              src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3919.447979917851!2d106.7043!3d10.7998!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x31752f38f9a4d8e5%3A0x6c91b7b3d7f0b8e!2s290%20N%C3%B4%20Trang%20Long%2C%20P12%2C%20Qu%E1%BA%ADn%20B%C3%ADnh%20Th%E1%BA%A1nh%2C%20Th%C3%A0nh%20ph%E1%BB%91%20H%E1%BB%93%20Ch%C3%AD%20Minh!5e0!3m2!1sen!2sus!4v1697041234567!5m2!1sen!2sus"
              width="100%"
              height="300"
              style="border:0;"
              allowfullscreen=""
              loading="lazy"
              referrerpolicy="no-referrer-when-downgrade"
            ></iframe>
          </div>
        </div>

        <!-- Right Column: Form -->
        <div class="col-lg-6">
          <div class="form-wrapper">
            <h2 class="form-title mb-4">Send Us A Message</h2>

            <form @submit.prevent="sendMessage" class="contact-form">
              <div class="row mb-4">
                <div class="col-md-6">
                  <label class="form-label">First Name</label>
                  <input
                    v-model="form.firstName"
                    class="form-control"
                    placeholder="First Name"
                    required
                  />
                </div>
                <div class="col-md-6">
                  <label class="form-label">Last Name</label>
                  <input
                    v-model="form.lastName"
                    class="form-control"
                    placeholder="Last Name"
                    required
                  />
                </div>
              </div>
              <div class="form-group mb-4">
                <label class="form-label">Your Email</label>
                <input
                  v-model="form.email"
                  type="email"
                  class="form-control"
                  placeholder="Your Email"
                  required
                />
              </div>
              <div class="form-group mb-4">
                <label class="form-label">Subject</label>
                <input
                  v-model="form.subject"
                  class="form-control"
                  placeholder="Subject"
                  required
                />
              </div>
              <div class="form-group mb-5">
                <label class="form-label">Message</label>
                <textarea
                  v-model="form.message"
                  class="form-control"
                  rows="5"
                  placeholder="Message"
                  required
                ></textarea>
              </div>
              <button
                type="submit"
                class="btn btn-primary w-100"
              >
                Send Message
                <i class="fas fa-arrow-right ms-2"></i>
              </button>
            </form>
          </div>
        </div>
      </div>
    </section>

    <!-- Footer Section with Phone Numbers -->
    <section class="footer-contact py-4">
      <div class="container">
        <div class="row text-center">
          <div class="col-md-4">
            <h6>Business</h6>
            <p>123 456 7890</p>
          </div>
          <div class="col-md-4">
            <h6>Orders</h6>
            <p>123 456 7890</p>
          </div>
          <div class="col-md-4">
            <h6>Support</h6>
            <p>123 456 7890</p>
          </div>
        </div>
      </div>
    </section>

    <Footer />
  </div>
</template>

<script setup>
import { ref } from 'vue';

const form = ref({
  firstName: '',
  lastName: '',
  email: '',
  subject: '',
  message: ''
});

const sendMessage = () => {
  alert('Your message has been sent successfully! We will respond within 24 hours.');
  form.value = { firstName: '', lastName: '', email: '', subject: '', message: '' };
};
</script>

<style scoped>
.contact-wrapper {
  background: linear-gradient(135deg, #f5f7fa 0%, #e4e9f2 100%);
  min-height: 100vh;
  font-family: 'Poppins', sans-serif;
  position: relative;
  overflow: hidden;
}

.contact-wrapper::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: radial-gradient(circle, rgba(255, 255, 255, 0.1) 10%, transparent 10%);
  background-size: 30px 30px;
  opacity: 0.5;
  z-index: 0;
}

.container {
  position: relative;
  z-index: 1;
}

.title {
  font-size: 3.5rem;
  font-weight: 800;
  background: linear-gradient(45deg, #2c3e50, #3498db);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  letter-spacing: 1px;
  transition: transform 0.3s ease;
}

.title:hover {
  transform: translateY(-3px);
}

.subtitle {
  color: #7f8c8d;
  font-size: 1.1rem;
  line-height: 1.6;
  font-weight: 400;
  opacity: 0.9;
}

.contact-info {
  font-size: 1.1rem;
  color: #34495e;
  line-height: 1.6;
  font-weight: 500;
}

.contact-info a {
  color: #3498db;
  text-decoration: none;
  transition: color 0.3s ease;
}

.contact-info a:hover {
  color: #2980b9;
  text-decoration: underline;
}

.map-container {
  border-radius: 15px;
  box-shadow: 0 15px 40px rgba(0, 0, 0, 0.15);
  overflow: hidden;
  position: relative;
}

.map-container::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(to bottom, rgba(0, 0, 0, 0.1), transparent);
  pointer-events: none;
}

.form-wrapper {
  background: #ffffff;
  padding: 40px;
  border-radius: 15px;
  box-shadow: 0 15px 40px rgba(0, 0, 0, 0.15);
  animation: fadeInUp 0.5s ease;
  border: 1px solid rgba(0, 0, 0, 0.05);
}

.form-title {
  font-size: 1.8rem;
  color: #2c3e50;
  font-weight: 700;
  letter-spacing: 0.5px;
}

.form-label {
  color: #34495e;
  font-weight: 500;
  font-size: 0.95rem;
  margin-bottom: 8px;
  display: block;
  transition: color 0.3s ease;
}

.form-control {
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 12px 15px;
  background: #f9f9f9;
  font-size: 1rem;
  transition: all 0.3s ease;
}

.form-control:focus {
  border-color: #3498db;
  background: #fff;
  box-shadow: 0 0 10px rgba(52, 152, 219, 0.2);
  outline: none;
}

.form-control:hover {
  border-color: #bdc3c7;
}

.btn-primary {
  background: linear-gradient(45deg, #3498db, #1abc9c);
  border: none;
  padding: 12px;
  border-radius: 8px;
  font-weight: 600;
  font-size: 1.1rem;
  transition: all 0.3s ease;
  box-shadow: 0 5px 15px rgba(52, 152, 219, 0.3);
}

.btn-primary:hover {
  background: linear-gradient(45deg, #2980b9, #16a085);
  transform: translateY(-3px);
  box-shadow: 0 8px 20px rgba(52, 152, 219, 0.4);
}

.btn-primary:active {
  transform: translateY(0);
  box-shadow: 0 3px 10px rgba(52, 152, 219, 0.2);
}

.footer-contact {
  background: linear-gradient(135deg, #f5f7fa 0%, #e4e9f2 100%);
  border-top: 1px solid rgba(0, 0, 0, 0.05);
  padding: 30px 0;
}

.footer-contact h6 {
  font-size: 1.1rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 8px;
  transition: color 0.3s ease;
}

.footer-contact h6:hover {
  color: #3498db;
}

.footer-contact p {
  font-size: 1rem;
  color: #7f8c8d;
  margin: 0;
  transition: color 0.3s ease;
}

.footer-contact p:hover {
  color: #34495e;
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@media (max-width: 992px) {
  .title {
    font-size: 2.5rem;
  }

  .form-wrapper {
    padding: 25px;
  }

  .map-container iframe {
    height: 200px;
  }

  .form-title {
    font-size: 1.5rem;
  }

  .btn-primary {
    font-size: 1rem;
    padding: 10px;
  }
}
</style>