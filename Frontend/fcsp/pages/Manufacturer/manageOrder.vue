<template>
  <div>
    <HeaderManu @logout="logout" />
    <div class="manufacturer-layout">
      <div class="main-content">
        <div class="container-fluid mt-4">
          <!-- Orders Section -->
          <div class="card">
            <div class="card-header bg-info text-white">
              <h4 class="mb-0">Danh sách đơn hàng</h4>
            </div>
            <div class="card-body">
              <div class="table-responsive">
                <table class="table table-striped table-hover">
                  <thead>
                    <tr>
                      <th>Mã đơn</th>
                      <th>Khách hàng</th>
                      <th>Ngày tạo</th>
                      <th>Trạng thái</th>
                      <th>Tổng tiền</th>
                      <th>Hành động</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="order in orders" :key="order.id">
                      <td>{{ order.code }}</td>
                      <td>{{ order.customer }}</td>
                      <td>{{ order.createdAt }}</td>
                      <td>
                        <span :class="{
                          'badge bg-success': order.status === 'Đã xác nhận',
                          'badge bg-warning text-dark': order.status === 'Chờ xác nhận',
                          'badge bg-danger': order.status === 'Đã hủy'
                        }">
                          {{ order.status }}
                        </span>
                      </td>
                      <td>{{ formatCurrency(order.total) }}</td>
                      <td>
                        <button class="btn btn-sm btn-primary" @click="viewOrder(order)"><i class="bi bi-eye"></i> Xem</button>
                      </td>
                    </tr>
                    <tr v-if="orders.length === 0">
                      <td colspan="6" class="text-center">Không có đơn hàng nào</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </div>

          <!-- Order Detail Modal (optional, hiển thị khi click Xem) -->
          <div v-if="showOrderModal" class="modal-overlay">
            <div class="modal-dialog">
              <div class="modal-content">
                <div class="modal-header bg-info">
                  <h5 class="modal-title text-white">Chi tiết đơn hàng</h5>
                  <button type="button" class="btn-close btn-close-white" @click="hideOrderModal"></button>
                </div>
                <div class="modal-body">
                  <div v-if="selectedOrder">
                    <p><b>Mã đơn:</b> {{ selectedOrder.code }}</p>
                    <p><b>Khách hàng:</b> {{ selectedOrder.customer }}</p>
                    <p><b>Ngày tạo:</b> {{ selectedOrder.createdAt }}</p>
                    <p><b>Trạng thái:</b> {{ selectedOrder.status }}</p>
                    <p><b>Tổng tiền:</b> {{ formatCurrency(selectedOrder.total) }}</p>
                    <!-- Thêm chi tiết sản phẩm nếu muốn -->
                  </div>
                </div>
              </div>
            </div>
          </div>

        </div>
      </div>
    </div>
  </div>
</template>

<script>
import HeaderManu from '@/components/HeaderManu.vue';
export default {
  name: 'ManageOrder',
  components: { HeaderManu },
  data() {
    return {
      orders: [
        { id: 1, code: 'ORD001', customer: 'Nguyễn Văn A', createdAt: '2024-06-01', status: 'Chờ xác nhận', total: 1200000 },
        { id: 2, code: 'ORD002', customer: 'Trần Thị B', createdAt: '2024-06-02', status: 'Đã xác nhận', total: 950000 },
        { id: 3, code: 'ORD003', customer: 'Lê Văn C', createdAt: '2024-06-03', status: 'Đã hủy', total: 500000 }
      ],
      showOrderModal: false,
      selectedOrder: null
    }
  },
  methods: {
    formatCurrency(value) {
      return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
      }).format(value);
    },
    viewOrder(order) {
      this.selectedOrder = order;
      this.showOrderModal = true;
    },
    hideOrderModal() {
      this.showOrderModal = false;
      this.selectedOrder = null;
    },
    logout() {
      this.$router.push('/login');
    }
  }
}
</script>

<style>
.manufacturer-layout {
  min-height: 100vh;
  background-color: #f8f9fa;
}

.main-content {
  padding: 20px;
}

.card {
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  border: none;
  margin-bottom: 1.5rem;
}

.card-header {
  border-bottom: 0;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-dialog {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  margin: 1.75rem auto;
  max-width: 500px;
  position: relative;
  width: 100%;
}

.modal-content {
  position: relative;
  display: flex;
  flex-direction: column;
  width: 100%;
  pointer-events: auto;
  background-color: #fff;
  background-clip: padding-box;
  border: 1px solid rgba(0, 0, 0, 0.2);
  border-radius: 0.3rem;
  outline: 0;
}

.modal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem;
  border-bottom: 1px solid #dee2e6;
  border-top-left-radius: 0.3rem;
  border-top-right-radius: 0.3rem;
}

.modal-body {
  position: relative;
  flex: 1 1 auto;
  padding: 1rem;
}

.btn-close {
  background: transparent;
  border: 0;
  padding: 0.5rem;
  cursor: pointer;
}

.btn-close-white {
  filter: invert(1) grayscale(100%) brightness(200%);
}
</style>
  