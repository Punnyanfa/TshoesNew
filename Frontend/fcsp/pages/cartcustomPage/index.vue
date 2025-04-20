<template>
  <div>
    <Header />
    <div class="container mt-5">
      <h1 class="text-center text-primary">Giỏ hàng của bạn</h1>

      <div v-if="cart.length > 0">
        <!-- Giỏ hàng có sản phẩm -->
        <div class="cart-items mt-4">
          <div v-for="item in cart" :key="item.id" class="cart-item-card">
            <div class="cart-item-content">
              <!-- Hiển thị hình ảnh thiết kế -->
              <div class="cart-item-image">
                <img :src="item.image" alt="Thiết kế giày" class="product-image" />
              </div>
              
              <div class="cart-item-details">
                <h4>{{ item.name }}</h4>
                <p class="price">{{ formatPrice(item.price) }}</p>
                
                <!-- Hiển thị thông tin thiết kế nếu có -->
                <div v-if="item.designData" class="design-info">
                  <p v-if="item.designData.customText" class="custom-text">
                    <strong>Văn bản tùy chỉnh:</strong> {{ item.designData.customText }}
                  </p>
                  <p class="timestamp">
                    <small>Thiết kế vào: {{ formatDate(item.designData.timestamp) }}</small>
                  </p>
                </div>
                
                <!-- Nút hiển thị thêm ảnh từ các góc khác -->
                <div v-if="item.previewImages && item.previewImages.length > 1" class="mt-2">
                  <button class="btn btn-sm btn-outline-secondary" @click="togglePreviewImages(item)">
                    {{ item.showPreviews ? 'Ẩn' : 'Xem' }} tất cả góc nhìn
                  </button>
                </div>
              </div>

              <div class="cart-item-actions">
                <button class="btn btn-danger" @click="removeFromCart(item.id)">Xóa</button>
              </div>
            </div>
            
            <!-- Hiển thị các góc nhìn khác khi được nhấp -->
            <div v-if="item.showPreviews && item.previewImages" class="preview-images-container">
              <div class="preview-images">
                <div v-for="(preview, index) in item.previewImages" :key="index" class="preview-image-item">
                  <img :src="preview" alt="Góc nhìn" class="preview-image" />
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Tổng tiền và nút thanh toán -->
        <div class="cart-footer mt-4">
          <h4>Tổng tiền: <strong>{{ formatPrice(totalPrice) }}</strong></h4>
          <div class="cart-actions">
            <NuxtLink to="/productPage" class="btn btn-outline-primary">Tiếp tục mua sắm</NuxtLink>
            <NuxtLink to="/checkout" class="btn btn-success checkout-btn">Tiến hành thanh toán</NuxtLink>
          </div>
        </div>
      </div>

      <div v-else class="text-center mt-5 empty-cart">
        <h4>Giỏ hàng của bạn đang trống!</h4>
        <NuxtLink to="/customdetailPage" class="btn btn-primary mt-3">Bắt đầu thiết kế giày</NuxtLink>
      </div>
    </div>
    <Footer />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue';
import Header from '~/components/Header.vue';
import Footer from '~/components/Footer.vue';

// 🛒 Load cart từ localStorage
const cart = ref([]);

// Hiển thị/ẩn các góc nhìn
const togglePreviewImages = (item) => {
  item.showPreviews = !item.showPreviews;
};

// Định dạng giá tiền VND
const formatPrice = (price) => {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price);
};

// Định dạng ngày giờ
const formatDate = (dateString) => {
  if (!dateString) return '';
  const date = new Date(dateString);
  return date.toLocaleString('vi-VN');
};

// Tính tổng tiền
const totalPrice = computed(() => cart.value.reduce((sum, item) => sum + (item.price || 0), 0));

// 🗑 Xóa sản phẩm khỏi giỏ hàng
const removeFromCart = (id) => {
  cart.value = cart.value.filter(item => item.id !== id);
};

// 🏗 Lưu giỏ hàng vào localStorage khi thay đổi
watch(cart, () => {
  localStorage.setItem('cart', JSON.stringify(cart.value));
}, { deep: true });

// 🔄 Khởi tạo cart từ localStorage
onMounted(() => {
  const savedCart = localStorage.getItem('cart');
  if (savedCart) {
    try {
      const parsedCart = JSON.parse(savedCart);
      // Thêm thuộc tính showPreviews cho mỗi item
      cart.value = parsedCart.map(item => ({
        ...item,
        showPreviews: false
      }));
    } catch (e) {
      console.error('Lỗi khi phân tích giỏ hàng:', e);
      cart.value = [];
    }
  }
});
</script>

<style scoped>
/* 🌟 Cấu trúc và màu sắc */
.container {
  max-width: 900px;
  margin: 0 auto;
  padding: 0 15px;
}

h1 {
  font-size: 2.5rem;
  font-weight: bold;
  color: #007bff;
  margin-bottom: 30px;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.1);
}

/* 🛒 Giỏ hàng */
.cart-items {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.cart-item-card {
  border-radius: 12px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
  overflow: hidden;
  background-color: #fff;
  transition: all 0.3s ease;
  border: 1px solid rgba(0, 0, 0, 0.05);
}

.cart-item-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
}

.cart-item-content {
  display: flex;
  padding: 20px;
  align-items: center;
}

.cart-item-image {
  width: 140px;
  height: 140px;
  border-radius: 10px;
  overflow: hidden;
  background-color: #f8f9fa;
  margin-right: 25px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;
}

.cart-item-image:hover {
  transform: scale(1.05);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.15);
}

.product-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.cart-item-details {
  flex: 1;
}

.cart-item-details h4 {
  margin: 0;
  font-size: 1.4rem;
  color: #333;
  font-weight: 600;
}

.price {
  font-size: 1.5rem;
  font-weight: bold;
  color: #e74c3c;
  margin: 10px 0;
  letter-spacing: 0.5px;
}

.design-info {
  margin-top: 12px;
  font-size: 0.95rem;
  color: #555;
}

.custom-text {
  background-color: #f8f9fa;
  padding: 8px 12px;
  border-radius: 6px;
  margin: 8px 0;
  border-left: 3px solid #007bff;
}

.timestamp {
  color: #777;
  margin: 8px 0;
  font-style: italic;
}

.cart-item-actions {
  margin-left: 20px;
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.btn {
  border-radius: 50px;
  font-weight: 600;
  padding: 10px 20px;
  transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
  position: relative;
  overflow: hidden;
  border: none;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  text-transform: uppercase;
  font-size: 0.85rem;
  letter-spacing: 0.5px;
}

.btn:hover {
  transform: translateY(-3px);
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.15);
}

.btn:active {
  transform: translateY(1px);
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
}

.btn-danger {
  background: linear-gradient(135deg, #dc3545, #bd2130);
  color: white;
  border: none;
}

.btn-danger:hover {
  background: linear-gradient(135deg, #c82333, #a71d2a);
  box-shadow: 0 5px 15px rgba(220, 53, 69, 0.3);
}

/* Phần hiển thị hình ảnh xem trước */
.preview-images-container {
  padding: 20px;
  background-color: #f8f9fa;
  border-top: 1px solid #eee;
  transition: all 0.3s ease;
}

.preview-images {
  display: flex;
  flex-wrap: wrap;
  gap: 12px;
  justify-content: flex-start;
}

.preview-image-item {
  width: 100px;
  height: 100px;
  border-radius: 10px;
  overflow: hidden;
  background-color: white;
  border: 1px solid #ddd;
  transition: all 0.3s ease;
}

.preview-image-item:hover {
  transform: scale(1.05);
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
  border-color: #007bff;
}

.preview-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

/* 📦 Phần footer giỏ hàng */
.cart-footer {
  background: linear-gradient(to right, #f9f9f9, #ffffff);
  padding: 25px;
  border-radius: 15px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
  margin-top: 30px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 20px;
  border: 1px solid rgba(0, 0, 0, 0.05);
}

.cart-footer h4 {
  font-size: 1.4rem;
  font-weight: bold;
  color: #333;
  margin: 0;
}

.cart-actions {
  display: flex;
  gap: 15px;
}

.btn-outline-primary {
  background: transparent;
  border: 2px solid #007bff;
  color: #007bff;
  font-weight: 600;
  transition: all 0.3s ease;
}

.btn-outline-primary:hover {
  background-color: #007bff;
  color: white;
  box-shadow: 0 5px 15px rgba(0, 123, 255, 0.2);
}

.checkout-btn {
  background: linear-gradient(135deg, #28a745, #20c997);
  font-weight: bold;
  color: white;
  padding: 12px 30px;
  border-radius: 50px;
  box-shadow: 0 4px 15px rgba(40, 167, 69, 0.3);
  transition: all 0.3s ease;
  border: none;
  position: relative;
  overflow: hidden;
}

.checkout-btn:hover {
  background: linear-gradient(135deg, #218838, #1aa179);
  transform: translateY(-3px);
  box-shadow: 0 7px 20px rgba(40, 167, 69, 0.4);
}

/* 🛒 Trường hợp giỏ hàng trống */
.empty-cart {
  color: #555;
  padding: 60px 0;
  background-color: #f9f9f9;
  border-radius: 15px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05);
  margin-top: 20px;
}

.empty-cart h4 {
  font-size: 1.6rem;
  font-weight: bold;
  color: #e74c3c;
  margin-bottom: 25px;
}

.empty-cart .btn {
  background: linear-gradient(135deg, #3498db, #2980b9);
  color: white;
  border-radius: 50px;
  padding: 12px 30px;
  text-transform: uppercase;
  font-weight: bold;
  transition: all 0.3s ease;
  border: none;
  box-shadow: 0 4px 15px rgba(52, 152, 219, 0.3);
  position: relative;
  overflow: hidden;
}

.empty-cart .btn:hover {
  background: linear-gradient(135deg, #2980b9, #2471a3);
  transform: translateY(-3px);
  box-shadow: 0 8px 25px rgba(52, 152, 219, 0.4);
}

/* Responsive */
@media (max-width: 768px) {
  .cart-item-content {
    flex-direction: column;
    align-items: flex-start;
  }
  
  .cart-item-image {
    width: 100%;
    height: 200px;
    margin: 0 0 15px 0;
  }
  
  .cart-item-actions {
    margin: 15px 0 0 0;
    align-self: flex-end;
    flex-direction: row;
    gap: 10px;
  }
  
  .cart-footer {
    flex-direction: column;
    align-items: center;
    text-align: center;
  }
  
  .cart-actions {
    width: 100%;
    flex-direction: column;
  }
  
  .checkout-btn, .cart-actions .btn {
    width: 100%;
    text-align: center;
  }
  
  .preview-image-item {
    width: 80px;
    height: 80px;
  }
}
</style>
