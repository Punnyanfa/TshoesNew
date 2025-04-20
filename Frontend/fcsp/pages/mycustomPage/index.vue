<template>
  <div class="container mt-5">
    <h1 class="text-center text-primary">Thiết kế của bạn</h1>

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
              <button class="btn btn-primary edit-btn" @click="editDesign(item)">Sửa</button>
              <button class="btn btn-success add-to-cart-btn" @click="duplicateToCart(item)">Thêm vào giỏ hàng</button>
              <button class="btn btn-info add-to-product-btn" @click="addToProduct(item)">Thêm vào sản phẩm</button>
              <button class="btn btn-danger delete-btn" @click="removeFromCart(item.id)">Xóa</button>
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
    </div>

    <div v-else class="text-center mt-5 empty-cart">
      <h4>Bạn chưa có thiết kế nào!</h4>
      <NuxtLink to="/customdetailPage" class="btn btn-primary mt-3">Bắt đầu thiết kế giày</NuxtLink>
    </div>
  </div>

  <!-- Modal cập nhật thông tin sản phẩm -->
  <div class="product-modal" v-if="showProductModal">
    <div class="product-modal-content">
      <div class="product-modal-header">
        <h3>Tạo sản phẩm từ thiết kế</h3>
        <button class="close-button" @click="showProductModal = false">×</button>
      </div>
      <div class="product-modal-body">
        <div class="product-preview">
          <img :src="selectedProduct.image" alt="Hình ảnh sản phẩm" class="product-preview-image" />
          
          <!-- Thêm phần xem trước các góc nhìn -->
          <div class="preview-angles-section" v-if="selectedProduct.previewImages && selectedProduct.previewImages.length > 1">
            <h4>Các góc nhìn</h4>
            <div class="preview-angles-container">
              <div v-for="(preview, index) in selectedProduct.previewImages" :key="index" 
                   class="preview-angle-item"
                   @click="selectedProduct.image = preview">
                <img :src="preview" alt="Góc nhìn" class="preview-angle-image" />
              </div>
            </div>
          </div>
        </div>
        
        <div class="product-form">
          <div class="form-group">
            <label for="productName">Tên sản phẩm:</label>
            <input type="text" id="productName" v-model="selectedProduct.name" class="form-control" />
          </div>
          
          <div class="form-group">
            <label for="productPrice">Giá (VNĐ):</label>
            <input type="number" id="productPrice" v-model="selectedProduct.price" class="form-control" />
          </div>
          
          <div class="form-group">
            <label for="productDescription">Mô tả:</label>
            <textarea id="productDescription" v-model="selectedProduct.description" class="form-control" rows="3"></textarea>
          </div>
          
          <div class="form-group" v-if="selectedProduct.designData && selectedProduct.designData.customText">
            <label for="customText">Văn bản tùy chỉnh:</label>
            <input type="text" id="customText" v-model="selectedProduct.designData.customText" class="form-control" />
          </div>
        </div>
        
        <div class="product-modal-actions">
          <button class="btn btn-secondary" @click="showProductModal = false">Hủy</button>
          <button class="btn btn-primary" @click="saveToProduct">Tạo sản phẩm</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue';

// 🛒 Load cart và drafts từ localStorage
const cart = ref([]);
const showProductModal = ref(false);
const selectedProduct = ref({
  id: 0,
  name: '',
  price: 0,
  image: '',
  description: '',
  designData: {
    customText: ''
  }
});

// Hiển thị/ẩn các góc nhìn
const togglePreviewImages = (item) => {
  item.showPreviews = !item.showPreviews;
};

// Chỉnh sửa thiết kế
const editDesign = (item) => {
  // Lưu thông tin sản phẩm đang chỉnh sửa vào localStorage
  localStorage.setItem('editingDesign', JSON.stringify(item));
  
  // Chuyển hướng đến trang thiết kế
  window.location.href = '/customdetailPage?edit=true&id=' + item.id;
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

// 🗑 Xóa sản phẩm khỏi giỏ hàng
const removeFromCart = (id) => {
  cart.value = cart.value.filter(item => item.id !== id);
};

// Tạo bản sao của thiết kế và thêm vào giỏ hàng
const duplicateToCart = (item) => {
  // Tạo một bản sao với ID mới
  const newItem = JSON.parse(JSON.stringify(item));
  newItem.id = Date.now();
  newItem.name = `${newItem.name} (Bản sao)`;
  
  // Thêm vào giỏ hàng
  cart.value.push(newItem);
  alert('Đã tạo bản sao thiết kế và thêm vào giỏ hàng!');
};

// Thêm thiết kế vào danh sách sản phẩm
const addToProduct = (item) => {
  // Chuẩn bị sản phẩm để hiển thị trong modal
  selectedProduct.value = {
    id: Date.now(),
    name: item.name,
    price: item.price,
    image: item.image,
    description: `Thiết kế tùy chỉnh từ ${item.name}`,
    designData: JSON.parse(JSON.stringify(item.designData || {})),
    previewImages: item.previewImages ? [...item.previewImages] : [],
    isCustomDesign: true
  };
  
  // Hiển thị modal
  showProductModal.value = true;
};

// Lưu thiết kế vào danh sách sản phẩm sau khi cập nhật thông tin
const saveToProduct = () => {
  // Lưu vào localStorage để có thể sử dụng trong trang sản phẩm
  const products = JSON.parse(localStorage.getItem('products') || '[]');
  
  // Thêm sản phẩm đã cập nhật vào danh sách
  products.push(selectedProduct.value);
  localStorage.setItem('products', JSON.stringify(products));
  
  // Đóng modal và hiển thị thông báo
  showProductModal.value = false;
  alert('Đã thêm thiết kế vào danh sách sản phẩm!');
};

// 🏗 Lưu giỏ hàng vào localStorage khi thay đổi
watch(cart, () => {
  localStorage.setItem('cart', JSON.stringify(cart.value));
}, { deep: true });

// 🔄 Khởi tạo cart và drafts từ localStorage
onMounted(() => {
  // Load cart
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
  
  // Load drafts
  const savedDrafts = localStorage.getItem('designDrafts');
  if (savedDrafts) {
    try {
      const parsedDrafts = JSON.parse(savedDrafts);
      // Thêm thuộc tính showPreviews cho mỗi item
      const draftsWithPreviews = parsedDrafts.map(item => ({
        ...item,
        showPreviews: false
      }));
      // Thêm drafts vào cart để hiển thị
      cart.value = [...cart.value, ...draftsWithPreviews];
    } catch (e) {
      console.error('Lỗi khi phân tích bản nháp:', e);
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
  width: 120px;
  height: 120px;
  border-radius: 8px;
  overflow: hidden;
  background-color: #f8f9fa;
  margin-right: 20px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
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
  font-size: 1.2rem;
  color: #333;
}

.price {
  font-size: 1.3rem;
  font-weight: bold;
  color: #e74c3c;
  margin: 8px 0;
}

.design-info {
  margin-top: 10px;
  font-size: 0.9rem;
}

.custom-text {
  background-color: #f8f9fa;
  padding: 6px 10px;
  border-radius: 4px;
  margin: 5px 0;
}

.timestamp {
  color: #777;
  margin: 5px 0;
}

.cart-item-actions {
  margin-left: 15px;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

/* Kiểu dáng chung cho tất cả các nút */
.btn {
  border-radius: 50px;
  font-weight: 600;
  padding: 8px 18px;
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

/* Thêm hiệu ứng gợn sóng khi click */
.btn::after {
  content: "";
  position: absolute;
  top: 50%;
  left: 50%;
  width: 5px;
  height: 5px;
  background: rgba(255, 255, 255, 0.5);
  opacity: 0;
  border-radius: 100%;
  transform: scale(1, 1) translate(-50%);
  transform-origin: 50% 50%;
}

.btn:active::after {
  animation: ripple-effect 0.6s ease-out;
}

@keyframes ripple-effect {
  0% {
    transform: scale(0, 0);
    opacity: 0.5;
  }
  20% {
    transform: scale(25, 25);
    opacity: 0.3;
  }
  100% {
    opacity: 0;
    transform: scale(40, 40);
  }
}

/* Thêm hiệu ứng shining effect */
.btn::before {
  content: "";
  position: absolute;
  top: 0;
  left: -100%;
  width: 50%;
  height: 100%;
  background: linear-gradient(to right, rgba(255,255,255,0) 0%, rgba(255,255,255,0.3) 100%);
  transform: skewX(-25deg);
  transition: all 0.75s;
}

.btn:hover::before {
  left: 125%;
}

/* Nút chỉnh sửa */
.edit-btn {
  background: linear-gradient(135deg, #007bff, #0056b3);
  color: white;
  border: none;
}

.edit-btn:hover {
  background: linear-gradient(135deg, #0069d9, #00489e);
}

.edit-btn::before {
  content: "";
  display: none;
}

/* Nút thêm vào giỏ hàng */
.add-to-cart-btn {
  background: linear-gradient(135deg, #28a745, #1e7e34);
  color: white;
  border: none;
}

.add-to-cart-btn:hover {
  background: linear-gradient(135deg, #218838, #186429);
}

.add-to-cart-btn::before {
  content: "";
  display: none;
}

/* Nút thêm vào sản phẩm */
.add-to-product-btn {
  background: linear-gradient(135deg, #17a2b8, #117a8b);
  color: white;
  border: none;
}

.add-to-product-btn:hover {
  background: linear-gradient(135deg, #138496, #0f6674);
}

.add-to-product-btn::before {
  content: "";
  display: none;
}

/* Nút xóa */
.delete-btn {
  background: linear-gradient(135deg, #dc3545, #bd2130);
  color: white;
}

.delete-btn:hover {
  background: linear-gradient(135deg, #c82333, #a71d2a);
}

.delete-btn::before {
  content: "";
  display: none;
}

/* Nút hiển thị góc nhìn */
.btn-outline-secondary {
  background-color: transparent;
  border: 2px solid #6c757d;
  color: #6c757d;
  font-weight: 500;
  transition: all 0.2s ease;
}

.btn-outline-secondary:hover {
  background-color: #6c757d;
  color: white;
}

/* Nút checkout */
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

.checkout-btn::before {
  content: "";
  display: none;
}

/* Phần hiển thị hình ảnh xem trước */
.preview-images-container {
  padding: 15px;
  background-color: #f8f9fa;
  border-top: 1px solid #eee;
  transition: all 0.3s ease;
}

.preview-images {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  justify-content: flex-start;
}

.preview-image-item {
  width: 90px;
  height: 90px;
  border-radius: 8px;
  overflow: hidden;
  background-color: white;
  border: 1px solid #ddd;
}

.preview-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

/* 📦 Phần footer giỏ hàng */
.cart-footer {
  background-color: #f8f9fa;
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
  margin-top: 30px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 20px;
}

.cart-footer h4 {
  font-size: 1.3rem;
  font-weight: bold;
  color: #333;
  margin: 0;
}

.cart-actions {
  display: flex;
  gap: 15px;
}

/* 🛒 Trường hợp giỏ hàng trống */
.empty-cart {
  color: #555;
  padding: 40px 0;
}

.empty-cart h4 {
  font-size: 1.5rem;
  font-weight: bold;
  color: #e74c3c;
  margin-bottom: 20px;
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

.empty-cart .btn::before {
  content: "";
  display: none;
}

.empty-cart .btn::after {
  content: "";
  position: absolute;
  top: 50%;
  left: 50%;
  width: 5px;
  height: 5px;
  background: rgba(255, 255, 255, 0.5);
  opacity: 0;
  border-radius: 100%;
  transform: scale(1, 1) translate(-50%);
  transform-origin: 50% 50%;
}

.empty-cart .btn:hover::after {
  animation: ripple 1s ease-out;
}

@keyframes ripple {
  0% {
    transform: scale(0, 0);
    opacity: 0.5;
  }
  20% {
    transform: scale(25, 25);
    opacity: 0.3;
  }
  100% {
    opacity: 0;
    transform: scale(40, 40);
  }
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
    flex-wrap: wrap;
    justify-content: flex-end;
    gap: 8px;
  }
  
  .cart-item-actions .btn {
    font-size: 0.85rem;
    padding: 6px 10px;
  }
  
  .cart-footer {
    flex-direction: column;
    align-items: center;
  }
  
  .cart-actions {
    width: 100%;
    flex-direction: column;
  }
  
  .checkout-btn, .cart-actions .btn {
    width: 100%;
    text-align: center;
  }
}

/* Product Modal */
.product-modal {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.7);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
  padding: 20px;
  animation: fadeIn 0.3s ease;
  backdrop-filter: blur(5px);
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

.product-modal-content {
  background-color: white;
  border-radius: 16px;
  width: 90%;
  max-width: 700px;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.25);
  animation: slideIn 0.3s ease;
  border: 1px solid rgba(255, 255, 255, 0.1);
}

@keyframes slideIn {
  from { transform: translateY(-50px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}

.product-modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px 25px;
  border-bottom: 1px solid #eee;
  background: linear-gradient(to right, #f9f9f9, #ffffff);
}

.product-modal-header h3 {
  margin: 0;
  color: #333;
  font-size: 1.5rem;
  font-weight: 700;
  background: linear-gradient(90deg, #333333, #666666);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.close-button {
  width: 36px;
  height: 36px;
  background: none;
  border: none;
  font-size: 28px;
  line-height: 1;
  color: #999;
  cursor: pointer;
  transition: all 0.3s ease;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.close-button:hover {
  color: #333;
  background-color: #f0f0f0;
  transform: rotate(90deg);
}

.product-modal-body {
  padding: 25px;
}

.product-preview {
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-bottom: 20px;
  background-color: #f9f9f9;
  border-radius: 8px;
  padding: 15px;
}

.product-preview-image {
  max-width: 100%;
  max-height: 200px;
  border-radius: 8px;
  object-fit: contain;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  margin-bottom: 15px;
}

/* Styles cho phần xem các góc nhìn */
.preview-angles-section {
  width: 100%;
  margin-top: 15px;
}

.preview-angles-section h4 {
  font-size: 1rem;
  color: #555;
  margin-bottom: 10px;
  text-align: center;
}

.preview-angles-container {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  justify-content: center;
}

.preview-angle-item {
  width: 70px;
  height: 70px;
  border-radius: 12px;
  overflow: hidden;
  cursor: pointer;
  border: 2px solid #eee;
  transition: all 0.3s ease;
  position: relative;
}

.preview-angle-item:hover {
  border-color: #007bff;
  transform: translateY(-5px) scale(1.05);
  box-shadow: 0 8px 15px rgba(0, 123, 255, 0.2);
  z-index: 1;
}

.preview-angle-item::after {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(rgba(0,0,0,0), rgba(0,0,0,0.3));
  opacity: 0;
  transition: opacity 0.3s;
}

.preview-angle-item:hover::after {
  opacity: 1;
}

.preview-angle-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.product-form {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-group label {
  font-weight: 600;
  color: #555;
  font-size: 0.9rem;
}

.form-control {
  padding: 10px 12px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
  transition: border-color 0.2s ease, box-shadow 0.2s ease;
}

.form-control:focus {
  border-color: #007bff;
  box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.15);
  outline: none;
}

.product-modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 15px;
  margin-top: 20px;
  padding-top: 15px;
  border-top: 1px solid #eee;
}

.product-modal-actions .btn {
  min-width: 120px;
}

.product-modal-actions .btn-primary {
  background: linear-gradient(135deg, #007bff, #0056b3);
  color: white;
  font-weight: 600;
  border: none;
}

.product-modal-actions .btn-primary:hover {
  background: linear-gradient(135deg, #0069d9, #004494);
  box-shadow: 0 5px 15px rgba(0, 123, 255, 0.3);
  transform: translateY(-3px);
}

.product-modal-actions .btn-primary::before {
  content: "";
  display: none;
}

.product-modal-actions .btn-secondary {
  background: linear-gradient(135deg, #6c757d, #545b62);
  color: white;
  font-weight: 500;
  border: none;
}

.product-modal-actions .btn-secondary:hover {
  background: linear-gradient(135deg, #5a6268, #4e555b);
  box-shadow: 0 5px 15px rgba(108, 117, 125, 0.3);
  transform: translateY(-3px);
}

.product-modal-actions .btn-secondary::before {
  content: "";
  display: none;
}

/* Responsive */
@media (max-width: 768px) {
  .product-modal-content {
    width: 100%;
    max-width: 100%;
  }
}
</style>
