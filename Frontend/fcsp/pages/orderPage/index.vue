<template>
    <div class="order-page">
      <Header />
      <section class="container my-5">
        <h1 class="text-center">Order Details</h1>
        <div v-if="loading" class="text-center py-5">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        </div>
        <div v-else>
          <div class="order-info">
            <div class="address-section">
              <h5>Địa Chỉ Nhận Hàng</h5>
              <div v-if="shippingAddress">
                <p class="recipient-info">
                  <strong>{{ shippingAddress.receiverName }}</strong>
                  <span class="phone">{{ shippingAddress.phoneNumber }}</span>
                </p>
                <p class="full-address">
                  {{ shippingAddress.address }}, 
                  <template v-if="shippingAddress.ward">{{ shippingAddress.ward }}, </template>
                  {{ shippingAddress.district }}, 
                  {{ shippingAddress.city }}
                </p>
                <!-- <div v-if="shippingAddress.isDefault" class="default-badge">Mặc định</div> -->
              </div>
              <div v-else class="no-address">
                <p>Chưa có địa chỉ nhận hàng</p>
              </div>
              <button class="btn btn-link" @click="toggleAddressModal">Thay Đổi</button>
            </div>
  
            <!-- Modal chọn địa chỉ (Bootstrap Modal) -->
            <div class="modal fade" id="addressModal" tabindex="-1" aria-labelledby="addressModalLabel" aria-hidden="true">
              <div class="modal-dialog">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title" id="addressModalLabel">Địa Chỉ Của Tôi</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>
                  <div class="modal-body">
                    <div class="address-list">
                      <div v-for="address in addresses" :key="address.id" class="address-item mb-3 p-3 border rounded">
                        <div class="form-check">
                          <input 
                            type="radio" 
                            :id="'address-' + address.id" 
                            name="shipping-address"
                            class="form-check-input"
                            :value="address" 
                            v-model="selectedAddress"
                          >
                          <label :for="'address-' + address.id" class="form-check-label">
                            <div class="d-flex justify-content-between align-items-start">
                              <div>
                                <strong>{{ address.receiverName }}</strong>
                                <span class="ms-2">{{ address.phoneNumber }}</span>
                                <div class="text-muted">
                                  {{ address.address }}, {{ address.ward }}, {{ address.district }}, {{ address.city }}
                                </div>
                              </div>
                              <div v-if="address.isDefault" class="default-badge">Mặc định</div>
                            </div>
                          </label>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
                    <button type="button" class="btn btn-primary" @click="confirmAddress">Xác nhận</button>
                  </div>
                </div>
              </div>
            </div>
  
  
            <div class="product-section">
              <h5>Sản phẩm đã chọn</h5>
              <div v-if="error" class="alert alert-danger">{{ error }}</div>
              <table class="table">
                <thead>
                  <tr>
                    <th style="width: 40%">Sản phẩm</th>
                    <th style="width: 20%">Đơn giá</th>
                    <th style="width: 20%">Số lượng</th>
                    <th style="width: 20%">Thành tiền</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="item in order?.items" :key="item.id">
                    <td>
                      <div class="d-flex align-items-center">
                        <img v-if="item.image" :src="item.image" alt="product" class="product-image me-2">
                        <div>
                          <div class="product-name">{{ item.name }}</div>
                          <div class="product-size text-muted">Size: {{ mappedSizes[item.id] || 'Loading...' }}</div>
                        </div>
                      </div>
                    </td>
                    <td>{{ formatPrice(item.price) }}</td>
                    <td>{{ item.quantity }}</td>
                    <td>{{ formatPrice(item.total) }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
  
            <div class="summary-section">
              <h5>Tổng số tiền</h5>
              <div class="d-flex justify-content-between mb-2">
                <span>Tổng tiền hàng:</span>
                <span class="text-primary">{{ formatPrice(order?.totalPrice) }}</span>
              </div>
              <div class="d-flex justify-content-between mb-2">
                <span>Phí vận chuyển:</span>
                <span>{{ formatPrice(order?.shippingCost) }}</span>
              </div>

              <!-- Voucher Section -->
              <div class="voucher-section mb-3">
                <div class="input-group">
                  <input 
                    type="text" 
                    class="form-control" 
                    v-model="voucherCode"
                    placeholder="Nhập mã giảm giá"
                    :disabled="voucherApplied"
                  >
                  <button 
                    v-if="!voucherApplied"
                    class="btn btn-outline-primary" 
                    @click="applyVoucher"
                    :disabled="!voucherCode"
                  >
                    Áp dụng
                  </button>
                  <button 
                    v-else
                    class="btn btn-outline-danger" 
                    @click="removeVoucher"
                  >
                    Xóa mã
                  </button>
                </div>
                <div v-if="errorMessage" class="mt-2 text-danger">
                  {{ errorMessage }}
                </div>
                <div v-if="voucherApplied" class="mt-2 text-success">
                  Đã áp dụng mã giảm giá: -{{ discountAmount.toLocaleString() }}đ
                </div>
              </div>

              <hr>
              <div class="d-flex justify-content-between fw-bold">
                <span>Tổng thanh toán:</span>
                <span class="text-danger fs-5">{{ formatPrice(finalTotal) }}</span>
              </div>
            </div>
  
            <button class="btn btn-primary w-100 mt-4" @click="placeOrder" :disabled="!shippingAddress || !order?.items?.length">
              Đặt hàng
            </button>
          </div>
        </div>
      </section>
      <Footer />
    </div>
    
  </template>
  
  <script setup>
  import { ref, onMounted, computed } from 'vue';
  import { shippingInfo } from '@/server/shipping-service';
  import Header from '@/components/Header.vue';
  import Footer from '@/components/Footer.vue';
  import { useRouter } from 'vue-router';
  import { getAllVouchers } from '~/server/ManageVoucher-service';
  import { postOrder } from '@/server/order-service';
  import { getSizeValueById, getSizeIdByValue } from '@/server/size-service';
  const order = ref(null);
  const loading = ref(true);
  const selectedAddress = ref(null);
  const shippingAddress = ref(null);
  const addresses = ref([]);
  const error = ref(null);
  const router = useRouter();
  
  const voucherCode = ref('');
  const voucherApplied = ref(false);
  const discountAmount = ref(0);
  const errorMessage = ref('');
  const totalBeforeDiscount = ref(0);
  const availableVouchers = ref([]);

  // Add new ref for mapped sizes
  const mappedSizes = ref({});

  // Function to load and map sizes for order items
  const loadSizesForItems = async () => {
    try {
      if (!order.value?.items) return;
      
      // Load size values for each item
      const sizePromises = order.value.items.map(async (item) => {
        const sizeValue = await getSizeValueById(parseInt(item.selectedSize));
        if (sizeValue) {
          mappedSizes.value[item.id] = sizeValue;
        }
      });
      
      await Promise.all(sizePromises);
    } catch (err) {
      console.error('Error loading sizes:', err);
    }
  };

  // Computed property for final total after voucher discount
  const finalTotal = computed(() => {
    if (!order.value) return 0;
    return calculateTotal();
  });
  
  const fetchOrder = async () => {
    try {
      loading.value = true;
      
      // Lấy dữ liệu đơn hàng từ sessionStorage
      const orderDataStr = sessionStorage.getItem('orderData');
      if (!orderDataStr) {
        error.value = 'Không tìm thấy thông tin đơn hàng';
        return;
      }

      const orderData = JSON.parse(orderDataStr);
      order.value = {
        id: Date.now(),
        items: orderData.items.map(item => ({
          id: item.id,
          name: item.name,
          price: parseFloat(item.price),
          quantity: item.quantity || 1,
          total: parseFloat(item.price) * (item.quantity || 1),
          image: item.image,
          selectedSize: item.selectedSize
        })),
        totalPrice: parseFloat(orderData.totalPrice),
        shippingCost: parseFloat(orderData.shippingCost),
        totalPayment: parseFloat(orderData.totalPayment)
      };

      // Load sizes after order data is loaded
      await loadSizesForItems();

    } catch (error) {
      console.error('Error loading order:', error);
      error.value = 'Không thể tải thông tin đơn hàng. Vui lòng thử lại.';
    } finally {
      loading.value = false;
    }
  };
  
  const fetchShippingAddress = async () => {
    try {
      loading.value = true;
      error.value = null;
      
      const userId = localStorage.getItem('userId');
      if (!userId) {
        error.value = 'Vui lòng đăng nhập để xem địa chỉ giao hàng';
        return;
      }

      const response = await shippingInfo(parseInt(userId));
      
      if (!response || !response.shippingInfos) {
        error.value = 'Không thể tải thông tin địa chỉ giao hàng';
        return;
      }

      // Xử lý dữ liệu địa chỉ từ response.shippingInfos
      const shippingData = response.shippingInfos;
      if (Array.isArray(shippingData)) {
        addresses.value = shippingData;
        // Tìm địa chỉ mặc định hoặc địa chỉ đầu tiên
        const defaultAddress = shippingData.find(addr => addr.isDefault === true);
        shippingAddress.value = defaultAddress || shippingData[0];
        selectedAddress.value = shippingAddress.value;
      } else {
        error.value = 'Định dạng dữ liệu địa chỉ không hợp lệ';
      }
    } catch (err) {
      console.error('Error fetching shipping address:', err);
      error.value = 'Đã có lỗi xảy ra khi tải địa chỉ giao hàng';
      addresses.value = [];
      shippingAddress.value = null;
      selectedAddress.value = null;
    } finally {
      loading.value = false;
    }
  };
  
  const confirmAddress = () => {
    try {
      if (!selectedAddress.value) {
        alert('Vui lòng chọn một địa chỉ giao hàng');
        return;
      }

      shippingAddress.value = selectedAddress.value;
      const addressModal = bootstrap.Modal.getInstance(document.getElementById('addressModal'));
      if (addressModal) {
        addressModal.hide();
      }
    } catch (err) {
      console.error('Error confirming address:', err);
      alert('Không thể cập nhật địa chỉ giao hàng. Vui lòng thử lại.');
    }
  };
  
  const updateAddress = () => {
    if (selectedAddress.value) {
      console.log('Updating address:', selectedAddress.value);
      const updateAddressModal = new bootstrap.Modal(document.getElementById('updateAddressModal'));
      updateAddressModal.hide();
      const addressModal = new bootstrap.Modal(document.getElementById('addressModal'));
      addressModal.show();
    } else {
      console.warn('No address selected for update');
    }
  };
  
  const handleUpdateAddress = (address) => {
    selectedAddress.value = address; // Set the selected address for update
    const updateAddressModal = new bootstrap.Modal(document.getElementById('updateAddressModal'));
    updateAddressModal.show(); // Show the update address modal
  };
  
  const toggleAddressModal = () => {
    try {
      if (!addresses.value.length) {
        alert('Bạn chưa có địa chỉ nào. Vui lòng thêm địa chỉ trong trang cài đặt.');
        return;
      }
      const addressModal = new bootstrap.Modal(document.getElementById('addressModal'));
      addressModal.show();
    } catch (err) {
      console.error('Error showing address modal:', err);
      alert('Không thể mở cửa sổ chọn địa chỉ. Vui lòng thử lại.');
    }
  };
  
  const placeOrder = async () => {
    try {
      if (!shippingAddress.value) {
        alert('Vui lòng chọn địa chỉ giao hàng');
        return;
      }

      if (!order.value || !order.value.items || order.value.items.length === 0) {
        alert('Không có sản phẩm nào trong đơn hàng');
        return;
      }

      // Get userId from localStorage
      const userId = localStorage.getItem('userId');
      if (!userId) {
        alert('Vui lòng đăng nhập để đặt hàng');
        return;
      }

      // Convert size values to size IDs and prepare order details
      const orderDetailsPromises = order.value.items.map(async (item) => {
        // Get the correct size ID for the size value
        const sizeId = await getSizeIdByValue(Number(item.selectedSize));
        if (!sizeId) {
          throw new Error(`Không tìm thấy size ID cho size ${item.selectedSize}`);
        }
        
        return {
          customShoeDesignId: parseInt(item.id),
          quantity: parseInt(item.quantity || 1),
          sizeId: sizeId // Use the looked up sizeId
        };
      });

      // Wait for all size ID lookups to complete
      const orderDetails = await Promise.all(orderDetailsPromises);

      // Prepare request body
      const orderData = {
        userId: parseInt(userId),
        shippingInfoId: parseInt(shippingAddress.value.id),
        paymentMethod: 0,
        orderDetails: orderDetails
      };

      console.log('Sending order data:', JSON.stringify(orderData, null, 2));

      // Call the postOrder API
      const response = await postOrder(orderData);
      console.log('Order response:', response);
      
      // Clear order data from sessionStorage
      sessionStorage.removeItem('orderData');

      // Check if we have a payment URL directly in the response
      if (response && response.paymentUrl) {
        window.location.href = response.paymentUrl;
        return;
      }
      
      // If no payment URL but order was successful
      if (response && response.code === 200) {
        alert('Đặt hàng thành công!');
        router.push('/');
        return;
      }

      // If we get here, something went wrong
      throw new Error(response?.message || 'Đặt hàng thất bại');
    } catch (err) {
      console.error('Error placing order:', err);
      const errorMessage = err.message || 'Có lỗi xảy ra khi đặt hàng. Vui lòng thử lại.';
      alert(errorMessage);
    }
  };
  
  const formatPrice = (price) => {
    if (!price) return '0 ₫';
    return new Intl.NumberFormat('vi-VN', {
      style: 'currency',
      currency: 'VND'
    }).format(price);
  };
  
  // Add function to load vouchers
  const loadAvailableVouchers = async () => {
    try {
      const response = await getAllVouchers();
      if (response.code === 200) {
        availableVouchers.value = response.data;
      }
    } catch (error) {
      console.error('Error loading vouchers:', error);
    }
  };

  // Update the applyVoucher function
  const applyVoucher = async () => {
    try {
      errorMessage.value = '';
      
      if (!voucherCode.value) {
        errorMessage.value = 'Vui lòng nhập mã giảm giá';
        return;
      }

      // Find the voucher in available vouchers
      const voucher = availableVouchers.value.find(v => 
        v.code.toLowerCase() === voucherCode.value.toLowerCase() && 
        !v.isUsed && 
        new Date(v.expiryDate) > new Date()
      );

      if (voucher) {
        voucherApplied.value = true;
        discountAmount.value = voucher.discountAmount;
        // Recalculate the total
        calculateTotal();
        alert('Áp dụng mã giảm giá thành công!');
      } else {
        // Check specific reasons why voucher is invalid
        const existingVoucher = availableVouchers.value.find(v => 
          v.code.toLowerCase() === voucherCode.value.toLowerCase()
        );

        if (!existingVoucher) {
          errorMessage.value = 'Mã giảm giá không tồn tại';
        } else if (existingVoucher.isUsed) {
          errorMessage.value = 'Mã giảm giá đã được sử dụng';
        } else if (new Date(existingVoucher.expiryDate) <= new Date()) {
          errorMessage.value = 'Mã giảm giá đã hết hạn';
        } else {
          errorMessage.value = 'Mã giảm giá không hợp lệ';
        }
      }
    } catch (error) {
      console.error('Error applying voucher:', error);
      errorMessage.value = 'Có lỗi xảy ra khi áp dụng mã giảm giá';
    }
  };

  // Update the removeVoucher function
  const removeVoucher = () => {
    voucherCode.value = '';
    voucherApplied.value = false;
    discountAmount.value = 0;
    errorMessage.value = '';
    calculateTotal();
  };
  
  const calculateTotal = () => {
    if (!order.value) return 0;
    
    // Calculate total before discount
    totalBeforeDiscount.value = order.value.totalPrice + (order.value.shippingCost || 0);
    
    // Calculate final total after discount
    const final = totalBeforeDiscount.value - discountAmount.value;
    
    // Update the order's total payment
    order.value.totalPayment = Math.max(0, final);
    
    return order.value.totalPayment;
  };
  
  onMounted(() => {
    Promise.all([
      fetchOrder(), 
      fetchShippingAddress(),
      loadAvailableVouchers() // Add this
    ]).catch(err => {
      console.error('Error during initialization:', err);
      error.value = 'Đã có lỗi xảy ra khi khởi tạo trang';
    });
  });
  </script>
  
  <style scoped>
  .order-page {
    background: #fff;
    min-height: 100vh;
    font-family: 'Poppins', sans-serif;
  }
  
  .address-section, .product-section, .summary-section {
    margin-bottom: 20px;
    padding: 20px;
    background: #f9f9f9;
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  }
  
  .table th, .table td {
    vertical-align: middle;
  }
  
  .recipient-info {
    margin-bottom: 0.5rem;
  }
  
  .recipient-info .phone {
    margin-left: 1rem;
    color: #666;
  }
  
  .full-address {
    margin-bottom: 0.5rem;
    line-height: 1.5;
  }
  
  .default-badge {
    display: inline-block;
    padding: 2px 8px;
    background-color: #e8f5e9;
    color: #2e7d32;
    border-radius: 4px;
    font-size: 0.875rem;
    margin-bottom: 0.5rem;
  }
  
  .no-address {
    color: #666;
    font-style: italic;
  }
  
  .btn-link {
    padding: 0;
    color: #1976d2;
    text-decoration: none;
  }
  
  .btn-link:hover {
    text-decoration: underline;
  }
  
  .product-image {
    width: 80px;
    height: 80px;
    object-fit: cover;
    border-radius: 4px;
  }
  
  .product-name {
    font-weight: 500;
    margin-bottom: 4px;
  }
  
  .product-size {
    font-size: 0.875rem;
  }
  
  .table {
    background: white;
  }
  
  .table th {
    background: #f8f9fa;
    font-weight: 600;
    color: #495057;
  }
  
  .summary-section {
    background: white;
  }
  
  .text-primary {
    color: #555555 !important;
  }
  
  .text-danger {
    color: #f44336 !important;
  }
  
  hr {
    margin: 1rem 0;
    border-color: #dee2e6;
  }
  
  .voucher-section {
    background: #f8f9fa;
    padding: 10px;
    border-radius: 4px;
  }
  
  .voucher-section .input-group {
    max-width: 400px;
  }
  
  .voucher-section .btn-outline-primary {
    border-color: #2196f3;
    color: #2196f3;
  }
  
  .voucher-section .btn-outline-primary:hover {
    background-color: #2196f3;
    color: white;
  }
  
  .text-success {
    color: #4caf50 !important;
  }
  </style>
  