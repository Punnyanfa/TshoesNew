<!-- Contact Us Template - Custom Shoe Theme -->
<template>
  <div class="contact-wrapper">
    <Header />
    
      <section class="container mt-5 py-5">
        <div class="row g-3 align-items-center"> <!-- Thay g-5 thành g-3 -->
          <!-- Left Column: Image with Overlay -->
          <div class="col-lg-6 mb-0"> <!-- Xóa mb-4 mb-lg-0 -->
            <div class="image-container">
            <img 
              src="data:image/jpeg;base64,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" 
              alt="Custom Shoe Product" 
              class="img-fluid rounded-3 shadow-lg"
              />
                <div class="image-overlay">
                  <h3 class="text-white">Thiết Kế Giày Của Riêng Bạn</h3>
                </div>
              </div>
            </div>

        <!-- Right Column: Enhanced Form -->
        <div class="col-lg-6">
          <div class="form-wrapper">
            <h1 class="title mb-4">Liên Hệ Với Chúng Tôi</h1>
            <p class="subtitle mb-5">
              Hãy chia sẻ ý tưởng hoặc câu hỏi của bạn - chúng tôi luôn sẵn sàng hỗ trợ!
            </p>

            <form @submit.prevent="sendMessage" class="contact-form">
              <div class="form-group mb-4">
                <label class="form-label">Họ và Tên</label>
                <input 
                  v-model="form.name" 
                  class="form-control" 
                  placeholder="Nhập tên của bạn" 
                  required 
                />
              </div>
              <div class="form-group mb-4">
                <label class="form-label">E-mail</label>
                <input 
                  v-model="form.email" 
                  type="email" 
                  class="form-control" 
                  placeholder="email@example.com" 
                  required 
                />
              </div>
              <div class="form-group mb-5">
                <label class="form-label">Tin Nhắn</label>
                <textarea 
                  v-model="form.message" 
                  class="form-control" 
                  rows="5" 
                  placeholder="Viết ý tưởng hoặc câu hỏi của bạn..." 
                  required
                ></textarea>
              </div>
              <button 
                type="submit" 
                class="btn btn-primary w-100"
              >
                Gửi Tin Nhắn
                <i class="fas fa-paper-plane ms-2"></i>
              </button>
            </form>
          </div>
        </div>
      </div>
    </section>

    <Footer />
  </div>
</template>

<script setup>
import { ref } from 'vue';

const form = ref({ name: '', email: '', message: '' });

const sendMessage = () => {
  alert('Tin nhắn của bạn đã được gửi thành công! Chúng tôi sẽ phản hồi trong 24h.');
  form.value = { name: '', email: '', message: '' };
};
</script>

<style scoped>
.contact-wrapper {
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  min-height: 100vh;
  font-family: 'Poppins', sans-serif;
}

.image-container {
  position: relative;
  overflow: hidden;
  border-radius: 15px;
  margin-right: -15px; /* Kéo hình ảnh sát hơn về phía biểu mẫu */
}

.img-fluid {
  height: 725px;
  object-fit: cover;
  transition: transform 0.5s ease;
}

.image-container:hover .img-fluid {
  transform: scale(1.05);
}

.image-overlay {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 20px;
  background: linear-gradient(to top, rgba(0,0,0,0.7), transparent);
}

.form-wrapper {
background: #ffffff;
  padding: 20px; /* Giảm từ 40px xuống 20px */
  border-radius: 15px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
  animation: fadeInUp 0.5s ease;
}

.title {
  font-size: 2.5rem;
  color: #1a1a1a;
  font-weight: 700;
  text-align: center;
  background: linear-gradient(45deg, #007bff, #00d4ff);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.subtitle {
  color: #666;
  font-size: 1.1rem;
  text-align: center;
}

.form-label {
  color: #333;
  font-weight: 600;
  margin-bottom: 8px;
  display: block;
}

.form-control {
  border: none;
  border-radius: 10px;
  padding: 15px;
  background: #f8f9fa;
  transition: all 0.3s ease;
  font-size: 1rem;
}

.form-control:focus {
  background: #fff;
  box-shadow: 0 0 15px rgba(0, 123, 255, 0.2);
  border-color: #007bff;
}

.btn-primary {
  background: linear-gradient(45deg, #007bff, #00d4ff);
  border: none;
  padding: 15px;
  border-radius: 50px;
  font-weight: 600;
  font-size: 1.1rem;
  transition: all 0.3s ease;
}

.btn-primary:hover {
  transform: translateY(-3px);
  box-shadow: 0 5px 15px rgba(0, 123, 255, 0.4);
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
.form-wrapper {
    padding: 15px; /* Giảm thêm trên màn hình nhỏ */
  }

  .image-container {
    margin-right: 0; /* Reset trên màn hình nhỏ */
  }
  
  .title {
    font-size: 2rem;
  }
  
  .img-fluid {
    height: 400px;
  }
}
</style>