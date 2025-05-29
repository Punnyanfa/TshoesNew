<template>
  <div>
    <Header />
    <div class="container mt-5">
      <h1 class="text-center text-primary">Your Designs</h1>

      <div v-if="cart.length > 0">
        <!-- Header and Clear All Button -->
        <div class="d-flex justify-content-between align-items-center mb-3">
          <h5>Design List ({{ cart.length }})</h5>
          <button class="btn btn-outline-danger btn-sm" @click="clearAllDesigns">
            <i class="fas fa-trash-alt mr-1"></i> Clear All
          </button>
        </div>

        <!-- Cart with items -->
        <div class="cart-items mt-4">
          <div v-for="item in cart" :key="item.id" class="cart-item-card">
            <div class="cart-item-content">
              <!-- Display design image -->
              <div class="cart-item-image">
                <img :src="item.image" alt="Shoe Design" class="product-image" />
              </div>

              <div class="cart-item-details">
                <h4>{{ item.name }}</h4>
                <p class="price">Base Price: {{ formatPrice(item.price) }}</p>
                <!-- <p class="price">Size: {{ item.size }}</p> -->
                <p v-if="item.surcharge && item.surcharge > 0" class="price surcharge">Surcharge: {{ formatPrice(item.surcharge) }}</p>
                <p v-if="item.surcharge && item.surcharge > 0" class="price total">Total: {{ formatPrice(item.price + item.surcharge) }}</p>

                <!-- Display design info if exists -->
                <div v-if="item.designData" class="design-info">
                  <p v-if="item.designData.customText" class="custom-text">
                    <strong>Custom Text:</strong> {{ item.designData.customText }}
                  </p>
                  <p class="timestamp">
                    <small>Designed on: {{ formatDate(item.designData.timestamp) }}</small>
                  </p>
                </div>

                <!-- Show button to preview all angles -->
                <div v-if="item.previewImages && item.previewImages.length > 1" class="mt-2">
                  <button class="btn btn-sm btn-outline-secondary" @click="togglePreviewImages(item)">
                    {{ item.showPreviews ? 'Hide' : 'View' }} all angles
                  </button>
                </div>
              </div>

              <div class="cart-item-actions">
                <button class="btn btn-primary edit-btn" @click="editDesign(item)">Edit</button>
                <button class="btn btn-success add-to-cart-btn" @click="duplicateToCart(item)">Add to Cart</button>
                <button class="btn btn-info add-to-product-btn" @click="addToProduct(item)" v-if="userRole === 'Designer'">Add as Product</button>
                <button class="btn btn-danger delete-btn" @click="removeFromCart(item.id)">Delete</button>
              </div>
            </div>

            <!-- Preview extra angles when toggled -->
            <div v-if="item.showPreviews && item.previewImages" class="preview-images-container">
              <div class="preview-images">
                <div v-for="(preview, index) in item.previewImages" :key="`preview_${item.id}_${index}`" class="preview-image-item">
                  <img :src="preview" alt="View Angle" class="preview-image" />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div v-else class="text-center mt-5 empty-cart">
        <h4>You don't have any designs yet!</h4>
        <NuxtLink to="/customPage" class="btn btn-primary mt-3">Start Designing Now</NuxtLink>
      </div>

      <!-- Modal for creating product -->
      <div class="product-modal" v-if="showProductModal">
        <div class="product-modal-content">
          <div class="product-modal-header">
            <h3>Create Product from Design</h3>
            <button class="close-button" @click="showProductModal = false">×</button>
          </div>
          <div class="product-modal-body">
            <div class="product-preview">
              <img :src="selectedProduct.image" alt="Product Image" class="product-preview-image" />

              <!-- Preview other view angles -->
              <div class="preview-angles-section" v-if="selectedProduct.previewImages && selectedProduct.previewImages.length > 1">
                <h4>View Angles</h4>
                <div class="preview-angles-container">
                  <div v-for="(preview, index) in selectedProduct.previewImages" :key="`modal_preview_${index}`" 
                       class="preview-angle-item"
                       @click="selectedProduct.image = preview">
                    <img :src="preview" alt="View Angle" class="preview-angle-image" />
                  </div>
                </div>
              </div>
            </div>

            <div class="product-form">
              <div class="form-group">
                <label for="productName">Product Name:</label>
                <input type="text" id="productName" v-model="selectedProduct.name" class="form-control" />
              </div>

              <div class="form-group">
                <label for="productTotalPrice">Total Price:</label>
                <input type="text" id="productTotalPrice" :value="formatPrice(selectedProduct.price + selectedProduct.surcharge)" class="form-control" readonly />
              </div>

              <div class="form-group">
                <label for="productCommission">Commission Price:</label>
                <div class="input-group">
                  <input type="number" id="productCommission" v-model="selectedProduct.commission" class="form-control" min="0" />
                  <span class="input-group-text">đ</span>
                </div>
              </div>

              <div class="form-group">
                <label for="productDescription">Description:</label>
                <textarea id="productDescription" v-model="selectedProduct.description" class="form-control" rows="3"></textarea>
              </div>

              <div class="form-group" v-if="selectedProduct.designData && selectedProduct.designData.customText">
                <label for="customText">Custom Text:</label>
                <input type="text" id="customText" v-model="selectedProduct.designData.customText" class="form-control" />
              </div>
            </div>

            <div class="product-modal-actions">
              <button class="btn btn-secondary" @click="showProductModal = false">Cancel</button>
              <button class="btn btn-primary" @click="saveToProduct">Create Product</button>
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
import { getMyCustom, deleteCustom, getMyCustomById } from '~/server/myCustom-service.js';
import { updateStatus } from '@/server/designUp-service'

const userRole = ref(localStorage.getItem('role') || '');

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
  surcharge: 0,
  commission: 0,
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
const editDesign = async (item) => {
  try {
    // Gọi API lấy chi tiết custom để lấy link file json
    const detail = await getMyCustomById(item.id);

    let editingItem = { ...item };
    if (detail && detail.data && detail.data.designData) {
      try {
        const response = await fetch(detail.data.designData);
        if (response.ok) {
          const designDataObj = await response.json();
          editingItem.designData = designDataObj;
        }
      } catch (e) {
        console.error('Không thể tải lại file json model 3D:', e);
      }
    }
    window.location.href = `/customPage/${item.id}?edit=true`;
  } catch (e) {
    alert('Không thể lấy dữ liệu chi tiết thiết kế!');
    console.error(e);
  }
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
async function removeFromCart(id) {
  if (!confirm('Bạn có chắc chắn muốn xóa thiết kế này?')) return;
  try {
    const result = await deleteCustom(id);
    if (result.code === 200) { 
      // Xóa khỏi cart local
      cart.value = cart.value.filter(item => item.id !== id);
      alert('Xóa thành công!');
    } else {
      alert(result.message || 'Xóa thất bại!');
    }
  } catch (e) {
    alert('Có lỗi khi xóa!');
    console.error(e);
  }
}

// Thêm thiết kế vào giỏ hàng
const duplicateToCart = (item) => {
  try {
    // Lấy giỏ hàng từ sessionStorage
    let cart = []
    const savedCart = sessionStorage.getItem('cart')
    if (savedCart) {
      cart = JSON.parse(savedCart)
    }
    
    // Tạo bản sao của item với ID mới
    const newCartItem = {
      id: Date.now(),
      customShoeDesignId: item.id,
      name: item.name,
      manufacturerId: item.manufacturerId,
      price: item.price,
      surcharge: item.surcharge,
      selectedSize: '40', // Đặt size mặc định là 40
      selectedQuantity: item.selectedQuantity || 1,
      previewImageUrl: item.image || item.previewImageUrl,
      designData: {
        colors: item.designData?.colors || {},
        textures: item.designData?.textures || {},
        imagesData: item.designData?.imagesData || {},
        customText: item.designData?.customText || '',
        textureParams: item.designData?.textureParams || {},
        timestamp: new Date().toISOString(),
        manufacturerId: item.manufacturerId
      },
      previewImages: item.previewImages || []
    }
    
    // Thêm vào giỏ hàng
    cart.push(newCartItem)
    
    // Lưu giỏ hàng vào sessionStorage
    sessionStorage.setItem('cart', JSON.stringify(cart))
    
    // Thông báo đã thêm vào giỏ hàng
    const totalPrice = newCartItem.price + newCartItem.surcharge
    const formattedTotalPrice = formatPrice(totalPrice)
    const formattedSurcharge = newCartItem.surcharge > 0 ? `\nPhụ phí tùy chỉnh: ${formatPrice(newCartItem.surcharge)}` : ''
    
    alert(`Sản phẩm thiết kế đã được thêm vào giỏ hàng thành công!\nGiá gốc: ${formatPrice(newCartItem.price)}${formattedSurcharge}\nTổng tiền: ${formattedTotalPrice}\nSize: ${newCartItem.selectedSize}`)
    
    // Chuyển hướng đến trang giỏ hàng
    setTimeout(() => {
      window.location.href = '/shoppingCartPage'
    }, 500)
  } catch (e) {
    console.error('Lỗi khi thêm vào giỏ hàng:', e)
    alert('Có lỗi xảy ra khi thêm vào giỏ hàng. Vui lòng thử lại.')
  }
}

// Thêm thiết kế vào danh sách sản phẩm
const addToProduct = async (item) => {
  if (confirm('Bạn có muốn đăng bán không?')) {
    try {
      // Kiểm tra token
      const token = localStorage.getItem('userToken');
      if (!token) {
        alert('Vui lòng đăng nhập để thực hiện chức năng này!');
        return;
      }

      const response = await updateStatus(item.id, 4);
      if (response && response.code === 200) {
        alert('Đã chuyển sang Pending thành công!');
        // Refresh data after successful update
        const userId = localStorage.getItem('userId');
        if (userId) {
          const apiData = await getMyCustom(userId);
          console.log('API Data:', apiData);
          if (apiData && apiData.data && Array.isArray(apiData.data.designs)) {
            cart.value = apiData.data.designs.map(item => ({
              id: item.id,
              name: item.name,
              image: item.previewImageUrl || null,
              price: item.total || 0,
              surcharge: item.servicePrice || 0,
              size: item.size || '',
              designData: item.customText ? { customText: item.customText } : undefined,
              previewImages: item.previewImageUrl ? [item.previewImageUrl] : [],
              showPreviews: false
            }));
          }
        }
      } else {
        alert(response?.message || 'Có lỗi xảy ra khi cập nhật trạng thái!');
      }
    } catch (error) {
      console.error('Lỗi khi cập nhật trạng thái:', error);
      if (error.response?.status === 401) {
        alert('Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại!');
      } else {
        alert(error.response?.data?.message || 'Có lỗi xảy ra khi cập nhật trạng thái!');
      }
    }
  }
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
      surcharge: selectedProduct.value.surcharge,
      commission: selectedProduct.value.commission || 0,
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
    
   
  } catch (e) {
    console.error('Lỗi khi dọn dẹp localStorage:', e);
  }
};

// 🔄 Khởi tạo cart và drafts từ localStorage
onMounted(async () => {
  cleanupStorage();
  // Gọi API lấy dữ liệu custom của user
  try {
    const userId = localStorage.getItem('userId');
    if (userId) {
      const apiData = await getMyCustom(userId);
      cart.value = (apiData && apiData.data && Array.isArray(apiData.data.designs))
        ? apiData.data.designs.map(item => ({
            id: item.id,
            name: item.name,
            image: item.previewImageUrl || null,
            price: item.total || 0,
            surcharge: item.servicePrice || 0,
            size: item.size || '',
            designData: item.customText ? { customText: item.customText } : undefined,
            previewImages: item.previewImageUrl ? [item.previewImageUrl] : [],
            showPreviews: false
          }))
        : [];
    } else {
      cart.value = [];
    }
  } catch (e) {
    console.error('Lỗi khi lấy dữ liệu từ API:', e);
    cart.value = [];
  }

  const urlParams = new URLSearchParams(window.location.search)
  const isEditing = urlParams.get('edit') === 'true'
  const editId = urlParams.get('id')

  if (isEditing && editId) {
    const editingDesignJson = localStorage.getItem('editingDesign')
    if (editingDesignJson) {
      try {
        const editingDesign = JSON.parse(editingDesignJson)
        if (editingDesign.id.toString() === editId.toString()) {
          // Gán lại các giá trị vào các biến reactive của bạn ở đây
          // Ví dụ:
          customProductName.value = editingDesign.name
          basePrice.value = editingDesign.price
          surcharge.value = editingDesign.surcharge
        }
      } catch (e) {
        console.error('Lỗi khi nạp lại thiết kế đang chỉnh sửa:', e)
      }
    }
  }

  // // Cập nhật lại userRole khi mounted (nếu cần)
  // userRole.value = localStorage.getItem('role') || '';
});

</script>

<style scoped>
/* 🌟 Cấu trúc và màu sắc */
.container {
  margin: 0 auto;
  padding: 0 15px;
}

h1 {
  font-size: 3.5rem;
  font-weight: 800;
  background: #AAAAAA;
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  letter-spacing: 1px;
  transition: transform 0.3s ease;
}

h1:hover {
  transform: translateY(-3px);
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
  color: #AAAAAA;
  margin: 8px 0;
}

.price.surcharge {
  font-size: 1.1rem;
  color: #888888;
  margin: 4px 0;
}

.price.total {
  font-size: 1.2rem;
  color: #AAAAAA;
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
  border-radius: 8px;
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
  background-color: #AAAAAA;
  color: white;
  border: none;
}

.edit-btn:hover {
  background-color: #888888;
}

/* Nút thêm vào giỏ hàng */
.add-to-cart-btn {
  background-color: #AAAAAA;
  color: white;
  border: none;
}

.add-to-cart-btn:hover {
  background-color: #888888;
}

/* Nút thêm vào sản phẩm */
.add-to-product-btn {
  background-color: #AAAAAA;
  color: white;
  border: none;
}

.add-to-product-btn:hover {
  background-color: #888888;
}

/* Nút xóa */
.delete-btn {
  background-color: #dc3545;
  color: white;
}

.delete-btn:hover {
  background-color: #c82333;
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
  color: #AAAAAA;
  margin-bottom: 20px;
}

.empty-cart .btn {
  background-color: #AAAAAA;
  color: white;
  border-radius: 8px;
  padding: 12px 30px;
  text-transform: uppercase;
  font-weight: bold;
  transition: all 0.3s ease;
  border: none;
  box-shadow: 0 4px 15px rgba(170, 170, 170, 0.3);
  position: relative;
  overflow: hidden;
}

.empty-cart .btn:hover {
  background-color: #888888;
  transform: translateY(-3px);
  box-shadow: 0 8px 25px rgba(170, 170, 170, 0.4);
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
  color: #AAAAAA;
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
  border-radius: 8px;
  overflow: hidden;
  cursor: pointer;
  border: 2px solid #eee;
  transition: all 0.3s ease;
  position: relative;
}

.preview-angle-item:hover {
  border-color: #AAAAAA;
  transform: translateY(-5px) scale(1.05);
  box-shadow: 0 8px 15px rgba(170, 170, 170, 0.2);
  z-index: 1;
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
  border-color: #AAAAAA;
  box-shadow: 0 0 0 3px rgba(170, 170, 170, 0.15);
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
  background-color: #AAAAAA;
  color: white;
  font-weight: 600;
  border: none;
}

.product-modal-actions .btn-primary:hover {
  background-color: #888888;
  box-shadow: 0 5px 15px rgba(170, 170, 170, 0.3);
  transform: translateY(-3px);
}

.product-modal-actions .btn-secondary {
  background-color: #6c757d;
  color: white;
  font-weight: 500;
  border: none;
}

.product-modal-actions .btn-secondary:hover {
  background-color: #5a6268;
  box-shadow: 0 5px 15px rgba(108, 117, 125, 0.3);
  transform: translateY(-3px);
}

/* Responsive */
@media (max-width: 768px) {
  .product-modal-content {
    width: 100%;
    max-width: 100%;
  }
  
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
</style>