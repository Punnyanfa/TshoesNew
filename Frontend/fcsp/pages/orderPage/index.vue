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
              <h5>Shipping Address</h5>
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
                <p>No shipping address yet</p>
              </div>
              <button class="btn btn-link" @click="toggleAddressModal">Change</button>
            </div>
  
            <!-- Modal chọn địa chỉ (Bootstrap Modal) -->
            <div class="modal fade" id="addressModal" tabindex="-1" aria-labelledby="addressModalLabel" aria-hidden="true">
              <div class="modal-dialog">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title" id="addressModalLabel">My Addresses</h5>
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
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-primary" @click="confirmAddress">Confirm</button>
                  </div>
                </div>
              </div>
            </div>
  
  
            <div class="product-section">
              <h5>Selected Products</h5>
              <div v-if="error" class="alert alert-danger">{{ error }}</div>
              <table class="table">
                <thead>
                  <tr>
                    <th style="width: 40%">Product</th>
                    <th style="width: 20%">Unit Price</th>
                    <th style="width: 20%">Quantity</th>
                    <th style="width: 20%">Subtotal</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="item in order?.items" :key="item.id">
                    <td>
                      <div class="d-flex align-items-center">
                        <img v-if="item.image" :src="item.image" alt="product" class="product-image me-2">
                        <div>
                          <div class="product-name">{{ item.name }}</div>
                          <div class="product-size text-muted">Size: {{ item.selectedSize || 'Loading...' }}</div>
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
              <h5>Order Summary</h5>
              <div class="d-flex justify-content-between mb-2">
                <span>Subtotal:</span>
                <span class="text-primary">{{ formatPrice(order?.totalPrice) }}</span>
              </div>
              <div class="d-flex justify-content-between mb-2">
                <span>Shipping Cost:</span>
                <span>{{ formatPrice(order?.shippingCost) }}</span>
              </div>

              <!-- Voucher Section -->
              <div class="voucher-section mb-3">
                <div class="input-group">
                  <input 
                    type="text" 
                    class="form-control" 
                    v-model="voucherCode"
                    placeholder="Enter discount code"
                    :disabled="voucherApplied"
                  >
                  <button 
                    v-if="!voucherApplied"
                    class="btn btn-outline-primary" 
                    @click="applyVoucher"
                    :disabled="!voucherCode"
                  >
                    Apply
                  </button>
                  <button 
                    v-else
                    class="btn btn-outline-danger" 
                    @click="removeVoucher"
                  >
                    Remove
                  </button>
                </div>
                <div v-if="errorMessage" class="mt-2 text-danger">
                  {{ errorMessage }}
                </div>
                <div v-if="voucherApplied" class="mt-2 text-success">
                  Applied discount code: -{{ discountAmount.toLocaleString() }}đ
                </div>
              </div>

              <!-- Payment Method Section -->
              <div class="payment-method-section mb-4">
                <h5 class="mb-3">Payment Method</h5>
                <div class="payment-methods">
                  <div 
                    class="payment-method-option"
                    :class="{ active: selectedPaymentMethod === 0 }"
                    @click="selectPaymentMethod(0)"
                  >
                    <div class="method-icon payos">
                      <i class="fas fa-credit-card"></i>
                    </div>
                    <div class="method-info">
                      <h6>PayOS</h6>
                      <p class="mb-0">Pay with credit/debit card</p>
                    </div>
                    <div class="method-radio">
                      <input 
                        type="radio" 
                        name="paymentMethod" 
                        :value="0" 
                        v-model="selectedPaymentMethod"
                      >
                    </div>
                  </div>

                  <div 
                    class="payment-method-option"
                    :class="{ active: selectedPaymentMethod === 1 }"
                    @click="selectPaymentMethod(1)"
                  >
                    <div class="method-icon wallet">
                      <i class="fas fa-wallet"></i>
                    </div>
                    <div class="method-info">
                      <h6>Wallet</h6>
                      <p class="mb-0">Pay with your wallet balance</p>
                      <small class="text-muted">Current balance: {{ getWalletBalanceDisplay }}</small>
                    </div>
                    <div class="method-radio">
                      <input 
                        type="radio" 
                        name="paymentMethod" 
                        :value="1" 
                        v-model="selectedPaymentMethod"
                      >
                    </div>
                  </div>
                </div>
              </div>

              <hr>
              <div class="d-flex justify-content-between fw-bold">
                <span>Total Payment:</span>
                <span class="text-danger fs-5">{{ formatPrice(finalTotal) }}</span>
              </div>
            </div>
  
            <button class="btn btn-primary w-100 mt-4" @click="placeOrder" :disabled="!shippingAddress || !order?.items?.length">
              Place Order
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
  import { getBalance } from '@/server/balance-service';
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
  const selectedVoucherId = ref(null);

  // Add new ref for mapped sizes
  const mappedSizes = ref({});

  // Add new ref for payment method
  const selectedPaymentMethod = ref(0); // 0 for PayOS, 1 for Wallet

  // Add new ref for wallet balance
  const walletBalance = ref(0);

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
      
      // Lấy dữ liệu đơn hàng từ localStorage -> Get order data from localStorage
      const userId = localStorage.getItem("userId");
      const orderDataStr = localStorage.getItem(`orderData_${userId}`);
      if (!orderDataStr) {
        error.value = 'Order information not found';
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
      error.value = 'Unable to load order information. Please try again.';
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
        error.value = 'Please login to view shipping addresses';
        return;
      }

      const response = await shippingInfo(parseInt(userId));
      
      if (!response || !response.shippingInfos) {
        error.value = 'Unable to load shipping address information';
        return;
      }

      // Xử lý dữ liệu địa chỉ từ response.shippingInfos -> Process address data from response.shippingInfos
      const shippingData = response.shippingInfos;
      if (Array.isArray(shippingData)) {
        addresses.value = shippingData;
        // Tìm địa chỉ mặc định hoặc địa chỉ đầu tiên
        const defaultAddress = shippingData.find(addr => addr.isDefault === true);
        shippingAddress.value = defaultAddress || shippingData[0];
        selectedAddress.value = shippingAddress.value;
      } else {
        error.value = 'Invalid address data format';
      }
    } catch (err) {
      console.error('Error fetching shipping address:', err);
      error.value = 'An error occurred while loading shipping addresses';
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
        alert('Please select a shipping address'); // Keep English
        return;
      }

      shippingAddress.value = selectedAddress.value;
      const addressModal = bootstrap.Modal.getInstance(document.getElementById('addressModal'));
      if (addressModal) {
        addressModal.hide();
      }
    } catch (err) {
      console.error('Error confirming address:', err);
      alert('Unable to update shipping address. Please try again.'); // Keep English
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
        router.push('/shippingPage');
        return;
      }
      const addressModal = new bootstrap.Modal(document.getElementById('addressModal'));
      addressModal.show();
    } catch (err) {
      console.error('Error showing address modal:', err);
      alert('Unable to open address selection window. Please try again.');
    }
  };
  
  const placeOrder = async () => {
    try {
      if (!shippingAddress.value) {
        alert('Please select a shipping address');
        return;
      }

      if (!order.value || !order.value.items || order.value.items.length === 0) {
        alert('No products in order');
        return;
      }

      const userId = localStorage.getItem('userId');
      if (!userId) {
        alert('Please login to place an order');
        return;
      }

      // Check wallet balance if payment method is wallet
      if (selectedPaymentMethod.value === 1) {
        await fetchWalletBalance();
        if (walletBalance.value < finalTotal.value) {
          alert(`Insufficient wallet balance. Your current balance is ${formatPrice(walletBalance.value)}. Please add more funds or choose another payment method.`);
          return;
        }
      }

      // Convert size values to size IDs and prepare order details
      const orderDetailsPromises = order.value.items.map(async (item) => {
        const sizeId = await getSizeIdByValue(Number(item.selectedSize));
        const manufacturerId = 2;
        if (!sizeId) {
          throw new Error(`Size ID not found for size ${item.selectedSize}`);
        }
        return {
          customShoeDesignId: parseInt(item.id),
          quantity: parseInt(item.quantity || 1),
          sizeId: sizeId,
          manufacturerId: manufacturerId
        };
      });

      const orderDetailsArr = await Promise.all(orderDetailsPromises);
      const orderDetail = orderDetailsArr[0];
      const voucherId = selectedVoucherId.value ? selectedVoucherId.value : null;

      const orderData = {
        userId: parseInt(userId),
        shippingInfoId: parseInt(shippingAddress.value.id),
        paymentMethod: selectedPaymentMethod.value,
        voucherId: voucherId,
        orderDetail: orderDetail,
      };

      console.log('Sending order data:', JSON.stringify(orderData, null, 2));

      const response = await postOrder(orderData);
      console.log('Order response:', response);
      
      localStorage.removeItem('orderData');

      // Check payment method first for specific handling
      if (selectedPaymentMethod.value === 1) { // Wallet payment
        if (response && response.code === 200) {
          // For wallet, successful order means redirect to success page
          router.push('/paymentSuccessPage');
        } else {
          // Wallet payment failed
          throw new Error(response?.message || 'Wallet payment failed');
        }
        return; // Exit after handling wallet payment
      }

      // For other payment methods (like PayOS = 0)
      if (response && response.paymentUrl) {
        // If a payment URL is returned, redirect there (e.g., to PayOS gateway)
        window.location.href = response.paymentUrl;
        return;
      }
      
      // If no payment URL but order is successful (code 200) for non-wallet methods
      if (response && response.code === 200) {
         alert('Order placed successfully!');
         router.push('/'); // Redirect to home or a general success page
         return;
      }

      // If none of the above, it's an error for non-wallet methods
      throw new Error(response?.message || 'Order failed');

    } catch (err) {
      console.error('Error placing order:', err);
      const errorMessage = err.message || 'An error occurred while placing the order. Please try again.';
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
        errorMessage.value = 'Please enter a discount code';
        return;
      }

      // Find the voucher in available vouchers
      const voucher = availableVouchers.value.find(v => 
        v.code.toLowerCase() === voucherCode.value.toLowerCase() && 
        !v.isUsed && 
        (v.status === 0 || v.status === '0') &&
        new Date(v.expiryDate) > new Date()
      );

      if (voucher) {
        voucherApplied.value = true;
        discountAmount.value = voucher.discountAmount;
        selectedVoucherId.value = voucher.id;
        // Recalculate the total
        calculateTotal();
        alert('Coupon applied successfully!'); // Keep English
      } else {
        // Check specific reasons why voucher is invalid
        const existingVoucher = availableVouchers.value.find(v => 
          v.code.toLowerCase() === voucherCode.value.toLowerCase()
        );

        if (!existingVoucher) {
          errorMessage.value = 'Discount code does not exist';
        } else if (existingVoucher.isUsed) {
          errorMessage.value = 'Discount code has been used';
        } else if (!(existingVoucher.status === 0 || existingVoucher.status === '0')) {
          errorMessage.value = 'Discount code is not active';
        } else if (new Date(existingVoucher.expiryDate) <= new Date()) {
          errorMessage.value = 'Discount code has expired';
        } else {
          errorMessage.value = 'Invalid discount code';
        }
      }
    } catch (error) {
      console.error('Error applying voucher:', error);
      errorMessage.value = 'An error occurred while applying the discount code';
    }
  };

  // Update the removeVoucher function
  const removeVoucher = () => {
    voucherCode.value = '';
    voucherApplied.value = false;
    discountAmount.value = 0;
    errorMessage.value = '';
    selectedVoucherId.value = null;
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
  
  // Add method to handle payment method selection
  const selectPaymentMethod = (method) => {
    selectedPaymentMethod.value = method;
  };
  
  // Add function to fetch wallet balance
  const fetchWalletBalance = async () => {
    try {
      const userId = localStorage.getItem('userId');
      if (!userId) return;
      
      const response = await getBalance(userId);
      if (response && response.data && response.data.balance !== undefined) {
        walletBalance.value = response.data.balance;
      }
    } catch (error) {
      console.error('Error fetching wallet balance:', error);
      walletBalance.value = 0;
    }
  };

  // Add wallet balance display in payment method section
  const getWalletBalanceDisplay = computed(() => {
    return formatPrice(walletBalance.value);
  });
  
  onMounted(() => {
    Promise.all([
      fetchOrder(), 
      fetchShippingAddress(),
      loadAvailableVouchers(),
      fetchWalletBalance() // Add this
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
    color: #2196f3 !important;
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

  .payment-method-section {
    background: #f8f9fa;
    padding: 16px;
    border-radius: 8px;
  }

  .payment-methods {
    display: flex;
    flex-direction: column;
    gap: 12px;
  }

  .payment-method-option {
    display: flex;
    align-items: center;
    gap: 16px;
    padding: 16px;
    background: white;
    border: 2px solid #e3e8ef;
    border-radius: 8px;
    cursor: pointer;
    transition: all 0.2s ease;
  }

  .payment-method-option:hover {
    border-color: #4f46e5;
    transform: translateY(-1px);
  }

  .payment-method-option.active {
    border-color: #4f46e5;
    background: #eef2ff;
  }

  .method-icon {
    width: 40px;
    height: 40px;
    border-radius: 8px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 20px;
  }

  .method-icon.payos {
    background-color: #e0e7ff;
    color: #4f46e5;
  }

  .method-icon.wallet {
    background-color: #dcfce7;
    color: #16a34a;
  }

  .method-info {
    flex: 1;
  }

  .method-info h6 {
    margin: 0 0 4px;
    font-size: 16px;
    font-weight: 600;
    color: #1a1f36;
  }

  .method-info p {
    font-size: 14px;
    color: #697386;
  }

  .method-radio input[type="radio"] {
    width: 20px;
    height: 20px;
    cursor: pointer;
  }

  @media (max-width: 768px) {
    .payment-method-option {
      padding: 12px;
    }

    .method-icon {
      width: 36px;
      height: 36px;
      font-size: 18px;
    }

    .method-info h6 {
      font-size: 14px;
    }

    .method-info p {
      font-size: 12px;
    }
  }
  </style>
  
