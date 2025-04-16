<template>
    <div class="container py-5">
      <h1 class="mb-4">Lịch sử đơn hàng</h1>
      
      <!-- Filter section -->
      <div class="card mb-4">
        <div class="card-body">
          <div class="row g-3">
            <div class="col-md-4">
              <label for="status" class="form-label">Trạng thái</label>
              <select v-model="filters.status" class="form-select" id="status">
                <option value="">Tất cả trạng thái</option>
                <option value="completed">Đã hoàn thành</option>
                <option value="processing">Đang xử lý</option>
                <option value="cancelled">Đã hủy</option>
              </select>
            </div>
            <div class="col-md-4">
              <label for="dateRange" class="form-label">Khoảng thời gian</label>
              <select v-model="filters.dateRange" class="form-select" id="dateRange">
                <option value="all">Tất cả thời gian</option>
                <option value="30days">30 ngày qua</option>
                <option value="6months">6 tháng qua</option>
                <option value="1year">1 năm qua</option>
              </select>
            </div>
            <div class="col-md-4 d-flex align-items-end">
              <button @click="applyFilters" class="btn btn-primary w-100">Lọc đơn hàng</button>
            </div>
          </div>
        </div>
      </div>
      
      <!-- Orders list -->
      <div v-if="filteredOrders.length > 0">
        <div class="card mb-3" v-for="order in paginatedOrders" :key="order.id">
          <div class="card-header d-flex justify-content-between align-items-center">
            <div>
              <span class="fw-bold">Đơn hàng #{{ order.id }}</span>
              <span class="ms-3 text-muted">{{ formatDate(order.date) }}</span>
            </div>
            <div>
              <span :class="getStatusBadgeClass(order.status)">{{ getStatusText(order.status) }}</span>
            </div>
          </div>
          <div class="card-body">
            <div class="row">
              <div class="col-md-8">
                <div v-for="(item, index) in order.items" :key="index" class="d-flex mb-2">
                  <div class="flex-shrink-0">
                    <img :src="item.image" alt="Product image" class="img-thumbnail" style="width: 60px; height: 60px; object-fit: cover;">
                  </div>
                  <div class="ms-3">
                    <h6 class="mb-0">{{ item.name }}</h6>
                    <div class="text-muted small">
                      <span>{{ item.quantity }} x {{ formatCurrency(item.price) }}</span>
                    </div>
                  </div>
                </div>
              </div>
              <div class="col-md-4 border-start">
                <div class="d-flex justify-content-between mb-2">
                  <span>Tổng tiền hàng:</span>
                  <span>{{ formatCurrency(order.subtotal) }}</span>
                </div>
                <div class="d-flex justify-content-between mb-2">
                  <span>Phí vận chuyển:</span>
                  <span>{{ formatCurrency(order.shipping) }}</span>
                </div>
                <div class="d-flex justify-content-between fw-bold">
                  <span>Tổng thanh toán:</span>
                  <span>{{ formatCurrency(order.total) }}</span>
                </div>
                <div class="mt-3">
                  <button class="btn btn-outline-primary btn-sm w-100 mb-2">Xem chi tiết</button>
                  <button v-if="order.status === 'completed'" class="btn btn-outline-secondary btn-sm w-100">Mua lại</button>
                  <button v-if="order.status === 'processing'" class="btn btn-outline-danger btn-sm w-100">Hủy đơn hàng</button>
                </div>
              </div>
            </div>
          </div>
        </div>
        
        <!-- Pagination -->
        <nav aria-label="Page navigation">
          <ul class="pagination justify-content-center">
            <li class="page-item" :class="{ disabled: currentPage === 1 }">
              <a class="page-link" href="#" @click.prevent="changePage(currentPage - 1)">Trước</a>
            </li>
            <li v-for="page in totalPages" :key="page" class="page-item" :class="{ active: page === currentPage }">
              <a class="page-link" href="#" @click.prevent="changePage(page)">{{ page }}</a>
            </li>
            <li class="page-item" :class="{ disabled: currentPage === totalPages }">
              <a class="page-link" href="#" @click.prevent="changePage(currentPage + 1)">Sau</a>
            </li>
          </ul>
        </nav>
      </div>
      
      <!-- Empty state -->
      <div v-else class="text-center py-5">
        <div class="mb-3">
          <i class="bi bi-bag-x" style="font-size: 3rem;"></i>
        </div>
        <h4>Không tìm thấy đơn hàng nào</h4>
        <p class="text-muted">Bạn chưa có đơn hàng nào hoặc không có đơn hàng phù hợp với bộ lọc.</p>
        <button class="btn btn-primary">Tiếp tục mua sắm</button>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, computed } from 'vue';
  
  // Sample order data
  const orders = ref([
    {
      id: '1001',
      date: '2023-04-15T08:30:00',
      status: 'completed',
      items: [
        { 
          name: 'Điện thoại iPhone 13', 
          quantity: 1, 
          price: 20990000,
          image: 'https://via.placeholder.com/60'
        },
        { 
          name: 'Ốp lưng iPhone 13', 
          quantity: 2, 
          price: 250000,
          image: 'https://via.placeholder.com/60'
        }
      ],
      subtotal: 21490000,
      shipping: 30000,
      total: 21520000
    },
    {
      id: '1002',
      date: '2023-04-10T14:45:00',
      status: 'processing',
      items: [
        { 
          name: 'Laptop Dell XPS 13', 
          quantity: 1, 
          price: 32990000,
          image: 'https://via.placeholder.com/60'
        }
      ],
      subtotal: 32990000,
      shipping: 0,
      total: 32990000
    },
    {
      id: '1003',
      date: '2023-03-28T11:20:00',
      status: 'cancelled',
      items: [
        { 
          name: 'Tai nghe Bluetooth Sony', 
          quantity: 1, 
          price: 2490000,
          image: 'https://via.placeholder.com/60'
        },
        { 
          name: 'Sạc dự phòng 10000mAh', 
          quantity: 1, 
          price: 590000,
          image: 'https://via.placeholder.com/60'
        }
      ],
      subtotal: 3080000,
      shipping: 30000,
      total: 3110000
    },
    {
      id: '1004',
      date: '2023-03-15T09:10:00',
      status: 'completed',
      items: [
        { 
          name: 'Áo thun nam', 
          quantity: 2, 
          price: 250000,
          image: 'https://via.placeholder.com/60'
        },
        { 
          name: 'Quần jeans', 
          quantity: 1, 
          price: 450000,
          image: 'https://via.placeholder.com/60'
        }
      ],
      subtotal: 950000,
      shipping: 30000,
      total: 980000
    },
    {
      id: '1005',
      date: '2023-02-20T16:35:00',
      status: 'completed',
      items: [
        { 
          name: 'Nồi cơm điện', 
          quantity: 1, 
          price: 1290000,
          image: 'https://via.placeholder.com/60'
        }
      ],
      subtotal: 1290000,
      shipping: 50000,
      total: 1340000
    }
  ]);
  
  // Filters
  const filters = ref({
    status: '',
    dateRange: 'all'
  });
  
  // Pagination
  const currentPage = ref(1);
  const itemsPerPage = 3;
  
  // Apply filters
  const applyFilters = () => {
    currentPage.value = 1;
  };
  
  // Filter orders based on selected filters
  const filteredOrders = computed(() => {
    let result = [...orders.value];
    
    // Filter by status
    if (filters.value.status) {
      result = result.filter(order => order.status === filters.value.status);
    }
    
    // Filter by date range
    if (filters.value.dateRange !== 'all') {
      const now = new Date();
      let startDate;
      
      switch (filters.value.dateRange) {
        case '30days':
          startDate = new Date(now.setDate(now.getDate() - 30));
          break;
        case '6months':
          startDate = new Date(now.setMonth(now.getMonth() - 6));
          break;
        case '1year':
          startDate = new Date(now.setFullYear(now.getFullYear() - 1));
          break;
        default:
          startDate = null;
      }
      
      if (startDate) {
        result = result.filter(order => new Date(order.date) >= startDate);
      }
    }
    
    return result;
  });
  
  // Calculate total pages
  const totalPages = computed(() => {
    return Math.ceil(filteredOrders.value.length / itemsPerPage);
  });
  
  // Get paginated orders
  const paginatedOrders = computed(() => {
    const startIndex = (currentPage.value - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    return filteredOrders.value.slice(startIndex, endIndex);
  });
  
  // Change page
  const changePage = (page) => {
    if (page >= 1 && page <= totalPages.value) {
      currentPage.value = page;
    }
  };
  
  // Format date
  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('vi-VN', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };
  
  // Format currency
  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('vi-VN', {
      style: 'currency',
      currency: 'VND'
    }).format(amount);
  };
  
  // Get status text
  const getStatusText = (status) => {
    switch (status) {
      case 'completed':
        return 'Đã hoàn thành';
      case 'processing':
        return 'Đang xử lý';
      case 'cancelled':
        return 'Đã hủy';
      default:
        return status;
    }
  };
  
  // Get status badge class
  const getStatusBadgeClass = (status) => {
    let baseClass = 'badge ';
    
    switch (status) {
      case 'completed':
        return baseClass + 'bg-success';
      case 'processing':
        return baseClass + 'bg-primary';
      case 'cancelled':
        return baseClass + 'bg-danger';
      default:
        return baseClass + 'bg-secondary';
    }
  };
  </script>
  
  <style scoped>
  /* Add Bootstrap icons */
  @import url("https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css");
  
  /* Add Bootstrap CSS */
  @import url("https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css");
  
  .card {
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
  }
  
  .card-header {
    background-color: #f8f9fa;
    border-bottom: 1px solid #eaeaea;
  }
  
  .img-thumbnail {
    border-radius: 6px;
  }
  
  .pagination .page-link {
    color: #0d6efd;
  }
  
  .pagination .page-item.active .page-link {
    background-color: #0d6efd;
    border-color: #0d6efd;
  }
  </style>