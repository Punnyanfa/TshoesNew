
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
              <p>{{ order.address }}</p>
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
                    <ul>
                      <li v-for="address in addresses" :key="address.id">
                        <input type="radio" :value="address" v-model="selectedAddress">
                        {{ address.name }} - {{ address.phone }} - {{ address.detail }}
                        <button class="btn btn-link" @click="handleUpdateAddress(address)">Cập nhật</button>
                      </li>
                    </ul>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
                    <button type="button" class="btn btn-primary" @click="confirmAddress">Xác nhận</button>
                  </div>
                </div>
              </div>
            </div>
  
            <!-- Modal cập nhật địa chỉ (Bootstrap Modal) -->
            <!-- <div class="modal fade" id="updateAddressModal" tabindex="-1" aria-labelledby="updateAddressModalLabel" aria-hidden="true">
              <div class="modal-dialog">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title" id="updateAddressModalLabel">Cập nhật địa chỉ</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>
                  <div class="modal-body">
                    <input v-model="selectedAddress.name" placeholder="Họ và tên" class="form-control mb-2" />
                    <input v-model="selectedAddress.phone" placeholder="Số điện thoại" class="form-control mb-2" />
                    <input v-model="selectedAddress.detail" placeholder="Địa chỉ cụ thể" class="form-control mb-2" />
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Trở Lại</button>
                    <button type="button" class="btn btn-primary" @click="updateAddress">Hoàn thành</button>
                  </div>
                </div>
              </div>
            </div> -->
  
            <div class="product-section">
              <h5>Sản phẩm</h5>
              <table class="table">
                <thead>
                  <tr>
                    <th>Sản phẩm</th>
                    <th>Đơn giá</th>
                    <th>Số lượng</th>
                    <th>Thành tiền</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="item in order.items" :key="item.id">
                    <td>{{ item.name }}</td>
                    <td>{{ item.price }}</td>
                    <td>{{ item.quantity }}</td>
                    <td>{{ item.total }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
  
            <div class="summary-section">
              <h5>Tổng số tiền</h5>
              <p>Tổng tiền hàng: {{ order.totalPrice }}</p>
              <p>Phí vận chuyển: {{ order.shippingCost }}</p>
              <p>Tổng thanh toán: {{ order.totalPayment }}</p>
            </div>
  
            <button class="btn btn-primary">Đặt hàng</button>
          </div>
        </div>
      </section>
      <Footer />
    </div>
    
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
 
  const order = ref(null);
  const loading = ref(true);
  const selectedAddress = ref(null);
  
  const addresses = ref([
    { id: 1, name: "Hoàng Đình Đức", phone: "(+84) 366 861 374", detail: "61/55 đường nguyễn công trứ, kp nội hoá 1, Phường Bình An, Thành Phố Dĩ An, Bình Dương" },
    { id: 2, name: "Hoàng Đình đức", phone: "(+84) 366 861 374", detail: "Hiền đao- tân tiến- Xã Tân Liên, Huyện Hướng Hóa, Quảng Trị" },
    // Add more addresses as needed
  ]);
  
  const fetchOrder = async () => {
    try {
      // Fake data
      order.value = {
        id: 1,
        address: addresses.value[0].detail,
        items: [
          { id: 1, name: "Áo Sơ Mi Nam Pie Car", price: 399.550, quantity: 1, total: 399.550 },
          { id: 2, name: "Bảo hiểm Thời trang", price: 3.999, quantity: 1, total: 3.999 }
        ],
        totalPrice: 399.550,
        shippingCost: 42.500,
        totalPayment: 442.050
      };
    } catch (error) {
      console.error('Error fetching order:', error);
    } finally {
      loading.value = false;
    }
  };
  
  const confirmAddress = () => {
    if (selectedAddress.value) {
      order.value.address = selectedAddress.value.detail;
      console.log('Address confirmed:', selectedAddress.value);
      const addressModal = new bootstrap.Modal(document.getElementById('addressModal'));
      addressModal.hide();
    } else {
      console.warn('No address selected');
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
    const addressModal = new bootstrap.Modal(document.getElementById('addressModal'));
    addressModal.show(); // Show the address modal when clicked
  };
  
  onMounted(fetchOrder);
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
  
  .btn-link {
    color: #007bff;
    text-decoration: none;
  }
  
  .btn-link:hover {
    text-decoration: underline;
  }
  </style>
  