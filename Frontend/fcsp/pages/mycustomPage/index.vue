<template>
  <div>
    <Header />
    <div class="container mt-5">
      <h1 class="text-center text-primary">Thiết kế của bạn</h1>

      <div v-if="cart.length > 0">
        <!-- Tiêu đề và nút xóa hết -->
        <div class="d-flex justify-content-between align-items-center mb-3">
          <h5>Danh sách thiết kế ({{ cart.length }})</h5>
          <button class="btn btn-outline-danger btn-sm" @click="clearAllDesigns">
            <i class="fas fa-trash-alt mr-1"></i> Xóa tất cả
          </button>
        </div>
        
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
                <p v-if="item.surcharge && item.surcharge > 0" class="price surcharge">Phụ phí: {{ formatPrice(item.surcharge) }}</p>
                <p v-if="item.surcharge && item.surcharge > 0" class="price total">Tổng: {{ formatPrice(item.price + item.surcharge) }}</p>
                
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
                <div v-for="(preview, index) in item.previewImages" :key="`preview_${item.id}_${index}`" class="preview-image-item">
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
                  <div v-for="(preview, index) in selectedProduct.previewImages" :key="`modal_preview_${index}`" 
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
    </div>
    <Footer />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue';
import Header from '~/components/Header.vue';
import Footer from '~/components/Footer.vue';

// Hàm giới hạn kích thước dữ liệu của giỏ hàng
const limitCartSize = (cartData) => {
  // Tạo bản sao để không ảnh hưởng đến dữ liệu gốc
  const limitedCart = JSON.parse(JSON.stringify(cartData));
  
  // Giới hạn số lượng phần tử trong giỏ hàng nếu quá lớn
  if (limitedCart.length > 10) {
    console.warn('Giỏ hàng có quá nhiều mục. Giữ 10 mục mới nhất.');
    limitedCart.splice(0, limitedCart.length - 10);
  }
  
  // Tối ưu dữ liệu của mỗi mục để giảm kích thước
  limitedCart.forEach(item => {
    // Giới hạn kích thước của previewImages thành tối đa 1 ảnh
    if (item.previewImages && item.previewImages.length > 1) {
      item.previewImages = [item.previewImages[0]];
    }
    
    // Loại bỏ các thuộc tính không cần thiết nếu có
    if (item.designData) {
      // Chỉ giữ lại các thuộc tính cần thiết
      const minimalDesignData = {
        customText: item.designData.customText || '',
        timestamp: item.designData.timestamp || new Date().toISOString()
      };
      item.designData = minimalDesignData;
    }
    
    // Loại bỏ các dữ liệu tạm thời không cần thiết
    delete item.showPreviews;
    delete item.selected;
    delete item.expanded;
    delete item.temporary;
  });
  
  return limitedCart;
};

// 🛒 Load cart và drafts từ localStorage
const cart = ref([]);
const showProductModal = ref(false);
const selectedProduct = ref({
  name: '',
  price: 0,
  image: '',
  description: '',
  designData: {
    customText: ''
  }
});

// Theo dõi modal để khóa/mở scroll
watch(showProductModal, (newValue) => {
  if (newValue) {
    // Khóa scroll khi modal hiển thị
    document.body.style.overflow = 'hidden';
  } else {
    // Khôi phục scroll khi modal đóng
    document.body.style.overflow = '';
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

// Hàm làm mới dữ liệu từ localStorage
const refreshDataFromStorage = () => {
  // Xóa cart hiện tại
  cart.value = [];
  
  // Tải lại dữ liệu từ localStorage
  const savedCart = localStorage.getItem('cart');
  if (savedCart) {
    try {
      const parsedCart = JSON.parse(savedCart);
      cart.value = parsedCart.map(item => ({
        ...item,
        showPreviews: false
      }));
    } catch (e) {
      console.error('Lỗi khi làm mới dữ liệu giỏ hàng:', e);
    }
  }
  
  // Kiểm tra dữ liệu bản nháp
  const savedDrafts = localStorage.getItem('designDrafts');
  if (savedDrafts) {
    try {
      const parsedDrafts = JSON.parse(savedDrafts);
      if (parsedDrafts && Array.isArray(parsedDrafts) && parsedDrafts.length > 0) {
        const existingIds = new Set(cart.value.map(item => item.id));
        const uniqueDrafts = parsedDrafts
          .filter(item => item && !existingIds.has(item.id))
          .map(item => ({
            ...item,
            id: item.id || `draft_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`,
            showPreviews: false
          }));
          
        cart.value = [...cart.value, ...uniqueDrafts];
      }
    } catch (e) {
      console.error('Lỗi khi làm mới dữ liệu bản nháp:', e);
    }
  }
  
  console.log(`Đã làm mới dữ liệu: ${cart.value.length} thiết kế`);
};

// 🗑 Xóa sản phẩm khỏi giỏ hàng
function removeFromCart(id) {
  try {
    // Xóa sản phẩm khỏi cart trong state
    cart.value = cart.value.filter(item => item.id !== id);
    
    // Xóa khỏi tất cả local storage liên quan
    
    // 1. Xóa khỏi designDrafts
    const designDrafts = JSON.parse(localStorage.getItem('designDrafts') || '[]');
    const updatedDrafts = designDrafts.filter(draft => draft.id !== id);
    localStorage.setItem('designDrafts', JSON.stringify(updatedDrafts));
    
    // 2. Xóa khỏi cart trong localStorage
    const storedCart = JSON.parse(localStorage.getItem('cart') || '[]');
    const updatedCart = storedCart.filter(item => item.id !== id);
    localStorage.setItem('cart', JSON.stringify(updatedCart));
    
    // 3. Xóa khỏi products trong localStorage
    const products = JSON.parse(localStorage.getItem('products') || '[]');
    const updatedProducts = products.filter(product => product.id !== id);
    localStorage.setItem('products', JSON.stringify(updatedProducts));
    
    // 4. Xóa bất kỳ key nào có chứa ID sản phẩm
    for (let i = 0; i < localStorage.length; i++) {
      const key = localStorage.key(i);
      try {
        const value = JSON.parse(localStorage.getItem(key) || '{}');
        // Nếu key chứa mảng, kiểm tra xem có phần tử nào có ID cần xóa không
        if (Array.isArray(value)) {
          const updated = value.filter(item => item.id !== id);
          if (updated.length !== value.length) {
            localStorage.setItem(key, JSON.stringify(updated));
          }
        }
        // Nếu key chứa object và có thuộc tính id trùng khớp
        else if (value && typeof value === 'object' && value.id === id) {
          localStorage.removeItem(key);
        }
      } catch (e) {
        // Bỏ qua nếu không phải JSON
      }
    }
    
    console.log(`Đã xóa sản phẩm ID: ${id} khỏi tất cả bộ nhớ`);
    
    // Làm mới trang sau khi xóa để đảm bảo dữ liệu được cập nhật
    setTimeout(() => {
      window.location.reload();
    }, 300);
  } catch (e) {
    console.error('Lỗi khi xóa sản phẩm:', e);
    alert('Có lỗi xảy ra khi xóa sản phẩm. Vui lòng thử lại.');
  }
}

// Thêm thiết kế vào giỏ hàng
const duplicateToCart = (item) => {
  try {
    // Lấy giỏ hàng hiện tại
    const cartItems = JSON.parse(localStorage.getItem('cart') || '[]');
    
    // Tạo bản sao của item với ID mới để tránh trùng lặp
    const newCartItem = {
      ...item,
      id: `cart_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`,
      showPreviews: false
    };
    
    // Thêm vào giỏ hàng
    cartItems.push(newCartItem);
    
    // Lưu lại vào localStorage
    localStorage.setItem('cart', JSON.stringify(cartItems));
    
    // Xóa khỏi danh sách nháp nếu đang tồn tại trong đó
    try {
      const designDrafts = JSON.parse(localStorage.getItem('designDrafts') || '[]');
      const updatedDrafts = designDrafts.filter(draft => draft.id !== item.id);
      localStorage.setItem('designDrafts', JSON.stringify(updatedDrafts));
    } catch (e) {
      console.error('Lỗi khi xóa thiết kế khỏi danh sách nháp:', e);
    }
    
    // Thông báo đã thêm vào giỏ hàng
    alert('Đã thêm thiết kế vào giỏ hàng!');
    
    // Chuyển hướng đến trang giỏ hàng
    setTimeout(() => {
      window.location.href = '/cartcustomPage';
    }, 500);
  } catch (e) {
    console.error('Lỗi khi thêm vào giỏ hàng:', e);
    alert('Có lỗi xảy ra khi thêm vào giỏ hàng. Vui lòng thử lại.');
  }
};

// Thêm thiết kế vào danh sách sản phẩm
const addToProduct = (item) => {
  // Chuẩn bị sản phẩm để hiển thị trong modal
  selectedProduct.value = {
    name: item.name,
    price: item.price,
    surcharge: item.surcharge,
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
  try {
    // Lưu vào localStorage để có thể sử dụng trong trang sản phẩm
    const products = JSON.parse(localStorage.getItem('products') || '[]');
    
    // Tối ưu hóa sản phẩm trước khi lưu
    const optimizedProduct = {
      name: selectedProduct.value.name,
      price: selectedProduct.value.price,
      image: selectedProduct.value.image,
      description: selectedProduct.value.description || '',
      designData: {
        customText: selectedProduct.value.designData?.customText || '',
        timestamp: selectedProduct.value.designData?.timestamp || new Date().toISOString()
      },
      // Chỉ lưu tối đa 1 hình ảnh xem trước
      previewImages: selectedProduct.value.previewImages && selectedProduct.value.previewImages.length > 0 
        ? [selectedProduct.value.previewImages[0]] 
        : [],
      isCustomDesign: true,
      id: Date.now() // Thêm ID duy nhất
    };
    
    // Thêm sản phẩm đã tối ưu vào danh sách
    products.push(optimizedProduct);
    
    // Giới hạn kích thước trước khi lưu để tránh vượt quá quota
    const limitedProducts = products.slice(-10); // Giới hạn 10 sản phẩm mới nhất
    
    // Xóa dữ liệu cũ trước khi lưu để tránh vượt quá quota
    localStorage.removeItem('products');
    
    // Lưu dữ liệu mới
    localStorage.setItem('products', JSON.stringify(limitedProducts));
    
    // Đóng modal và hiển thị thông báo
    showProductModal.value = false;
    alert('Đã thêm thiết kế vào danh sách sản phẩm!');
  } catch (error) {
    if (error instanceof DOMException && error.name === 'QuotaExceededError') {
      alert('Lỗi: Bộ nhớ cục bộ đã đầy. Vui lòng xóa bớt các sản phẩm không cần thiết trước khi thêm mới.');
      console.error('Lỗi lưu trữ: Đã vượt quá quota localStorage', error);
      
      // Cố gắng xóa dữ liệu cũ và lưu lại với ít sản phẩm hơn
      try {
        // Xóa dữ liệu cũ
        localStorage.removeItem('products');
        
        // Tạo mảng chỉ có sản phẩm mới
        const singleProduct = [{
          name: selectedProduct.value.name,
          price: selectedProduct.value.price,
          image: selectedProduct.value.image,
          description: selectedProduct.value.description || '',
          id: Date.now()
        }];
        
        // Lưu chỉ sản phẩm mới
        localStorage.setItem('products', JSON.stringify(singleProduct));
        showProductModal.value = false;
        alert('Đã lưu sản phẩm với thông tin tối thiểu do bộ nhớ hạn chế.');
      } catch (e) {
        console.error('Không thể lưu ngay cả với dữ liệu tối thiểu', e);
        alert('Không thể lưu sản phẩm do bộ nhớ đã đầy. Vui lòng xóa dữ liệu trình duyệt và thử lại.');
      }
    } else {
      console.error('Lỗi lưu trữ sản phẩm:', error);
      alert('Có lỗi khi lưu sản phẩm. Vui lòng thử lại.');
    }
  }
};

// Chức năng xóa tất cả thiết kế
const clearAllDesigns = () => {
  // Xác nhận trước khi xóa
  if (!confirm('Bạn có chắc chắn muốn xóa tất cả thiết kế không?')) {
    return;
  }
  
  // Xóa khỏi mảng cart hiện tại
  cart.value = [];
  
  // Xóa khỏi localStorage
  localStorage.removeItem('cart');
  localStorage.removeItem('designDrafts');
  
  // Làm mới dữ liệu từ localStorage (đảm bảo mọi thứ đã được xóa)
  refreshDataFromStorage();
  
  // Thông báo xóa thành công
  alert('Đã xóa tất cả thiết kế!');
};

// Xóa dữ liệu cũ trong localStorage khi không cần thiết
const cleanupStorage = () => {
  try {
    // Xóa các dữ liệu tạm thời hoặc không cần thiết
    const keysToClean = ['tempDesign', 'lastViewedItems', 'viewHistory', 'recentlyViewed'];
    keysToClean.forEach(key => {
      localStorage.removeItem(key);
    });
    
    // Giới hạn kích thước của các dữ liệu đã lưu
    const designDrafts = JSON.parse(localStorage.getItem('designDrafts') || '[]');
    if (designDrafts.length > 5) {
      localStorage.setItem('designDrafts', JSON.stringify(designDrafts.slice(-5)));
    }
    
    console.log('Đã dọn dẹp localStorage để giảm nguy cơ vượt quota');
  } catch (e) {
    console.error('Lỗi khi dọn dẹp localStorage:', e);
  }
};

// 🔄 Khởi tạo cart và drafts từ localStorage
onMounted(() => {
  // Dọn dẹp localStorage trước khi load dữ liệu
  cleanupStorage();
  
  // Gọi hàm làm mới dữ liệu để tải từ localStorage
  refreshDataFromStorage();
});
</script>

<style scoped>
/* 🌟 Cấu trúc và màu sắc */
.container {
  /* max-width: 900px; */
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
  margin: 0px 0px 20px 0px;
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

.price.surcharge {
  font-size: 1.1rem;
  color: #f39c12;
  margin: 4px 0;
}

.price.total {
  font-size: 1.2rem;
  color: #2ecc71;
  margin: 4px 0;
  border-top: 1px dashed #ddd;
  padding-top: 6px;
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

/* Nút chỉnh sửa */
.edit-btn {
  background: linear-gradient(135deg, #007bff, #0056b3);
  color: white;
  border: none;
}

.edit-btn:hover {
  background: linear-gradient(135deg, #0069d9, #00489e);
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

/* Nút thêm vào sản phẩm */
.add-to-product-btn {
  background: linear-gradient(135deg, #17a2b8, #117a8b);
  color: white;
  border: none;
}

.add-to-product-btn:hover {
  background: linear-gradient(135deg, #138496, #0f6674);
}

/* Nút xóa */
.delete-btn {
  background: linear-gradient(135deg, #dc3545, #bd2130);
  color: white;
}

.delete-btn:hover {
  background: linear-gradient(135deg, #c82333, #a71d2a);
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
  font-size: 1.5rem;
  font-weight: 700;
  background: linear-gradient(90deg, #333333, #666666);
  background-clip: text;
  -webkit-background-clip: text;
  color: transparent;
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
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
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

/* Responsive */
@media (max-width: 768px) {
  .product-modal-content {
    width: 100%;
    max-width: 100%;
  }
}
</style>